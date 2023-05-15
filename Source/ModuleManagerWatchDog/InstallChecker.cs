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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SIO = System.IO;

namespace ModuleManagerWatchDog
{
	internal class InstallChecker
	{
		internal void Execute()
		{
			try
			{
				// Always check for being the unique Assembly loaded. This will avoid problems in the future.
				String msg = this.CheckMyself();

				if (null == msg)
					msg = this.CheckModuleManager();

				if (null == msg && SanityLib.IsEnforceable(1, 8))
					 msg = this.CheckModuleManagerDoppelganger18();

				if (null == msg && SanityLib.IsEnforceable(1, 12))
					 msg = this.CheckModuleManagerConflict112();

				if (null == msg && SanityLib.IsEnforceable(1, 12))
					 msg = this.CheckModuleManagerDoppelganger112();

				Handle(msg);
			}
			catch (Exception e)
			{
				Log.error(e.ToString());
				GUI.Dialogs.ShowStopperAlertBox.Show(e.ToString());
			}
		}

		private void Handle(string msg)
		{
			if ( null != msg && Globals.Instance.IsValid)
			{
				string msg2 = this.AutoFix();
				if (null != msg2)
					GUI.Dialogs.ShowRebootTheGameAlertBox.Show(msg2);
				else
					GUI.Dialogs.ShowStopperAlertBox.Show(msg);
			}
			else if (null != msg)
				GUI.Dialogs.ShowStopperAlertBox.Show(msg);
			else if (!Globals.Instance.IsValid)
			{ 
				// If we get here, we have a sane installment. Let's remember how the user wants his installment, so
				// we don't keep pesking the user everytime some idiot forces the installation of something the user
				// doesn't wants to use.
				Globals.Instance.PrefersMyFork = IsModuleManagerMyFork();
			}
		}

		private const string ASSEMBLY_NAME = "ModuleManager";
		private const string MYFORK_FILENAME = ASSEMBLY_NAME + ".dll";
		private const string MYFORK_ASMTITTLE = "Module Manager /L";

		private string CheckMyself()
		{
			IEnumerable<AssemblyLoader.LoadedAssembly> loaded = SanityLib.FetchLoadedAssembliesByName(this.GetType().Namespace);

#if DEBUG
			Log.dbg("CheckMyself");
			foreach (AssemblyLoader.LoadedAssembly la in loaded)
				Log.dbg("{0} :: {1}", la.assembly.FullName, la.assembly.Location);
#endif

			// Obviously, would be pointless to check for it not being installed! (0 == count). :)
			if (1 != loaded.Count()) return ErrorMessage.ERR_MMWD_DUPLICATED;
			if (!SanityLib.CheckIsOnGameData(loaded.First().path))
				return ErrorMessage.ERR_MMWD_WRONGPLACE;
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

			if (0 == loaded.Count()) return ErrorMessage.ERR_MM_ABSENT;
			if (!SanityLib.CheckIsOnGameData(loaded.First().path))
				return ErrorMessage.ERR_MM_WRONGPLACE;
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
			if (1 != loaded.Count()) return ErrorMessage.ERR_MM_DOPPELGANGER;
			return null;
		}

		// KSP 1.12 makes my life harder with this new way of loading Assemblies electing by version.
		//
		// This will make badly installed DDLs a bit more dificult to diagnose.
		//
		// Now I have to detected if MM/L is installed together Canonical MM by brute force.
		//
		// If no MM/L is installed, I will let it go as is.
		private static readonly string GAMEDATA = SanityLib.GetPathFor("GameData");
		private static readonly string GAMEDATAMMDLL = SIO.Path.Combine("GameData", MYFORK_FILENAME);
		static readonly string DASHMMDLL = GAMEDATAMMDLL.Replace("GameData", "");
		static readonly string FULLMMPATH = SIO.Path.Combine(GAMEDATA, MYFORK_FILENAME);
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
				(SIO.File.Exists(FULLMMPATH) && !assembly.Location.EndsWith(GAMEDATAMMDLL))
				||
				(assemblyTittle.StartsWith(MYFORK_ASMTITTLE) && !assembly.Location.EndsWith(DASHMMDLL))
				||
				(assembly.Location.EndsWith(DASHMMDLL) && !assemblyTittle.StartsWith(MYFORK_ASMTITTLE))
				)
				return ErrorMessage.ERR_MM_CONFLICT;
			return null;
		}

		// Trashing KSP's Assembly Loader/Resolver is essentially common place, but sometimes these guys outdo themselves.
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

			if (hits > 1) return ErrorMessage.ERR_MM_DOPPELGANGER;
			return null;
		}

		private bool IsModuleManagerMyFork()
		{
			IEnumerable<System.Reflection.Assembly> loaded = SanityLib.FetchAssembliesByName(ASSEMBLY_NAME);
#if DEBUG
			Log.dbg("IsModuleManagerMyFork");
			foreach (System.Reflection.Assembly a in loaded)
				Log.dbg("{0} :: {1}", a.FullName, a.Location);
#endif
			Assembly assembly = loaded.First();
			AssemblyTitleAttribute attributes = (AssemblyTitleAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute), false);
			string assemblyTittle = attributes.Title ?? "";
			Log.dbg("First ({0}) = {1} :: {2}", assemblyTittle, assembly.FullName, assembly.Location);
			return (assemblyTittle.StartsWith(MYFORK_ASMTITTLE));
		}

		private string AutoFix()
		{
			Log.info("This KSP is inconsistent. Trying the AutoFix.");
			string r = Globals.Instance.PrefersMyFork ? this.ForceMyFork() : this.ForceForum();
			if (null == r) Log.info("AutoFix failed.");
			return r;
		}

		private string ForceMyFork()
		{
			int deletedFiles = 0;
			SIO.DirectoryInfo d = new SIO.DirectoryInfo(GAMEDATA);
			SIO.FileInfo[] Files = d.GetFiles(ASSEMBLY_NAME + "*.dll"); //Getting Text files
			foreach(SIO.FileInfo file in Files ) try
			{
				if (this.IsMyFork(file)) continue;
				SIO.File.Delete(file.FullName);
				++deletedFiles;
			}
			catch (Exception e)
			{
				Log.error("Unexpected error \"{0}\"caugh by ForceMyFork while handling {1}", e.Message, file.Name);
			}
			Log.detail("Removing Forum's MM was {0}.", 0 == deletedFiles ? "unsucessful" : "sucessful");
			return 0 != deletedFiles ? ErrorMessage.ERR_MM_FORUMDELETED: null;
		}

		private string ForceForum()
		{
			int deletedFiles = 0;
			SIO.DirectoryInfo d = new SIO.DirectoryInfo(GAMEDATA);
			SIO.FileInfo[] Files = d.GetFiles(ASSEMBLY_NAME + "*.dll"); //Getting Text files
			foreach(SIO.FileInfo file in Files ) try
			{
				if (!IsMyFork(file)) continue;
				SIO.File.Delete(file.FullName);
				++deletedFiles;
			}
			catch (Exception e)
			{
				Log.error("Unexpected error \"{0}\" caugh by ForceForum while handling {1}", e.Message, file.Name);
			}
			Log.detail("Removing MM/L was {0}.", 0 == deletedFiles ? "unsucessful" : "sucessful");
			return 0 != deletedFiles ? ErrorMessage.ERR_MM_MYFORKDELETED: null;
		}


		private bool IsMyFork(SIO.FileInfo file)
		{
			Log.dbg("IsMyFork {0}", file);
			if (1 == Versioning.version_major && Versioning.version_minor >= 8) return this.IsMyFork18(file);

			Assembly assembly = Assembly.ReflectionOnlyLoad(SIO.File.ReadAllBytes(file.FullName));
			try
			{
				AssemblyTitleAttribute attributes = (AssemblyTitleAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute), false);
				string assemblyTittle = attributes.Title ?? "";
				return assemblyTittle.StartsWith(MYFORK_ASMTITTLE);
			}
			catch (Exception e) // My fork has the AssemblyTitleAttribute
			{
				return false;
			}
		}

		// I can't load the AsmFile using Assembly.ReflectionOnlyLoad because on KSP >= 1.8, KSP just shortcircuits the Assemly loading into the first
		// (or with the highest Version on KSP 1.12), screwing any attempt to use the Runtime's Reflection to inspect such files.
		private static readonly byte[] REFERENCE =
		{
			// Module Manager /L, unicode16, raw.
			0x4D, 0x00, 0x6F, 0x00, 0x64, 0x00, 0x75, 0x00, 0x6C, 0x00, 0x65, 0x00, 0x20, 0x00, 0x4D, 0x00, 0x61, 0x00, 0x6E, 0x00, 0x61, 0x00, 0x67, 0x00, 0x65, 0x00, 0x72, 0x00, 0x20, 0x00, 0x2F, 0x00, 0x4C, 0x00
		};
		private bool IsMyFork18(SIO.FileInfo file)
		{
			if (!MYFORK_FILENAME.Equals(file.Name)) return false;
			byte[] raw = SIO.File.ReadAllBytes(file.FullName);
			return 0 < Util.Toolbox.IndexOf(raw, REFERENCE);
		}

	}
}
