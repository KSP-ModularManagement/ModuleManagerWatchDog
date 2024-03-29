/*
	This file is part of Module Manager Watch Dog
		©2020-2024 Lisias T : http://lisias.net <support@lisias.net>

	Module Manager Watch Dog is licensed as follows:
		* SKL 1.0 : https://ksp.lisias.net/SKL-1_0.txt

	Module Manager Watchdog is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

	You should have received a copy of the SKL Standard License 1.0
	along with Module Manager Watch Dog. If not, see
	<https://ksp.lisias.net/SKL-1_0.txt>.

*/
using System;
using SIO = System.IO;

namespace WatchDog.ModuleManager
{
	public static class SelfCleaning
	{
		private const string DELETEME = ".delete-me";
		private static readonly string GAMEDATA = SanityLib.GetPathFor("GameData");

		internal static bool CheckUninstalled(string directory, string dllName)
		{
			string dirpathname = SIO.Path.Combine(GAMEDATA, directory);
			string dllpathname = SIO.Path.Combine(GAMEDATA, dllName);
			return SIO.File.Exists(dllpathname) && !SIO.Directory.Exists(dirpathname);
		}

		internal static bool KillThis(string dllName)
		{
			string pathname = SIO.Path.Combine(GAMEDATA, dllName);
			bool r = SIO.File.Exists(pathname);
			if (r) Delete(pathname);
			else
			{
				string tempname = pathname + DELETEME;
				if (SIO.File.Exists(tempname)) SIO.File.Delete(tempname);
			}
			return r;
		}

		private static void Delete(string filename)
		{
			Log.dbg("Deleting {0}", filename);
			if (SIO.File.Exists(filename)) try
			{
				SIO.File.Delete(filename);
			}
			catch (Exception e) when (e is System.UnauthorizedAccessException || e is System.Security.SecurityException)
			{
				// Oukey, we are in Windows and it locks the DLL file once it's loaded.
				// But we can rename it, and delete it later.
				string tempname = filename + DELETEME;
				if (SIO.File.Exists(tempname)) SIO.File.Delete(tempname);
				SIO.File.Move(filename, tempname);
			}
		}

	}
}
