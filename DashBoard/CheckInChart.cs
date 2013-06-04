using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DashBoard
{
	public partial class CheckInChart : Form
	{
		private readonly IEnumerable<CheckIn> checkins;
		private int barWidth=4;
		private int barHeight=600;
		private double minutesPerDay=60*24;
		private int numberOfDays = 8*4*7;

		public CheckInChart(IEnumerable<CheckIn> checkins)
		{
			this.checkins = checkins;
			InitializeComponent();
			this.Width= (barWidth+1)*numberOfDays;
			this.Height = barHeight;
			FormBorderStyle = FormBorderStyle.None;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			DrawBackgrounds(e.Graphics);
			for (int i = 0; i < numberOfDays; i++)
			{
				PaintDay(e.Graphics, i);
			}
		}

		private void DrawBackgrounds(Graphics g)
		{
			for (int day = 0; day < numberOfDays; day++)
			{
				DrawBackground(g, day);
			}
		}
		private void DrawBackground(Graphics g, int day)
		{
			int startX = GetStartXForDay(day);
			if (Meeting.IsWeekend(day))
			{
				g.FillRectangle(Brushes.DarkGray, startX, 0, barWidth, barHeight);
				g.FillRectangle(Brushes.Firebrick, startX, ScaleHeight(8 * 60), barWidth, ScaleHeight((9+5) * 60));
				
			}
			else
			{
				g.FillRectangle(Brushes.DarkGray, startX, 0, barWidth, barHeight);
				g.FillRectangle(Brushes.LightGray, startX, ScaleHeight(8 * 60), barWidth, ScaleHeight(9 * 60));
				g.FillRectangle(Brushes.Firebrick, startX, ScaleHeight(17 * 60), barWidth, ScaleHeight(5 * 60));
		
			}
				}

		
		private void PaintDay(Graphics g, int day)
		{
			int startX = GetStartXForDay(day);
			foreach (var checkin in checkins.Where(x=>x.day == day))
			{
				DrawCircle(g,startX + (barWidth/2), ScaleHeight(checkin.minute),ScaleLinesOfCode(checkin.linesOfCode));
			}
		}

		private int GetStartXForDay(int day)
		{
			return day*(barWidth +1);
		}

		private int ScaleLinesOfCode(int linesOfCode)
		{
			return (int) Math.Log10(linesOfCode*barWidth/Math.E)*3;
		}

		

		private void DrawCircle(Graphics g, int x, int y, int radius)
		{
			g.FillEllipse(MeetingsChart.GetSemiTransparentBlack(),x-radius, y-radius,radius*2, radius*2);
		}

		private int ScaleHeight(int minute)
		{
			double percent = (minute/minutesPerDay);
			return (int) (percent*barHeight);
		}
	}
}
