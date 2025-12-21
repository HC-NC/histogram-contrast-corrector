namespace Histogram_Contrast_Corrector.DataClasses
{
    public class BandData : IDisposable
    {
        private string _tmpDir;
        private string _tmpPath;

        private RasterData _raster;
        private string _name;

        private int _xSize;
        private int _ySize;

        private float[]? _values;
        private bool _isLoaded;

        private bool _ignoreZero;

        private float _minimum;
        private float _maximum;

        private int[]? _histogram;
        private float[]? _assesmentValues;

        private float _histogramSum = 0;

        public RasterData Raster => _raster;
        public string Name => _name;

        public int XSize => _xSize;
        public int YSize => _ySize;

        public bool IgnoreZero => _ignoreZero;

        public float Minimum => _minimum;
        public float Maximum => _maximum;

        public float[]? Values
        {
            get
            {
                if (!_isLoaded)
                    LoadValues();

                return _values;
            }
        }

        public int[]? Histogram => _histogram;
        public float[]? AssesmentValues => _assesmentValues;

        public BandData(RasterData raster, string name, int xSize, int ySize, float[] values, bool ignoreZero)
        {
            _tmpDir = Path.Combine(Application.StartupPath, "_temp");
            _tmpPath = Path.Combine(_tmpDir, $"{GetHashCode()}.bandtmp");

            _raster = raster;
            _name = name;

            _xSize = xSize;
            _ySize = ySize;

            _values = values;
            _isLoaded = true;

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

        private void LoadValues()
        {
            if (_isLoaded && _values is not null)
                return;

            if (!File.Exists(_tmpPath))
                return;

            _values = new float[XSize * YSize];

            using (BinaryReader reader = new BinaryReader(File.Open(_tmpPath, FileMode.Open)))
            {
                for (int i = 0; i < _values.Length; i++)
                    _values[i] = reader.ReadSingle();
            }

            _isLoaded = true;
        }

        public void UnloadValues()
        {
            if (!_isLoaded && _values is null) 
                return;

            if (!Directory.Exists(_tmpDir))
                Directory.CreateDirectory(_tmpDir);

            using (BinaryWriter writer = new BinaryWriter(File.Open(_tmpPath, FileMode.OpenOrCreate)))
            {
                foreach (float v in _values)
                    writer.Write(v);
            }

            _values = null;
            _isLoaded = false;
        }

        public void CalculateMinMax()
        {
            if (!_isLoaded)
                LoadValues();

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
            if (!_isLoaded)
                LoadValues();

            if (_values is null)
                return;

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
            if (!_isLoaded)
                LoadValues();

            if (_values is null)
                return 0;

            if (0 > x || x > _xSize)
                return 0;

            if (0 > y || y > _ySize)
                return 0;

            return _values[y * _xSize + x];
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
