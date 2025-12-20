namespace Histogram_Contrast_Corrector
{
    partial class WorkSpace
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            contrastCorrectorToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog1 = new OpenFileDialog();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            toolStripProgressBar1 = new ToolStripProgressBar();
            splitContainer1 = new SplitContainer();
            treeView1 = new TreeView();
            viewBox = new PictureBox();
            treeContextMenuStrip = new ContextMenuStrip(components);
            histogramToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            removeToolStripMenuItem = new ToolStripMenuItem();
            openFileBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            notifyIcon1 = new NotifyIcon(components);
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)viewBox).BeginInit();
            treeContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, toolsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1178, 33);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(54, 29);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openToolStripMenuItem.Size = new Size(223, 34);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(220, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
            exitToolStripMenuItem.Size = new Size(223, 34);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { contrastCorrectorToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(69, 29);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // contrastCorrectorToolStripMenuItem
            // 
            contrastCorrectorToolStripMenuItem.Name = "contrastCorrectorToolStripMenuItem";
            contrastCorrectorToolStripMenuItem.Size = new Size(260, 34);
            contrastCorrectorToolStripMenuItem.Text = "Contrast Corrector";
            contrastCorrectorToolStripMenuItem.Click += contrastCorrectorToolStripMenuItem_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, toolStripProgressBar1 });
            statusStrip1.Location = new Point(0, 712);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1178, 32);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(179, 25);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripProgressBar1
            // 
            toolStripProgressBar1.Name = "toolStripProgressBar1";
            toolStripProgressBar1.Size = new Size(100, 24);
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 33);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(treeView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(viewBox);
            splitContainer1.Size = new Size(1178, 679);
            splitContainer1.SplitterDistance = 392;
            splitContainer1.TabIndex = 4;
            // 
            // treeView1
            // 
            treeView1.Dock = DockStyle.Fill;
            treeView1.Location = new Point(0, 0);
            treeView1.Name = "treeView1";
            treeView1.ShowNodeToolTips = true;
            treeView1.Size = new Size(392, 679);
            treeView1.TabIndex = 0;
            treeView1.AfterSelect += UpdateImage;
            treeView1.NodeMouseClick += treeView1_NodeMouseClick;
            // 
            // viewBox
            // 
            viewBox.Dock = DockStyle.Fill;
            viewBox.Location = new Point(0, 0);
            viewBox.Name = "viewBox";
            viewBox.Size = new Size(782, 679);
            viewBox.TabIndex = 1;
            viewBox.TabStop = false;
            viewBox.Paint += viewBox_Paint;
            viewBox.DoubleClick += ResetViewBox;
            viewBox.MouseDown += viewBox_MouseDown;
            viewBox.MouseEnter += viewBox_MouseEnter;
            viewBox.MouseLeave += viewBox_MouseLeave;
            viewBox.MouseMove += viewBox_MouseMove;
            viewBox.MouseUp += viewBox_MouseUp;
            viewBox.Resize += ResetViewBox;
            // 
            // treeContextMenuStrip
            // 
            treeContextMenuStrip.ImageScalingSize = new Size(24, 24);
            treeContextMenuStrip.Items.AddRange(new ToolStripItem[] { histogramToolStripMenuItem, aboutToolStripMenuItem, toolStripSeparator2, removeToolStripMenuItem });
            treeContextMenuStrip.Name = "treeContextMenuStrip";
            treeContextMenuStrip.Size = new Size(169, 106);
            // 
            // histogramToolStripMenuItem
            // 
            histogramToolStripMenuItem.Name = "histogramToolStripMenuItem";
            histogramToolStripMenuItem.Size = new Size(168, 32);
            histogramToolStripMenuItem.Text = "Histogram";
            histogramToolStripMenuItem.Click += histogramToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(168, 32);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(165, 6);
            // 
            // removeToolStripMenuItem
            // 
            removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            removeToolStripMenuItem.Size = new Size(168, 32);
            removeToolStripMenuItem.Text = "Remove";
            removeToolStripMenuItem.Click += removeToolStripMenuItem_Click;
            // 
            // openFileBackgroundWorker
            // 
            openFileBackgroundWorker.WorkerReportsProgress = true;
            openFileBackgroundWorker.WorkerSupportsCancellation = true;
            openFileBackgroundWorker.DoWork += openFileBackgroundWorker_DoWork;
            openFileBackgroundWorker.ProgressChanged += openFileBackgroundWorker_ProgressChanged;
            openFileBackgroundWorker.RunWorkerCompleted += openFileBackgroundWorker_RunWorkerCompleted;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            // 
            // WorkSpace
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1178, 744);
            Controls.Add(splitContainer1);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "WorkSpace";
            ShowIcon = false;
            Text = "HCC";
            FormClosing += WorkSpace_FormClosing;
            Load += WorkSpace_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)viewBox).EndInit();
            treeContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private OpenFileDialog openFileDialog1;
        private StatusStrip statusStrip1;
        private SplitContainer splitContainer1;
        private TreeView treeView1;
        private PictureBox viewBox;
        private ContextMenuStrip treeContextMenuStrip;
        private ToolStripMenuItem histogramToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem removeToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem contrastCorrectorToolStripMenuItem;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripProgressBar toolStripProgressBar1;
        private System.ComponentModel.BackgroundWorker openFileBackgroundWorker;
        private NotifyIcon notifyIcon1;
    }
}
