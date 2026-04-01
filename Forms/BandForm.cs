using Histogram_Contrast_Corrector.DataClasses;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Diagnostics.Metrics;
using Histogram_Contrast_Corrector.Properties;

namespace Histogram_Contrast_Corrector
{
    public partial class BandForm : Form
    {
        private BandData _band;

        public BandForm(BandData band)
        {
            InitializeComponent();

            _band = band ?? throw new ArgumentNullException(nameof(band));
        }

        private async void BandForm_Load(object sender, EventArgs e)
        {
            this.Text = $"{_band.Raster.Name} / {_band.Name}";
            propertyGrid1.SelectedObject = _band;

            if (_band.Histogram != null && _band.AssesmentValues != null)
            {
                BuildPlot(_band.Histogram, _band.AssesmentValues);
                return;
            }

            try
            {
                this.Text += $" ({Resources.StatusCalculating})";

                await Task.Run(() => _band.CalculateHistogram());

                if (_band.Histogram != null && _band.AssesmentValues != null)
                {
                    this.Text = $"{_band.Raster.Name} / {_band.Name}";
                    BuildPlot(_band.Histogram, _band.AssesmentValues);
                }
                else
                {
                    MessageBox.Show(Resources.ErrHistSize,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Resources.ErrLoadBand} {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void BuildPlot(int[] histogram, float[] assesmentValues)
        {
            var plotModel = new PlotModel();

            var xAxis = new LinearAxis { Position = AxisPosition.Bottom, Minimum = _band.Minimum, Maximum = _band.Maximum, Title = Resources.AxisValues };
            var yAxisHist = new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = histogram.Max(), Key = "axesY1", Title = Resources.AxisCount };
            var yAxisLine = new LinearAxis { Position = AxisPosition.Right, Minimum = 0, Maximum = 1.0, Key = "axesY2", Title = Resources.AxisAssessment };

            plotModel.Axes.Add(xAxis);
            plotModel.Axes.Add(yAxisHist);
            plotModel.Axes.Add(yAxisLine);

            var lineSeries = new LineSeries
            {
                YAxisKey = "axesY2",
                Color = OxyColor.FromRgb(255, 0, 0),
                Title = Resources.SeriesAssessment
            };

            if (histogram.Length > 1000)
            {
                var areaSeries = new AreaSeries
                {
                    YAxisKey = "axesY1",
                    Color = OxyColor.FromArgb(150, 0, 122, 204),
                    Fill = OxyColor.FromArgb(50, 0, 122, 204),
                    Title = Resources.SeriesHistogram
                };

                for (int i = 0; i < histogram.Length; i++)
                {
                    double x = i + _band.Minimum;
                    areaSeries.Points.Add(new DataPoint(x, histogram[i]));
                    lineSeries.Points.Add(new DataPoint(x, assesmentValues[i]));
                }
                plotModel.Series.Add(areaSeries);
            }
            else
            {
                var histSeries = new HistogramSeries
                {
                    YAxisKey = "axesY1",
                    FillColor = OxyColor.FromArgb(150, 0, 122, 204),
                    StrokeColor = OxyColor.FromRgb(0, 122, 204),
                    StrokeThickness = 0.5,
                    Title = Resources.SeriesHistogram
                };

                for (int i = 0; i < histogram.Length; i++)
                {
                    double startX = i + _band.Minimum;
                    histSeries.Items.Add(new HistogramItem(startX, startX + 1, histogram[i], 0));
                    lineSeries.Points.Add(new DataPoint(startX, assesmentValues[i]));
                }
                plotModel.Series.Add(histSeries);
            }

            plotModel.Series.Add(lineSeries);

            // Применяем модель к вьюверу
            plotView1.Model = plotModel;
        }

        private void plotView1_DoubleClick(object sender, EventArgs e)
        {
            if (plotView1.Model != null)
            {
                plotView1.Model.ResetAllAxes();
                plotView1.InvalidatePlot(true);
            }
        }

        private void BandForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            propertyGrid1.SelectedObject = null;
            _band.Unload();
        }
    }
}
