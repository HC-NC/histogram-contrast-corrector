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
            ToolStripLabel toolStripLabel1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContrastCorrectorForm));
            continueButton = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            cancelButton = new Button();
            panel1 = new Panel();
            methodComboBox = new ComboBox();
            label1 = new Label();
            plotView1 = new OxyPlot.WindowsForms.PlotView();
            panel2 = new Panel();
            label2 = new Label();
            numericUpDown1 = new NumericUpDown();
            trackBar1 = new TrackBar();
            splitContainer1 = new SplitContainer();
            pictureBox1 = new PictureBox();
            toolStrip2 = new ToolStrip();
            rasterNameToolStripLabel = new ToolStripLabel();
            toolStrip1 = new ToolStrip();
            blueToolStripLable = new ToolStripLabel();
            greenToolStripLable = new ToolStripLabel();
            redToolStripLable = new ToolStripLabel();
            toolStripLabel1 = new ToolStripLabel();
            flowLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            toolStrip2.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripLabel1.Name = "toolStripLabel1";
            resources.ApplyResources(toolStripLabel1, "toolStripLabel1");
            // 
            // continueButton
            // 
            continueButton.DialogResult = DialogResult.Continue;
            resources.ApplyResources(continueButton, "continueButton");
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
            cancelButton.DialogResult = DialogResult.Cancel;
            resources.ApplyResources(cancelButton, "cancelButton");
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
            // panel2
            // 
            resources.ApplyResources(panel2, "panel2");
            panel2.Controls.Add(label2);
            panel2.Controls.Add(numericUpDown1);
            panel2.Controls.Add(trackBar1);
            panel2.Name = "panel2";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // numericUpDown1
            // 
            numericUpDown1.DecimalPlaces = 2;
            numericUpDown1.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            resources.ApplyResources(numericUpDown1, "numericUpDown1");
            numericUpDown1.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // trackBar1
            // 
            resources.ApplyResources(trackBar1, "trackBar1");
            trackBar1.LargeChange = 100;
            trackBar1.Maximum = 1000;
            trackBar1.Minimum = 1;
            trackBar1.Name = "trackBar1";
            trackBar1.SmallChange = 10;
            trackBar1.TickFrequency = 100;
            trackBar1.Value = 100;
            trackBar1.ValueChanged += trackBar1_ValueChanged;
            // 
            // splitContainer1
            // 
            resources.ApplyResources(splitContainer1, "splitContainer1");
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(plotView1);
            splitContainer1.Panel1.Controls.Add(panel2);
            splitContainer1.Panel1.Controls.Add(panel1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(pictureBox1);
            splitContainer1.Panel2.Controls.Add(toolStrip2);
            splitContainer1.Panel2.Controls.Add(toolStrip1);
            // 
            // pictureBox1
            // 
            resources.ApplyResources(pictureBox1, "pictureBox1");
            pictureBox1.Name = "pictureBox1";
            pictureBox1.TabStop = false;
            // 
            // toolStrip2
            // 
            resources.ApplyResources(toolStrip2, "toolStrip2");
            toolStrip2.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip2.ImageScalingSize = new Size(24, 24);
            toolStrip2.Items.AddRange(new ToolStripItem[] { rasterNameToolStripLabel });
            toolStrip2.Name = "toolStrip2";
            // 
            // rasterNameToolStripLabel
            // 
            rasterNameToolStripLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            rasterNameToolStripLabel.Name = "rasterNameToolStripLabel";
            resources.ApplyResources(rasterNameToolStripLabel, "rasterNameToolStripLabel");
            // 
            // toolStrip1
            // 
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.ImageScalingSize = new Size(24, 24);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, blueToolStripLable, greenToolStripLable, redToolStripLable });
            resources.ApplyResources(toolStrip1, "toolStrip1");
            toolStrip1.Name = "toolStrip1";
            // 
            // blueToolStripLable
            // 
            blueToolStripLable.Alignment = ToolStripItemAlignment.Right;
            blueToolStripLable.Image = Properties.Resources.blue_sqaure;
            resources.ApplyResources(blueToolStripLable, "blueToolStripLable");
            blueToolStripLable.Name = "blueToolStripLable";
            // 
            // greenToolStripLable
            // 
            greenToolStripLable.Alignment = ToolStripItemAlignment.Right;
            greenToolStripLable.Image = Properties.Resources.green_sqaure;
            resources.ApplyResources(greenToolStripLable, "greenToolStripLable");
            greenToolStripLable.Name = "greenToolStripLable";
            // 
            // redToolStripLable
            // 
            redToolStripLable.Alignment = ToolStripItemAlignment.Right;
            redToolStripLable.Image = Properties.Resources.red_sqaure;
            redToolStripLable.Name = "redToolStripLable";
            resources.ApplyResources(redToolStripLable, "redToolStripLable");
            // 
            // ContrastCorrectorForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelButton;
            Controls.Add(splitContainer1);
            Controls.Add(flowLayoutPanel1);
            Name = "ContrastCorrectorForm";
            Load += ContrastCorrectorForm_Load;
            flowLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            toolStrip2.ResumeLayout(false);
            toolStrip2.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
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
        private Panel panel2;
        private TrackBar trackBar1;
        private NumericUpDown numericUpDown1;
        private Label label2;
        private SplitContainer splitContainer1;
        private PictureBox pictureBox1;
        private ToolStrip toolStrip1;
        private ToolStrip toolStrip2;
        private ToolStripLabel rasterNameToolStripLabel;
        private ToolStripLabel blueToolStripLable;
        private ToolStripLabel greenToolStripLable;
        private ToolStripLabel redToolStripLable;
    }
}