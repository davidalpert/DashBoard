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
		private int numberOfDays = numberOfWeeks*7;
		protected double TeamSize;
		private static int numberOfWeeks = 2*4;

		public MeetingsChart(IEnumerable<Meeting> meetings, int teamSize)
		{
			this.meetings = meetings;
			this.TeamSize = teamSize;
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
				Brush black = GetSemiTransparentBlack();
				g.FillRectangle(black, startX + difference, ScaleHeight(meeting.minute), width, ScaleHeightLength(meeting.Length));
			}
		}

		public static SolidBrush GetSemiTransparentBlack()
		{
			return new SolidBrush(Color.FromArgb(190,Color.Black));
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

		public int GetTotalManHours()
		{
			return meetings.Sum(m => m.Length*m.NumberOfPeople)/60;
		}

		public int GetPercentOfWeek()
		{
			return  (int) Math.Round(GetTotalManHours()*100.0/(TeamSize *40 * numberOfWeeks));
		}
	}
}
