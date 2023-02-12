/*
	This file is part of Module Manager Watch Dog
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
using System;

using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ModuleManagerWatchDog
{
	public static class SanityLib
	{
		public static bool IsEnforceable(int kspMajor, int kspMinor)
		{
			ConfigNode cn = GameDatabase.Instance.GetConfigNode("ModuleManagerWatchDog/WatchDog");
			if (null == cn) return false;

			int versionMajor = Versioning.version_major;
			int versionMinor = Versioning.version_minor;;
			{ 
				string versionOverride = cn.GetValue("ForceRulesAsItWas");
				if (null != versionOverride) try
				{
					string[] v = versionOverride.Split('.');
					versionMajor = Int16.Parse(v[0]);
					versionMinor = Int16.Parse(v[1]);
				}
				catch
				{
					versionMajor = Versioning.version_major;
					versionMinor = Versioning.version_minor;
				}
			}

			bool r = false;
			string[] enforcedRules = cn.GetValues("EnforceRulesFor");
			foreach (string ev in enforcedRules) try
			{
				string[] v = ev.Split('.');
				r =
					versionMajor >= kspMajor
					&&
					versionMinor >= kspMinor
					&&
					kspMajor == Int16.Parse(v[0])
					&&
					kspMinor == Int16.Parse(v[1])
				;
				if (r) break;
			}
			catch (Exception e)
			{
				Log.error("IsEnforceable : {0}. Rules are being enforced.", e.ToString());
				return true;
			}

			Log.detail("Current version {0}.{1} is{2}subject to the rules for {3}.{4}."
					, Versioning.version_major, Versioning.version_minor
					, r ? " " : " not "
					, kspMajor, kspMinor
				);
			return r;
		}

		/**
		 * If you need to fetch Assemblies being loaded or not (i.e., including the ones that KSP
		 * didn't managed to finish the loading by faulty dependencies), you need to use this one.
		 */
		public static IEnumerable<System.Reflection.Assembly> FetchAssembliesByName(string assemblyName)
		{
			return from a in System.AppDomain.CurrentDomain.GetAssemblies()
					where assemblyName == a.GetName().Name
					orderby a.Location ascending
					select a
				;
		}

		/**
		 * If you are interested only on assemblies that were properly loaded by KSP, this is the one you want.
		 */
		public static IEnumerable<AssemblyLoader.LoadedAssembly> FetchLoadedAssembliesByName(string assemblyName)
		{ 
			return from a in AssemblyLoader.loadedAssemblies
					let ass = a.assembly
					where assemblyName == ass.GetName().Name
					orderby a.path ascending
					select a
				;
		}

		public static bool CheckIsOnGameData(string pathname, string filename = null)
		{
			string fullpath = Path.GetFullPath(pathname);
			string directory = Path.GetDirectoryName(fullpath);
			string gamedata = Path.GetFullPath(GetPathFor("GameData"));
			return directory.Equals(gamedata)
				&& ((null == filename) || filename == Path.GetFileName(fullpath));
		}

		public static string[] GetFromConfig(string nodeName, string valueName)
		{
			nodeName = nodeName.Replace("_", "");
			ConfigNode cn = GameDatabase.Instance.GetConfigNode("ModuleManagerWatchDog/Plugins/"+nodeName+"/"+nodeName);
			if (null == cn) return new string[]{};
			if ("WatchDog" != cn.name) return new string[]{};
			return cn.GetValues(valueName);
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
