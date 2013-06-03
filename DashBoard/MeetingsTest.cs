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
			var meetings = new[] { 
				new Meeting(1, 8, 0, 10, 5),
				new Meeting(2, 8, 0, 10, 5),
				new Meeting(3, 8, 0, 10, 5),
				new Meeting(4, 8, 0, 10, 5),
				new Meeting(5, 8, 0, 10, 5),
				new Meeting(1, 13, 0, 60, 3),
				new Meeting(5, 10, 30, 60, 3) }; 

			ApprovalTests.WinForms.WinFormsApprovals.Verify(new MeetingsChart(meetings));

		}
	}
}