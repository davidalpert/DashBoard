using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DashBoard
{
	public partial class MeetingsChart : Form
	{
		private readonly Meeting[] meetings;
		private int barWidth=10;
		private int barHeight=600;
		private double minutesPerDay=60*24;
		private int numberOfDays = 2*4*7;

		public MeetingsChart(Meeting[] meetings)
		{
			this.meetings = meetings;
			InitializeComponent();
			this.Width= (barWidth+1)*numberOfDays;
			this.Height = barHeight + 60;
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
			if (IsWeekend(day))
			{
				g.FillRectangle(Brushes.DarkGray, startX, 0, barWidth, barHeight);
				g.FillRectangle(Brushes.Firebrick, startX, ScaleHeight(8 * 60), barWidth, ScaleHeight((9) * 60));
			}
			else
			{
				g.FillRectangle(Brushes.DarkGray, startX, 0, barWidth, barHeight);
				g.FillRectangle(Brushes.LightGray, startX, ScaleHeight(8 * 60), barWidth, ScaleHeight(9 * 60));
			}
				}

		private bool IsWeekend(int day)
		{
			int dayOfWeek = day%7;
			return (dayOfWeek == 0 || dayOfWeek == 6);
		}

		private void PaintDay(Graphics g, int day)
		{
			int startX = GetStartXForDay(day);
			foreach (var meeting in meetings.Where(x=>x.day == day))
			{
				g.FillRectangle(Brushes.Black,startX, ScaleHeight(meeting.minute),barWidth, meeting.Length);
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


		private int ScaleHeight(int minute)
		{
			double percent = (minute/minutesPerDay);
			return (int) (percent*barHeight);
		}
	}
}
