/*
	This file is part of Watch Dog for PD-Launcher, a component from Module Manager Watch Dog
		©2020-2023 Lisias T : http://lisias.net <support@lisias.net>

	Module Manager Watch Dog is licensed as follows:
		* SKL 1.0 : https://ksp.lisias.net/SKL-1_0.txt

	Module Manager Watchdog is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

	You should have received a copy of the SKL Standard License 1.0
	along with Module Manager Watch Dog. If not, see
	<https://ksp.lisias.net/SKL-1_0.txt>.

*/
using System.IO;
using System.Diagnostics;

namespace WatchDog.PDLauncher
{
	public static class SanityLib
	{
		private static int _PdLauncherPID = -1;
		public static bool IsPdLauncherRunning
		{
			get
			{
				string target = PdLauncherPath.ToLower();
				if (!File.Exists(PdLauncherPath)) return false;
				foreach (Process p in Process.GetProcesses())
				{
					string path = p.MainModule.FileName.ToLower();
					if (!target.Equals(path)) continue;
					_PdLauncherPID = p.Id;
					return true;
				}
				_PdLauncherPID = -1;
				return false;
			}
		}

		private static string _PDLAUNCHERPATH = null;
		public static string PdLauncherPath
		{
			get
			{
				return _PDLAUNCHERPATH??(_PDLAUNCHERPATH=GetPathFor("PDLauncher", "LauncherPatcher.exe"));
			}
		}

		public static void KillPdLauncher()
		{
			if (-1 == _PdLauncherPID) throw new System.DllNotFoundException(_PDLAUNCHERPATH);
			Process.GetProcessById(_PdLauncherPID).Kill();
			Process.GetProcessById(_PdLauncherPID).WaitForExit(); // To be sure we will have enough RAM and VRAM for loading things on KSP.
			_PdLauncherPID = -1;
		}

		public static string GetPathFor(string path, params string[] paths)
		{
			string r = Path.GetFullPath(KSPUtil.ApplicationRootPath);
			r = Path.Combine(r, path);
			foreach (string p in paths)
				r = Path.Combine(r, p);
			return r;
		}
	}
}
