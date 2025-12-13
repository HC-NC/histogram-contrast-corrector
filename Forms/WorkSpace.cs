using OSGeo.GDAL;

namespace Histogram_Contrast_Corrector
{
    public partial class WorkSpace : Form
    {
        private string _imgPath;

        public WorkSpace()
        {
            InitializeComponent();

            //_imgPath = @"D:\Институт\ПГИС\Лабораторные ПГИС\1\LC08_L1TP_143021_20211224_20211230_01_T1\LC08_L1TP_143021_20211224_20211230_01_T1_B2.TIF";
            _imgPath = @"D:\Институт\ПГИС\Лабораторные ПГИС\1\LC08_L1TP_143021_20211224_20211230_01_T1\LC08.tif";
        }

        private void WorkSpace_Load(object sender, EventArgs e)
        {
            Dataset ds = Gdal.Open(_imgPath, Access.GA_ReadOnly);

            MessageBox.Show(ds.RasterCount.ToString());
        }
    }
}
