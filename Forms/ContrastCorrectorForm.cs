using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Histogram_Contrast_Corrector
{
    public partial class ContrastCorrectorForm : Form
    {
        private ICorrectionMethod _correctionMethod;

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
                    break;
                case CorrectionMethods.Negative:
                    _correctionMethod = new NegativeCorrection();
                    break;
                case CorrectionMethods.Log:
                    _correctionMethod = new LogCorrection();
                    break;
                case CorrectionMethods.Exp:
                    _correctionMethod = new ExpCorrection();
                    break;
                case CorrectionMethods.Power:
                    _correctionMethod = new PowerCorrection();
                    break;

                default:
                    return;
            }

            DrawPlot();
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

    }

    public class LinearCorrection : ICorrectionMethod
    {
        public float F(float x)
        {
            return x; 
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

        public void SetA(float a)
        {
            return;
        }
    }

    public class LogCorrection : ICorrectionMethod
    {       
        public float F(float x)
        {
            return MathF.Log(x);
        }

        public void SetA(float a)
        {
            return;
        }
    }
    public class ExpCorrection : ICorrectionMethod
    {
        private float _a;

        public float F(float x)
        {
            return MathF.Exp(_a * x) - 1f;
        }

        public void SetA(float a)
        {
            _a = a;
        }
    }

    public class PowerCorrection : ICorrectionMethod
    {
        private float _a;

        public float F(float x)
        {
            return MathF.Pow(x, _a);
        }

        public void SetA(float a)
        {
            _a = a;
        }
    }

}
