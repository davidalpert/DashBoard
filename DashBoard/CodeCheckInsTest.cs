using System;
using System.Collections.Generic;
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
		private Random random = new Random(2000);

		[TestMethod]
		public void TestCheckins()
		{
			var checkins = new[] { new CheckIn(1, 9, 30, 10)
				, new CheckIn(1, 14, 30, 10)
				, new CheckIn(1, 19, 30, 100)
				, new CheckIn(2, 10, 0, 500) }; 

			WinFormsApprovals.Verify(new CheckInChart(checkins));

		}

		[TestMethod]
		public void GenerateOther()
		{
			var checkins = Enumerable.Range(0, 8*4*7)
			                         .SelectMany(c => GenerateForDay(c));
			WinFormsApprovals.Verify(new CheckInChart(checkins));
		}
		private IEnumerable<CheckIn> GenerateForDay(int day)
		{
			var number = random.Next(Meeting.IsWeekend(day) ? Meeting.IsEveryOtherWeekend(day)?0:4: 3);
			for (int i = 0; i < number; i++)
			{
			  Func<int> daytime = () => random.Next(9, 17); 
			  Func<int> evening = () => random.Next(17, 20);
			  Func<int> night = () => random.Next(20, 24);
				Func<int> time = daytime;
				int dice = random.Next(100);
				if ( 90 < dice)
				{
					time = night;
				}
				else if (75 < dice)
				{
					time = evening;
				}

				var hour = time();

				yield return new CheckIn(day, hour , random.Next(60), random.Next(10,1000));
			}
		}

		[TestMethod]
		public void GenerateHunter()
		{
			var start = new DateTime(2012, 12, 30);
			string csvFile = @"C:\code\DashBoard\DashBoard\checkins.csv";
      var checkins = ProcessCsvFile(csvFile)
				.Select(p => new {FileCount = int.Parse(p[0]), Date = DateTime.Parse(p[1])})
				.Select(c => new CheckIn((c.Date - start).Days, c.Date.Hour, c.Date.Minute,c.FileCount));

			WinFormsApprovals.Verify(new CheckInChart(checkins));
		}



		public static IEnumerable<string[]> ProcessCsvFile(string csvFile)
		{
			return File.ReadAllLines(csvFile).Select(l => l.Split(',').ToArray());
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
			return (new []{0,6}.Contains(dayOfWeek));
		}
		public static bool IsEveryOtherWeekend(int day)
		{
			int dayOfWeek = day%14;
			return (new []{0,13}.Contains(dayOfWeek));
		}
	}
}
