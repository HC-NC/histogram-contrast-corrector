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
        Negative
    }

    public interface ICorrectionMethod
    {
        public float F(float x);
    }

    public class LinearCorrection : ICorrectionMethod
    {
        public float F(float x)
        {
            return x; 
        }
    }

    public class NegativeCorrection : ICorrectionMethod
    {
        public float F(float x)
        {
            return 1f - x; 
        }
    }
}
