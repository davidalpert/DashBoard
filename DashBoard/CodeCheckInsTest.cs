using System;
using System.Diagnostics;
using ApprovalTests.Reporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DashBoard
{
	[TestClass]
	public class CodeCheckInsTest
	{
		[TestMethod]
		public void TestCheckins()
		{
			var checkins = new[] { new CheckIn(1, 9, 30, 10)
				, new CheckIn(1, 14, 30, 10)
				, new CheckIn(1, 19, 30, 100)
				, new CheckIn(2, 10, 0, 500) }; 

			ApprovalTests.WinForms.WinFormsApprovals.Verify(new CheckInChart(checkins));

		}
		
	}

	public class CheckIn
	{
		public readonly int day;
		public readonly int linesOfCode;
		public int minute;

		public CheckIn(int day, int hour, int minute, int linesOfCode)
		{
			this.day = day;
			this.linesOfCode = linesOfCode;
			this.minute = hour*60 + minute;
		}
	}
	public class Meeting
	{
		public readonly int day;
		public readonly int Length;
		public int minute;

		public Meeting(int day, int hour, int minute, int length, int numberOfPeople)
		{
			this.day = day;
			this.Length = length;
			this.minute = hour*60 + minute;
		}
	}
}
