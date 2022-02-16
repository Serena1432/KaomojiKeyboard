namespace KaomojiKeyboard
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showKeyboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.applicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.emojisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.emojiManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshEmojiDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openEmojiDataFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openEmojiDataFileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.showEmojiDataFileLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetEmojiDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(0, 23);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(772, 239);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "KaomojiKeyboard";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showKeyboardToolStripMenuItem,
            this.exitToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(166, 48);
            // 
            // showKeyboardToolStripMenuItem
            // 
            this.showKeyboardToolStripMenuItem.Name = "showKeyboardToolStripMenuItem";
            this.showKeyboardToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.showKeyboardToolStripMenuItem.Text = "Show Keyboard...";
            this.showKeyboardToolStripMenuItem.Click += new System.EventHandler(this.showKeyboardToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(165, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applicationToolStripMenuItem,
            this.emojisToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(772, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // applicationToolStripMenuItem
            // 
            this.applicationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.applicationToolStripMenuItem.Name = "applicationToolStripMenuItem";
            this.applicationToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.applicationToolStripMenuItem.Text = "Application";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.optionsToolStripMenuItem.Text = "Settings...";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // emojisToolStripMenuItem
            // 
            this.emojisToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.emojiManagerToolStripMenuItem,
            this.refreshEmojiDataToolStripMenuItem,
            this.openEmojiDataFileToolStripMenuItem,
            this.resetEmojiDataToolStripMenuItem});
            this.emojisToolStripMenuItem.Name = "emojisToolStripMenuItem";
            this.emojisToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.emojisToolStripMenuItem.Text = "Emojis";
            // 
            // emojiManagerToolStripMenuItem
            // 
            this.emojiManagerToolStripMenuItem.Name = "emojiManagerToolStripMenuItem";
            this.emojiManagerToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.emojiManagerToolStripMenuItem.Text = "Emoji Manager...";
            this.emojiManagerToolStripMenuItem.Click += new System.EventHandler(this.emojiManagerToolStripMenuItem_Click);
            // 
            // refreshEmojiDataToolStripMenuItem
            // 
            this.refreshEmojiDataToolStripMenuItem.Name = "refreshEmojiDataToolStripMenuItem";
            this.refreshEmojiDataToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.refreshEmojiDataToolStripMenuItem.Text = "Refresh Emoji Data...";
            this.refreshEmojiDataToolStripMenuItem.Click += new System.EventHandler(this.refreshEmojiDataToolStripMenuItem_Click);
            // 
            // openEmojiDataFileToolStripMenuItem
            // 
            this.openEmojiDataFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openEmojiDataFileToolStripMenuItem1,
            this.showEmojiDataFileLocationToolStripMenuItem});
            this.openEmojiDataFileToolStripMenuItem.Name = "openEmojiDataFileToolStripMenuItem";
            this.openEmojiDataFileToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.openEmojiDataFileToolStripMenuItem.Text = "Emoji Data File...";
            // 
            // openEmojiDataFileToolStripMenuItem1
            // 
            this.openEmojiDataFileToolStripMenuItem1.Name = "openEmojiDataFileToolStripMenuItem1";
            this.openEmojiDataFileToolStripMenuItem1.Size = new System.Drawing.Size(242, 22);
            this.openEmojiDataFileToolStripMenuItem1.Text = "Open Emoji Data File...";
            this.openEmojiDataFileToolStripMenuItem1.Click += new System.EventHandler(this.openEmojiDataFileToolStripMenuItem1_Click);
            // 
            // showEmojiDataFileLocationToolStripMenuItem
            // 
            this.showEmojiDataFileLocationToolStripMenuItem.Name = "showEmojiDataFileLocationToolStripMenuItem";
            this.showEmojiDataFileLocationToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.showEmojiDataFileLocationToolStripMenuItem.Text = "Show Emoji Data File Location...";
            this.showEmojiDataFileLocationToolStripMenuItem.Click += new System.EventHandler(this.showEmojiDataFileLocationToolStripMenuItem_Click);
            // 
            // resetEmojiDataToolStripMenuItem
            // 
            this.resetEmojiDataToolStripMenuItem.Name = "resetEmojiDataToolStripMenuItem";
            this.resetEmojiDataToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.resetEmojiDataToolStripMenuItem.Text = "Reset Emoji Data...";
            this.resetEmojiDataToolStripMenuItem.Click += new System.EventHandler(this.resetEmojiDataToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.helpToolStripMenuItem.Text = "About";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 262);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kaomoji Keyboard v1.0.0 - made by Meir in 2022";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showKeyboardToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem applicationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem emojisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshEmojiDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openEmojiDataFileToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem resetEmojiDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem emojiManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openEmojiDataFileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem showEmojiDataFileLocationToolStripMenuItem;
    }
}

