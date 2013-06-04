using System.IO;
using ApprovalTests.Core;
using ApprovalTests.Reporters;

namespace DashBoard
{
	public class MyImageReporter: IEnvironmentAwareReporter
	{
		public void Report(string approved, string received)
		{
			ClipboardReporter.INSTANCE.Report(approved, received);
			var a = new FileInfo(approved);
			if (a.Exists && a.Length > 50)
			{
				TortoiseImageDiffReporter.INSTANCE.Report(approved,received);
			}
			else
			{
				FileLauncherReporter.INSTANCE.Report(approved,received);
			}

		}
			
		public bool IsWorkingInThisEnvironment(string forFile)
		{
			return TortoiseImageDiffReporter.INSTANCE.IsWorkingInThisEnvironment(forFile);
		}
	}
}