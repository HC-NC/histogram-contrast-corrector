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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkSpace));
            splitContainer1 = new SplitContainer();
            treeView1 = new TreeView();
            viewBox = new PictureBox();
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
            treeContextMenuStrip = new ContextMenuStrip(components);
            histogramToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            removeToolStripMenuItem = new ToolStripMenuItem();
            openFileBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            notifyIcon1 = new NotifyIcon(components);
            saveFileDialog1 = new SaveFileDialog();
            contrastCorrectionBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)viewBox).BeginInit();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            treeContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(splitContainer1, "splitContainer1");
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(splitContainer1.Panel1, "splitContainer1.Panel1");
            splitContainer1.Panel1.Controls.Add(treeView1);
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(splitContainer1.Panel2, "splitContainer1.Panel2");
            splitContainer1.Panel2.Controls.Add(viewBox);
            // 
            // treeView1
            // 
            resources.ApplyResources(treeView1, "treeView1");
            treeView1.Name = "treeView1";
            treeView1.ShowNodeToolTips = true;
            treeView1.AfterSelect += UpdateImage;
            treeView1.NodeMouseClick += treeView1_NodeMouseClick;
            // 
            // viewBox
            // 
            resources.ApplyResources(viewBox, "viewBox");
            viewBox.Name = "viewBox";
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
            // menuStrip1
            // 
            resources.ApplyResources(menuStrip1, "menuStrip1");
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, toolsToolStripMenuItem });
            menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            resources.ApplyResources(fileToolStripMenuItem, "fileToolStripMenuItem");
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            // 
            // openToolStripMenuItem
            // 
            resources.ApplyResources(openToolStripMenuItem, "openToolStripMenuItem");
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(toolStripSeparator1, "toolStripSeparator1");
            toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // exitToolStripMenuItem
            // 
            resources.ApplyResources(exitToolStripMenuItem, "exitToolStripMenuItem");
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            resources.ApplyResources(toolsToolStripMenuItem, "toolsToolStripMenuItem");
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { contrastCorrectorToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            // 
            // contrastCorrectorToolStripMenuItem
            // 
            resources.ApplyResources(contrastCorrectorToolStripMenuItem, "contrastCorrectorToolStripMenuItem");
            contrastCorrectorToolStripMenuItem.Name = "contrastCorrectorToolStripMenuItem";
            contrastCorrectorToolStripMenuItem.Click += contrastCorrectorToolStripMenuItem_Click;
            // 
            // openFileDialog1
            // 
            resources.ApplyResources(openFileDialog1, "openFileDialog1");
            // 
            // statusStrip1
            // 
            resources.ApplyResources(statusStrip1, "statusStrip1");
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, toolStripProgressBar1 });
            statusStrip1.Name = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            resources.ApplyResources(toolStripStatusLabel1, "toolStripStatusLabel1");
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            // 
            // toolStripProgressBar1
            // 
            resources.ApplyResources(toolStripProgressBar1, "toolStripProgressBar1");
            toolStripProgressBar1.Name = "toolStripProgressBar1";
            // 
            // treeContextMenuStrip
            // 
            resources.ApplyResources(treeContextMenuStrip, "treeContextMenuStrip");
            treeContextMenuStrip.ImageScalingSize = new Size(24, 24);
            treeContextMenuStrip.Items.AddRange(new ToolStripItem[] { histogramToolStripMenuItem, aboutToolStripMenuItem, toolStripSeparator2, removeToolStripMenuItem });
            treeContextMenuStrip.Name = "treeContextMenuStrip";
            // 
            // histogramToolStripMenuItem
            // 
            resources.ApplyResources(histogramToolStripMenuItem, "histogramToolStripMenuItem");
            histogramToolStripMenuItem.Name = "histogramToolStripMenuItem";
            histogramToolStripMenuItem.Click += histogramToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            resources.ApplyResources(aboutToolStripMenuItem, "aboutToolStripMenuItem");
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(toolStripSeparator2, "toolStripSeparator2");
            toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // removeToolStripMenuItem
            // 
            resources.ApplyResources(removeToolStripMenuItem, "removeToolStripMenuItem");
            removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            removeToolStripMenuItem.Click += removeToolStripMenuItem_Click;
            // 
            // openFileBackgroundWorker
            // 
            openFileBackgroundWorker.WorkerReportsProgress = true;
            openFileBackgroundWorker.WorkerSupportsCancellation = true;
            openFileBackgroundWorker.DoWork += openFileBackgroundWorker_DoWork;
            openFileBackgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            openFileBackgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            // 
            // notifyIcon1
            // 
            resources.ApplyResources(notifyIcon1, "notifyIcon1");
            // 
            // saveFileDialog1
            // 
            resources.ApplyResources(saveFileDialog1, "saveFileDialog1");
            // 
            // contrastCorrectionBackgroundWorker
            // 
            contrastCorrectionBackgroundWorker.WorkerReportsProgress = true;
            contrastCorrectionBackgroundWorker.WorkerSupportsCancellation = true;
            contrastCorrectionBackgroundWorker.DoWork += contrastCorrectionBackgroundWorker_DoWork;
            contrastCorrectionBackgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            contrastCorrectionBackgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            // 
            // WorkSpace
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "WorkSpace";
            ShowIcon = false;
            FormClosing += WorkSpace_FormClosing;
            Load += WorkSpace_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)viewBox).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
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
        private SaveFileDialog saveFileDialog1;
        private System.ComponentModel.BackgroundWorker contrastCorrectionBackgroundWorker;
    }
}
