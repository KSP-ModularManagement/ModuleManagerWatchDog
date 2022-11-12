/*
	This file is part of Module Manager Watch Dog
		©2020-22 Lisias T : http://lisias.net <support@lisias.net>

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
					 msg = CheckModuleManagerDoppelganger18();

				if (null == msg && SanityLib.IsEnforceable(1, 12))
					 msg = CheckModuleManagerConflict112();

				if (null == msg && SanityLib.IsEnforceable(1, 12))
					 msg = CheckModuleManagerDoppelganger112();


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

#if DEBUG
			Log.dbg("CheckMyself");
			foreach (AssemblyLoader.LoadedAssembly la in loaded)
				Log.dbg("{0} :: {1}", la.assembly.FullName, la.assembly.Location);
#endif

			// Obviously, would be pointless to check for it not being installed! (0 == count). :)
			if (1 != loaded.Count()) return "There're more than one MM Watch Dog on this KSP installment! Please delete all but the one you intend to use!";
			if (!SanityLib.CheckIsOnGameData(loaded.First().path))
				return "666_ModuleManagerWatchDog.dll <b>must be</b> directly on GameData and not inside any subfolder (i.e., it must be in the same place ModuleManager.dll is). Please move 666_ModuleManagerWatchDog.dll directly into GameData.";
			return null;
		}

		private string CheckModuleManager()
		{
			IEnumerable<AssemblyLoader.LoadedAssembly> loaded = SanityLib.FetchLoadedAssembliesByName(ASSEMBLY_NAME);

#if DEBUG
			Log.dbg("CheckModuleManager");
			foreach (AssemblyLoader.LoadedAssembly la in loaded)
				Log.dbg("{0} :: {1}", la.assembly.FullName, la.assembly.Location);
#endif

			if (0 == loaded.Count()) return "There's no Module Manager on this KSP installment! You need to install Module Manager!";
			if (!SanityLib.CheckIsOnGameData(loaded.First().path))
				return "ModuleManager.dll <b>must be</b> directly on GameData and not inside any subfolder. Please move ModuleManager.dll directly into GameData.";
			return null;
		}

		private string CheckModuleManagerDoppelganger18()
		{
			Log.dbg("CheckModuleManagerDoppelganger18");

			IEnumerable<System.Reflection.Assembly> loaded = SanityLib.FetchAssembliesByName(ASSEMBLY_NAME);
#if DEBUG
			Log.dbg("CheckModuleManager18");
			foreach (System.Reflection.Assembly a in loaded)
				Log.dbg("{0} :: {1}", a.FullName, a.Location);
#endif
			if (1 != loaded.Count()) return "There're more than one Module Manager on this KSP installment! Please delete all but the one you intend to use!";
			return null;
		}

		// KSP 1.12 makes my life harder with this new way of loading Assemblies electing by version.
		//
		// This will make badly installed DDLs a bit more dificult to diagnose.
		//
		// Now I have to detected if MM/L is installed together Canonical MM by brute force.
		//
		// If no MM/L is installed, I will let it go as is.
		static readonly string GAMEDATAMMDLL = System.IO.Path.Combine("GameData", "ModuleManager.dll");
		static readonly string DASHMMDLL = GAMEDATAMMDLL.Replace("GameData", "");
		static readonly string FULLMMPATH = SanityLib.GetPathFor("GameData", "ModuleManager.dll");
		private string CheckModuleManagerConflict112()
		{
			IEnumerable<System.Reflection.Assembly> loaded = SanityLib.FetchAssembliesByName(ASSEMBLY_NAME);
#if DEBUG
			Log.dbg("CheckModuleManager112");
			foreach (System.Reflection.Assembly a in loaded)
				Log.dbg("{0} :: {1}", a.FullName, a.Location);
#endif
			Assembly assembly = loaded.First();
			AssemblyTitleAttribute attributes = (AssemblyTitleAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute), false);
			string assemblyTittle = attributes.Title ?? "";
			Log.dbg("First ({0}) = {1} :: {2}", assemblyTittle, assembly.FullName, assembly.Location);
			if (
				(System.IO.File.Exists(FULLMMPATH) && !assembly.Location.EndsWith(GAMEDATAMMDLL))
				||
				(assemblyTittle.StartsWith("Module Manager /L") && !assembly.Location.EndsWith(DASHMMDLL))
				||
				(assembly.Location.EndsWith(DASHMMDLL) && !assemblyTittle.StartsWith("Module Manager /L"))
				)
				return "There're conflicting Module Manager versions on your instalment! You need to choose one version and remove the other(s)!";
			return null;
		}

		// Trashing KSP's Assembly Loader/Resolver is essemtially common place, but sometimes these guys outdo themselves.
		//
		// Besides preventing multiple instances of the ModuleManager Assembly from being loaded, since KSP 1.12
		// **they still start them nevertheless**! This means the multiple ModuleManager's instances from the same Assembly can run
		// in parallel, **duplicating** all patching!
		//
		// DAMN, SQUAD!!! :(
		//
		// See https://github.com/net-lisias-ksp/ModuleManagerWatchDog/issues/6 for details.
		//
		private string CheckModuleManagerDoppelganger112()
		{
			Log.dbg("CheckModuleManagerDoppelganger112");

			int hits = 0;
			foreach (AssemblyLoader.LoadedAssembly m in AssemblyLoader.loadedAssemblies)
				if (ASSEMBLY_NAME.Equals(m.assembly.GetName().Name)) ++hits;

			if (hits > 1) return "There're more than one Module Manager on this KSP installment! Please delete all but the one you intend to use!";
			return null;
		}

	}
}
