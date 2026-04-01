using Histogram_Contrast_Corrector.DataClasses;
using Histogram_Contrast_Corrector.Properties;
using OxyPlot;
using OxyPlot.Series;
using System.Drawing.Imaging;

namespace Histogram_Contrast_Corrector
{
    public partial class ContrastCorrectorForm : Form
    {
        private const int MAX_PREVIEW_SIZE = 1200;

        private ICorrectionMethod _correctionMethod;

        private readonly RasterData _rasterData;
        private readonly BandData? _targetBand;
        private readonly bool _applyToAllBands;

        private bool _isUpdating = false;

        private PlotModel _plotModel;
        private LineSeries _lineSeries;

        private float[]? _previewR, _previewG, _previewB;
        private int _previewWidth, _previewHeight;
        private float _rMin, _rMax, _gMin, _gMax, _bMin, _bMax;
        private float[]? _rAssesment, _gAssesment, _bAssesment;

        private Bitmap? _previewBitmap;

        public ICorrectionMethod CorrectionMethod => _correctionMethod;

        public ContrastCorrectorForm(RasterData rasterData)
        {
            InitializeComponent();
            _rasterData = rasterData ?? throw new ArgumentNullException(nameof(rasterData));
            _applyToAllBands = true;
            _targetBand = null;

            InitForm();
        }

        public ContrastCorrectorForm(BandData bandData)
        {
            InitializeComponent();
            _targetBand = bandData ?? throw new ArgumentNullException(nameof(bandData));
            _rasterData = bandData.Raster;
            _applyToAllBands = false;

            InitForm();
        }

        private void InitForm()
        {
            rasterNameToolStripLabel.Text = _rasterData.ToString();

            _plotModel = new PlotModel();
            _lineSeries = new LineSeries { Color = OxyColor.FromRgb(0, 122, 204) };
            _plotModel.Series.Add(_lineSeries);
            plotView1.Model = _plotModel;
        }

        private void ContrastCorrectorForm_Load(object sender, EventArgs e)
        {
            UpdatePreviewData();

            methodComboBox.Items.Clear();
            methodComboBox.Items.Add(Resources.MethodLinear);
            methodComboBox.Items.Add(Resources.MethodNegative);
            methodComboBox.Items.Add(Resources.MethodLog);
            methodComboBox.Items.Add(Resources.MethodPower);
            methodComboBox.Items.Add(Resources.MethodExp);

            methodComboBox.SelectedIndex = 0;
        }

        private void UpdatePreviewData()
        {
            int maxOriginalSize = Math.Max(_rasterData.Width, _rasterData.Height);

            int step = maxOriginalSize > MAX_PREVIEW_SIZE ? maxOriginalSize / MAX_PREVIEW_SIZE : 1;
            if (step < 1) step = 1;

            _previewWidth = _rasterData.Width / step;
            _previewHeight = _rasterData.Height / step;

            _previewR = new float[_previewWidth * _previewHeight];
            _previewG = new float[_previewWidth * _previewHeight];
            _previewB = new float[_previewWidth * _previewHeight];

            float[]? rOrig, gOrig, bOrig;

            if (!_applyToAllBands && _targetBand != null)
            {
                rOrig = gOrig = bOrig = _targetBand.Values;

                _rMin = _gMin = _bMin = _targetBand.Minimum;
                _rMax = _gMax = _bMax = _targetBand.Maximum;

                _rAssesment = _targetBand.AssesmentValues;
                if (_rAssesment == null)
                {
                    _targetBand.CalculateHistogram();
                    _rAssesment = _targetBand.AssesmentValues;
                }
                _gAssesment = _bAssesment = _rAssesment;

                string bandStr = _targetBand.ToString();
                redToolStripLable.Text = bandStr;
                greenToolStripLable.Text = bandStr;
                blueToolStripLable.Text = bandStr;
            }
            else
            {
                var rBand = _rasterData.GetBand(_rasterData.RedID);
                var gBand = _rasterData.GetBand(_rasterData.GreenID);
                var bBand = _rasterData.GetBand(_rasterData.BlueID);

                if (rBand == null || gBand == null || bBand == null) return;

                redToolStripLable.Text = rBand.ToString();
                greenToolStripLable.Text = gBand.ToString();
                blueToolStripLable.Text = bBand.ToString();

                rOrig = rBand.Values;
                gOrig = (gBand != rBand) ? gBand.Values : rOrig;

                if (bBand != rBand && bBand != gBand) bOrig = bBand.Values;
                else if (bBand == rBand) bOrig = rOrig;
                else bOrig = gOrig;

                _rMin = rBand.Minimum; _rMax = rBand.Maximum;
                _gMin = gBand.Minimum; _gMax = gBand.Maximum;
                _bMin = bBand.Minimum; _bMax = bBand.Maximum;

                _rAssesment = GetValidAssessment(rBand);
                _gAssesment = GetValidAssessment(gBand);
                _bAssesment = GetValidAssessment(bBand);
            }

            if (rOrig == null || gOrig == null || bOrig == null) return;

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

        private float[]? GetValidAssessment(BandData band)
        {
            if (band.AssesmentValues == null)
                band.CalculateHistogram();
            return band.AssesmentValues;
        }

        private void plotView1_DoubleClick(object sender, EventArgs e)
        {
            plotView1.Model.ResetAllAxes();
            plotView1.InvalidatePlot(true);
        }

        private void methodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedMethod = GetMethods();

            switch (selectedMethod)
            {
                case CorrectionMethods.Linear:
                    _correctionMethod = new LinearCorrection();
                    panel2.Visible = false;
                    break;
                case CorrectionMethods.Negative:
                    _correctionMethod = new NegativeCorrection();
                    panel2.Visible = false;
                    break;
                case CorrectionMethods.Log:
                    _correctionMethod = new LogCorrection();
                    panel2.Visible = true;
                    numericUpDown1.Value = 2.0m;
                    break;
                case CorrectionMethods.Exp:
                    _correctionMethod = new ExpCorrection();
                    panel2.Visible = true;
                    numericUpDown1.Value = 1.0m;
                    break;
                case CorrectionMethods.Power:
                    _correctionMethod = new PowerCorrection();
                    panel2.Visible = true;
                    numericUpDown1.Value = 1.0m;
                    break;
                default:
                    _correctionMethod = new LinearCorrection();
                    panel2.Visible = false;
                    break;
            }

            DrawPlot();
            ApplyPreview();
        }

        public CorrectionMethods GetMethods()
        {
            return (CorrectionMethods)methodComboBox.SelectedIndex;
        }

        private void DrawPlot()
        {
            if (_correctionMethod == null) return;

            _lineSeries.Points.Clear();

            for (int i = 0; i <= 100; i++)
            {
                float x = i / 100f;
                _lineSeries.Points.Add(new DataPoint(x, _correctionMethod.F(x)));
            }

            plotView1.InvalidatePlot(true);
        }

        private void ApplyPreview()
        {
            if (_correctionMethod == null || _previewR == null || _previewG == null || _previewB == null) return;
            if (_rAssesment == null || _gAssesment == null || _bAssesment == null) return;

            if (_previewBitmap == null || _previewBitmap.Width != _previewWidth || _previewBitmap.Height != _previewHeight)
            {
                _previewBitmap?.Dispose();
                _previewBitmap = new Bitmap(_previewWidth, _previewHeight, PixelFormat.Format32bppArgb);
            }

            var bmpData = _previewBitmap.LockBits(
                new Rectangle(0, 0, _previewWidth, _previewHeight),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);

            try
            {
                unsafe
                {
                    byte* scan0 = (byte*)bmpData.Scan0;
                    int stride = bmpData.Stride;
                    CorrectionMethods currentMethod = GetMethods();
                    int width = _previewWidth;

                    Parallel.For(0, _previewHeight, y =>
                    {
                        byte* row = scan0 + (y * stride);

                        for (int x = 0; x < width; x++)
                        {
                            int idx = y * width + x;
                            int offset = x * 4;

                            float rFinal = CalculateCorrectedValue(_previewR[idx], _rMin, _rMax, _rAssesment, currentMethod);
                            float gFinal = CalculateCorrectedValue(_previewG[idx], _gMin, _gMax, _gAssesment, currentMethod);
                            float bFinal = CalculateCorrectedValue(_previewB[idx], _bMin, _bMax, _bAssesment, currentMethod);

                            row[offset] = (byte)Math.Clamp((bFinal - _bMin) / Math.Max(_bMax - _bMin, 1f) * 255, 0, 255);     // B
                            row[offset + 1] = (byte)Math.Clamp((gFinal - _gMin) / Math.Max(_gMax - _gMin, 1f) * 255, 0, 255); // G
                            row[offset + 2] = (byte)Math.Clamp((rFinal - _rMin) / Math.Max(_rMax - _rMin, 1f) * 255, 0, 255); // R
                            row[offset + 3] = 255; // Alpha
                        }
                    });
                }
            }
            finally
            {
                _previewBitmap.UnlockBits(bmpData);
            }

            pictureBox1.Image = _previewBitmap;
        }

        private float CalculateCorrectedValue(float value, float minimum, float maximum, float[] assesment, CorrectionMethods method)
        {
            float v = value - minimum;

            if (v < 0 || v >= assesment.Length)
                return minimum;

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
                    float expA = _correctionMethod.GetA();
                    c = (maximum - 1) / (MathF.Exp(expA) - 1f);
                    return c * _correctionMethod.F(assesmentVal);

                case CorrectionMethods.Log:
                    float logA = _correctionMethod.GetA();
                    c = (maximum - 1) / (MathF.Log(1f + (logA - 1f)) / MathF.Log(logA));
                    return c * _correctionMethod.F(assesmentVal);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (_isUpdating) return;
            _isUpdating = true;

            trackBar1.Value = Math.Clamp((int)(100 * numericUpDown1.Value), trackBar1.Minimum, trackBar1.Maximum);
            _correctionMethod?.SetA((float)numericUpDown1.Value);

            DrawPlot();
            ApplyPreview();
            _isUpdating = false;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (_isUpdating) return;
            _isUpdating = true;

            numericUpDown1.Value = Math.Clamp(trackBar1.Value / 100m, numericUpDown1.Minimum, numericUpDown1.Maximum);
            _correctionMethod?.SetA((float)numericUpDown1.Value);

            DrawPlot();
            ApplyPreview();
            _isUpdating = false;
        }
    }
}
