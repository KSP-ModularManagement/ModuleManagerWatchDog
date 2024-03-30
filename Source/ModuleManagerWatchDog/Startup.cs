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
using UnityEngine;

namespace WatchDog.ModuleManager
{
	[KSPAddon(KSPAddon.Startup.Instantly, true)]
	internal class Startup : MonoBehaviour
	{
		private void Start()
		{
			Log.force("Version {0}", ModuleManagerWatchDog.Version.Text);
			{
				bool safeToKillMyself = true;
				// Check if ModuleManager /L should be uninstalled
				if (SelfCleaning.CheckUninstalled("ModuleManager", "ModuleManager.dll"))
				{
					Log.warn("ModuleManager /L's directory was removed. ModuleManagerWatchDog is removing ModuleManager /L from the Loading System, but some `*.delete-me` files may be left in your `GameData` until the next boot. Nothing bad will happen by leaving them there, however.");
					SelfCleaning.KillThis("ModuleManager.dll");
					safeToKillMyself = false;
				}

				// Check if ModuleManagerWatchDog should be uninstalled
				if (SelfCleaning.CheckUninstalled("ModuleManagerWatchDog", "666_ModuleManagerWatchDog.dll"))
				{
					if (safeToKillMyself)
					{
						Log.warn("ModuleManagerWatchDog's directory was removed. The bootstrap is removing itself from the Loading System, but you may need to delete manually some `*.delete-me` files in your `GameData`. Nothing bad will happen by leaving them there, however.");
						SelfCleaning.KillThis("666_ModuleManagerWatchDog.dll");
					}
					else
						Log.warn("ModuleManagerWatchDog's directory was removed, but some housekeeping is still in progress. It will remove itself in the next boot.");
					// We are dead. No further actions should be allowed.
					return;
				}
			}
			{
				InstallChecker ic = new InstallChecker();
				ic.Execute();
			}
		}

		internal static bool quitOnDestroy = false;
		private void OnDestroy()
		{
			Globals.Instance.Save();
			if (quitOnDestroy)
				Application.Quit();
		}
	}
}
