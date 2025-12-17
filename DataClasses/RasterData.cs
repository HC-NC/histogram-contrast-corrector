

namespace Histogram_Contrast_Corrector.DataClasses
{
    internal class RasterData
    {
        private string _name;
        private string _fullPath;

        private int _xSize;
        private int _ySize;

        private bool _ignoreZero;

        private List<BandData> _bands;

        public RasterData(string name, string fullPath, int xSize, int ySize, bool ignoreZero)
        {
            _name = name;
            _fullPath = fullPath;

            _xSize = xSize;
            _ySize = ySize;

            _ignoreZero = ignoreZero;

            _bands = new List<BandData>();
        }

        public void AddBand(BandData band)
        {
            _bands.Add(band);
        }
    }
}
