using System;
using System.Collections.Generic;
using System.Linq;
using ApprovalTests.WinForms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DashBoard
{
	[TestClass]
	public class BugTest
	{
		private Random random = new Random(4);

		[TestMethod]
		public void TestEmptyBugChart()
		{
			WinFormsApprovals.Verify(new BugsChart(new Bug[0]));
		}

		[TestMethod]
		public void TestBugChart()
		{
			var bugs = Enumerable.Range(0, 60).SelectMany(i => GenerateBugs(i));
			WinFormsApprovals.Verify(new BugsChart(bugs));
		}

		[TestMethod]
		public void TestHunterBugChart()
		{
			var bugs = Enumerable.Range(0, 60).SelectMany(i => GenerateHunterBugs(i));
			WinFormsApprovals.Verify(new BugsChart(bugs));
		}

		private IEnumerable<Bug> GenerateHunterBugs(int i)
		{
			int bugs = random.Next(5) - 3;
			for (int j = 0; j < bugs; j++)
			{
				yield return new Bug(i, 1);
			}
		}

		private IEnumerable<Bug> GenerateBugs(int i)
		{
			int bugs = random.Next(4) - 1;
			for (int j = 0; j < bugs; j++)
			{
				yield return new Bug(i, random.Next(70));
			}
		}
	}

	public class Bug
	{
		public readonly int day;
		private readonly int duration;

		public Bug(int day, int duration)
		{
			this.day = day;
			this.duration = duration;
		}

		public int LastDay
		{
			get { return day + duration; }
		}

		public bool IsOpenOnDay(int i)
		{
			return day <= i && i <= LastDay;
		}
	}
}