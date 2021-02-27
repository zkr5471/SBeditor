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
		List<StringBuilder> _data;
		Bitmap _bitmap;

		readonly Size MaxBitmapSize = new Size(1000, 600);
		readonly Size CharSize = new Size(10, 12);

		bool IsMouseDown = false;
		Point CursorPos = new Point();

		public Form1()
		{
			InitializeComponent();

			_data = new List<StringBuilder>();
			_bitmap = new Bitmap(MaxBitmapSize.Width, MaxBitmapSize.Height);

			_data.Add(new StringBuilder());
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
			Graphics gra = Graphics.FromImage(_bitmap);
			Font font = new Font("MeiryoKe_Console", 10);

			int draw_Y = 0;

			gra.Clear(Color.FromArgb(30, 30, 30));

			foreach (var line in _data)
			{
				gra.DrawString(line.ToString(), font, Brushes.White, 0, draw_Y);

				draw_Y += CharSize.Height;
			}

			pictureBox1.Image = _bitmap;

			font.Dispose();
			gra.Dispose();
		}

		private void Openfile(string path)
		{
			_data.Clear();

			using (var file = new StreamReader(path))
			{
				string line;
				while ((line = file.ReadLine()) != null)
				{
					_data.Add(new StringBuilder(line));
				}
			}
		}

		private void InsertString(Point pos, string str)
		{
			if (str == string.Empty)
				return;

			if (pos.Y < 0 || pos.Y >= _data.Count())
			{
				return;
			}

			if (pos.X < 0 || pos.X > _data[pos.Y].Length)
			{
				return;
			}

			var list = new List<string>(str.Split('\n'));
			list[0] = _data[pos.Y].ToString().Substring(0, pos.X) + list[0];
			list[list.Count() - 1] += _data[pos.Y].ToString().Substring(pos.X);

			var listSB = new List<StringBuilder>();
			foreach (var i in list)
				listSB.Add(new StringBuilder(i));

			_data.RemoveAt(pos.Y);
			_data.InsertRange(pos.Y, listSB);
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

			Render();
		}
	}
}