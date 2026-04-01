using OSGeo.GDAL;
using System.ComponentModel;

using Histogram_Contrast_Corrector.Properties;

namespace Histogram_Contrast_Corrector.DataClasses
{
    public class BandData : IDisposable
    {
        private readonly object _lockObj = new object();

        private RasterData _raster;
        private string _name;

        private int _width;
        private int _height;

        private float[]? _values;
        private int _bandIndex;

        private bool _ignoreZero;

        private float _minimum;
        private float _maximum;

        private int[]? _histogram;
        private float[]? _assesmentValues;

        private float _histogramSum = 0;
        private bool _isDisposed = false;

        public RasterData Raster => _raster;
        public string Name => _name;

        public int Width => _width;
        public int Height => _height;

        public bool IgnoreZero => _ignoreZero;

        public float Minimum => _minimum;
        public float Maximum => _maximum;

        [Browsable(false)]
        public float[]? Values
        {
            get
            {
                if (_values is null)
                {
                    lock (_lockObj)
                    {
                        if (_values is null)
                        {
                            LoadValuesFromGDAL();
                        }
                    }
                }
                return _values;
            }
        }

        [Browsable(false)]
        public int[]? Histogram => _histogram;

        [Browsable(false)]
        public float[]? AssesmentValues => _assesmentValues;

        public BandData(RasterData raster, string name, int width, int height, int bandIndex, bool ignoreZero)
        {
            _raster = raster ?? throw new ArgumentNullException(nameof(raster));
            _name = name;

            _width = width;
            _height = height;

            _bandIndex = bandIndex;
            _ignoreZero = ignoreZero;
        }

        private void LoadValuesFromGDAL()
        {
            try
            {
                using (Dataset ds = Gdal.Open(_raster.Path, Access.GA_ReadOnly))
                {
                    if (ds == null)
                        throw new Exception($"{Resources.ErrGdalOpen} {_raster.Path}");

                    using (Band gdalBand = ds.GetRasterBand(_bandIndex))
                    {
                        int arraySize = _width * _height;
                        _values = new float[arraySize];

                        CPLErr err = gdalBand.ReadRaster(0, 0, _width, _height, _values, _width, _height, 0, 0);

                        if (err != CPLErr.CE_None)
                            throw new Exception($"{Resources.ErrGdalRead} {_bandIndex}. Code: {err}");
                    }
                }

                CalculateMinMax();
            }
            catch (Exception ex)
            {
                _values = null;
                throw new ApplicationException($"{Resources.ErrLoadBand} {_name}: {ex.Message}", ex);
            }
        }

        public void Unload()
        {
            lock (_lockObj)
            {
                _values = null; // Выгружаем ТОЛЬКО "тяжелые" значения пикселей
            }
        }

        public void CalculateMinMax()
        {
            float[]? data = Values;

            if (data is null || data.Length == 0)
                return;

            if (_ignoreZero)
            {
                var nonZeroValues = data.AsParallel().Where(v => v != 0);

                if (nonZeroValues.Any())
                {
                    _minimum = nonZeroValues.Min();
                    _maximum = nonZeroValues.Max();
                }
                else
                {
                    _minimum = 0;
                    _maximum = 0;
                }
            }
            else
            {
                _minimum = data.AsParallel().Min();
                _maximum = data.AsParallel().Max();
            }
        }

        public void CalculateHistogram()
        {
            float[]? data = Values;

            if (data is null)
                return;

            if (_minimum >= _maximum)
                CalculateMinMax();

            int histogramSize = (int)(_maximum - _minimum) + 1;

            if (histogramSize <= 0)
                throw new Exception(Resources.ErrHistSize);

            _histogram = new int[histogramSize];

            float localMin = _minimum;
            bool ignoreZero = _ignoreZero;

            Parallel.ForEach(
                data,
                () => new int[histogramSize],
                (v, loopState, localHist) =>
                {
                    if (ignoreZero && v == 0)
                        return localHist;

                    int index = (int)(v - localMin);
                    if (index >= 0 && index < histogramSize)
                    {
                        localHist[index]++;
                    }
                    return localHist;
                },
                localHist =>
                {
                    lock (_lockObj)
                    {
                        for (int i = 0; i < histogramSize; i++)
                        {
                            _histogram[i] += localHist[i];
                        }
                    }
                }
            );

            _histogramSum = _histogram.Sum();
            CalculateAssesment();
        }

        private void CalculateAssesment()
        {
            if (_histogram is null || _histogramSum == 0)
                return;

            _assesmentValues = new float[_histogram.Length];
            _assesmentValues[0] = _histogram[0] / _histogramSum;

            for (int i = 1; i < _histogram.Length; i++)
            {
                _assesmentValues[i] = _assesmentValues[i - 1] + (_histogram[i] / _histogramSum);
            }
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            _values = null;
            _histogram = null;
            _assesmentValues = null;

            _isDisposed = true;
            GC.SuppressFinalize(this);
        }

        public override string ToString() => Name;
    }
}
