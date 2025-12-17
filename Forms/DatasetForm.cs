using Histogram_Contrast_Corrector.DataClasses;

namespace Histogram_Contrast_Corrector
{
    public partial class DatasetForm : Form
    {
        private RasterData _raster;

        public DatasetForm(RasterData raster)
        {
            InitializeComponent();

            _raster = raster;
        }
    }
}
