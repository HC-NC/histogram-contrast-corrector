namespace Histogram_Contrast_Corrector
{
    partial class ContrastCorrectorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContrastCorrectorForm));
            continueButton = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            cancelButton = new Button();
            panel1 = new Panel();
            methodComboBox = new ComboBox();
            label1 = new Label();
            plotView1 = new OxyPlot.WindowsForms.PlotView();
            flowLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // continueButton
            // 
            resources.ApplyResources(continueButton, "continueButton");
            continueButton.DialogResult = DialogResult.Continue;
            continueButton.Name = "continueButton";
            continueButton.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(flowLayoutPanel1, "flowLayoutPanel1");
            flowLayoutPanel1.Controls.Add(continueButton);
            flowLayoutPanel1.Controls.Add(cancelButton);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // cancelButton
            // 
            resources.ApplyResources(cancelButton, "cancelButton");
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            resources.ApplyResources(panel1, "panel1");
            panel1.Controls.Add(methodComboBox);
            panel1.Controls.Add(label1);
            panel1.Name = "panel1";
            // 
            // methodComboBox
            // 
            resources.ApplyResources(methodComboBox, "methodComboBox");
            methodComboBox.FormattingEnabled = true;
            methodComboBox.Name = "methodComboBox";
            methodComboBox.SelectedIndexChanged += methodComboBox_SelectedIndexChanged;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // plotView1
            // 
            resources.ApplyResources(plotView1, "plotView1");
            plotView1.Name = "plotView1";
            plotView1.PanCursor = Cursors.Hand;
            plotView1.ZoomHorizontalCursor = Cursors.SizeWE;
            plotView1.ZoomRectangleCursor = Cursors.SizeNWSE;
            plotView1.ZoomVerticalCursor = Cursors.SizeNS;
            plotView1.DoubleClick += plotView1_DoubleClick;
            // 
            // ContrastCorrectorForm
            // 
            AcceptButton = continueButton;
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelButton;
            Controls.Add(plotView1);
            Controls.Add(panel1);
            Controls.Add(flowLayoutPanel1);
            Name = "ContrastCorrectorForm";
            Load += ContrastCorrectorForm_Load;
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button continueButton;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button cancelButton;
        private Panel panel1;
        private Label label1;
        private ComboBox methodComboBox;
        private OxyPlot.WindowsForms.PlotView plotView1;
    }
}