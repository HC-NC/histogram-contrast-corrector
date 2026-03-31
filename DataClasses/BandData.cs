using OSGeo.GDAL;
using System.ComponentModel;
using System.Runtime;

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
                    lock (_lockObj) // Защищаем критическую секцию
                    {
                        // Проверяем еще раз внутри лока
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
            _raster = raster;
            _name = name;

            _width = width;
            _height = height;

            _bandIndex = bandIndex;
            _ignoreZero = ignoreZero;
        }

        private void LoadValuesFromGDAL()
        {
            using (Dataset ds = Gdal.Open(_raster.Path, Access.GA_ReadOnly))
            using (Band gdalBand = ds.GetRasterBand(_bandIndex))
            {
                int arraySize = _width * _height;
                _values = new float[arraySize];

                gdalBand.ReadRaster(0, 0, _width, _height, _values, _width, _height, 0, 0);
            }

            CalculateMinMax();
        }

        public void Unload()
        {
            if (_values is null)
                return;

            _values = null;
        }

        public void Dispose()
        {
            _values = null;
            _histogram = null;
            _assesmentValues = null;
        }

        public void CalculateMinMax()
        {
            float[]? data = Values;

            if (data is null || data.Length == 0)
                return;

            if (_ignoreZero)
            {
                // Используем PLINQ для параллельной фильтрации и поиска
                var nonZeroValues = data.AsParallel().Where(v => v != 0);

                // Проверяем, есть ли вообще не нулевые элементы
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
                // Если нули игнорировать не нужно, просто ищем по всему массиву параллельно
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
            _histogram = new int[histogramSize];

            int totalLength = _width * _height;
            float localMin = _minimum;
            bool ignoreZero = _ignoreZero;

            // 1. Определяем, сколько потоков мы задействуем
            int threadCount = Environment.ProcessorCount;

            // 2. Создаем массив гистограмм для каждого потока отдельно
            int[][] threadHistograms = new int[threadCount][];
            for (int i = 0; i < threadCount; i++)
            {
                threadHistograms[i] = new int[histogramSize];
            }

            // 3. Простой Parallel.For по индексам потоков (без замыканий на тяжелые делегаты!)
            Parallel.For(0, threadCount, threadIndex =>
            {
                // Вычисляем кусок массива для конкретного потока
                int itemsPerThread = totalLength / threadCount;
                int startIdx = threadIndex * itemsPerThread;
                int endIdx = (threadIndex == threadCount - 1) ? totalLength : startIdx + itemsPerThread;

                int[] myHist = threadHistograms[threadIndex];

                for (int i = startIdx; i < endIdx; i++)
                {
                    float v = data[i];

                    if (ignoreZero && v == 0)
                        continue;

                    int index = (int)(v - localMin);

                    if (index >= 0 && index < histogramSize)
                    {
                        myHist[index]++;
                    }
                }
            });

            // 4. Схлопываем результаты всех потоков в одну гистограмму
            for (int t = 0; t < threadCount; t++)
            {
                for (int i = 0; i < histogramSize; i++)
                {
                    _histogram[i] += threadHistograms[t][i];
                }
            }

            _histogramSum = _histogram.Sum();
            CalculateAssesment();
        }

        private void CalculateAssesment()
        {
            if (_histogram is null || _histogramSum == 0)
                return;

            _assesmentValues = new float[_histogram.Length];

            _assesmentValues[0] = _histogram[0] / _histogramSum;

            // Этот цикл оставляем обычным (однопоточным)
            for (int i = 1; i < _histogram.Length; i++)
            {
                _assesmentValues[i] = _assesmentValues[i - 1] + (_histogram[i] / _histogramSum);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
