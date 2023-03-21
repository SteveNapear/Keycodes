namespace Keycodes {
	partial class Form1 {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.mMenu = new System.Windows.Forms.MenuStrip();
            this.MenuPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.HexBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFont = new System.Windows.Forms.ToolStripMenuItem();
            this.PrintersCB = new System.Windows.Forms.ToolStripComboBox();
            this.MenuQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.mPanel = new System.Windows.Forms.Panel();
            this.mMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mMenu
            // 
            this.mMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuPreview,
            this.HexBtn,
            this.MenuFont,
            this.PrintersCB,
            this.MenuQuit});
            this.mMenu.Location = new System.Drawing.Point(0, 0);
            this.mMenu.Name = "mMenu";
            this.mMenu.ShowItemToolTips = true;
            this.mMenu.Size = new System.Drawing.Size(607, 31);
            this.mMenu.TabIndex = 0;
            // 
            // MenuPreview
            // 
            this.MenuPreview.Name = "MenuPreview";
            this.MenuPreview.Size = new System.Drawing.Size(60, 27);
            this.MenuPreview.Text = "Preview";
            this.MenuPreview.ToolTipText = "Print preview";
            this.MenuPreview.Click += new System.EventHandler(this.OnPrintPreview);
            // 
            // HexBtn
            // 
            this.HexBtn.AutoSize = false;
            this.HexBtn.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.HexBtn.Name = "HexBtn";
            this.HexBtn.Size = new System.Drawing.Size(70, 27);
            this.HexBtn.Text = "Hex";
            this.HexBtn.ToolTipText = "Switch between Hex and Decimal";
            this.HexBtn.Click += new System.EventHandler(this.OnHexBtn_Click);
            // 
            // MenuFont
            // 
            this.MenuFont.Name = "MenuFont";
            this.MenuFont.Size = new System.Drawing.Size(43, 27);
            this.MenuFont.Text = "Font";
            this.MenuFont.ToolTipText = "Select font and color";
            this.MenuFont.Click += new System.EventHandler(this.OnNewFont);
            // 
            // PrintersCB
            // 
            this.PrintersCB.AutoSize = false;
            this.PrintersCB.BackColor = System.Drawing.Color.Navy;
            this.PrintersCB.DropDownWidth = 200;
            this.PrintersCB.ForeColor = System.Drawing.Color.White;
            this.PrintersCB.Name = "PrintersCB";
            this.PrintersCB.Size = new System.Drawing.Size(200, 23);
            this.PrintersCB.ToolTipText = "List of available printers";
            // 
            // MenuQuit
            // 
            this.MenuQuit.Name = "MenuQuit";
            this.MenuQuit.Size = new System.Drawing.Size(42, 27);
            this.MenuQuit.Text = "Quit";
            this.MenuQuit.ToolTipText = "Exit program";
            this.MenuQuit.Click += new System.EventHandler(this.OnQuit);
            // 
            // mPanel
            // 
            this.mPanel.Location = new System.Drawing.Point(61, 71);
            this.mPanel.Name = "mPanel";
            this.mPanel.Size = new System.Drawing.Size(484, 248);
            this.mPanel.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(607, 370);
            this.Controls.Add(this.mPanel);
            this.Controls.Add(this.mMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mMenu;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Keyboard codes:";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.OnLoad);
            this.mMenu.ResumeLayout(false);
            this.mMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mMenu;
		private System.Windows.Forms.Panel mPanel;
		private System.Windows.Forms.ToolStripMenuItem MenuFont;
		private System.Windows.Forms.ToolStripMenuItem MenuQuit;
		private System.Windows.Forms.ToolStripComboBox PrintersCB;
		private System.Windows.Forms.ToolStripMenuItem MenuPreview;
        private System.Windows.Forms.ToolStripMenuItem HexBtn;
	}
}

