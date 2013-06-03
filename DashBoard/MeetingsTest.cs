using ApprovalTests.Reporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DashBoard
{
	[TestClass]
	[UseReporter(typeof(DiffReporter))]
	public class MeetingsTest
	{
	
		[TestMethod]
		public void TestMeetings()
		{
			var checkins = new[] { new Meeting(1, 9, 30, 10, 5)
				, new Meeting(1, 8, 30, 10, 6)
				, new Meeting(1, 19, 30, 60, 3) }; 

			//ApprovalTests.WinForms.WinFormsApprovals.Verify(new CheckInChart(checkins));

		}
	}
}