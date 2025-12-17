using Histogram_Contrast_Corrector.DataClasses;
using OSGeo.GDAL;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Drawing.Drawing2D;

namespace Histogram_Contrast_Corrector
{
    public partial class WorkSpace : Form
    {
        private Graphics _graphics;

        private Image? _img;
        private Point _mouseDown;
        private int _startx = 0; // offset of image when mouse was pressed
        private int _starty = 0;
        private int _imgx = 0; // current offset of image
        private int _imgy = 0;

        private bool _mousepressed = false; // true as long as left mousebutton is pressed
        private bool _mouseOnPicture = false;
        private float _zoom = 1;

        private List<RasterData> _rasters;

        public WorkSpace()
        {
            InitializeComponent();

            openFileDialog1.Filter = "All files|*.*|TIFF|*.tif";

            _graphics = splitContainer1.Panel2.CreateGraphics();

            _rasters = new List<RasterData>();
        }

        ~WorkSpace()
        {
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
                //toolStripStatusLabel1.Text = "Current: " + openFileDialog1.SafeFileName;

                //_curDS = Gdal.Open(_curFilePath, Access.GA_ReadOnly);

                //Draw(_curDS.GetRasterBand(1));

                ReadData(openFileDialog1.FileName, openFileDialog1.SafeFileName);
            }
        }

        private void ReadData(string filePath, string fileName, bool ignoreZero = true)
        {
            Dataset dataset = Gdal.Open(filePath, Access.GA_ReadOnly);

            RasterData raster = new RasterData(fileName, filePath, dataset.RasterXSize, dataset.RasterYSize, ignoreZero);

            for (int i = 1; i <= dataset.RasterCount; i++)
            {
                Band band = dataset.GetRasterBand(i);

                float[] values = new float[band.XSize * band.YSize];

                band.ReadRaster(0, 0, band.XSize, band.YSize, values, band.XSize, band.YSize, 0, 0);

                BandData bandData = new BandData(band.XSize, band.YSize, values, ignoreZero);
                bandData.CalculateMinMax();

                raster.AddBand(bandData);
            }

            dataset.Close();

            _rasters.Add(raster);

            if (0 < raster.BandsCount && raster.BandsCount < 3)
                raster.SetViewBands(0, 0, 0);
            else if (raster.BandsCount >= 3)
                raster.SetViewBands(0, 1, 2);

            UpdateRastersTree(raster);

            raster.CalculateBandsHistogram();
        }

        private void UpdateRastersTree(RasterData raster)
        {
            TreeNode node = new TreeNode(raster.Name);

            node.ToolTipText = raster.FullPath;
            node.Tag = raster;

            for (int i = 0; i < raster.BandsCount; i++)
            {
                BandData? band = raster.GetBand(i);

                if (band is null)
                    continue;

                TreeNode bandNode = new TreeNode(string.Format("Band: {0}", i + 1));
                bandNode.Tag = band;

                node.Nodes.Add(bandNode);
            }

            treeView1.Nodes.Add(node);
            treeView1.SelectedNode = node;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node is not null)
            {
                switch (e.Node.Tag)
                {
                    case RasterData rasterData:
                        _img = rasterData.GetBitmap();
                        break;
                    case BandData bandData:
                        TreeNode? parrent = e.Node.Parent;

                        if (parrent is null || parrent.Tag is null)
                        {
                            _img = null;
                            break;
                        }

                        RasterData raster = parrent.Tag as RasterData;

                        if (raster is null)
                        {
                            _img = null;
                            break;
                        }

                        _img = raster.GetBitmap();
                        break;
                    default:
                        _img = null;
                        break;
                }
            }
            else
                _img = null;

            UpdateImage(sender, e);
        }

        private void UpdateImage(object sender, EventArgs e)
        {
            if (_img is null)
                return;

            _zoom = Math.Min(
             ((float)viewBox.Height / (float)_img.Height) * (_img.VerticalResolution / _graphics.DpiY),
             ((float)viewBox.Width / (float)_img.Width) * (_img.HorizontalResolution / _graphics.DpiX)
            );

            _imgx = (int)((viewBox.Width / 2f - _img.Width * _zoom / 2f) / _zoom);
            _imgy = (int)((viewBox.Height / 2f - _img.Height * _zoom / 2f) / _zoom);

            viewBox.Refresh();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            if (e.Node is null)
                return;

            treeContextMenuStrip.Tag = e.Node.Tag;

            switch (e.Node.Tag)
            {
                case RasterData raster:
                    histogramToolStripMenuItem.Visible = false;
                    aboutToolStripMenuItem.Visible = true;
                    toolStripSeparator2.Visible = true;
                    removeToolStripMenuItem.Visible = true;
                    break;
                case BandData band:
                    histogramToolStripMenuItem.Visible = true;
                    aboutToolStripMenuItem.Visible = false;
                    toolStripSeparator2.Visible = false;
                    removeToolStripMenuItem.Visible = false;
                    break;
            }

            treeContextMenuStrip.Show(sender as Control, e.X, e.Y);
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

            //toolStripStatusLabel2.Text = $"XSize: {band.XSize} YSize: {band.YSize} Min: {minV} Max: {maxV}";

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

            var histSeries = new HistogramSeries();
            var lineSeries = new LineSeries();

            int sm = histogram.Sum();
            double tmp = 0;

            for (int i = 0; i < histogram.Length; i++)
            {
                histSeries.Items.Add(new HistogramItem(i + minV, i + minV + 1, histogram[i], 0));

                tmp += histogram[i];
                lineSeries.Points.Add(new DataPoint(i + minV, tmp / sm));
            }

            PlotModel plot = new PlotModel();

            plot.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Minimum = minV, Maximum = maxV });
            plot.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Minimum = 0, Maximum = histogram.Max(), Key = "axesY1" });
            plot.Axes.Add(new LinearAxis() { Position = AxisPosition.Right, Minimum = 0, Maximum = 1d, Key = "axesY2" });

            histSeries.YAxisKey = "axesY1";
            lineSeries.YAxisKey = "axesY2";

            lineSeries.Color = OxyColor.FromRgb(255, 0, 0);

            plot.Series.Add(histSeries);
            plot.Series.Add(lineSeries);

            //plotView1.Model = plot;

            _img = bitmap;

            // Fit whole image
            //_zoom = Math.Min(
            // ((float)pictureBox1.Height / (float)_img.Height) * (_img.VerticalResolution / _graphics.DpiY),
            // ((float)pictureBox1.Width / (float)_img.Width) * (_img.HorizontalResolution / _graphics.DpiX)
            //);

            // Fit width
            _zoom = ((float)viewBox.Width / (float)_img.Width) *
            (_img.HorizontalResolution / _graphics.DpiX);

            _imgy = (int)((viewBox.Height / 2f - _img.Height * _zoom / 2f) / _zoom);

            viewBox.Refresh();
            //_curDS?.Close();
        }

        private void plotView1_DoubleClick(object sender, EventArgs e)
        {
            //plotView1.Model.ResetAllAxes();
            //plotView1.Refresh();
        }

        private void viewBox_Paint(object sender, PaintEventArgs e)
        {
            if (_img is null)
            {
                e.Graphics.Clear(Color.White);
                return;
            }

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.ScaleTransform(_zoom, _zoom);
            e.Graphics.DrawImage(_img, _imgx, _imgy);
        }

        private void viewBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!_mousepressed)
                {
                    _mousepressed = true;
                    _mouseDown = e.Location;
                    _startx = _imgx;
                    _starty = _imgy;
                }
            }
        }

        private void viewBox_MouseEnter(object sender, EventArgs e)
        {
            _mouseOnPicture = true;
        }

        private void viewBox_MouseLeave(object sender, EventArgs e)
        {
            _mouseOnPicture = false;
        }

        private void viewBox_MouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs mouse = e as MouseEventArgs;

            if (mouse.Button == MouseButtons.Left)
            {
                Point mousePosNow = mouse.Location;

                // the distance the mouse has been moved since mouse was pressed
                int deltaX = mousePosNow.X - _mouseDown.X;
                int deltaY = mousePosNow.Y - _mouseDown.Y;

                // calculate new offset of image based on the current zoom factor
                _imgx = (int)(_startx + (deltaX / _zoom));
                _imgy = (int)(_starty + (deltaY / _zoom));

                viewBox.Refresh();
            }
        }

        private void viewBox_MouseUp(object sender, MouseEventArgs e)
        {
            _mousepressed = false;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (_mouseOnPicture)
            {
                float oldzoom = _zoom;

                if (e.Delta > 0)
                {
                    _zoom *= 1.1f;
                }
                else if (e.Delta < 0)
                {
                    _zoom *= 0.9f;
                }

                Point mousePosNow = e.Location;

                Point pBoxLocation = this.PointToClient(viewBox.Parent.PointToScreen(viewBox.Location));

                // Where location of the mouse in the pictureframe
                int x = mousePosNow.X - pBoxLocation.X;
                int y = mousePosNow.Y - pBoxLocation.Y;

                // Where in the IMAGE is it now
                int oldimagex = (int)(x / oldzoom);
                int oldimagey = (int)(y / oldzoom);

                // Where in the IMAGE will it be when the new zoom i made
                int newimagex = (int)(x / _zoom);
                int newimagey = (int)(y / _zoom);

                // Where to move image to keep focus on one point
                _imgx = newimagex - oldimagex + _imgx;
                _imgy = newimagey - oldimagey + _imgy;

                viewBox.Refresh();  // calls imageBox_Paint
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.Right:
                        _imgx -= (int)(viewBox.Width * 0.1f / _zoom);
                        viewBox.Refresh();
                        break;

                    case Keys.Left:
                        _imgx += (int)(viewBox.Width * 0.1f / _zoom);
                        viewBox.Refresh();
                        break;

                    case Keys.Down:
                        _imgy -= (int)(viewBox.Height * 0.1f / _zoom);
                        viewBox.Refresh();
                        break;

                    case Keys.Up:
                        _imgy += (int)(viewBox.Height * 0.1f / _zoom);
                        viewBox.Refresh();
                        break;

                    case Keys.PageDown:
                        _imgy -= (int)(viewBox.Height * 0.9f / _zoom);
                        viewBox.Refresh();
                        break;

                    case Keys.PageUp:
                        _imgy += (int)(viewBox.Height * 0.9f / _zoom);
                        viewBox.Refresh();
                        break;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeContextMenuStrip.Tag is null)
                return;

            BandData? band = treeContextMenuStrip.Tag as BandData;

            if (band is null) 
                return;

            BandForm bandForm = new BandForm(band);
            bandForm.Show(this);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeContextMenuStrip.Tag is null)
                return;

            RasterData? raster = treeContextMenuStrip.Tag as RasterData;

            if (raster is null)
                return;

            DatasetForm datasetForm = new DatasetForm(raster);
            datasetForm.Show(this);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeContextMenuStrip.Tag is null)
                return;

            RasterData? raster = treeContextMenuStrip.Tag as RasterData;

            if (raster is null)
                return;

            treeView1.Nodes.RemoveAt(_rasters.IndexOf(raster));
            _rasters.Remove(raster);

            if (treeView1.Nodes.Count == 0)
            {
                _img = null;
                viewBox.Refresh();
            }
        }
    }
}
