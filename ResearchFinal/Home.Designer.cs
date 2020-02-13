namespace ResearchFinal
{
    partial class Home
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            this.pnlBoard = new System.Windows.Forms.Panel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.cotrolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoRunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.highScoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.highScoreToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.resetHighScoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblTimer = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBoard
            // 
            this.pnlBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBoard.Location = new System.Drawing.Point(0, 24);
            this.pnlBoard.Name = "pnlBoard";
            this.pnlBoard.Size = new System.Drawing.Size(1027, 637);
            this.pnlBoard.TabIndex = 0;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cotrolToolStripMenuItem,
            this.highScoreToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1027, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // cotrolToolStripMenuItem
            // 
            this.cotrolToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGameToolStripMenuItem,
            this.startGameToolStripMenuItem,
            this.autoRunToolStripMenuItem});
            this.cotrolToolStripMenuItem.Name = "cotrolToolStripMenuItem";
            this.cotrolToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.cotrolToolStripMenuItem.Text = "Cotrol";
            // 
            // newGameToolStripMenuItem
            // 
            this.newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            this.newGameToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newGameToolStripMenuItem.Text = "New game";
            this.newGameToolStripMenuItem.Click += new System.EventHandler(this.newGameToolStripMenuItem_Click);
            // 
            // startGameToolStripMenuItem
            // 
            this.startGameToolStripMenuItem.Name = "startGameToolStripMenuItem";
            this.startGameToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.startGameToolStripMenuItem.Text = "Start";
            // 
            // autoRunToolStripMenuItem
            // 
            this.autoRunToolStripMenuItem.Name = "autoRunToolStripMenuItem";
            this.autoRunToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.autoRunToolStripMenuItem.Text = "Auto";
            // 
            // highScoreToolStripMenuItem
            // 
            this.highScoreToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.highScoreToolStripMenuItem1,
            this.resetHighScoreToolStripMenuItem});
            this.highScoreToolStripMenuItem.Name = "highScoreToolStripMenuItem";
            this.highScoreToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.highScoreToolStripMenuItem.Text = "Score";
            // 
            // highScoreToolStripMenuItem1
            // 
            this.highScoreToolStripMenuItem1.Name = "highScoreToolStripMenuItem1";
            this.highScoreToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.highScoreToolStripMenuItem1.Text = "High score";
            this.highScoreToolStripMenuItem1.Click += new System.EventHandler(this.highScoreToolStripMenuItem_Click);
            // 
            // resetHighScoreToolStripMenuItem
            // 
            this.resetHighScoreToolStripMenuItem.Name = "resetHighScoreToolStripMenuItem";
            this.resetHighScoreToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.resetHighScoreToolStripMenuItem.Text = "Reset high score";
            this.resetHighScoreToolStripMenuItem.Click += new System.EventHandler(this.resetHighScoreToolStripMenuItem_Click);
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimer.Location = new System.Drawing.Point(500, 6);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(43, 15);
            this.lblTimer.TabIndex = 2;
            this.lblTimer.Text = "00:00";
            // 
            // Home
            // 
            this.ClientSize = new System.Drawing.Size(1027, 661);
            this.Controls.Add(this.lblTimer);
            this.Controls.Add(this.pnlBoard);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Home";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel pnlBoard;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem cotrolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoRunToolStripMenuItem;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.ToolStripMenuItem highScoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem highScoreToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem resetHighScoreToolStripMenuItem;
    }
}