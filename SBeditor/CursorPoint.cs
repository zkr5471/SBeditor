using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SBeditor
{
	public partial class Form1 : Form
	{
		struct CursorPoint
		{
			public int X, Y;

			private List<StringBuilder> source
			{
				get
				{
					return Form1.Instance.SourceData;
				}
			}

			private StringBuilder cur_line
			{
				get
				{
					return source[Y];
				}
			}

			public CursorPoint(int x, int y)
			{
				X = x;
				Y = y;
			}

			public void Next()
			{
				X++;
				if (X > cur_line.Length)
				{
					if (Y < source.Count() - 1)
					{
						X = 0;
						Y++;
					}
					else
					{
						X--;
					}
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
						X = cur_line.Length;
					}
					else
					{
						X = Y = 0;
					}
				}
			}

			public void Up()
			{
				if (Y == 0)
				{
					X = 0;
					return;
				}
				else
				{
					Y--;
					int len = cur_line.Length;
					if (X > len) X = len;
				}
			}

			public void Down()
			{
				Y++;

				if (Y >= source.Count())
				{
					Y--;
					X = cur_line.Length;
					return;
				}

				int len = cur_line.Length;
				if (X > len) X = len;
			}
		}
	}
}