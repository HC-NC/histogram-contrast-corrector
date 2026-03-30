using OxyPlot;
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
        }

        public void Dispose()
        {
            _values = null;
            _histogram = null;
            _assesmentValues = null;
        }

        public void CalculateMinMax()
        {
            if (_values is null || _values.Length == 0)
                return;

            if (_ignoreZero)
            {
                // Используем PLINQ для параллельной фильтрации и поиска
                var nonZeroValues = _values.AsParallel().Where(v => v != 0);

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
                _minimum = _values.AsParallel().Min();
                _maximum = _values.AsParallel().Max();
            }
        }

        public void CalculateHistogram()
        {
            if (_values is null)
                return;

            if (_minimum >= _maximum)
                CalculateMinMax();

            int histogramSize = (int)(_maximum - _minimum) + 1;
            _histogram = new int[histogramSize];

            int totalLength = _width * _height;
            float localMin = _minimum;
            bool ignoreZero = _ignoreZero;

            // Запускаем параллельный цикл с изоляцией данных для каждого потока
            Parallel.For<int[]>(
                0,
                totalLength,

                // 1. Инициализация: создаем локальный массив для каждого потока
                () => new int[histogramSize],

                // 2. Тело цикла: каждый поток считает пиксели в свой собственный массив
                (i, state, localHist) =>
                {
                    float v = _values[i];

                    if (ignoreZero && v == 0)
                        return localHist;

                    int index = (int)(v - localMin);

                    if (index >= 0 && index < histogramSize)
                    {
                        localHist[index]++;
                    }

                    return localHist;
                },

                // 3. Финал: безопасно складываем результаты всех потоков в главный массив
                localHist =>
                {
                    lock (_histogram)
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

        public void CalculateAssesment()
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
