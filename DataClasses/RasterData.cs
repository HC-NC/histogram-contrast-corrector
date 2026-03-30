using OSGeo.GDAL;
using System.ComponentModel;
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

        public void Dispose()
        {
            foreach (BandData b in _bands)
                b.Dispose();

            _bands.Clear();

            _bitmap.Dispose();
        }

        public static RasterData Load(string filePath, string fileName, bool ignoreZero)
        {
            // 1. Инициализируем GDAL
            Gdal.AllRegister();

            // 2. Открываем датасет
            using (Dataset ds = Gdal.Open(filePath, Access.GA_ReadOnly))
            {
                int width = ds.RasterXSize;
                int height = ds.RasterYSize;

                RasterData rasterData = new RasterData(fileName, filePath, width, height, ignoreZero);

                // Проходим по всем каналам
                for (int i = 1; i <= ds.RasterCount; i++)
                {
                    using (Band gdalBand = ds.GetRasterBand(i))
                    {
                        float[] buffer = new float[width * height];

                        // Вычитываем данные из GDAL в массив C# за один раз!
                        gdalBand.ReadRaster(0, 0, width, height, buffer, width, height, 0, 0);

                        BandData bandData = new BandData(rasterData, $"Band {i}", width, height, buffer, ignoreZero);
                        rasterData.AddBand(bandData);
                    } // gdalBand.Dispose() вызовется автоматически здесь благодаря using
                }

                return rasterData;
            } // ds.Dispose() вызовется автоматически здесь. Файл закрыт, память C++ освобождена!
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

        public void CalculateBandsHistogram(BackgroundWorker? worker)
        {
            for (int i = 0; i < _bands.Count; i++)
            {
                worker?.ReportProgress((int)((float)i / _bands.Count * 100f), $"Calculating the band histogram ({Name}\\{_bands[i].Name})");
                _bands[i].CalculateHistogram();
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

            int width = Width;
            int height = Height;

            // Блокируем память битмапа
            BitmapData bmpData = _bitmap.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);

            try
            {
                unsafe
                {
                    byte* ptr = (byte*)bmpData.Scan0;

                    // Распараллеливаем по строкам изображения
                    Parallel.For(0, height, y =>
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int pixelIndex = y * width + x;

                            float r = redBand.GetPixelValue(x, y);
                            float g = greenBand.GetPixelValue(x, y);
                            float b = blueBand.GetPixelValue(x, y);

                            // Пропускаем нули, если нужно
                            if (_ignoreZero && r == 0 && g == 0 && b == 0)
                                continue;

                            // Масштабирование
                            byte redByte = r == 0 ? (byte)0 : (byte)((r - redBand.Minimum) / (redBand.Maximum - redBand.Minimum) * 255);
                            byte greenByte = g == 0 ? (byte)0 : (byte)((g - greenBand.Minimum) / (greenBand.Maximum - greenBand.Minimum) * 255);
                            byte blueByte = b == 0 ? (byte)0 : (byte)((b - blueBand.Minimum) / (blueBand.Maximum - blueBand.Minimum) * 255);

                            // В формате Format32bppArgb байты идут в порядке: B, G, R, A
                            int offset = y * bmpData.Stride + x * 4;
                            ptr[offset] = blueByte;
                            ptr[offset + 1] = greenByte;
                            ptr[offset + 2] = redByte;
                            ptr[offset + 3] = 255; // Alpha
                        }
                    });
                }
            }
            finally
            {
                _bitmap.UnlockBits(bmpData);
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
