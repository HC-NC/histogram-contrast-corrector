using Histogram_Contrast_Corrector.DataClasses;
using OSGeo.GDAL;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace Histogram_Contrast_Corrector
{
    public partial class WorkSpace : Form
    {
        private CultureInfo _culture;

        private string _tmpDir;

        private Dataset? _dataset;
        private Dataset? _saveDataset;

        private Driver _driver;

        private List<RasterData> _rasters;

        public WorkSpace()
        {
            InitializeComponent();

            _culture = CultureInfo.CurrentUICulture;

            _tmpDir = Path.Combine(Application.StartupPath, "_temp");

            openFileDialog1.Filter = (_culture.Name == "ru-RU" ? "Все файлы" : "All files") + "|*.tif;*.img;*.png;*.jpg;*.gif|TIFF|*.tif|IMG|*.img|PNG|*.png|JPEG|*.jpg|GIF|*.gif";
            saveFileDialog1.Filter = "TIFF|*.tif";

            _rasters = new List<RasterData>();
        }

        ~WorkSpace()
        {
            _dataset?.Dispose();
            _saveDataset?.Dispose();
        }

        private void WorkSpace_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Visible = false;
            toolStripProgressBar1.Visible = false;

            splitContainer2.Panel2Collapsed = true;
        }

        private void WorkSpace_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Directory.Exists(_tmpDir))
            {
                DirectoryInfo dir = new DirectoryInfo(_tmpDir);
                foreach (FileInfo f in dir.GetFiles())
                {
                    f.Delete();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileBackgroundWorker.IsBusy || contrastCorrectionBackgroundWorker.IsBusy)
            {
                notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
                notifyIcon.BalloonTipTitle = "Operation in progress!";
                notifyIcon.BalloonTipText = "Wait for the current operation to complete.";

                notifyIcon.ShowBalloonTip(1000);

                return;
            }

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileOpenParamForm openParamForm = new FileOpenParamForm(openFileDialog1.SafeFileName, Path.GetDirectoryName(openFileDialog1.FileName));

                if (openParamForm.ShowDialog(this) == DialogResult.OK)
                {
                    toolStripProgressBar1.Visible = true;
                    toolStripStatusLabel1.Visible = true;
                    openFileBackgroundWorker.RunWorkerAsync(openParamForm.IgnoreZero);
                }
            }
        }

        private RasterData? ReadData(BackgroundWorker worker, DoWorkEventArgs e, string filePath, string fileName, bool ignoreZero = true)
        {
            RasterData raster;

            try
            {
                _dataset = Gdal.Open(filePath, Access.GA_ReadOnly);

                raster = new RasterData(fileName, Path.GetDirectoryName(filePath), _dataset.RasterXSize, _dataset.RasterYSize, ignoreZero);

                float[] values;

                for (int i = 1; i <= _dataset.RasterCount; i++)
                {
                    worker.ReportProgress((int)((float)i / _dataset.RasterCount * 100f), _culture.Name == "ru-RU" ? $"Чтение растра (Канал {i})" : $"Read raster (Band {i})");

                    Band band = _dataset.GetRasterBand(i);

                    values = new float[band.XSize * band.YSize];

                    band.ReadRaster(0, 0, band.XSize, band.YSize, values, band.XSize, band.YSize, 0, 0);

                    string bandName = string.Format(_culture.Name == "ru-RU" ? "Канал: {0}" : "Band: {0}", i);

                    BandData bandData = new BandData(raster, bandName, band.XSize, band.YSize, values, ignoreZero);
                    bandData.CalculateMinMax();

                    raster.AddBand(bandData);
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                worker.CancelAsync();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                _dataset?.Close();
            }

            if (raster is null)
                return null;

            _rasters.Add(raster);

            if (0 < raster.BandsCount && raster.BandsCount < 3)
                raster.SetViewBands(0, 0, 0);
            else if (raster.BandsCount >= 3)
                raster.SetViewBands(0, 1, 2);

            raster.CalculateBandsHistogram(worker);

            return raster;
        }

        private void UpdateRastersTree(RasterData? raster)
        {
            if (raster is null)
                return;

            TreeNode node = new TreeNode(raster.Name);

            node.ToolTipText = string.Format("{0}\\{1}", raster.Path, raster.Name);
            node.Tag = raster;

            for (int i = 0; i < raster.BandsCount; i++)
            {
                BandData? band = raster.GetBand(i);

                if (band is null)
                    continue;

                TreeNode bandNode = new TreeNode(band.Name);
                bandNode.Tag = band;

                node.Nodes.Add(bandNode);
            }

            treeView1.Nodes.Add(node);
            treeView1.SelectedNode = node;
        }

        private void UpdateImage(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode is not null)
            {
                switch (treeView1.SelectedNode.Tag)
                {
                    case RasterData rasterData:
                        viewport.UpdateImage(rasterData.GetBitmap(), rasterData.InterpolationMode);
                        break;
                    case BandData bandData:
                        viewport.UpdateImage(bandData.Raster.GetBitmap(), bandData.Raster.InterpolationMode);
                        break;
                    default:
                        viewport.UpdateImage(null);
                        break;
                }
            }
            else
                viewport.UpdateImage(null);
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

            RasterForm rasterForm = new RasterForm(raster);
            if (rasterForm.ShowDialog(this) == DialogResult.OK)
            {
                UpdateImage(sender, e);
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeContextMenuStrip.Tag is null)
                return;

            if (_culture.Name == "ru-RU")
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить этот растр из рабочей области?", "Удалить растр", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to remove this raster from workspace?", "Remove Raster", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }

            RasterData? raster = treeContextMenuStrip.Tag as RasterData;

            if (raster is null)
                return;

            treeView1.Nodes.RemoveAt(_rasters.IndexOf(raster));

            raster.Dispose();
            _rasters.Remove(raster);

            if (treeView1.Nodes.Count == 0)
            {
                viewport.UpdateImage(null);
            }
        }

        private void contrastCorrectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode is null)
                return;

            if (openFileBackgroundWorker.IsBusy || contrastCorrectionBackgroundWorker.IsBusy)
            {
                notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
                notifyIcon.BalloonTipTitle = "Operation in progress!";
                notifyIcon.BalloonTipText = "Wait for the current operation to complete.";

                notifyIcon.ShowBalloonTip(1000);

                return;
            }

            if (_culture.Name == "ru-RU")
            {
                if (MessageBox.Show(string.Format("Примените коррекцию контрастности ко всем каналам {0}?", treeView1.SelectedNode.Tag.ToString()), "Коррекция контраста", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            else
            {
                if (MessageBox.Show(string.Format("Apply contrast correction to all bands of {0}?", treeView1.SelectedNode.Tag.ToString()), "Contrast Correction", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }

            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            toolStripProgressBar1.Visible = true;
            toolStripStatusLabel1.Visible = true;

            contrastCorrectionBackgroundWorker.RunWorkerAsync(treeView1.SelectedNode.Tag);
        }

        private float[] ContrastCorrection(BackgroundWorker worker, string reportName, float[] values, float[] assesment, float minimum, float maximum, bool ignoreZero)
        {
            float[] newValues = new float[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                if (i % (values.Length / 100) == 0)
                    worker.ReportProgress((int)((float)i / values.Length * 100), reportName);

                float v = values[i] - minimum;

                if ((ignoreZero && values[i] == 0) || v < 0 || v >= assesment.Length)
                {
                    newValues[i] = 0;
                    continue;
                }

                newValues[i] = minimum + (maximum - minimum) * assesment[(int)v];
            }

            return newValues;
        }

        private RasterData ContrastCorrection(RasterData raster, BackgroundWorker worker, string path)
        {
            RasterData newRaster = new RasterData(Path.GetFileName(path), Path.GetDirectoryName(path), raster.XSize, raster.YSize, raster.IgnoreZero);

            try
            {
                _dataset = Gdal.Open(Path.Combine(raster.Path, raster.Name), Access.GA_ReadOnly);

                _driver = Gdal.GetDriverByName("GTiff");

                _saveDataset = _driver.Create(path, raster.XSize, raster.YSize, raster.BandsCount, _dataset.GetRasterBand(1).DataType, ["TILED=YES", "COMPRESS=PACKBITS"]);

                _saveDataset.SetProjection(_dataset.GetProjection());

                double[] geoTransform = new double[6];
                _dataset.GetGeoTransform(geoTransform);
                _saveDataset.SetGeoTransform(geoTransform);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _dataset?.Close();
            }

            float[] newValues;

            for (int i = 0; i < raster.BandsCount; i++)
            {
                BandData? band = raster.GetBand(i);

                if (band is null)
                    continue;

                float[]? values = band.Values;

                if (values is null)
                    continue;

                float[]? assesment = band.AssesmentValues;

                if (assesment is null)
                {
                    band.CalculateHistogram();
                    assesment = band.AssesmentValues;
                }

                if (assesment is null)
                    continue;

                newValues = ContrastCorrection(worker, _culture.Name == "ru-RU" ? $"Коррекция {raster.Name}\\{band.Name} в {newRaster.Name}" : $"Correction {raster.Name}\\{band.Name} to {newRaster.Name}", values, assesment, band.Minimum, band.Maximum, band.IgnoreZero);

                try
                {
                    _saveDataset?.GetRasterBand(i + 1).WriteRaster(0, 0, band.XSize, band.YSize, newValues, band.XSize, band.YSize, 0, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                BandData newBand = new BandData(newRaster, band.Name, band.XSize, band.YSize, newValues, band.IgnoreZero);
                newBand.CalculateMinMax();

                band.UnloadValues();

                newRaster.AddBand(newBand);
            }

            _saveDataset?.Close();

            _rasters.Add(newRaster);

            if (0 < newRaster.BandsCount && newRaster.BandsCount < 3)
                newRaster.SetViewBands(0, 0, 0);
            else if (newRaster.BandsCount >= 3)
                newRaster.SetViewBands(0, 1, 2);

            newRaster.CalculateBandsHistogram(null);

            return newRaster;
        }

        private RasterData ContrastCorrection(BandData band, BackgroundWorker worker, string path)
        {
            RasterData newRaster = new RasterData(Path.GetFileName(path), Path.GetDirectoryName(path), band.Raster.XSize, band.Raster.YSize, band.Raster.IgnoreZero);

            float[]? values = band.Values;

            if (values is null)
                return newRaster;

            float[]? assesment = band.AssesmentValues;

            if (assesment is null)
            {
                band.CalculateHistogram();
                assesment = band.AssesmentValues;
            }

            band.UnloadValues();

            if (assesment is null)
                return newRaster;

            float[] newValues = ContrastCorrection(worker, _culture.Name == "ru-RU" ? $"Коррекция {band.Raster.Name} в {newRaster.Name}" : $"Correction {band.Raster.Name} to {newRaster.Name}", values, assesment, band.Minimum, band.Maximum, band.IgnoreZero);

            try
            {
                _dataset = Gdal.Open(Path.Combine(band.Raster.Path, band.Raster.Name), Access.GA_ReadOnly);

                _driver = Gdal.GetDriverByName("GTiff");

                _saveDataset = _driver.Create(path, band.XSize, band.YSize, 1, _dataset.GetRasterBand(1).DataType, ["TILED=YES", "COMPRESS=PACKBITS"]);

                _saveDataset.SetProjection(_dataset.GetProjection());

                double[] geoTransform = new double[6];
                _dataset.GetGeoTransform(geoTransform);
                _saveDataset.SetGeoTransform(geoTransform);

                _saveDataset.GetRasterBand(1).WriteRaster(0, 0, band.XSize, band.YSize, newValues, band.XSize, band.YSize, 0, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _dataset?.Close();
                _saveDataset?.Close();
            }

            BandData newBand = new BandData(newRaster, _culture.Name == "ru-RU" ? "Канал: 1" : "Band: 1", band.XSize, band.YSize, newValues, band.IgnoreZero);
            newBand.CalculateMinMax();

            newRaster.AddBand(newBand);

            _rasters.Add(newRaster);

            newRaster.SetViewBands(0, 0, 0);

            newRaster.CalculateBandsHistogram(null);

            return newRaster;
        }

        private void openFileBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker? worker = sender as BackgroundWorker;

            if (worker is null)
                return;

            bool ignoreZero = e.Argument is bool && (bool)e.Argument;

            e.Result = ReadData(worker, e, openFileDialog1.FileName, openFileDialog1.SafeFileName, ignoreZero);
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
            toolStripStatusLabel1.Text = e.UserState?.ToString();
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error is not null)
            {
                notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
                notifyIcon.BalloonTipTitle = "Operation error!";
                notifyIcon.BalloonTipText = e.Error.Message;

                notifyIcon.ShowBalloonTip(5000);
            }
            else if (e.Cancelled)
            {
                notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
                notifyIcon.BalloonTipTitle = "Operation cancelled!";
                notifyIcon.BalloonTipText = "The current operation was interrupted.";

                notifyIcon.ShowBalloonTip(5000);
            }
            else
            {
                notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon.BalloonTipTitle = "Operation completed!";
                notifyIcon.BalloonTipText = "The current operation has been completed.";

                notifyIcon.ShowBalloonTip(5000);

                UpdateRastersTree(e.Result as RasterData);
            }

            toolStripProgressBar1.Visible = false;
            toolStripStatusLabel1.Visible = false;
        }

        private void contrastCorrectionBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker? worker = sender as BackgroundWorker;

            if (worker is null)
                return;

            switch (e.Argument)
            {
                case RasterData rasterData:
                    e.Result = ContrastCorrection(rasterData, worker, saveFileDialog1.FileName);
                    break;
                case BandData bandData:
                    e.Result = ContrastCorrection(bandData, worker, saveFileDialog1.FileName);
                    break;
            }
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel2Collapsed = !splitContainer2.Panel2Collapsed;
        }
    }
}
