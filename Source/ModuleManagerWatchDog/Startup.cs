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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ModuleManagerWatchDog
{
	[KSPAddon(KSPAddon.Startup.Instantly, true)]
	internal class Startup : MonoBehaviour
	{
		private void Start()
		{
			Log.force("Version {0}", Version.Text);

			try
			{
				// Always check for being the unique Assembly loaded. This will avoid problems in the future.
				String msg = CheckMyself();

				if (null == msg)
					msg = CheckModuleManager();

				if (null == msg && SanityLib.IsEnforceable(1, 8))
					 msg = CheckModuleManager18();

				if (null == msg && SanityLib.IsEnforceable(1, 12))
					 msg = CheckModuleManager112();

				if ( null != msg )
					GUI.ShowStopperAlertBox.Show(msg);
			}
			catch (Exception e)
			{
				Log.error(e.ToString());
				GUI.ShowStopperAlertBox.Show(e.ToString());
			}
		}

		private const string ASSEMBLY_NAME		= "ModuleManager";

		private string CheckMyself()
		{
			IEnumerable<AssemblyLoader.LoadedAssembly> loaded = SanityLib.FetchLoadedAssembliesByName(this.GetType().Namespace);

			// Obviously, would be pointless to check for it not being installed! (0 == count). :)
			if (1 != loaded.Count()) return "There're more than one MM Watch Dog on this KSP installment! Please delete all but the one you intend to use!";
			if (!SanityLib.CheckIsOnGameData(loaded.First().path))
				return "666_ModuleManagerWatchDog.dll <b>must be</b> directly on GameData and not inside any subfolder (i.e., it must be in the same place ModuleManager.dll is). Please move 666_ModuleManagerWatchDog.dll directly into GameData.";
			return null;
		}

		private string CheckModuleManager()
		{
			IEnumerable<AssemblyLoader.LoadedAssembly> loaded = SanityLib.FetchLoadedAssembliesByName(ASSEMBLY_NAME);

			if (0 == loaded.Count()) return "There's no Module Manager on this KSP installment! You need to install Module Manager!";
			if (!SanityLib.CheckIsOnGameData(loaded.First().path))
				return "ModuleManager.dll <b>must be</b> directly on GameData and not inside any subfolder. Please move ModuleManager.dll directly into GameData.";
			return null;
		}

		private string CheckModuleManager18()
		{
			IEnumerable<System.Reflection.Assembly> loaded = SanityLib.FetchAssembliesByName(ASSEMBLY_NAME);

			if (1 != loaded.Count()) return "There're more than one Module Manager on this KSP installment! Please delete all but the one you intend to use!";
			return null;
		}

		// KSP 1.12 makes my life harder with this new way of loading Assemblies electing by version.
		// This will make badly installed DDLs a bit more dificult to diagnose.
		// Now I have to detected if MM/L is installed together Canonical MM by brute force.
		// If no MM/L is installed, I will let it go as is.
		private string CheckModuleManager112()
		{
			IEnumerable<System.Reflection.Assembly> loaded = SanityLib.FetchAssembliesByName(ASSEMBLY_NAME);
			System.Reflection.Assembly assembly = loaded.First();
			AssemblyTitleAttribute attributes = (AssemblyTitleAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute), false);
			string assemblyTittle = attributes.Title ?? "";
			if (
				(System.IO.File.Exists("GameData/ModuleManager.dll") && !assembly.Location.EndsWith("GameData/ModuleManager.dll"))
				||
				(assemblyTittle.StartsWith("Module Manager /L") && !assembly.Location.EndsWith("/ModuleManager.dll"))
				||
				(assembly.Location.EndsWith("/ModuleManager.dll") && !assemblyTittle.StartsWith("Module Manager /L"))
				)
				return "There're conflicting Module Manager versions on your instalment! You need to choose one version and remove the other(s)!";
			return null;
		}
	}
}
