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

		public Form1()
		{
			InitializeComponent();

			_data = new List<StringBuilder>();
			_bitmap = new Bitmap(MaxBitmapSize.Width, MaxBitmapSize.Height);

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

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{

		}

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{

		}

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{

		}
	}
}