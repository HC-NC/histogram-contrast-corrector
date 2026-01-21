using Histogram_Contrast_Corrector.DataClasses;
using System.Drawing.Drawing2D;

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

            pathTextBox.Text = _raster.Path;
            xSizeTextBox.Text = _raster.XSize.ToString();
            ySizeTextBox.Text = _raster.YSize.ToString();
            ignoreZeroCheckBox.Checked = _raster.IgnoreZero;

            for (int i = 0; i < _raster.BandsCount; i++)
            {
                string item = _raster.GetBand(i).Name;

                redComboBox.Items.Add(item);
                greenComboBox.Items.Add(item);
                blueComboBox.Items.Add(item);
            }

            redComboBox.SelectedIndex = _raster.RedID;
            greenComboBox.SelectedIndex = _raster.GreenID;
            blueComboBox.SelectedIndex = _raster.BlueID;

            interpolationComboBox.Items.AddRange(Enum.GetNames(typeof(InterpolationMode)));
            interpolationComboBox.Items.RemoveAt(interpolationComboBox.Items.Count - 1);
            interpolationComboBox.SelectedIndex = (int)_raster.InterpolationMode;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            _raster.SetViewBands(redComboBox.SelectedIndex, greenComboBox.SelectedIndex, blueComboBox.SelectedIndex);
            _raster.InterpolationMode = (InterpolationMode)interpolationComboBox.SelectedIndex;
            this.Close();
        }
    }
}
