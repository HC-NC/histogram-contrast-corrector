using Histogram_Contrast_Corrector.DataClasses;

namespace Histogram_Contrast_Corrector
{
    public partial class RasterForm : Form
    {
        private RasterData _raster;

        public RasterForm(RasterData raster)
        {
            InitializeComponent();

            _raster = raster;
        }

        private void RasterForm_Load(object sender, EventArgs e)
        {
            this.Text = _raster.Name;

            pathTextBox.Text = _raster.FullPath;
            xSizeTextBox.Text = _raster.XSize.ToString();
            ySizeTextBox.Text = _raster.YSize.ToString();
            ignoreZeroCheckBox.Checked = _raster.IgnoreZero;

            for (int i = 0; i < _raster.BandsCount; i++)
            {
                string item = string.Format("Band: {0}", i + 1);

                redComboBox.Items.Add(item);
                greenComboBox.Items.Add(item);
                blueComboBox.Items.Add(item);
            }

            redComboBox.SelectedIndex = _raster.RedID;
            greenComboBox.SelectedIndex = _raster.GreenID;
            blueComboBox.SelectedIndex = _raster.BlueID;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            _raster.SetViewBands(redComboBox.SelectedIndex, greenComboBox.SelectedIndex, blueComboBox.SelectedIndex);
            this.Close();
        }
    }
}
