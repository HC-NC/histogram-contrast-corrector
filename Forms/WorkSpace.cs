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

        private List<RasterData> _rasters;

        private ICorrectionMethod _correction;
        private CorrectionMethods _correctionMethod;

        public WorkSpace()
        {
            InitializeComponent();

            Gdal.SetCacheMax(128 * 1024 * 1024); // 128 Мегабайт

            _culture = CultureInfo.CurrentUICulture;

            _tmpDir = Path.Combine(Application.StartupPath, "_temp");
            
            openFileDialog1.Filter = (_culture.Name == "ru-RU" ? "Все файлы" : "All files") + "|*.tif;*.img;*.png;*.jpg;*.gif|TIFF|*.tif|IMG|*.img|PNG|*.png|JPEG|*.jpg|GIF|*.gif";

            _rasters = new List<RasterData>();
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
                notifyIcon.BalloonTipText = "Wait for the current operation to complete";

                notifyIcon.ShowBalloonTip(100);

                return;
            }

            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                FileOpenParamForm openParamForm = new FileOpenParamForm(openFileDialog1.SafeFileName, Path.GetDirectoryName(openFileDialog1.FileName));

                if (openParamForm.ShowDialog(this) == DialogResult.OK)
                {
                    toolStripStatusLabel1.Text = $"Открываем: {openFileDialog1.SafeFileName}";

                    toolStripProgressBar1.Visible = true;
                    toolStripStatusLabel1.Visible = true;

                    openFileBackgroundWorker.RunWorkerAsync(openParamForm.IgnoreZero);
                }
            }
        }

        private void UpdateRastersTree(RasterData? raster)
        {
            if (raster is null)
                return;

            _rasters.Add(raster);
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
                if (MessageBox.Show(this, "Вы уверены, что хотите удалить этот растр из рабочей области?", "Удалить растр", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            else
            {
                if (MessageBox.Show(this, "Are you sure you want to remove this raster from workspace?", "Remove Raster", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
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

        private void contrastCorrector_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode is null || treeView1.SelectedNode.Tag is null)
                return;

            if (openFileBackgroundWorker.IsBusy || contrastCorrectionBackgroundWorker.IsBusy)
            {
                notifyIcon.ShowBalloonTip(100, "Wait", "Operation in progress!", ToolTipIcon.Warning);
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
                exportBandsCount = 1; // Всегда 1 канал
                originalName = bd.Name;
            }
            else return;

            // 1. Показываем форму выбора метода
            if (correctorForm.ShowDialog(this) != DialogResult.Continue)
                return;

            _correction = correctorForm.CorrectionMethod;
            _correctionMethod = correctorForm.GetMethods();

            // 2. Настраиваем фильтры в зависимости от количества каналов
            if (exportBandsCount > 4)
            {
                // Многоканальные данные (только проф. форматы)
                saveFileDialog1.Filter = "GeoTIFF (*.tif)|*.tif|Erdas Imagine (*.img)|*.img";
            }
            else if (exportBandsCount == 4)
            {
                // 4 канала (RGB + Alpha)
                saveFileDialog1.Filter = "GeoTIFF (*.tif)|*.tif|Erdas Imagine (*.img)|*.img|PNG Image (*.png)|*.png";
            }
            else
            {
                // 1-3 канала (Подходят все популярные форматы)
                saveFileDialog1.Filter = "GeoTIFF (*.tif)|*.tif|Erdas Imagine (*.img)|*.img|PNG Image (*.png)|*.png|JPEG Image (*.jpg)|*.jpg";
            }

            // Задаем имя без расширения по умолчанию
            string cleanName = Path.GetFileNameWithoutExtension(originalName);
            saveFileDialog1.FileName = "corrected_" + cleanName;

            if (saveFileDialog1.ShowDialog(this) != DialogResult.OK)
                return;

            // Решаем проблему "потерянного расширения"
            string savePath = saveFileDialog1.FileName;
            string extension = Path.GetExtension(savePath);

            if (string.IsNullOrEmpty(extension))
            {
                // Если расширения нет, берем его из выбранного фильтра
                string[] filterExtensions = new string[] { ".tif", ".img", ".png", ".jpg" };
                int filterIndex = saveFileDialog1.FilterIndex - 1; // FilterIndex начинается с 1

                if (filterIndex >= 0 && filterIndex < filterExtensions.Length)
                {
                    savePath += filterExtensions[filterIndex];
                }
                else
                {
                    savePath += ".tif"; // Запасной вариант
                }
            }

            toolStripProgressBar1.Visible = true;
            toolStripStatusLabel1.Visible = true;

            // 3. Формируем "посылку" для фонового потока
            var argument = new
            {
                Target = target,
                SavePath = savePath, // Используем вычищенный путь с расширением!
                IgnoreZero = parentIgnoreZero
            };

            contrastCorrectionBackgroundWorker.RunWorkerAsync(argument);
        }

        private void RunContrastCorrection(object target, string savePath, BackgroundWorker worker)
        {
            RasterData? srcRaster = null;
            int targetBandIndex = -1; // -1 означает, что обрабатываем все каналы растра

            if (target is RasterData raster)
            {
                srcRaster = raster;
            }
            else if (target is BandData band)
            {
                srcRaster = band.Raster;
                targetBandIndex = srcRaster.GetBands().IndexOf(band) + 1;
            }

            if (srcRaster == null) return;

            Dataset? srcDataset = null;
            Dataset? tempDataset = null;
            Dataset? finalDataset = null;

            // Определяем финальный формат
            string ext = Path.GetExtension(savePath).ToLower();
            bool needsConversion = (ext == ".png" || ext == ".jpg" || ext == ".jpeg");

            // Путь для временного файла TIFF (если нужна конвертация)
            string workingPath = needsConversion
                ? Path.Combine(Path.GetDirectoryName(savePath) ?? "", "temp_" + Guid.NewGuid().ToString() + ".tif")
                : savePath;

            try
            {
                srcDataset = Gdal.Open(srcRaster.Path, Access.GA_ReadOnly);
                // Временный файл ВСЕГДА пишем как GTiff
                Driver tiffDriver = Gdal.GetDriverByName("GTiff");

                int bandsCount = (targetBandIndex == -1) ? srcRaster.BandsCount : 1;

                // Создаем датасет с поддержкой построчной записи
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
                        string reportName = $"Обработка канала {b}...";

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
                                            float cLog = (bandData.Maximum - 1) / (MathF.Log(2) / MathF.Log(_correction.GetA()));
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

                // ⚠️ ОБЯЗАТЕЛЬНО сбрасываем буферы на диск перед конвертацией!
                tempDataset.FlushCache();

                // 🔥 МАГИЯ ГЕНЕРАЦИИ JPEG / PNG
                if (needsConversion)
                {
                    worker.ReportProgress(99, "Конвертация в финальный формат...");

                    string driverName = "PNG";
                    if (ext == ".jpg" || ext == ".jpeg") driverName = "JPEG";

                    Driver finalDriver = Gdal.GetDriverByName(driverName);

                    // Метод CreateCopy идеально подходит и работает БЕЗ ошибок!
                    finalDataset = finalDriver.CreateCopy(savePath, tempDataset, 0, null, null, null);
                    finalDataset.FlushCache();
                }
            }
            finally
            {
                // Закрываем все датасеты
                srcDataset?.Dispose();
                tempDataset?.Dispose();
                finalDataset?.Dispose();

                // Если создавали временный файл — удаляем его
                if (needsConversion && File.Exists(workingPath))
                {
                    try { File.Delete(workingPath); } catch { /* Игнорируем ошибки удаления */ }
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
            BackgroundWorker? worker = sender as BackgroundWorker;
            if (worker is null) return;

            bool ignoreZero = e.Argument is bool && (bool)e.Argument;

            // Извлекаем имя файла прямо из пути (сработает и для диалога, и для нашего авто-открытия)
            string filePath = openFileDialog1.FileName;
            string safeName = Path.GetFileName(filePath);

            // Твоя быстрая статика
            RasterData raster = RasterData.Load(filePath, safeName, ignoreZero);
            e.Result = raster;

            for (int i = 0; i < raster.BandsCount; i++)
            {
                BandData? band = raster.GetBand(i);
                if (band is null) continue;

                band.CalculateHistogram();
                band.Unload();

                if (worker.WorkerReportsProgress)
                {
                    int percentComplete = (int)((float)(i + 1) / raster.BandsCount * 100);
                    worker.ReportProgress(percentComplete, $"Обработан канал {band.Name}");
                }
            }
        }

        private void openFileBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error is not null)
                notifyIcon.ShowBalloonTip(100, "Error!", e.Error.Message, ToolTipIcon.Error);
            else if (e.Cancelled)
                notifyIcon.ShowBalloonTip(100, "Cancelled!", "Операция прервана", ToolTipIcon.Warning);
            else
            {
                notifyIcon.ShowBalloonTip(100, "Готово!", "Файл успешно загружен", ToolTipIcon.Info);

                UpdateRastersTree(e.Result as RasterData);
            }

            toolStripProgressBar1.Visible = false;
            toolStripStatusLabel1.Visible = false;
        }

        private void contrastCorrectionBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker? worker = sender as BackgroundWorker;
            if (worker == null) return;

            // Распаковываем анонимный тип через dynamic
            dynamic? args = e.Argument;

            if (args == null) return;

            // Вызываем метод построчного сохранения (тот самый, экономный)
            RunContrastCorrection(args.Target, args.SavePath, worker);

            // Передаем результат в RunWorkerCompleted
            e.Result = new { FilePath = args.SavePath, IgnoreZero = args.IgnoreZero };
        }

        private void contrastCorrectionBackgroundWorker_RunWorkerCompleted(object sender,  RunWorkerCompletedEventArgs e)
        {
            if (e.Error is not null)
            {
                notifyIcon.ShowBalloonTip(100, "Error!", e.Error.Message, ToolTipIcon.Error);
                toolStripProgressBar1.Visible = false;
                toolStripStatusLabel1.Visible = false;
            }
            else if (e.Cancelled)
            {
                notifyIcon.ShowBalloonTip(100, "Cancelled!", "Операция прервана", ToolTipIcon.Warning);
                toolStripProgressBar1.Visible = false;
                toolStripStatusLabel1.Visible = false;
            }
            else
            {
                notifyIcon.ShowBalloonTip(100, "Сохранено!", "Файл записан. Начинаем загрузку в проект...", ToolTipIcon.Info);

                // Распаковываем результат сохранения
                dynamic? result = e.Result;

                if (result == null) return;

                string newFilePath = result.FilePath;
                bool ignoreZero = result.IgnoreZero;

                // 🔥 ЦЕПНАЯ РЕАКЦИЯ: Запускаем воркер открытия для только что созданного файла!
                // Передаем ignoreZero. Путь к файлу воркер открытия возьмет из глобальной переменной диалога 
                // или мы чуть-чуть поправим openFileBackgroundWorker_DoWork

                // Чтобы openFileBackgroundWorker знал, какой файл открывать, 
                // подменим имя в openFileDialog (так как твой DoWork завязан на него)
                openFileDialog1.FileName = newFilePath;

                // Включаем статус-бары обратно для второй операции
                toolStripProgressBar1.Visible = true;
                toolStripStatusLabel1.Visible = true;

                // Запуск воркера открытия!
                openFileBackgroundWorker.RunWorkerAsync(ignoreZero);
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

            if (splitContainer2.Panel2Collapsed)
                toolStripButton1.Image = Properties.Resources.show;
            else
                toolStripButton1.Image = Properties.Resources.hide;

            UpdateDisplaySettings(sender, e);
        }

        private void UpdateDisplaySettings(object sender, EventArgs e)
        {
            if (splitContainer2.Panel2Collapsed)
                return;

            if (treeView1.SelectedNode == null)
            {
                splitContainer2.Panel2Collapsed = true;
                toolStripButton1.Image = Properties.Resources.show;
                return;
            }

            RasterData raster;

            switch (treeView1.SelectedNode.Tag)
            {
                case RasterData rasterData:
                    raster = rasterData;
                    break;
                case BandData bandData:
                    raster = bandData.Raster;
                    break;
                default:
                    splitContainer2.Panel2Collapsed = true;
                    toolStripButton1.Image = Properties.Resources.show;
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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateImage(sender, e);
            UpdateDisplaySettings(sender, e);
        }

        private void acceptDisplaySettingsButton_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                splitContainer2.Panel2Collapsed = true;
                toolStripButton1.Image = Properties.Resources.show;

                notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
                notifyIcon.BalloonTipTitle = "Operation error!";
                notifyIcon.BalloonTipText = "Display settings not applied";

                notifyIcon.ShowBalloonTip(100);

                return;
            }

            RasterData raster;

            switch (treeView1.SelectedNode.Tag)
            {
                case RasterData rasterData:
                    raster = rasterData;
                    break;
                case BandData bandData:
                    raster = bandData.Raster;
                    break;
                default:
                    splitContainer2.Panel2Collapsed = true;
                    toolStripButton1.Image = Properties.Resources.show;

                    notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
                    notifyIcon.BalloonTipTitle = "Operation error!";
                    notifyIcon.BalloonTipText = "Display settings not applied";

                    notifyIcon.ShowBalloonTip(100);

                    return;
            }

            raster.SetViewBands(redComboBox.SelectedIndex, greenComboBox.SelectedIndex, blueComboBox.SelectedIndex);
            raster.InterpolationMode = (InterpolationMode)interpolationComboBox.SelectedIndex;

            UpdateImage(sender, e);

            splitContainer2.Panel2Collapsed = true;
            toolStripButton1.Image = Properties.Resources.show;
        }
    }
}
