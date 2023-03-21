using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using SNL;

namespace Keycodes {
	//////////////////////////////////////////////////////////////////////
	public partial class Form1 : Form {
#if DEBUG
		public bool bDebug = true;
#else
        public bool bDebug = false;
#endif

		public Rectangle rWA;
		public string sFF = "Courier New";
		public string sVF = "Times New Roman";
		public Font stdF = null;
		public Font TestF = null;
		public Color color = Color.Black;
		private const string userRoot = "HKEY_CURRENT_USER";
		private string subkey = Application.ProductName;
		public bool bBold = false;
		public bool bItalic = false;
		public bool bHex = false;
		private string keyName;
		public PrinterSettings PS = null;
		public StringFormat sfc = new StringFormat();
		//initialization
		private CSN_Asm Info = null;
		private CSN_Security Security = null;

		//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		public Form1() /*Constructor*/		{
			int ix; FontStyle FS = new FontStyle();

			Security = new CSN_Security();
			Info = new CSN_Asm(Assembly.GetExecutingAssembly());
			InitializeComponent();

			keyName = userRoot + "\\" + subkey;
			string fontname = (string)Registry.GetValue(keyName, "font", sVF);
			if (fontname != null) {
				ix = (int)Registry.GetValue(keyName, "FontBold", 0);
				bBold = (ix > 0);
				ix = (int)Registry.GetValue(keyName, "FontItalic", 0);
				bItalic = (ix > 0);
			} else fontname = "Times New Roman";
			FS = (bBold ? FontStyle.Bold : FontStyle.Regular) | (bItalic ? FontStyle.Italic : FontStyle.Regular);
			stdF = new Font(fontname, 20, FS, GraphicsUnit.Pixel);
			sfc.Alignment = StringAlignment.Center;
			sfc.LineAlignment = StringAlignment.Near;
			TestF = new Font(FontFamily.GenericSerif, 20, FontStyle.Italic);
		}
		//================================================================
		private void OnLoad(object sender, EventArgs e) //setup image
		{
			float y, wd, ht, em; string st;

			rWA = this.ClientRectangle;
			em = rWA.Height / 50.0f;
			stdF = new Font(sVF, em, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Pixel);
			mMenu.Font = stdF;
			this.Font = stdF;

			ht = stdF.Height * 1.5f;
			wd = rWA.Width;
			mMenu.SetBounds(0, 0, rWA.Width, (int)ht);

			SetupPrinters();

			ht = (rWA.Height - mMenu.Height);
			y = mMenu.Bottom;
			mPanel.SetBounds(0, (int)y, rWA.Width, (int)ht);
			mPanel.BackColor = Color.White;
			mPanel.Paint += new PaintEventHandler(OnP_Paint);
			st = Info.sTitle + " version " + Info.sVersion +
			", " + Info.sCopyright +
			 (Info.Is64 ? " --64-- " : " --32-- ");
			if (bDebug) st += " ***DEBUG***";
			this.Text = st;
		}
		//================================================================
		private void SetupPrinters()    //setup the Printer Combobox
		{
			int ix, idft = -1;

			PrintDialog dlg = new PrintDialog();
			string sdft = dlg.PrinterSettings.PrinterName;

			PrintersCB.BeginUpdate();
			PrintersCB.Items.Clear();
			for (ix = 0; ix < PrinterSettings.InstalledPrinters.Count; ix++) {
				string sP = PrinterSettings.InstalledPrinters[ix];
				PrintersCB.Items.Add(sP);
				if (sP == sdft) idft = ix;
			}
			PrintersCB.SelectedIndex = (idft < 0) ? 0 : idft;
			PrintersCB.EndUpdate();
			PS = dlg.PrinterSettings;
			dlg.Dispose();
		}
		//================================================================
		private Font GenFont(string fontname, float em, bool bBld, bool bIt)    //Generate a font based on parameters
		{
			Font font = null; string se = "";
			bool bnone = !(bBld | bIt);
			string fn = fontname;
			FontStyle fs = (bBld ? FontStyle.Bold : FontStyle.Regular) | (bIt ? FontStyle.Italic : FontStyle.Regular);
			try { font = new Font(fontname, em, fs, GraphicsUnit.Pixel); } catch (Exception exp) {
				se = "Problem encountered while generating font:\n" +
					"name: " + fontname + ",em = " + em.ToString("f2") +
					", Bold = " + ((bBld) ? 'T' : 'F') + ", Italic = " + ((bIt) ? 'T' : 'F') +
					"\nError: " + exp.Message;
				MessageBox.Show(se);
				return null;
			}
			return font;
		}
		//================================================================
		void OnP_Paint(object sender, PaintEventArgs e) //draw panel
		{
			Panel P = sender as Panel;
			if (P == null) return;

			DrawKeycodes(e.Graphics, P.ClientRectangle);
		}
		//================================================================
		private void OnQuit(object sender, EventArgs e) {
			int ix = 0;
			if (stdF != null) {
				Registry.SetValue(keyName, "font", stdF.Name);
				ix = bBold ? 1 : 0; Registry.SetValue(keyName, "FontBold", ix);
				ix = bItalic ? 1 : 0; Registry.SetValue(keyName, "FontItalic", ix);
			}
			Close();
		}
		//================================================================
		private void OnNewFont(object sender, EventArgs e) {
			FontDialog dlg = new FontDialog();
			dlg.ShowApply = false;
			dlg.ShowColor = true;
			dlg.ShowEffects = true;
			dlg.AllowScriptChange = true;
			dlg.AllowSimulations = false;
			dlg.AllowVectorFonts = false;
			dlg.AllowVerticalFonts = false;
			dlg.FontMustExist = true;
			if (stdF != null) dlg.Font = new Font(stdF, stdF.Style);
			dlg.Color = color;
			if (dlg.ShowDialog() != DialogResult.OK) { dlg.Dispose(); return; }
			Font fnt = dlg.Font;
			stdF = new Font(dlg.Font, dlg.Font.Style);
			bBold = stdF.Bold;
			bItalic = stdF.Italic;
			color = dlg.Color;
			mPanel.Invalidate();
			dlg.Dispose();
		}
		//================================================================
		private void OnPrintPreview(object sender, EventArgs e) {
			PrintPreviewDialog dlg = new PrintPreviewDialog();

			PrintDocument doc = new PrintDocument();
			doc.DocumentName = "Keycode page";
			PS.DefaultPageSettings.Landscape = true;
			doc.PrinterSettings = PS;
			dlg.Document = doc;

			Form dlgform = dlg.FindForm();
			dlgform.WindowState = FormWindowState.Maximized;
			dlgform.Text = "To print on the " + PS.PrinterName + ", click on the printer icon on the far left.";
			dlgform.Icon = this.Icon;
			doc.PrintPage += new PrintPageEventHandler(PrintPage);
			DialogResult dr = dlg.ShowDialog();
			doc.Dispose();
			dlg.Dispose();
		}
		//================================================================
		void PrintPage(object sender, PrintPageEventArgs e) {
			PrintDocument doc = sender as PrintDocument;
			bool bPreview = doc.PrintController.IsPreview;
			e.HasMorePages = false;
			RectangleF rA = e.MarginBounds;
			SizeF hm = new SizeF(e.PageSettings.HardMarginX, e.PageSettings.HardMarginY);
			if (!bPreview) { rA.X -= hm.Width; rA.Y -= hm.Height; }
			Graphics g = e.Graphics;
			DrawKeycodes(g, rA);
		}
		//================================================================
		private void DrawKeycodes(Graphics g, RectangleF rA) {
			int row, col, v, nc = 16; char c; string  st, ss, sXD;
			StringFormat sf = new StringFormat();
			float lft, top, mid, x, y, dx, dy, em;

			dx = rA.Width / (2 + nc);
			dy = rA.Height / (2 + nc);

			HexBtn.Text = bHex ? "Hex" : "Dec";
			sXD = bHex ? "X" : "D";

			SolidBrush br = new SolidBrush(color);
			Pen pen = new Pen(br);
			em = rA.Height / 25.0f;
			if (em < 10.0f) return;

			Font font = GenFont(stdF.Name, em, bBold, bItalic);
			Font nfont = GenFont(sFF, em * 0.9f, true, false);
			Font smfont = GenFont(sFF, em * 0.35f, false, false);
			pen.Width = 0.0f;
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Center;

			lft = rA.X + (rA.Width - nc * dx) / 2;
			mid = rA.X + rA.Width / 2;
			top = rA.Y + (rA.Height - nc * dy) / 2;
			pen.Color = color;
			pen.Width = 2.0f;
			y = rA.Y + nfont.Height / 2;
			for (col = 0; col <= nc; col++) {   //draw vertical lines
				x = lft + col * dx;
				g.DrawLine(Pens.Black, x, top, x, top + nc * dy);
				if (col < nc) g.DrawString(col.ToString(sXD), nfont, Brushes.Black, x + dx / 2, y, sf);
			}

			sf.LineAlignment = StringAlignment.Far;
			for (row = 0; row <= nc; row++) {       //draw horizontal lines
				st = (row * nc).ToString(sXD);
				y = top + row * dy;
				if (row < nc) g.DrawString(st, nfont, Brushes.Black, rA.X + dx / 2, y + dy, sf);
				g.DrawLine(Pens.Black, lft, y, rA.X + rA.Width - dx, y);
			}

			sf.LineAlignment = StringAlignment.Center;
			sf.Alignment = StringAlignment.Center;
			Pen pen1 = new Pen(Brushes.Black);
			pen1.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
			pen.Width = 0.0f;
			for (row = 0; row < nc; row++) {
				for (col = 0; col < nc; col++) {
					ss = "";
					v = row * nc + col;
					c = (char)v;
					//get specials
					if (char.IsWhiteSpace(c)) ss += "ws";
					if (char.IsControl(c)) ss += "ctrl";
					if (char.IsSymbol(c)) ss += "sym";
					if (char.IsPunctuation(c)) ss += "punc";
					if (char.IsSeparator(c)) ss += "sep";
					if (char.IsHighSurrogate(c)) ss += "hs";
					if (char.IsLowSurrogate(c)) ss += "ls";
					st = c.ToString();
					x = rA.X + dx + col * dx;
					y = rA.Y + dy + row * dy;
					if (char.IsWhiteSpace(c)) {
						RectangleF r = new RectangleF(x, y, dx, dy);
						//						g.FillRectangle(Brushes.LightGray, r);
					}
					g.DrawLine(pen1, x + dx / 2, y, x + dx / 2, y + dy);
					g.DrawLine(pen1, x, y + dy / 2, x + dx, y + dy / 2);
					g.DrawString(st, font, br, x + dx / 2, y + dy / 2, sf);
					if (ss.Length > 0) g.DrawString(ss, smfont, Brushes.Black, x + dx / 2, y + dy - smfont.Height / 2, sf);
				}
			}
			pen1.Dispose();

			//add font name to top of page if room
			if (rA.Y > 0) {
				Font tfont = new Font("Times New Roman",em,FontStyle.Italic,GraphicsUnit.Pixel);
				sf.Alignment = StringAlignment.Center;
				st = "Keycodes using " + font.Name + " font";
				g.DrawString(st, tfont, Brushes.Black, mid, rA.Y - font.Height, sf);
				g.DrawString(Info.sCopyright, tfont, Brushes.Black, mid, rA.Bottom, sf);
				tfont.Dispose();
			}

			if (stdF != null) stdF.Dispose(); stdF = font;
			sf.Dispose();
			pen.Dispose();
			br.Dispose();
			font.Dispose();
			nfont.Dispose();
			smfont.Dispose();
		}
		//================================================================
		private void OnHexBtn_Click(object sender, EventArgs e) {
			bHex = !bHex;
			mPanel.Invalidate();
		}
	}
}
