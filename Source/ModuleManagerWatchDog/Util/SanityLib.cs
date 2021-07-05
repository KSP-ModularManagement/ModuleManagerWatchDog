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
		public static IEnumerable<AssemblyLoader.LoadedAssembly> FetchDllsByAssemblyName(string assemblyName)
		{
			return from a in AssemblyLoader.loadedAssemblies
					let ass = a.assembly
					where assemblyName == ass.GetName().Name
					orderby a.path ascending
					select a;
		}

		public static bool CheckIsOnGameData(string path)
		{
			string[] subpaths = Path.GetDirectoryName(Path.GetFullPath(path)).Split(Path.PathSeparator);
			return "GameData" == subpaths[subpaths.Length-1];
		}
	}
}
