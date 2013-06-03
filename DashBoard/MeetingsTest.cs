using System.Collections.Generic;
using System.Linq;
using ApprovalTests.Reporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DashBoard
{
	[TestClass]
	public class MeetingsTest
	{

		[TestMethod]
		public void TestMeetings()
		{
			var meetings = WeekDays().Select(i => new Meeting(i, 8, 0, 10, 5)).Concat(new[]
				{
					new Meeting(1, 13, 0, 60, 3),
					new Meeting(5, 10, 30, 30, 20),
					new Meeting(8, 9, 0, 90, 10)
				});
			ApprovalTests.WinForms.WinFormsApprovals.Verify(new MeetingsChart(meetings));
		}

		private static IEnumerable<int> WeekDays()
		{
			return Enumerable.Range(0,4*7*2).Where(i => !Meeting.IsWeekend(i));
		}
	}
}