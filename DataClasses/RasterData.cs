using OSGeo.GDAL;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Histogram_Contrast_Corrector.DataClasses
{
    public class RasterData : IDisposable
    {
        private string _name;
        private string _path;

        private int _width;
        private int _height;

        private bool _ignoreZero;

        private List<BandData> _bands;

        private int _redID;
        private int _greenID;
        private int _blueID;

        private Bitmap _bitmap;
        private bool _isNotUpdated = false;

        public string Name => _name;
        public string Path => _path;

        public int Width => _width;
        public int Height => _height;

        public bool IgnoreZero => _ignoreZero;

        public int BandsCount => _bands.Count;

        public int RedID => _redID;
        public int GreenID => _greenID;
        public int BlueID => _blueID;

        public InterpolationMode InterpolationMode { get; set; } = InterpolationMode.NearestNeighbor;

        public RasterData(string name, string path, int width, int height, bool ignoreZero)
        {
            _name = name;
            _path = path;

            _width = width;
            _height = height;

            _bitmap = new Bitmap(Width, Height);

            _ignoreZero = ignoreZero;

            _bands = new List<BandData>();
        }

        public List<BandData> GetBands() => _bands;

        public void Dispose()
        {
            foreach (BandData b in _bands)
                b.Dispose();

            _bands.Clear();

            _bitmap.Dispose();
        }

        public static RasterData Load(string filePath, string fileName, bool ignoreZero)
        {
            Gdal.AllRegister();

            using (Dataset ds = Gdal.Open(filePath, Access.GA_ReadOnly))
            {
                int width = ds.RasterXSize;
                int height = ds.RasterYSize;

                RasterData rasterData = new RasterData(fileName, filePath, width, height, ignoreZero);

                for (int i = 1; i <= ds.RasterCount; i++)
                {
                    BandData bandData = new BandData(rasterData, $"Band {i}", width, height, i, true);
                    rasterData.AddBand(bandData);
                }

                if (0 < rasterData.BandsCount && rasterData.BandsCount < 3)
                    rasterData.SetViewBands(0, 0, 0);
                else if (rasterData.BandsCount >= 3)
                    rasterData.SetViewBands(0, 1, 2);

                return rasterData;
            }
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

            int width = Width;
            int height = Height;

            float[]? rData = redBand.Values;
            float[]? gData = greenBand.Values;
            float[]? bData = blueBand.Values;

            if (rData is null || gData is null || bData is null)
                return null;

            BitmapData bmpData = _bitmap.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);

            try
            {
                unsafe
                {
                    byte* ptr = (byte*)bmpData.Scan0;

                    bool ignoreZero = _ignoreZero;

                    float redMin = redBand.Minimum;
                    float redMax = redBand.Maximum;
                    float greenMin = greenBand.Minimum;
                    float greenMax = greenBand.Maximum;
                    float blueMin = blueBand.Minimum;
                    float blueMax = blueBand.Maximum;

                    Parallel.For(0, height, y =>
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int idx = y * width + x;
                            float r = rData[idx];
                            float g = gData[idx];
                            float b = bData[idx];

                            int offset = y * bmpData.Stride + x * 4;

                            if (ignoreZero && r == 0 && g == 0 && b == 0)
                            {
                                ptr[offset] = 0;     // Blue
                                ptr[offset + 1] = 0; // Green
                                ptr[offset + 2] = 0; // Red
                                ptr[offset + 3] = 0; // Alpha
                                continue;
                            }

                            byte redByte = r == 0 ? (byte)0 : (byte)((r - redMin) / (redMax - redMin) * 255);
                            byte greenByte = g == 0 ? (byte)0 : (byte)((g - greenMin) / (greenMax - greenMin) * 255);
                            byte blueByte = b == 0 ? (byte)0 : (byte)((b - blueMin) / (blueMax - blueMin) * 255);

                            ptr[offset] = blueByte;      // B
                            ptr[offset + 1] = greenByte;  // G
                            ptr[offset + 2] = redByte;    // R
                            ptr[offset + 3] = 255;        // A 
                        }
                    });
                }
            }
            finally
            {
                _bitmap.UnlockBits(bmpData);

                redBand.Unload();
                greenBand.Unload();
                blueBand.Unload();
            }

            _isNotUpdated = true;
            return _bitmap;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
