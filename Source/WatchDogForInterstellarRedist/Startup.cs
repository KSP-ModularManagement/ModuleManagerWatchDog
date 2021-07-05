/*
	This file is part of Watch Dog for Intestellar Redist, a component from Module Manager Watch Dog
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

using UnityEngine;

using ModuleManagerWatchDog;

namespace WatchDogForInterstellarRedist
{
	[KSPAddon(KSPAddon.Startup.Instantly, true)]
	internal class Startup : MonoBehaviour
	{
		private void Start()
		{
			Log.force("Version {0}", ModuleManagerWatchDog.Version.Text);

			try
			{
				// Always check for being the unique Assembly loaded. This will avoid problems in the future.
				{
					String msg = CheckMyself();
					if ( null != msg )
						GUI.ShowStopperAlertBox.Show(msg);
				}

				// On KSP < 1.8, duplicated Redists are not a problem, besides being a good idea to avoid them.
				// By not being a problem, I let the user choose or not to be pickly (I am!) by patching the respective value on the
				// configuration Config. 
				if (SanityLib.IsExempted(Versioning.version_major, Versioning.version_minor))
					return;

				{
					String msg = CheckScaleRedist();
					if ( null != msg )
						GUI.ShowStopperAlertBox.Show(msg);
				}
			}
			catch (Exception e)
			{
				Log.error(e.ToString());
				GUI.ShowStopperAlertBox.Show(e.ToString());
			}
		}

		private String CheckMyself()
		{
			IEnumerable<AssemblyLoader.LoadedAssembly> loaded = SanityLib.FetchDllsByAssemblyName("WatchDogForInterstellarRedist");

			// Obviously, would be pointless to check for it not being installed! (0 == count). :)
			if (1 != loaded.Count()) return "There're more than one Interstellar Redist Watch Dog on this KSP instalment! Please delete all but the one on GameData/ModuleManagerWatchDog !";
			return null;
		}

		private String CheckScaleRedist()
		{
			{
				string[] knownClients = SanityLib.GetFromConfig("Interstellar_Redist", "knownClient");
				bool found = false;
				foreach (string s in knownClients)
					found |= SanityLib.FetchDllsByAssemblyName(s).Any();

				// No known clients found, no need to check for it.
				if (!found) return null;
			}

			IEnumerable<AssemblyLoader.LoadedAssembly> loaded = SanityLib.FetchDllsByAssemblyName("Interstellar_Redist");

			if (0 == loaded.Count()) return "There's no Interstellar_Redist.dll on this KSP instalment, besides you having installed known DLL(s) that need it!!";
			if (1 != loaded.Count()) return "There're more than one Interstellar_Redist.dll on this KSP instalment! Please delete all but the GameData/Interstellar_Redist.dll one!";
			if (!SanityLib.CheckIsOnGameData(loaded.First<AssemblyLoader.LoadedAssembly>().path, "Interstellar_Redist.dll"))
				return "Interstellar_Redist.dll <b>must be</b> directly on GameData and not inside any subfolder. Please move Interstellar_Redist.dll directly into GameData.";
			return null;
		}
	}
}
