using System.ComponentModel;

namespace Histogram_Contrast_Corrector.DataClasses
{
    public class BandData : IDisposable
    {
        private RasterData _raster;
        private string _name;

        private int _width;
        private int _height;

        private float[]? _values;

        private bool _ignoreZero;

        private float _minimum;
        private float _maximum;

        private int[]? _histogram;
        private float[]? _assesmentValues;

        private float _histogramSum = 0;

        public RasterData Raster => _raster;
        public string Name => _name;

        public int Width => _width;
        public int Height => _height;

        public bool IgnoreZero => _ignoreZero;

        public float Minimum => _minimum;
        public float Maximum => _maximum;

        [Browsable(false)]
        public float[]? Values => _values;

        [Browsable(false)]
        public int[]? Histogram => _histogram;

        [Browsable(false)]
        public float[]? AssesmentValues => _assesmentValues;

        public BandData(RasterData raster, string name, int width, int height, float[] values, bool ignoreZero)
        {
            _raster = raster;
            _name = name;

            _width = width;
            _height = height;

            _values = values;

            _ignoreZero = ignoreZero;

            _minimum = _values.Max();
            _maximum = _values.Min();
        }

        public void Dispose()
        {
            _values = null;
            _histogram = null;
            _assesmentValues = null;
        }

        public void CalculateMinMax()
        {
            if (_values is null)
                return;

            foreach (float v in _values)
            {
                if (_ignoreZero && v == 0)
                    continue;

                _minimum = MathF.Min(_minimum, v);
                _maximum = MathF.Max(_maximum, v);
            }
        }

        public void CalculateHistogram()
        {
            if (_values is null)
                return;

            if (_minimum >=  _maximum)
                CalculateMinMax();

            _histogram = new int[(int)(_maximum - _minimum) + 1];

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    double v = _values[y * _width + x];

                    if (_ignoreZero && v == 0)
                        continue;

                    _histogram[(int)(v - _minimum)] += 1;
                }
            }

            _histogramSum = _histogram.Sum();

            CalculateAssesment();
        }

        public void CalculateAssesment()
        {
            if (_histogram is null)
                return;

            _assesmentValues = new float[_histogram!.Length];

            _assesmentValues[0] = _histogram[0] / _histogramSum;

            for (int i = 1; i < _histogram.Length; i++)
            {
                _assesmentValues[i] = _assesmentValues[i - 1] + (_histogram[i] / _histogramSum);
            }
        }

        public float GetPixelValue(int x, int y)
        {
            if (_values is null)
                return 0;

            if (0 > x || x >= _width)
                return 0;

            if (0 > y || y >= _height)
                return 0;

            return _values[y * _width + x];
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
