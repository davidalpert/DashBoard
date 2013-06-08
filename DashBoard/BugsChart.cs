using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace DashBoard
{
	public partial class BugsChart : Form
	{
		private readonly IEnumerable<Bug> bugs;
		private int barWidth=10;
		private int barHeight=15;
		private int xBar=500;
		private double minutesPerDay=60*24;
		private int numberOfDays;
		private Dictionary<Bug, Color> colors;

		public BugsChart(IEnumerable<Bug> bugs)
		{
			this.bugs = bugs.ToArray();
			var wheel = new Wheel();
			wheel.AddRange(new[] { Color.FromArgb(255, 240, 240, 240), Color.FromArgb(255, 230, 230, 230) });
			colors = this.bugs.ToDictionary(b => b, b => wheel.Next());
			InitializeComponent();
			numberOfDays = 60;//this.bugs.Max(b => b.day + 2);
			Width= ((barWidth+1)*(numberOfDays+2)) + 10;
			Height = xBar + 10;
			FormBorderStyle = FormBorderStyle.None;
			BackColor = Color.White;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			DrawBackgrounds(e.Graphics);
			DrawBugs(e.Graphics);
		}

		private void DrawBugs(Graphics g)
		{
			for (int i = 0; i < numberOfDays; i++)
			{
				int day = i;
				var today = bugs.Where(b => b.IsOpenOnDay(i) && b.LastDay != i).ToArray();
				var tomorrow = bugs.Where(b => b.IsOpenOnDay(i+1)).ToArray();
				var lines = (from b in today.Reverse()
				select new 
					{
						X1 = GetStartXForDay(day),
						Y1 = GetYPositionFor(b, today, day),
						X2 = GetStartXForDay(day + 1),
						Y2 = GetYPositionFor(b, tomorrow, day + 1),
            LastDay = b.IsLastDay(day+1),
						Bug = b
					}).ToArray();
				foreach (var line in lines.Where(l => !l.LastDay))
				{
					int grey = 240;
					//var pen = new Pen(Color.FromArgb(255,grey,grey,grey), 15);
					var pen = new Pen(colors[line.Bug],15);
					g.DrawLine(pen,line.X1, line.Y1, line.X2, line.Y2);
				}
				foreach (var line in lines)
				{
					if (line.LastDay)
					{
						g.DrawString("x",new Font("courier",12,FontStyle.Bold),
							Brushes.Red,(float)line.X1-8,(float)line.Y1-11 );
					}
					else
					{
						var pen = new Pen(Color.Black, 1);
						g.DrawLine(pen, line.X1, line.Y1, line.X2, line.Y2);
					
					}
				}
			}

		}

		private int GetYPositionFor(Bug bug, Bug[] bugs, int day)
		{
			return (xBar - (GetPosition(bugs,bug,day) * barHeight) - (barHeight/2));
		}

		private int GetPosition(IEnumerable<Bug> tomorrow, Bug bug, int day)
		{
			var i = 0;
			foreach (var tomorrowBug in tomorrow)
			{
				 if (tomorrowBug.LastDay != day)
				 {
					 i++;
				 }
				if (tomorrowBug == bug)
				{
					return i;
				}
			}
			return -1;
		}

		private void DrawBackgrounds(Graphics g)
		{
			for (int day = 0; day < numberOfDays+1; day++)
			{
				DrawBackground(g, day);
			}
			int startX = GetStartXForDay(0);
			Pen black = new Pen(Color.Black,3);
			g.DrawLine(black, startX,5,startX,xBar);
			g.DrawLine(black, startX,xBar,Width,xBar);
		}
		private void DrawBackground(Graphics g, int day)
		{
			int startX = GetStartXForDay(day);
				g.FillRectangle(Brushes.LightGray, startX, 0, barWidth, xBar);
		}
		private int GetStartXForDay(int day)
		{
			return barWidth + (day * (barWidth + 1));
		}

	}

	public class Wheel : List<Color>
	{
		private int i = 0;
		public Color Next()
		{
			return this[i++%this.Count()];
		}
	}
}
