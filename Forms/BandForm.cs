using Histogram_Contrast_Corrector.DataClasses;

namespace Histogram_Contrast_Corrector
{
    public partial class BandForm : Form
    {
        private BandData _band;

        public BandForm(BandData band)
        {
            InitializeComponent();

            _band = band;
        }
    }
}
