namespace Histogram_Contrast_Corrector.DataClasses
{
    public class RasterData
    {
        private string _name;
        private string _fullPath;

        private int _xSize;
        private int _ySize;

        private bool _ignoreZero;

        private List<BandData> _bands;

        private int _redID;
        private int _greenID;
        private int _blueID;

        private Bitmap _bitmap;
        private bool _isNotUpdated = false;

        public string Name => _name;
        public string FullPath => _fullPath;

        public int XSize => _xSize;
        public int YSize => _ySize;

        public bool IgnoreZero => _ignoreZero;

        public int BandsCount => _bands.Count;

        public RasterData(string name, string fullPath, int xSize, int ySize, bool ignoreZero)
        {
            _name = name;
            _fullPath = fullPath;

            _xSize = xSize;
            _ySize = ySize;

            _bitmap = new Bitmap(XSize, YSize);

            _ignoreZero = ignoreZero;

            _bands = new List<BandData>();
        }

        public void AddBand(BandData band)
        {
            _bands.Add(band);
        }

        public void SetViewBands(int redID, int greenID, int blueID)
        {
            _redID = redID;
            _greenID = greenID;
            _blueID = blueID;

            _isNotUpdated = false;
        }

        public void CalculateBandsHistogram()
        {
            foreach (BandData band in _bands)
            {
                band.CalculateHistogram();
            }
        }

        public BandData? GetBand(int bandIndex)
        {
            if (0 > bandIndex || bandIndex > BandsCount)
                return null;

            return _bands[bandIndex];
        }

        public Bitmap? GetBitmap()
        {
            if (_isNotUpdated)
                return _bitmap;

            BandData? redBand = GetBand(_redID);
            BandData? greenBand = GetBand(_greenID);
            BandData? blueBand = GetBand(_blueID);

            if (redBand is null || greenBand is null || blueBand is null)
                return null;

            for (int y = 0; y < YSize; y++)
            {
                for (int x = 0; x < XSize; x++)
                {
                    float r = redBand.GetPixelValue(x, y);
                    float g = greenBand.GetPixelValue(x, y);
                    float b = blueBand.GetPixelValue(x, y);

                    if (_ignoreZero && r == 0 && g == 0 && b == 0)
                        continue;

                    int red = r == 0 ? 0 : (int)((r - redBand.Minimum) / (redBand.Maximum - redBand.Minimum) * 255);
                    int green = g == 0 ? 0 : (int)((g - greenBand.Minimum) / (greenBand.Maximum - greenBand.Minimum) * 255);
                    int blue = b == 0 ? 0 : (int)((b - blueBand.Minimum) / (blueBand.Maximum - blueBand.Minimum) * 255);

                    Color color = Color.FromArgb(red, green, blue);
                    _bitmap.SetPixel(x, y, color);
                }
            }

            _isNotUpdated = true;
            return _bitmap;
        }
    }
}
