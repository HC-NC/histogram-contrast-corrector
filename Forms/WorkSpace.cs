using Histogram_Contrast_Corrector.DataClasses;
using OSGeo.GDAL;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Globalization;
using Histogram_Contrast_Corrector.Properties;

namespace Histogram_Contrast_Corrector
{
    public partial class WorkSpace : Form
    {
        private List<RasterData> _rasters;
        private ICorrectionMethod _correction;
        private CorrectionMethods _correctionMethod;

        public WorkSpace()
        {
            InitializeComponent();

            Gdal.SetCacheMax(128 * 1024 * 1024);
            
            openFileDialog1.Filter = Resources.OpenFileDialogFilter ?? "All files|*.tif;*.img;*.png;*.jpg;*.gif|TIFF|*.tif|IMG|*.img|PNG|*.png|JPEG|*.jpg|GIF|*.gif";

            _rasters = new List<RasterData>();
        }

        private void WorkSpace_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Visible = false;
            toolStripProgressBar1.Visible = false;

            splitContainer2.Panel2Collapsed = true;
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
                notifyIcon.BalloonTipTitle = Resources.OpInProgressTitle ?? "Operation in progress!";
                notifyIcon.BalloonTipText = Resources.OpInProgressText ?? "Wait for the current operation to complete";

                notifyIcon.ShowBalloonTip(100);

                return;
            }

            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                FileOpenParamForm openParamForm = new FileOpenParamForm(openFileDialog1.SafeFileName, Path.GetDirectoryName(openFileDialog1.FileName));

                if (openParamForm.ShowDialog(this) == DialogResult.OK)
                {
                    toolStripStatusLabel1.Text = string.Format(Resources.OpeningStatus ?? "Opening: {0}", openFileDialog1.SafeFileName);

                    toolStripProgressBar1.Visible = true;
                    toolStripStatusLabel1.Visible = true;

                    openFileBackgroundWorker.RunWorkerAsync(openParamForm.IgnoreZero);
                }
            }
        }

        private void UpdateRastersTree(RasterData? raster)
        {
            if (raster is null) return;

            _rasters.Add(raster);
            TreeNode node = new TreeNode(raster.Name);

            node.ToolTipText = string.Format("{0}\\{1}", raster.Path, raster.Name);
            node.Tag = raster;

            for (int i = 0; i < raster.BandsCount; i++)
            {
                BandData? band = raster.GetBand(i);
                if (band is null) continue;

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
            if (e.Button != MouseButtons.Right || e.Node is null)
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
            if (treeContextMenuStrip.Tag is BandData band)
            {
                BandForm bandForm = new BandForm(band);
                bandForm.Show(this);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeContextMenuStrip.Tag is RasterData raster)
            {
                RasterForm rasterForm = new RasterForm(raster);
                if (rasterForm.ShowDialog(this) == DialogResult.OK)
                {
                    UpdateImage(sender, e);
                }
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeContextMenuStrip.Tag is not RasterData raster)
                return;

            string msg = Resources.RemoveConfirmMsg ?? "Are you sure you want to remove this raster from workspace?";
            string title = Resources.RemoveConfirmTitle ?? "Remove Raster";

            if (MessageBox.Show(this, msg, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            treeView1.Nodes.RemoveAt(_rasters.IndexOf(raster));
            raster.Dispose();
            _rasters.Remove(raster);

            if (treeView1.Nodes.Count == 0)
            {
                viewport.UpdateImage(null);
            }
        }

        private void contrastCorrector_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is null)
                return;

            if (openFileBackgroundWorker.IsBusy || contrastCorrectionBackgroundWorker.IsBusy)
            {
                notifyIcon.ShowBalloonTip(100,
                    Resources.OpInProgressTitle ?? "Wait",
                    Resources.OpInProgressText ?? "Operation in progress!",
                    ToolTipIcon.Warning);
                return;
            }

            object target = treeView1.SelectedNode.Tag;
            ContrastCorrectorForm correctorForm;

            bool parentIgnoreZero = false;
            int exportBandsCount = 0;
            string originalName = "";

            if (target is RasterData rd)
            {
                correctorForm = new ContrastCorrectorForm(rd);
                parentIgnoreZero = rd.IgnoreZero;
                exportBandsCount = rd.BandsCount;
                originalName = rd.Name;
            }
            else if (target is BandData bd)
            {
                correctorForm = new ContrastCorrectorForm(bd);
                parentIgnoreZero = bd.IgnoreZero;
                exportBandsCount = 1;
                originalName = bd.Name;
            }
            else return;

            if (correctorForm.ShowDialog(this) != DialogResult.Continue)
                return;

            _correction = correctorForm.CorrectionMethod;
            _correctionMethod = correctorForm.GetMethods();

            if (exportBandsCount > 4)
                saveFileDialog1.Filter = "GeoTIFF (*.tif)|*.tif|Erdas Imagine (*.img)|*.img";
            else if (exportBandsCount == 4)
                saveFileDialog1.Filter = "GeoTIFF (*.tif)|*.tif|Erdas Imagine (*.img)|*.img|PNG Image (*.png)|*.png";
            else
                saveFileDialog1.Filter = "GeoTIFF (*.tif)|*.tif|Erdas Imagine (*.img)|*.img|PNG Image (*.png)|*.png|JPEG Image (*.jpg)|*.jpg";

            string cleanName = Path.GetFileNameWithoutExtension(originalName);
            saveFileDialog1.FileName = "corrected_" + cleanName;

            if (saveFileDialog1.ShowDialog(this) != DialogResult.OK)
                return;

            string savePath = saveFileDialog1.FileName;
            string extension = Path.GetExtension(savePath);

            if (string.IsNullOrEmpty(extension))
            {
                string[] filterExtensions = { ".tif", ".img", ".png", ".jpg" };
                int filterIndex = saveFileDialog1.FilterIndex - 1;

                savePath += (filterIndex >= 0 && filterIndex < filterExtensions.Length)
                    ? filterExtensions[filterIndex]
                    : ".tif";
            }

            toolStripProgressBar1.Visible = true;
            toolStripStatusLabel1.Visible = true;

            var argument = Tuple.Create(target, savePath, parentIgnoreZero);
            contrastCorrectionBackgroundWorker.RunWorkerAsync(argument);
        }

        private void RunContrastCorrection(object target, string savePath, BackgroundWorker worker)
        {
            RasterData? srcRaster = null;
            int targetBandIndex = -1;

            if (target is RasterData raster)
                srcRaster = raster;
            else if (target is BandData band)
            {
                srcRaster = band.Raster;
                targetBandIndex = srcRaster.GetBands().IndexOf(band) + 1;
            }

            if (srcRaster == null) return;

            Dataset? srcDataset = null;
            Dataset? tempDataset = null;
            Dataset? finalDataset = null;

            string ext = Path.GetExtension(savePath).ToLower();
            bool needsConversion = (ext == ".png" || ext == ".jpg" || ext == ".jpeg");

            string workingPath = needsConversion
                ? Path.Combine(Path.GetDirectoryName(savePath) ?? "", "temp_" + Guid.NewGuid().ToString() + ".tif")
                : savePath;

            try
            {
                srcDataset = Gdal.Open(srcRaster.Path, Access.GA_ReadOnly);
                Driver tiffDriver = Gdal.GetDriverByName("GTiff");

                int bandsCount = (targetBandIndex == -1) ? srcRaster.BandsCount : 1;

                tempDataset = tiffDriver.Create(workingPath, srcRaster.Width, srcRaster.Height, bandsCount,
                    srcDataset.GetRasterBand(1).DataType, ["TILED=YES", "COMPRESS=PACKBITS"]);

                tempDataset.SetProjection(srcDataset.GetProjection());
                double[] geoTransform = new double[6];
                srcDataset.GetGeoTransform(geoTransform);
                tempDataset.SetGeoTransform(geoTransform);

                float[] rowBuffer = new float[srcRaster.Width];
                int currentDstBand = 1;

                for (int b = 1; b <= srcDataset.RasterCount; b++)
                {
                    if (targetBandIndex != -1 && b != targetBandIndex)
                        continue;

                    BandData? bandData = srcRaster.GetBand(b - 1);
                    if (bandData == null) continue;

                    float[]? assesment = bandData.AssesmentValues;
                    if (assesment == null)
                    {
                        bandData.CalculateHistogram();
                        assesment = bandData.AssesmentValues;
                    }
                    if (assesment == null) continue;

                    using (Band srcBand = srcDataset.GetRasterBand(b))
                    using (Band dstBand = tempDataset.GetRasterBand(currentDstBand))
                    {
                        string reportTemplate = Resources.ProcessingBandStatus ?? "Processing band {0}...";
                        string reportName = string.Format(reportTemplate, b);

                        for (int y = 0; y < srcRaster.Height; y++)
                        {
                            srcBand.ReadRaster(0, y, srcRaster.Width, 1, rowBuffer, srcRaster.Width, 1, 0, 0);

                            for (int x = 0; x < srcRaster.Width; x++)
                            {
                                float val = rowBuffer[x];
                                if ((bandData.IgnoreZero && val == 0) || val < bandData.Minimum)
                                {
                                    rowBuffer[x] = 0;
                                    continue;
                                }

                                int idx = (int)(val - bandData.Minimum);
                                if (idx >= 0 && idx < assesment.Length)
                                {
                                    float normVal = assesment[idx];
                                    switch (_correctionMethod)
                                    {
                                        case CorrectionMethods.Exp:
                                            float cExp = (bandData.Maximum - 1) / (MathF.Exp(_correction.GetA()) - 1);
                                            rowBuffer[x] = cExp * _correction.F(normVal);
                                            break;
                                        case CorrectionMethods.Log:
                                            float logA = _correction.GetA();
                                            float cLog = (bandData.Maximum - 1) / (MathF.Log(1f + (logA - 1f)) / MathF.Log(logA));
                                            rowBuffer[x] = cLog * _correction.F(normVal);
                                            break;
                                        default:
                                            rowBuffer[x] = bandData.Minimum + (bandData.Maximum - bandData.Minimum) * _correction.F(normVal);
                                            break;
                                    }
                                }
                            }

                            dstBand.WriteRaster(0, y, srcRaster.Width, 1, rowBuffer, srcRaster.Width, 1, 0, 0);

                            if (y % (srcRaster.Height / 20 + 1) == 0)
                            {
                                int progress = (int)((float)y / srcRaster.Height * 100);
                                worker.ReportProgress(progress, reportName);
                            }
                        }
                    }
                    currentDstBand++;
                }

                tempDataset.FlushCache();

                if (needsConversion)
                {
                    worker.ReportProgress(99, Resources.ConvertingStatus ?? "Converting to final format...");

                    string driverName = (ext == ".jpg" || ext == ".jpeg") ? "JPEG" : "PNG";
                    Driver finalDriver = Gdal.GetDriverByName(driverName);

                    finalDataset = finalDriver.CreateCopy(savePath, tempDataset, 0, null, null, null);
                    finalDataset.FlushCache();
                }
            }
            finally
            {
                srcDataset?.Dispose();
                tempDataset?.Dispose();
                finalDataset?.Dispose();

                if (needsConversion && File.Exists(workingPath))
                {
                    try { File.Delete(workingPath); } catch { }
                }
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
            toolStripStatusLabel1.Text = e.UserState?.ToString();
        }

        private void openFileBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (sender is not BackgroundWorker worker) return;

            bool ignoreZero = e.Argument is bool b && b;

            string filePath = openFileDialog1.FileName;
            string safeName = Path.GetFileName(filePath);

            RasterData raster = RasterData.Load(filePath, safeName, ignoreZero);
            e.Result = raster;

            string bandReportTemplate = Resources.ProcessedBandStatus ?? "Processed band {0}";

            for (int i = 0; i < raster.BandsCount; i++)
            {
                BandData? band = raster.GetBand(i);
                if (band is null) continue;

                band.CalculateHistogram();
                band.Unload();

                if (worker.WorkerReportsProgress)
                {
                    int percentComplete = (int)((float)(i + 1) / raster.BandsCount * 100);
                    worker.ReportProgress(percentComplete, string.Format(bandReportTemplate, band.Name));
                }
            }
        }

        private void openFileBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error is not null)
                notifyIcon.ShowBalloonTip(100, "Error!", e.Error.Message, ToolTipIcon.Error);
            else if (e.Cancelled)
                notifyIcon.ShowBalloonTip(100, "Cancelled!", "The operation was interrupted", ToolTipIcon.Warning);
            else
            {
                notifyIcon.ShowBalloonTip(100, "Done!", "The file has been successfully uploaded", ToolTipIcon.Info);

                UpdateRastersTree(e.Result as RasterData);
            }

            toolStripProgressBar1.Visible = false;
            toolStripStatusLabel1.Visible = false;
        }

        private void contrastCorrectionBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (sender is not BackgroundWorker worker || e.Argument is not Tuple<object, string, bool> args)
                return;

            RunContrastCorrection(args.Item1, args.Item2, worker);
            e.Result = Tuple.Create(args.Item2, args.Item3);
        }

        private void contrastCorrectionBackgroundWorker_RunWorkerCompleted(object sender,  RunWorkerCompletedEventArgs e)
        {
            if (e.Error is not null)
            {
                notifyIcon.ShowBalloonTip(100, "Error!", e.Error.Message, ToolTipIcon.Error);
            }
            else if (e.Cancelled)
            {
                notifyIcon.ShowBalloonTip(100, "Cancelled!", "The operation was interrupted", ToolTipIcon.Warning);
            }
            else if (e.Result is Tuple<string, bool> result)
            {
                notifyIcon.ShowBalloonTip(100,
                    Resources.SavedTitle ?? "Saved!",
                    Resources.FileSavedMsg ?? "File saved. Loading into project...",
                    ToolTipIcon.Info);

                string newFilePath = result.Item1;
                bool ignoreZero = result.Item2;

                openFileDialog1.FileName = newFilePath;

                toolStripProgressBar1.Visible = true;
                toolStripStatusLabel1.Visible = true;

                openFileBackgroundWorker.RunWorkerAsync(ignoreZero);
            }

            if (e.Error != null || e.Cancelled)
            {
                toolStripProgressBar1.Visible = false;
                toolStripStatusLabel1.Visible = false;
            }
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog(this);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel2Collapsed = !splitContainer2.Panel2Collapsed;

            toolStripButton1.Image = splitContainer2.Panel2Collapsed
                ? Properties.Resources.show
                : Properties.Resources.hide;

            UpdateDisplaySettings(sender, e);
        }

        private void UpdateDisplaySettings(object sender, EventArgs e)
        {
            if (splitContainer2.Panel2Collapsed)
                return;

            if (treeView1.SelectedNode?.Tag is not object target)
            {
                ResetDisplayButton();
                return;
            }

            RasterData raster = target switch
            {
                RasterData rd => rd,
                BandData bd => bd.Raster,
                _ => null
            };

            if (raster == null)
            {
                ResetDisplayButton();
                return;
            }

            redComboBox.Items.Clear();
            greenComboBox.Items.Clear();
            blueComboBox.Items.Clear();

            for (int i = 0; i < raster.BandsCount; i++)
            {
                string item = raster.GetBand(i).Name;
                redComboBox.Items.Add(item);
                greenComboBox.Items.Add(item);
                blueComboBox.Items.Add(item);
            }

            redComboBox.SelectedIndex = raster.RedID;
            greenComboBox.SelectedIndex = raster.GreenID;
            blueComboBox.SelectedIndex = raster.BlueID;

            if (interpolationComboBox.Items.Count == 0)
            {
                interpolationComboBox.Items.AddRange(Enum.GetNames(typeof(InterpolationMode)));
                interpolationComboBox.Items.RemoveAt(interpolationComboBox.Items.Count - 1);
            }

            interpolationComboBox.SelectedIndex = (int)raster.InterpolationMode;
        }

        private void ResetDisplayButton()
        {
            splitContainer2.Panel2Collapsed = true;
            toolStripButton1.Image = Properties.Resources.show;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateImage(sender, e);
            UpdateDisplaySettings(sender, e);
        }

        private void acceptDisplaySettingsButton_Click(object sender, EventArgs e)
        {
            RasterData? raster = null;

            if (treeView1.SelectedNode?.Tag is RasterData rd)
                raster = rd;
            else if (treeView1.SelectedNode?.Tag is BandData bd)
                raster = bd.Raster;

            if (raster == null)
            {
                ResetDisplayButton();
                notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
                notifyIcon.BalloonTipTitle = Resources.OpErrorTitle ?? "Operation error!";
                notifyIcon.BalloonTipText = Resources.SettingsNotAppliedMsg ?? "Display settings not applied";
                notifyIcon.ShowBalloonTip(100);
                return;
            }

            raster.SetViewBands(redComboBox.SelectedIndex, greenComboBox.SelectedIndex, blueComboBox.SelectedIndex);
            raster.InterpolationMode = (InterpolationMode)interpolationComboBox.SelectedIndex;

            UpdateImage(sender, e);
            ResetDisplayButton();
        }
    }
}
