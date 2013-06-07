using System;
using System.Collections.Generic;
using System.Linq;
using ApprovalTests.WinForms;
using ApprovalUtilities.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DashBoard
{
	[TestClass]
	public class MeetingsTest
	{
		[TestMethod]
		public void TestEmptyMeetings()
		{
			WinFormsApprovals.Verify(new MeetingsChart(new Meeting[0], 10));
		}

		[TestMethod]
		public void TestMeetings()
		{
			var meetings = WeekDays().Select(i => new Meeting(i, 8, 0, 10, 5)).Concat(new[]
				{
					new Meeting(1, 13, 0, 60, 3),
					new Meeting(5, 10, 30, 30, 20),
					new Meeting(8, 9, 0, 90, 10)
				});
			WinFormsApprovals.Verify(new MeetingsChart(meetings, 10));
		}

		[TestMethod]
		public void HunterMeetings()
		{
			var meetings = Mondays()
				.Select(i => new Meeting(i, 13, 15, 20, 2))
				.Concat(new[] {new Meeting(4*7 - 3, 13, 0, 60, 5), new Meeting(8*7 - 3, 13, 0, 60, 5)});
			var chart = new MeetingsChart(meetings, 5);
			Console.WriteLine("Total Man Hours:{0}".FormatWith(chart.GetTotalManHours()));
			Console.WriteLine("Percent Of Week:{0}".FormatWith(chart.GetPercentOfWeek()));
			WinFormsApprovals.Verify(chart);
		}

		private static IEnumerable<int> RandomTwiceAWeek()
		{
			return Enumerable.Range(0, 4*7*2).Where(i => !Meeting.IsWeekend(i));
		}

		private static IEnumerable<int> LastDayOfMonths()
		{
			return Enumerable.Range(0, 4*7*2).Where(i => i%30 == 1);
		}

		private static IEnumerable<int> Mondays()
		{
			return Enumerable.Range(0, 4*7*2).Where(i => i%7 == 1);
		}

		private static IEnumerable<int> WeekDays()
		{
			return Enumerable.Range(0, 4*7*2).Where(i => !Meeting.IsWeekend(i));
		}
	}
}