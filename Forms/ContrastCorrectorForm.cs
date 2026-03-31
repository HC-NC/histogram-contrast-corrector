using OxyPlot;
using OxyPlot.Series;
using Histogram_Contrast_Corrector.DataClasses;

namespace Histogram_Contrast_Corrector
{
    public partial class ContrastCorrectorForm : Form
    {
        private ICorrectionMethod _correctionMethod;

        private RasterData _rasterData;
        private BandData? _targetBand;
        private bool _applyToAllBands;

        private bool _isUpdating = false;

        private PlotModel _plotModel;
        private LineSeries _lineSeries;

        private float[]? _previewR, _previewG, _previewB;
        private int _previewWidth, _previewHeight;
        private float _rMin, _rMax, _gMin, _gMax, _bMin, _bMax;
        private float[]? _rAssesment, _gAssesment, _bAssesment;

        private Bitmap? _previewBitmap;

        public ICorrectionMethod CorrectionMethod => _correctionMethod;

        // Конструктор для всего растра
        public ContrastCorrectorForm(RasterData rasterData)
        {
            InitializeComponent();
            _rasterData = rasterData;
            _applyToAllBands = true;
            _targetBand = null;

            InitForm();
        }

        // Конструктор для конкретного канала
        public ContrastCorrectorForm(BandData bandData)
        {
            InitializeComponent();
            _rasterData = bandData.Raster;
            _applyToAllBands = false;
            _targetBand = bandData;

            InitForm();
        }

        private void InitForm()
        {
            rasterNameToolStripLabel.Text = _rasterData.ToString();

            _plotModel = new PlotModel();
            _lineSeries = new LineSeries();
            _plotModel.Series.Add(_lineSeries);
            plotView1.Model = _plotModel;
        }

        private void ContrastCorrectorForm_Load(object sender, EventArgs e)
        {
            UpdatePreviewData();

            methodComboBox.Items.AddRange(Enum.GetNames<CorrectionMethods>());
            methodComboBox.SelectedIndex = 0;
        }

        private void UpdatePreviewData()
        {
            // Задаем максимальный размер стороны для превью (например, 1200 пикселей)
            int maxPreviewSize = 1200;
            int step = 1;

            // Вычисляем шаг (step) динамически на основе самой длинной стороны
            int maxOriginalSize = Math.Max(_rasterData.Width, _rasterData.Height);

            if (maxOriginalSize > maxPreviewSize)
            {
                step = maxOriginalSize / maxPreviewSize;

                // На всякий случай страхуемся, чтобы step не стал равен 0
                if (step < 1) step = 1;
            }
            else
            {
                // Если картинка и так меньше 1200 пикселей, берем ее один к одному
                step = 1;
            }

            _previewWidth = _rasterData.Width / step;
            _previewHeight = _rasterData.Height / step;

            _previewR = new float[_previewWidth * _previewHeight];
            _previewG = new float[_previewWidth * _previewHeight];
            _previewB = new float[_previewWidth * _previewHeight];

            // Ссылки на оригинальные массивы
            float[]? rOrig, gOrig, bOrig;

            if (!_applyToAllBands && _targetBand != null)
            {
                // 🔥 СЛУЧАЙ 1: Выбран конкретный канал. 
                // Дублируем его данные во все три переменные, чтобы получить ЧБ картинку
                rOrig = _targetBand.Values;
                gOrig = rOrig;
                bOrig = rOrig;

                _rMin = _gMin = _bMin = _targetBand.Minimum;
                _rMax = _gMax = _bMax = _targetBand.Maximum;

                // Проверяем и считаем ассессмент для целевого канала
                _rAssesment = _targetBand.AssesmentValues;
                if (_rAssesment == null)
                {
                    _targetBand.CalculateHistogram();
                    _rAssesment = _targetBand.AssesmentValues;
                }
                _gAssesment = _rAssesment;
                _bAssesment = _rAssesment;

                // Обновляем лейблы на форме
                redToolStripLable.Text = _targetBand.ToString();
                greenToolStripLable.Text = _targetBand.ToString();
                blueToolStripLable.Text = _targetBand.ToString();
            }
            else
            {
                // 🌈 СЛУЧАЙ 2: Выбран весь растр (твоя оригинальная логика)
                var rBand = _rasterData.GetBand(_rasterData.RedID);
                var gBand = _rasterData.GetBand(_rasterData.GreenID);
                var bBand = _rasterData.GetBand(_rasterData.BlueID);

                if (rBand == null || gBand == null || bBand == null) return;

                redToolStripLable.Text = rBand.ToString();
                greenToolStripLable.Text = gBand.ToString();
                blueToolStripLable.Text = bBand.ToString();

                rOrig = rBand.Values;

                if (gBand != rBand) gOrig = gBand.Values;
                else gOrig = rOrig;

                if (bBand != rBand && bBand != gBand) bOrig = bBand.Values;
                else if (bBand == rBand) bOrig = rOrig;
                else bOrig = gOrig;

                _rMin = rBand.Minimum; _rMax = rBand.Maximum;
                _gMin = gBand.Minimum; _gMax = gBand.Maximum;
                _bMin = bBand.Minimum; _bMax = bBand.Maximum;

                // Получение ассессментов для каждого канала...
                _rAssesment = rBand.AssesmentValues;
                if (_rAssesment == null) { rBand.CalculateHistogram(); _rAssesment = rBand.AssesmentValues; }

                _gAssesment = gBand.AssesmentValues;
                if (_gAssesment == null) { gBand.CalculateHistogram(); _gAssesment = gBand.AssesmentValues; }

                _bAssesment = bBand.AssesmentValues;
                if (_bAssesment == null) { bBand.CalculateHistogram(); _bAssesment = bBand.AssesmentValues; }
            }

            if (rOrig == null || gOrig == null || bOrig == null) return;

            // Наполнение массивов превью
            for (int y = 0; y < _previewHeight; y++)
            {
                for (int x = 0; x < _previewWidth; x++)
                {
                    int origIdx = (y * step) * _rasterData.Width + (x * step);
                    int prevIdx = y * _previewWidth + x;

                    _previewR[prevIdx] = rOrig[origIdx];
                    _previewG[prevIdx] = gOrig[origIdx];
                    _previewB[prevIdx] = bOrig[origIdx];
                }
            }
        }

        private void plotView1_DoubleClick(object sender, EventArgs e)
        {
            plotView1.Model.ResetAllAxes();
            plotView1.Refresh();
        }

        private void methodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((CorrectionMethods)methodComboBox.SelectedIndex)
            {
                default:
                case CorrectionMethods.Linear:
                    _correctionMethod = new LinearCorrection();
                    panel2.Visible = false;
                    ApplyPreview();
                    break;
                case CorrectionMethods.Negative:
                    _correctionMethod = new NegativeCorrection();
                    panel2.Visible = false;
                    ApplyPreview();
                    break;
                case CorrectionMethods.Log:
                    _correctionMethod = new LogCorrection();
                    panel2.Visible = true;
                    numericUpDown1.Value = 2;
                    break;
                case CorrectionMethods.Exp:
                    _correctionMethod = new ExpCorrection();
                    panel2.Visible = true;
                    numericUpDown1.Value = 1;
                    break;
                case CorrectionMethods.Power:
                    _correctionMethod = new PowerCorrection();
                    panel2.Visible = true;
                    numericUpDown1.Value = 1;
                    break;
            }

            DrawPlot();
        }

        public CorrectionMethods GetMethods()
        {
            return (CorrectionMethods)methodComboBox.SelectedIndex;
        }

        private void DrawPlot()
        {
            if (_correctionMethod == null) return;

            // Очищаем только точки, не пересоздавая саму серию и модель
            _lineSeries.Points.Clear();

            for (int i = 0; i <= 100; i++)
            {
                float x = i / 100f;
                _lineSeries.Points.Add(new DataPoint(x, _correctionMethod.F(x)));
            }

            // Заставляем OxyPlot перерисовать только данные (это ОЧЕНЬ быстро)
            plotView1.InvalidatePlot(true);
        }

        private void ApplyPreview()
        {
            if (_correctionMethod == null || _previewR == null || _previewG == null || _previewB == null) return;

            if (_previewBitmap == null)
                _previewBitmap = new Bitmap(_previewWidth, _previewHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            var bmpData = _previewBitmap.LockBits(
                new Rectangle(0, 0, _previewWidth, _previewHeight),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0;
                CorrectionMethods currentMethod = GetMethods();

                Parallel.For(0, _previewHeight, y =>
                {
                    for (int x = 0; x < _previewWidth; x++)
                    {
                        int idx = y * _previewWidth + x;
                        int offset = y * bmpData.Stride + x * 4;

                        // Читаем сырые значения из превью-кэша
                        float rRaw = _previewR[idx];
                        float gRaw = _previewG[idx];
                        float bRaw = _previewB[idx];

                        // Обрабатываем каждый канал через ТВОЮ оригинальную логику
                        float rFinal = CalculateCorrectedValue(rRaw, _rMin, _rMax, _rAssesment, currentMethod);
                        float gFinal = CalculateCorrectedValue(gRaw, _gMin, _gMax, _gAssesment, currentMethod);
                        float bFinal = CalculateCorrectedValue(bRaw, _bMin, _bMax, _bAssesment, currentMethod);

                        // Для вывода на экран (в битмап) нам в любом случае нужно 
                        // отнормировать полученное значение в диапазон 0-255!
                        ptr[offset] = (byte)Math.Clamp((bFinal - _bMin) / (_bMax - _bMin) * 255, 0, 255); // B
                        ptr[offset + 1] = (byte)Math.Clamp((gFinal - _gMin) / (_gMax - _gMin) * 255, 0, 255); // G
                        ptr[offset + 2] = (byte)Math.Clamp((rFinal - _rMin) / (_rMax - _rMin) * 255, 0, 255); // R
                        ptr[offset + 3] = 255; // Alpha ( непрозрачный )
                    }
                });
            }

            _previewBitmap.UnlockBits(bmpData);
            pictureBox1.Image = _previewBitmap;
        }

        private float CalculateCorrectedValue(float value, float minimum, float maximum, float[] assesment, CorrectionMethods method)
        {
            float v = value - minimum;

            if (v < 0 || v >= assesment.Length)
                return 0;

            float assesmentVal = assesment[(int)v];
            float c;

            switch (method)
            {
                case CorrectionMethods.Linear:
                case CorrectionMethods.Negative:
                case CorrectionMethods.Power:
                default:
                    return minimum + (maximum - minimum) * _correctionMethod.F(assesmentVal);

                case CorrectionMethods.Exp:
                    c = (maximum - 1) / (MathF.Exp(_correctionMethod.GetA()) - 1);
                    return c * _correctionMethod.F(assesmentVal);

                case CorrectionMethods.Log:
                    c = (maximum - 1) / (MathF.Log(1f + (_correctionMethod.GetA() - 1f)) / MathF.Log(_correctionMethod.GetA()));
                    return c * _correctionMethod.F(assesmentVal);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (_isUpdating) return;
            _isUpdating = true;

            trackBar1.Value = (int)(100 * numericUpDown1.Value);
            _correctionMethod?.SetA((float)numericUpDown1.Value);

            DrawPlot();
            ApplyPreview();
            _isUpdating = false;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (_isUpdating) return;
            _isUpdating = true;

            numericUpDown1.Value = trackBar1.Value / 100m;
            _correctionMethod?.SetA((float)numericUpDown1.Value);

            DrawPlot();
            ApplyPreview();
            _isUpdating = false;
        }
    }

    public enum CorrectionMethods
    {
        Linear,
        Negative,
        Log,
        Power,
        Exp
    }

    public interface ICorrectionMethod
    {
        public float F(float x);
        public void SetA(float a);
        public float GetA();

    }

    public class LinearCorrection : ICorrectionMethod
    {
        public float F(float x)
        {
            return x;
        }

        public float GetA() => 0;

        public void SetA(float a)
        {
            return;
        }
    }

    public class NegativeCorrection : ICorrectionMethod
    {
        public float F(float x)
        {
            return 1f - x;
        }

        public float GetA() => 0;

        public void SetA(float a)
        {
            return;
        }
    }

    public class LogCorrection : ICorrectionMethod
    {
        private float _a = 2f; 

        public float F(float x)
        {
            return MathF.Log(1f + (_a - 1f) * x) / MathF.Log(_a);
        }

        public float GetA() => _a;

        public void SetA(float a)
        {
            _a = a;
        }
    }

    public class ExpCorrection : ICorrectionMethod
    {
        private float _a = 1f;

        public float F(float x)
        {
            return MathF.Exp(_a * x) - 1f;
        }

        public float GetA() => _a;

        public void SetA(float a)
        {
            _a = a;
        }
    }

    public class PowerCorrection : ICorrectionMethod
    {
        private float _a = 1f;

        public float F(float x)
        {
            return MathF.Pow(x, _a);
        }

        public float GetA() => _a;

        public void SetA(float a)
        {
            _a = a;
        }
    }

}
