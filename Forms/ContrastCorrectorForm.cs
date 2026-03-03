using OxyPlot;
using OxyPlot.Series;

namespace Histogram_Contrast_Corrector
{
    public partial class ContrastCorrectorForm : Form
    {
        private ICorrectionMethod? _correctionMethod;

        public ICorrectionMethod CorrectionMethod => _correctionMethod;

        public ContrastCorrectorForm()
        {
            InitializeComponent();
        }

        private void ContrastCorrectorForm_Load(object sender, EventArgs e)
        {
            methodComboBox.Items.AddRange(Enum.GetNames<CorrectionMethods>());
            methodComboBox.SelectedIndex = 0;
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
                    panel2.Visible = false;
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
                default:
                    return;
            }

            DrawPlot();
        }

        public CorrectionMethods GetMethods()
        {
            return (CorrectionMethods)methodComboBox.SelectedIndex;
        }

        private void DrawPlot()
        {
            var lineSeries = new LineSeries();

            float x;

            for (int i = 0; i <= 100; i++)
            {
                x = i / 100f;
                lineSeries.Points.Add(new DataPoint(x, _correctionMethod.F(x)));
            }

            PlotModel plot = new PlotModel();
            plot.Series.Add(lineSeries);
            plotView1.Model = plot;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            trackBar1.Value = (int)(100 * numericUpDown1.Value);
            _correctionMethod?.SetA((float)numericUpDown1.Value);
            DrawPlot();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown1.Value = trackBar1.Value / 100m;
            _correctionMethod?.SetA((float)numericUpDown1.Value);
            DrawPlot();
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

        public float GetA()
        {
            return 0;
        }

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

        public float GetA()
        {
            return 0;
        }

        public void SetA(float a)
        {
            return;
        }
    }

    public class LogCorrection : ICorrectionMethod
    {
        public float F(float x)
        {
            return MathF.Log(x + 1);
        }

        public float GetA()
        {
            return 0;
        }

        public void SetA(float a)
        {
            return;
        }
    }

    public class ExpCorrection : ICorrectionMethod
    {
        private float _a = 1f;

        public float F(float x)
        {
            return MathF.Exp(_a * x) - 1f;
        }

        public float GetA()
        {
            return _a;
        }

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

        public float GetA()
        {
            return _a;
        }

        public void SetA(float a)
        {
            _a = a;
        }
    }

}
