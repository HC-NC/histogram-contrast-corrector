using Histogram_Contrast_Corrector.DataClasses;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Diagnostics.Metrics;

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

        private void BandForm_Load(object sender, EventArgs e)
        {
            this.Text = $"{_band.Raster.Name}/{_band.Name}";


            int[]? histogram = _band.Histogram;

            if (histogram is null)
            {
                _band.CalculateHistogram();
                histogram = _band.Histogram;
            }

            if (histogram is null)
            {
                this.Close();
                return;
            }

            var histSeries = new HistogramSeries();
            var lineSeries = new LineSeries();

            int sm = histogram.Sum();
            double tmp = 0;

            for (int i = 0; i < histogram.Length; i++)
            {
                histSeries.Items.Add(new HistogramItem(i + _band.Minimum, i + _band.Minimum + 1, histogram[i], 0));

                tmp += histogram[i];
                lineSeries.Points.Add(new DataPoint(i + _band.Minimum, tmp / sm));
            }

            PlotModel plot = new PlotModel();

            plot.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Minimum = _band.Minimum, Maximum = _band.Maximum });
            plot.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Minimum = 0, Maximum = histogram.Max(), Key = "axesY1" });
            plot.Axes.Add(new LinearAxis() { Position = AxisPosition.Right, Minimum = 0, Maximum = 1d, Key = "axesY2" });

            histSeries.YAxisKey = "axesY1";
            lineSeries.YAxisKey = "axesY2";

            lineSeries.Color = OxyColor.FromRgb(255, 0, 0);

            plot.Series.Add(histSeries);
            plot.Series.Add(lineSeries);

            plotView1.Model = plot;
        }

        private void plotView1_DoubleClick(object sender, EventArgs e)
        {
            plotView1.Model.ResetAllAxes();
            plotView1.Refresh();
        }
    }
}
