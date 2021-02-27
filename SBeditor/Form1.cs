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

		struct CursorPoint
		{
			public int X, Y;

			public CursorPoint(int x, int y)
			{
				X = x;
				Y = y;
			}

			public void Next()
			{
				X++;
				if (X > Form1.Instance.SourceData[Y].Length)
				{
					X = 0;
					Y++;
				}
			}

			public void Back()
			{
				X--;
				if (X < 0)
				{
					if (Y > 0)
					{
						Y--;
						X = Form1.Instance.SourceData[Y].Length;
					}
					else
					{
						X = Y = 0;
					}
				}
			}
		}

		public List<StringBuilder> SourceData;
		public Bitmap EditorBitmap;

		readonly Size MaxBitmapSize = new Size(1000, 600);
		readonly Size CharSize = new Size(10, 12);

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
			Font font = new Font("MeiryoKe_Console", 10);

			int draw_Y = 0;

			gra.Clear(Color.FromArgb(30, 30, 30));

			foreach (var line in SourceData)
			{
				gra.DrawString(line.ToString(), font, Brushes.White, 0, draw_Y);

				draw_Y += CharSize.Height;
			}

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
			
		}

		private void Form1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if( CheckKeyChar(e.KeyChar))
			{
				InsertString(CursorPos, e.KeyChar.ToString());

				CursorPos.Next();

				Render();
			}
		}
	}
}