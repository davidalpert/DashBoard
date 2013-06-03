using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ApprovalTests.Reporters;
using ApprovalTests.WinForms;
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

		[TestMethod]
		public void GenerateHunter()
		{
			var start = new DateTime(2013, 1, 1);
			string csvFile = @"C:\code\DashBoard\DashBoard\checkins.csv";
        string[] lines = File.ReadAllLines(csvFile);
			var checkins = lines.Select(l => l.Split(',').ToArray())
				.Select(p => new {FileCount = int.Parse(p[0]), Date = DateTime.Parse(p[1])})
				.Select(c => new CheckIn((c.Date - start).Days, c.Date.Hour, c.Date.Minute,c.FileCount));

			WinFormsApprovals.Verify(new CheckInChart(checkins));
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
		public int NumberOfPeople { get; set; }
		public readonly int day;
		public readonly int Length;
		public int minute;

		public Meeting(int day, int hour, int minute, int length, int numberOfPeople)
		{
			NumberOfPeople = numberOfPeople;
			this.day = day;
			this.Length = length;
			this.minute = hour*60 + minute;
		}

		public static bool IsWeekend(int day)
		{
			int dayOfWeek = day%7;
			return (dayOfWeek == 0 || dayOfWeek == 6);
		}
	}
}
