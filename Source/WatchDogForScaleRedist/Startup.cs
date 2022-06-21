/*
	This file is part of Watch Dog for Scale Redist, a component from Module Manager Watch Dog
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

using UnityEngine;

using ModuleManagerWatchDog;

namespace WatchDogForScaleRedist
{
	[KSPAddon(KSPAddon.Startup.Instantly, true)]
	internal class Startup : MonoBehaviour
	{
		private void Start()
		{
			Log.force("Version {0}", ModuleManagerWatchDog.Version.Text);

			if (SanityLib.FetchLoadedAssembliesByName("WatchDogInstallChecker").Any())
				StartCoroutine("Coroutine");
			else
				this.Execute();
		}

		private System.Collections.IEnumerator Coroutine()
		{
			// If InstallChecker is loaded, burn some time to tive it time to do its job.
			for (int i = 600; i >= 0; --i)	// 
				yield return null;
			this.Execute();
			yield break;
		}

		private void Execute()
		{
			try
			{
				// Always check for being the unique Assembly loaded. This will avoid problems in the future.
				String msg = CheckMyself();

				// If no known clients found, no need to further checks.
				if (null == msg && IsNeeded())
				{
					Log.detail("Clients found. Checking integrity...");

					if ( null == msg )
						msg = CheckRedist();

					if (null == msg && SanityLib.IsEnforceable(1, 8))
						msg = CheckRedist18();

					if (null == msg)
						Log.detail("System is good to go.");
				}
				else
					Log.detail("No clients found. Sanity Check aborted.");

				if ( null != msg )
					GUI.ShowStopperAlertBox.Show(msg);
			}
			catch (Exception e)
			{
				Log.error(e.ToString());
				GUI.ShowStopperAlertBox.Show(e.ToString());
			}
		}

		private const string ASSEMBLY_NAME		= "Scale_Redist";
		private const string DLL_FILENAME		= "999_Scale_Redist.dll";
		private const string ERR_MULTIPLE_TOOL	= "There're more than one Scale Redist Watch Dog on this KSP installment! Please delete all but the one on GameData/ModuleManagerWatchDog/Plugins !";
		private const string ERR_MISSING_DLL	= "There's no Scale Redist dll on this KSP installment, besides you having installed known DLL(s) that need it!!";
		private const string ERR_MULTIPLE_DLL	= "There're more than one Scale Redist dll on this KSP installment! Please delete all but the GameData/999_Scale_Redist.dll one!";
		private const string ERR_MISPLACED_DLL	= "Scale Redist dll <b>must be</b> directly on GameData (and not inside any subfolder) and <b>should</b> be named 999_Scale_Redist.dll . Please move 999_Scale_Redist.dll directly into GameData.";

		private string CheckMyself()
		{
			IEnumerable<AssemblyLoader.LoadedAssembly> loaded = SanityLib.FetchLoadedAssembliesByName(this.GetType().Namespace);

			// Obviously, would be pointless to check for it not being installed! (0 == count). :)
			if (1 != loaded.Count()) return ERR_MULTIPLE_TOOL;
			return null;
		}

		private bool IsNeeded()
		{
			string[] knownClients = SanityLib.GetFromConfig(ASSEMBLY_NAME, "knownClient");

			foreach (string s in knownClients) if (SanityLib.FetchAssembliesByName(s).Any())
				return true;

			return false;
		}

		private string CheckRedist()
		{
			IEnumerable<AssemblyLoader.LoadedAssembly> loaded = SanityLib.FetchLoadedAssembliesByName(ASSEMBLY_NAME);
			if (0 == loaded.Count()) return ERR_MISSING_DLL;
			return null;
		}

		// On KSP < 1.8, duplicated Redists are not a problem, besides being a good idea to avoid them.
		// However, KSP 1.12 mangled a bit how DLLs are loaded, masking when more than one Assembly with the same name is loaded.
		// This may render the diagnosis a bit confusing
		private string CheckRedist18()
		{
			IEnumerable<AssemblyLoader.LoadedAssembly> loaded = SanityLib.FetchLoadedAssembliesByName(ASSEMBLY_NAME);

			if (1 != loaded.Count()) return ERR_MULTIPLE_DLL;
			if (!SanityLib.CheckIsOnGameData(loaded.First().path, DLL_FILENAME))
				return ERR_MISPLACED_DLL;
			return null;
		}
	}
}
