using Histogram_Contrast_Corrector.DataClasses;
using System.Drawing.Drawing2D;
using Histogram_Contrast_Corrector.Properties;

namespace Histogram_Contrast_Corrector
{
    public partial class RasterForm : Form
    {
        private RasterData _raster;

        public RasterForm(RasterData raster)
        {
            InitializeComponent();

            _raster = raster ?? throw new ArgumentNullException(nameof(raster));
        }

        private void RasterForm_Load(object sender, EventArgs e)
        {
            this.Text = _raster.Name;

            pathTextBox.Text = _raster.Path;
            xSizeTextBox.Text = _raster.Width.ToString();
            ySizeTextBox.Text = _raster.Height.ToString();
            ignoreZeroCheckBox.Checked = _raster.IgnoreZero;

            for (int i = 0; i < _raster.BandsCount; i++)
            {
                var band = _raster.GetBand(i);
                string bandName = band?.Name ?? $"Band {i + 1}";

                redComboBox.Items.Add(bandName);
                greenComboBox.Items.Add(bandName);
                blueComboBox.Items.Add(bandName);
            }

            redComboBox.SelectedIndex = _raster.RedID;
            greenComboBox.SelectedIndex = _raster.GreenID;
            blueComboBox.SelectedIndex = _raster.BlueID;

            FillInterpolationModes();
        }

        private void FillInterpolationModes()
        {
            interpolationComboBox.Items.Clear();

            interpolationComboBox.Items.Add(Resources.InterpNearest);
            interpolationComboBox.Items.Add(Resources.InterpBilinear);
            interpolationComboBox.Items.Add(Resources.InterpBicubic);
            interpolationComboBox.Items.Add(Resources.InterpHighQualityBilinear);
            interpolationComboBox.Items.Add(Resources.InterpHighQualityBicubic);

            int selectedIdx = _raster.InterpolationMode switch
            {
                InterpolationMode.NearestNeighbor => 0,
                InterpolationMode.Bilinear => 1,
                InterpolationMode.Bicubic => 2,
                InterpolationMode.HighQualityBilinear => 3,
                InterpolationMode.HighQualityBicubic => 4,
                _ => 0 
            };

            interpolationComboBox.SelectedIndex = selectedIdx;
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            _raster.SetViewBands(redComboBox.SelectedIndex, greenComboBox.SelectedIndex, blueComboBox.SelectedIndex);

            _raster.InterpolationMode = interpolationComboBox.SelectedIndex switch
            {
                0 => InterpolationMode.NearestNeighbor,
                1 => InterpolationMode.Bilinear,
                2 => InterpolationMode.Bicubic,
                3 => InterpolationMode.HighQualityBilinear,
                4 => InterpolationMode.HighQualityBicubic,
                _ => InterpolationMode.NearestNeighbor
            };

            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
