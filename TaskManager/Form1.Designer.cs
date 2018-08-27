namespace TaskManager
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.backupDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generatePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteOldDoneTasksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTaskByCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SUBJECT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SHORTDESCR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EXECUTOR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATEINIT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DEADLINE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MAKEDATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MAKETASK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.MANAGER = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COMMITDATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COMMITMAKE = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PRIORITY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATEDOC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NUMBERDOC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EXECUTORID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MANAGERID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripSeparator1,
            this.toolStripButton4,
            this.toolStripButton5,
            this.toolStripButton6,
            this.toolStripSplitButton1,
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1180, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(124, 22);
            this.toolStripButton1.Text = "НОВЕ ЗАВДАННЯ";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(86, 22);
            this.toolStripButton2.Text = "ОНОВИТИ";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(51, 22);
            this.toolStripButton3.Text = "ЗВІТ";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(111, 22);
            this.toolStripButton4.Text = "ВСІ ЗАВДАННЯ";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(88, 22);
            this.toolStripButton5.Text = "ВИКОНАНІ";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(103, 22);
            this.toolStripButton6.Text = "НЕВИКОНАНІ";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backupDBToolStripMenuItem,
            this.restoreDBToolStripMenuItem,
            this.generatePasswordToolStripMenuItem,
            this.deleteOldDoneTasksToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(75, 22);
            this.toolStripDropDownButton1.Text = "ADMIN";
            this.toolStripDropDownButton1.Visible = false;
            // 
            // backupDBToolStripMenuItem
            // 
            this.backupDBToolStripMenuItem.Name = "backupDBToolStripMenuItem";
            this.backupDBToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.backupDBToolStripMenuItem.Text = "Backup DB";
            // 
            // restoreDBToolStripMenuItem
            // 
            this.restoreDBToolStripMenuItem.Name = "restoreDBToolStripMenuItem";
            this.restoreDBToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.restoreDBToolStripMenuItem.Text = "Restore DB";
            // 
            // generatePasswordToolStripMenuItem
            // 
            this.generatePasswordToolStripMenuItem.Name = "generatePasswordToolStripMenuItem";
            this.generatePasswordToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.generatePasswordToolStripMenuItem.Text = "Generate passwords";
            this.generatePasswordToolStripMenuItem.Click += new System.EventHandler(this.generatePasswordToolStripMenuItem_Click);
            // 
            // deleteOldDoneTasksToolStripMenuItem
            // 
            this.deleteOldDoneTasksToolStripMenuItem.Name = "deleteOldDoneTasksToolStripMenuItem";
            this.deleteOldDoneTasksToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.deleteOldDoneTasksToolStripMenuItem.Text = "Delete old done tasks";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(1180, 679);
            this.splitContainer1.SplitterDistance = 606;
            this.splitContainer1.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.SUBJECT,
            this.SHORTDESCR,
            this.EXECUTOR,
            this.DATEINIT,
            this.DEADLINE,
            this.MAKEDATE,
            this.MAKETASK,
            this.MANAGER,
            this.COMMITDATE,
            this.COMMITMAKE,
            this.PRIORITY,
            this.DATEDOC,
            this.NUMBERDOC,
            this.EXECUTORID,
            this.MANAGERID});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1180, 606);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reloadToolStripMenuItem,
            this.allTaskToolStripMenuItem,
            this.doneToolStripMenuItem,
            this.undoneToolStripMenuItem,
            this.newTaskToolStripMenuItem,
            this.newTaskByCurrentToolStripMenuItem,
            this.reportToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(225, 180);
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.reloadToolStripMenuItem.Text = "Оновити";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // allTaskToolStripMenuItem
            // 
            this.allTaskToolStripMenuItem.Name = "allTaskToolStripMenuItem";
            this.allTaskToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.allTaskToolStripMenuItem.Text = "Всі завдання";
            this.allTaskToolStripMenuItem.Click += new System.EventHandler(this.allTaskToolStripMenuItem_Click);
            // 
            // doneToolStripMenuItem
            // 
            this.doneToolStripMenuItem.Name = "doneToolStripMenuItem";
            this.doneToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.doneToolStripMenuItem.Text = "Виконані";
            this.doneToolStripMenuItem.Click += new System.EventHandler(this.doneToolStripMenuItem_Click);
            // 
            // undoneToolStripMenuItem
            // 
            this.undoneToolStripMenuItem.Name = "undoneToolStripMenuItem";
            this.undoneToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.undoneToolStripMenuItem.Text = "Невиконані";
            this.undoneToolStripMenuItem.Click += new System.EventHandler(this.undoneToolStripMenuItem_Click);
            // 
            // newTaskToolStripMenuItem
            // 
            this.newTaskToolStripMenuItem.Name = "newTaskToolStripMenuItem";
            this.newTaskToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.newTaskToolStripMenuItem.Text = "Нове завдання";
            this.newTaskToolStripMenuItem.Click += new System.EventHandler(this.newTaskToolStripMenuItem_Click);
            // 
            // newTaskByCurrentToolStripMenuItem
            // 
            this.newTaskByCurrentToolStripMenuItem.Name = "newTaskByCurrentToolStripMenuItem";
            this.newTaskByCurrentToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.newTaskByCurrentToolStripMenuItem.Text = "Нове завдання з поточного";
            this.newTaskByCurrentToolStripMenuItem.Click += new System.EventHandler(this.newTaskByCurrentToolStripMenuItem_Click);
            // 
            // reportToolStripMenuItem
            // 
            this.reportToolStripMenuItem.Name = "reportToolStripMenuItem";
            this.reportToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.reportToolStripMenuItem.Text = "Звіт";
            this.reportToolStripMenuItem.Click += new System.EventHandler(this.reportToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.deleteToolStripMenuItem.Text = "Видалити поточний запис";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // ID
            // 
            this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Visible = false;
            this.ID.Width = 43;
            // 
            // SUBJECT
            // 
            this.SUBJECT.HeaderText = "ТЕМА";
            this.SUBJECT.MinimumWidth = 160;
            this.SUBJECT.Name = "SUBJECT";
            this.SUBJECT.Width = 160;
            // 
            // SHORTDESCR
            // 
            this.SHORTDESCR.HeaderText = "ОПИС ЗАДАЧІ";
            this.SHORTDESCR.MinimumWidth = 200;
            this.SHORTDESCR.Name = "SHORTDESCR";
            this.SHORTDESCR.Width = 200;
            // 
            // EXECUTOR
            // 
            this.EXECUTOR.HeaderText = "ВИКОНАВЕЦЬ";
            this.EXECUTOR.Name = "EXECUTOR";
            this.EXECUTOR.Width = 106;
            // 
            // DATEINIT
            // 
            dataGridViewCellStyle1.NullValue = null;
            this.DATEINIT.DefaultCellStyle = dataGridViewCellStyle1;
            this.DATEINIT.HeaderText = "ПОЧАТОК";
            this.DATEINIT.Name = "DATEINIT";
            this.DATEINIT.Width = 85;
            // 
            // DEADLINE
            // 
            dataGridViewCellStyle2.NullValue = null;
            this.DEADLINE.DefaultCellStyle = dataGridViewCellStyle2;
            this.DEADLINE.HeaderText = "ТЕРМІН ВИКОНАННЯ";
            this.DEADLINE.Name = "DEADLINE";
            this.DEADLINE.Width = 132;
            // 
            // MAKEDATE
            // 
            dataGridViewCellStyle3.NullValue = null;
            this.MAKEDATE.DefaultCellStyle = dataGridViewCellStyle3;
            this.MAKEDATE.HeaderText = "ДАТА ВИКОНАННЯ";
            this.MAKEDATE.Name = "MAKEDATE";
            this.MAKEDATE.ReadOnly = true;
            this.MAKEDATE.Width = 123;
            // 
            // MAKETASK
            // 
            this.MAKETASK.HeaderText = "ВИКОНАНО";
            this.MAKETASK.Name = "MAKETASK";
            this.MAKETASK.Width = 74;
            // 
            // MANAGER
            // 
            this.MANAGER.HeaderText = "ПОСТАНОВНИК ЗАДАЧІ";
            this.MANAGER.Name = "MANAGER";
            this.MANAGER.Width = 145;
            // 
            // COMMITDATE
            // 
            dataGridViewCellStyle4.NullValue = null;
            this.COMMITDATE.DefaultCellStyle = dataGridViewCellStyle4;
            this.COMMITDATE.HeaderText = "ДАТА ПІДТВЕРДЖЕННЯ ВИКОНАННЯ";
            this.COMMITDATE.Name = "COMMITDATE";
            this.COMMITDATE.Width = 214;
            // 
            // COMMITMAKE
            // 
            this.COMMITMAKE.HeaderText = "ПІДТВЕРДЖЕННЯ ВИКОНАННЯ";
            this.COMMITMAKE.Name = "COMMITMAKE";
            this.COMMITMAKE.Width = 166;
            // 
            // PRIORITY
            // 
            this.PRIORITY.HeaderText = "ПРІОРИТЕТ";
            this.PRIORITY.Name = "PRIORITY";
            this.PRIORITY.Width = 94;
            // 
            // DATEDOC
            // 
            dataGridViewCellStyle5.Format = "d";
            dataGridViewCellStyle5.NullValue = null;
            this.DATEDOC.DefaultCellStyle = dataGridViewCellStyle5;
            this.DATEDOC.HeaderText = "ДАТА СЛУЖБОВОЇ";
            this.DATEDOC.Name = "DATEDOC";
            this.DATEDOC.Width = 121;
            // 
            // NUMBERDOC
            // 
            this.NUMBERDOC.HeaderText = "НОМЕР СЛУЖБОВОЇ";
            this.NUMBERDOC.Name = "NUMBERDOC";
            this.NUMBERDOC.Width = 129;
            // 
            // EXECUTORID
            // 
            this.EXECUTORID.HeaderText = "EXECUTORID";
            this.EXECUTORID.Name = "EXECUTORID";
            this.EXECUTORID.ReadOnly = true;
            this.EXECUTORID.Visible = false;
            this.EXECUTORID.Width = 102;
            // 
            // MANAGERID
            // 
            this.MANAGERID.HeaderText = "MANAGERID";
            this.MANAGERID.Name = "MANAGERID";
            this.MANAGERID.ReadOnly = true;
            this.MANAGERID.Visible = false;
            this.MANAGERID.Width = 97;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 704);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TaskManager";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allTaskToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem doneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newTaskToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newTaskByCurrentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSplitButton1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem backupDBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreDBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generatePasswordToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripMenuItem deleteOldDoneTasksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SUBJECT;
        private System.Windows.Forms.DataGridViewTextBoxColumn SHORTDESCR;
        private System.Windows.Forms.DataGridViewTextBoxColumn EXECUTOR;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATEINIT;
        private System.Windows.Forms.DataGridViewTextBoxColumn DEADLINE;
        private System.Windows.Forms.DataGridViewTextBoxColumn MAKEDATE;
        private System.Windows.Forms.DataGridViewCheckBoxColumn MAKETASK;
        private System.Windows.Forms.DataGridViewTextBoxColumn MANAGER;
        private System.Windows.Forms.DataGridViewTextBoxColumn COMMITDATE;
        private System.Windows.Forms.DataGridViewCheckBoxColumn COMMITMAKE;
        private System.Windows.Forms.DataGridViewTextBoxColumn PRIORITY;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATEDOC;
        private System.Windows.Forms.DataGridViewTextBoxColumn NUMBERDOC;
        private System.Windows.Forms.DataGridViewTextBoxColumn EXECUTORID;
        private System.Windows.Forms.DataGridViewTextBoxColumn MANAGERID;
    }
}

