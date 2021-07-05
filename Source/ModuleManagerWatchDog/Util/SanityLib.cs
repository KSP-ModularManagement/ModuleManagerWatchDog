/*
	This file is part of Module Manager Watch Dog
	(C) 2020-21 Lisias T : http://lisias.net <support@lisias.net>

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

using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ModuleManagerWatchDog
{
	public static class SanityLib
	{
		private static string _EnforcedVersion;
		public static bool IsExempted(int kspMajor, int kspMinor)
		{
			ConfigNode cn = GameDatabase.Instance.GetConfigNode("ModuleManagerWatchDog/WatchDoc.cfg");
			if (null != cn)
			{
				string value = cn.GetValue("EnforceRulesFromKSPVersion");
				_EnforcedVersion = string.IsNullOrEmpty(value) ? "1.8" : value;
			}

			try
			{
				string[] v = _EnforcedVersion.Split('.');
				return
					kspMajor <= Int16.Parse(v[0])
					&&
					kspMinor < Int16.Parse(v[1])
				;
			}
			catch (Exception e)
			{
				Log.error("CheckIsEnvorceable : {0}", e.ToString());
				return true;
			}
		}

		public static IEnumerable<AssemblyLoader.LoadedAssembly> FetchDllsByAssemblyName(string assemblyName)
		{
			return from a in AssemblyLoader.loadedAssemblies
					let ass = a.assembly
					where assemblyName == ass.GetName().Name
					orderby a.path ascending
					select a;
		}

		public static bool CheckIsOnGameData(string path, string filename = null)
		{
			string fullpath = Path.GetFullPath(path);
			string[] subpaths = Path.GetDirectoryName(fullpath).Split(Path.DirectorySeparatorChar);
			return "GameData" == subpaths[subpaths.Length-1] && (null == filename) ? true : filename == Path.GetFileName(fullpath);
		}

		public static string[] GetFromConfig(string nodeName, string valueName)
		{
			ConfigNode cn = GameDatabase.Instance.GetConfigNode("ModuleManagerWatchDog/WatchDoc.cfg");
			if (null == cn) return new string[]{};

			cn = cn.GetNode("WatchDog");
			if (null == cn) return new string[]{};

			cn = cn.GetNode(nodeName);
			if (null == cn) return new string[]{};

			return cn.GetValues(valueName);
		}
	}
}
