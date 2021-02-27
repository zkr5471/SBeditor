using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SBeditor
{
	public partial class Form1 : Form
	{
		public static Form1 Instance
		{
			get;
			private set;
		}

		public List<StringBuilder> SourceData;
		public Bitmap EditorBitmap;

		readonly Size MaxBitmapSize = new Size(1000, 600);
		readonly Size CharSize = new Size(7, 12);
		readonly string FontName = "MeiryoKe_Console";

		bool IsMouseDown = false;
		CursorPoint CursorPos;

		public Form1()
		{
			InitializeComponent();

			SourceData = new List<StringBuilder>();
			EditorBitmap = new Bitmap(MaxBitmapSize.Width, MaxBitmapSize.Height);

			SourceData.Add(new StringBuilder());

			Instance = this;
		}

		private void Form1_Shown(object sender, EventArgs e)
		{
			Render();
		}

		private void Form1_Resize(object sender, EventArgs e)
		{

		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				Openfile(openFileDialog1.FileName);
				Render();
			}
		}

		private void Render()
		{
			Graphics gra = Graphics.FromImage(EditorBitmap);
			Font font = new Font(FontName, 10);

			int draw_Y = 0;

			gra.Clear(Color.FromArgb(30, 30, 30));

			foreach (var line in SourceData)
			{
				int draw_X = 0;

				foreach(char ch in line.ToString())
				{
					gra.DrawString(ch.ToString(), font, Brushes.White, draw_X, draw_Y);
					draw_X += CharSize.Width;
				}

				draw_Y += CharSize.Height;
			}

			gra.FillRectangle(Brushes.White, CursorPos.X * CharSize.Width, CursorPos.Y * CharSize.Height, 2, CharSize.Height);

			pictureBox1.Image = EditorBitmap;

			font.Dispose();
			gra.Dispose();
		}

		private void Openfile(string path)
		{
			SourceData.Clear();

			using (var file = new StreamReader(path))
			{
				string line;
				while ((line = file.ReadLine()) != null)
				{
					SourceData.Add(new StringBuilder(line));
				}
			}
		}

		private void InsertString(CursorPoint pos, string str)
		{
			if (str == string.Empty)
				return;

			if (pos.Y < 0 || pos.Y >= SourceData.Count())
			{
				return;
			}

			if (pos.X < 0 || pos.X > SourceData[pos.Y].Length)
			{
				return;
			}

			var list = new List<string>(str.Split('\n'));
			list[0] = SourceData[pos.Y].ToString().Substring(0, pos.X) + list[0];
			list[list.Count() - 1] += SourceData[pos.Y].ToString().Substring(pos.X);

			var listSB = new List<StringBuilder>();
			foreach (var i in list)
				listSB.Add(new StringBuilder(i));

			SourceData.RemoveAt(pos.Y);
			SourceData.InsertRange(pos.Y, listSB);
		}

		bool CheckKeyChar(char c)
		{
			string str = "!@#$%^&*()_+~`-={}[]\\|;':\"<>,./? ";

			if (Char.IsDigit(c))
				return true;

			if (Char.IsLetter(c))
				return true;

			if (str.IndexOf(c) >= 0)
				return true;

			return false;
		}



		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			IsMouseDown = true;


		}

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{

		}

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
			IsMouseDown = false;


		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Up:
				{
					CursorPos.Up();
					break;
				}

				case Keys.Down:
				{
					CursorPos.Down();
					break;
				}

				case Keys.Left:
				{
					CursorPos.Back();
					break;
				}

				case Keys.Right:
				{
					CursorPos.Next();
					break;
				}
			}

			Render();
		//	MessageBox.Show($"{CursorPos.X}, {CursorPos.Y}");
		}

		private void Form1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (CheckKeyChar(e.KeyChar))
			{
				InsertString(CursorPos, e.KeyChar.ToString());

				CursorPos.Next();

				Render();
			}
		}
	}
}