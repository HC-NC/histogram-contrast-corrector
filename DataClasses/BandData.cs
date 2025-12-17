

using System.Diagnostics.Metrics;
using System.Drawing;

namespace Histogram_Contrast_Corrector.DataClasses
{
    internal class BandData
    {
        private int _xSize;
        private int _ySize;

        private float[] _values;

        private float _minimum;
        private float _maximum;

        private int[]? _histogram;

        public BandData(int xSize, int ySize, float[] values)
        {
            _xSize = xSize;
            _ySize = ySize;

            _values = values;

            _minimum = _values.Max();
            _maximum = _values.Min();
        }

        public void CalculateMinMax(bool ignoreZero)
        {
            foreach (float v in _values)
            {
                if (ignoreZero && v == 0)
                    continue;

                _minimum = MathF.Min(_minimum, v);
                _maximum = MathF.Max(_maximum, v);
            }
        }

        public void CalculateHistogram(bool ignoreZero)
        {
            if (_minimum >=  _maximum)
                CalculateMinMax(ignoreZero);

            _histogram = new int[(int)(_maximum - _minimum) + 1];

            for (int y = 0; y < _ySize; y++)
            {
                for (int x = 0; x < _xSize; x++)
                {
                    double v = _values[y * _xSize + x];

                    if (ignoreZero && v == 0)
                        continue;

                    _histogram[(int)(v - _minimum)] += 1;
                }
            }
        }
    }
}
