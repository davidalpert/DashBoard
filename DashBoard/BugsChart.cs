using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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
			wheel.AddRange(Enumerable.Range(0, 20).Select(i => Color.FromArgb(215, 150 + i * 5,20, 0)));
			colors = this.bugs.ToDictionary(b => b, b => wheel.Next());
			InitializeComponent();
			numberOfDays = this.bugs.Max(b => b.day + 2);
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
				var today = bugs.Where(b => b.IsOpenOnDay(i) && b.LastDay != i).ToArray();
				var tomorrow = bugs.Where(b => b.IsOpenOnDay(i+1)).ToArray();
				foreach(var b in today)
				{
					var start = GetYPositionFor(b, today,i);
					var end = GetYPositionFor(b, tomorrow, i + 1);
					
					var pen = new Pen(colors[b], 5);
					g.DrawLine(pen, GetStartXForDay(i),start,GetStartXForDay(i+1),end);
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
