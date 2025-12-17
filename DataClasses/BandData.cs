using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Histogram_Contrast_Corrector.DataClasses
{
    public class BandData
    {
        private RasterData _raster;
        private string _name;

        private int _xSize;
        private int _ySize;

        private float[] _values;

        private bool _ignoreZero;

        private float _minimum;
        private float _maximum;

        private int[]? _histogram;

        public RasterData Raster => _raster;
        public string Name => _name;

        public float Minimum => _minimum;
        public float Maximum => _maximum;

        public int[]? Histogram => _histogram;

        public BandData(RasterData raster, string name, int xSize, int ySize, float[] values, bool ignoreZero)
        {
            _raster = raster;
            _name = name;

            _xSize = xSize;
            _ySize = ySize;

            _values = values;

            _ignoreZero = ignoreZero;

            _minimum = _values.Max();
            _maximum = _values.Min();
        }

        public void CalculateMinMax()
        {
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
            if (_minimum >=  _maximum)
                CalculateMinMax();

            _histogram = new int[(int)(_maximum - _minimum) + 1];

            for (int y = 0; y < _ySize; y++)
            {
                for (int x = 0; x < _xSize; x++)
                {
                    double v = _values[y * _xSize + x];

                    if (_ignoreZero && v == 0)
                        continue;

                    _histogram[(int)(v - _minimum)] += 1;
                }
            }
        }

        public float GetPixelValue(int x, int y)
        {
            if (0 > x || x > _xSize)
                return 0;

            if (0 > y || y > _ySize)
                return 0;

            return _values[y * _xSize + x];
        }
    }
}
