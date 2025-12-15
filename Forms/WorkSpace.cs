using OSGeo.GDAL;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Histogram_Contrast_Corrector
{
    public partial class WorkSpace : Form
    {
        private string? _curFilePath;

        private Dataset? _curDS;

        public WorkSpace()
        {
            InitializeComponent();

            toolStripStatusLabel1.Text = "";
            toolStripStatusLabel2.Text = "";

            openFileDialog1.Filter = "All files|*.*|TIFF|*.tif";
        }

        ~WorkSpace()
        {
            _curDS?.Dispose();
        }

        private void WorkSpace_Load(object sender, EventArgs e)
        {
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _curFilePath = openFileDialog1.FileName;
                toolStripStatusLabel1.Text = "Current: " + openFileDialog1.SafeFileName;

                _curDS = Gdal.Open(_curFilePath, Access.GA_ReadOnly);
                Draw(_curDS.GetRasterBand(1));
            }
        }

        private void Draw(Band band)
        {
            double[] argout = new double[2];

            band.ComputeRasterMinMax(argout, 0);

            band.GetMinimum(out double maxV, out _);
            band.GetMaximum(out double minV, out _);

            double[] values = new double[band.XSize * band.YSize];

            band.ReadRaster(0, 0, band.XSize, band.YSize, values, band.XSize, band.YSize, 0, 0);

            foreach (double v in values)
            {
                if (v == 0)
                    continue;

                minV = Math.Min(minV, v);
                maxV = Math.Max(maxV, v);
            }

            toolStripStatusLabel2.Text = $"XSize: {band.XSize} YSize: {band.YSize} Min: {minV} Max: {maxV}";

            int[] histogram = new int[(int)(maxV - minV) + 1];
            Bitmap bitmap = new Bitmap(band.XSize, band.YSize);

            for (int y = 0; y < band.YSize; y++)
            {
                for (int x = 0; x < band.XSize; x++)
                {
                    double v = values[y * band.XSize + x];

                    if (v == 0)
                    {
                        bitmap.SetPixel(x, y, Color.Black);
                        continue;
                    }

                    histogram[(int)(v - minV)] += 1;

                    int c = (int)((v - minV) / (maxV - minV) * 255);
                    Color color = Color.FromArgb(c, c, c);
                    bitmap.SetPixel(x, y, color);
                }
            }

            var series = new HistogramSeries();

            for (int i = 0; i < histogram.Length; i++)
            {
                series.Items.Add(new HistogramItem(i + minV, i + minV + 1, histogram[i], 0));
            }

            PlotModel plot = new PlotModel();
            plot.Series.Add(series);

            plot.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Minimum = minV, Maximum = maxV });
            plot.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Minimum = 0, Maximum = histogram.Max() });

            plotView1.Model = plot;
            pictureBox1.Image = bitmap;
        }

        private void plotView1_DoubleClick(object sender, EventArgs e)
        {
            plotView1.Model.ResetAllAxes();
            plotView1.Refresh();
        }
    }
}
