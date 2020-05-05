
namespace CountDownk_Nload
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码
        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClearSelect = new System.Windows.Forms.Button();
            this.pbMain = new System.Windows.Forms.ProgressBar();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.clistDirs = new System.Windows.Forms.CheckedListBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listLost = new System.Windows.Forms.ListBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.listBrowse = new System.Windows.Forms.ListBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.eventLog = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.AutoScroll = true;
            this.splitContainer3.Panel1.Controls.Add(this.btnSave);
            this.splitContainer3.Panel1.Controls.Add(this.btnClearSelect);
            this.splitContainer3.Panel1.Controls.Add(this.pbMain);
            this.splitContainer3.Panel1.Controls.Add(this.btnSelect);
            this.splitContainer3.Panel1.Controls.Add(this.btnClear);
            this.splitContainer3.Panel1.Controls.Add(this.btnAll);
            this.splitContainer3.Panel1.Controls.Add(this.btnOpen);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.clistDirs);
            this.splitContainer3.Size = new System.Drawing.Size(266, 450);
            this.splitContainer3.SplitterDistance = 110;
            this.splitContainer3.TabIndex = 6;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 70);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(77, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "保存文本";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnClearSelect
            // 
            this.btnClearSelect.Location = new System.Drawing.Point(95, 41);
            this.btnClearSelect.Name = "btnClearSelect";
            this.btnClearSelect.Size = new System.Drawing.Size(77, 23);
            this.btnClearSelect.TabIndex = 14;
            this.btnClearSelect.Text = "取消选择";
            this.btnClearSelect.UseVisualStyleBackColor = true;
            this.btnClearSelect.Click += new System.EventHandler(this.BtnClearSelect_Click);
            // 
            // pbMain
            // 
            this.pbMain.Location = new System.Drawing.Point(95, 70);
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(160, 23);
            this.pbMain.TabIndex = 13;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(12, 41);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(77, 23);
            this.btnSelect.TabIndex = 12;
            this.btnSelect.Text = "条件选择";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(178, 12);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(77, 23);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(95, 12);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(77, 23);
            this.btnAll.TabIndex = 10;
            this.btnAll.Text = "全选";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.BtnAll_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(77, 23);
            this.btnOpen.TabIndex = 9;
            this.btnOpen.Text = "打开";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // clistDirs
            // 
            this.clistDirs.CheckOnClick = true;
            this.clistDirs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clistDirs.HorizontalScrollbar = true;
            this.clistDirs.Location = new System.Drawing.Point(0, 0);
            this.clistDirs.Name = "clistDirs";
            this.clistDirs.Size = new System.Drawing.Size(266, 336);
            this.clistDirs.Sorted = true;
            this.clistDirs.TabIndex = 7;
            this.toolTip.SetToolTip(this.clistDirs, "双击打开目录");
            this.clistDirs.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ClistDirs_ItemCheck);
            this.clistDirs.DoubleClick += new System.EventHandler(this.ClistDirs_DoubleClick);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listLost);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lblStatus);
            this.splitContainer2.Panel2.Controls.Add(this.listBrowse);
            this.splitContainer2.Size = new System.Drawing.Size(530, 450);
            this.splitContainer2.SplitterDistance = 118;
            this.splitContainer2.TabIndex = 4;
            // 
            // listLost
            // 
            this.listLost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listLost.FormattingEnabled = true;
            this.listLost.HorizontalScrollbar = true;
            this.listLost.ItemHeight = 12;
            this.listLost.Location = new System.Drawing.Point(0, 0);
            this.listLost.Name = "listLost";
            this.listLost.Size = new System.Drawing.Size(530, 118);
            this.listLost.Sorted = true;
            this.listLost.TabIndex = 3;
            this.listLost.SelectedIndexChanged += new System.EventHandler(this.ListLost_SelectedIndexChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(0, 316);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(29, 12);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "状态";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // listBrowse
            // 
            this.listBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBrowse.FormattingEnabled = true;
            this.listBrowse.HorizontalScrollbar = true;
            this.listBrowse.ItemHeight = 12;
            this.listBrowse.Location = new System.Drawing.Point(0, 0);
            this.listBrowse.Name = "listBrowse";
            this.listBrowse.Size = new System.Drawing.Size(530, 328);
            this.listBrowse.Sorted = true;
            this.listBrowse.TabIndex = 4;
            this.toolTip.SetToolTip(this.listBrowse, "双击打开图片");
            this.listBrowse.SelectedIndexChanged += new System.EventHandler(this.ListBrowse_SelectedIndexChanged);
            this.listBrowse.DoubleClick += new System.EventHandler(this.ClistDirs_DoubleClick);
            // 
            // eventLog
            // 
            this.eventLog.SynchronizingObject = this;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.Text = "=缺件检测器=";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.eventLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox listLost;
        private System.Windows.Forms.ListBox listBrowse;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.CheckedListBox clistDirs;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.ProgressBar pbMain;
        private System.Windows.Forms.Button btnClearSelect;
        public System.Diagnostics.EventLog eventLog;
        private System.Windows.Forms.Button btnSave;
    }
}

