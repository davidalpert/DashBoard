using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DashBoard
{
	public partial class MeetingsChart : Form
	{
		private readonly IEnumerable<Meeting> meetings;
		private readonly int startOfDay = 7*60;
		private readonly int endOfDay = 18*60;
		private int barWidth=10;
		private int barHeight=600;
		private double minutesPerDay=60*24;
		private int numberOfDays = 2*4*7;
		protected double TeamSize = 10;

		public MeetingsChart(IEnumerable<Meeting> meetings)
		{
			this.meetings = meetings;
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
				g.FillRectangle(Brushes.Firebrick, startX, ScaleHeight(8 * 60), barWidth, ScaleHeightLength((9) * 60));
			}
			else
			{
				g.FillRectangle(Brushes.DarkGray, startX, 0, barWidth, barHeight);
				g.FillRectangle(Brushes.LightGray, startX, ScaleHeight(8 * 60), barWidth, ScaleHeightLength(9 * 60));
			}
				}

		private void PaintDay(Graphics g, int day)
		{
			int startX = GetStartXForDay(day);
			foreach (var meeting in meetings.Where(x=>x.day == day))
			{
				int width = ScaleWidth(meeting.NumberOfPeople);
				int difference = (barWidth - width)/2;
				g.FillRectangle(Brushes.Black, startX + difference, ScaleHeight(meeting.minute), width, ScaleHeightLength(meeting.Length));
			}
		}

		

		private int GetStartXForDay(int day)
		{
			return day*(barWidth +1);
		}

		private int ScaleWidth(int people)
		{
			double percent = (people / TeamSize);
			return (int)(percent * barWidth);
		}


		private int ScaleHeightLength(int minutes)
		{
			return ScaleHeight(startOfDay + minutes);
		}

		private int ScaleHeight(int minute)
		{
			int adjustMin = minute - startOfDay;
			double adjustTotalDay = endOfDay - startOfDay;
			double percent = (adjustMin/adjustTotalDay);
			return (int) (percent*barHeight);
		}
	}
}
