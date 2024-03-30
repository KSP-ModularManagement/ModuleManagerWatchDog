/*
	This file is part of WatchDog.InstallChecker, a component for Module Manager Watch Dog
		©2020-2024 LisiasT : http://lisias.net <support@lisias.net>

	KSP Enhanced /L is licensed as follows:
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

namespace WatchDog.InstallChecker
{
	[KSPAddon(KSPAddon.Startup.Instantly, true)]
	internal class Startup : MonoBehaviour
	{
		private readonly SanityLib.UpdateData[] UPDATEABLES = new SanityLib.UpdateData[] {
				new SanityLib.UpdateData(
					"Module Manager WatchDog Main Assembly",
					System.IO.Path.Combine("ModuleManagerWatchDog",
						System.IO.Path.Combine("Plugins",
							System.IO.Path.Combine("PluginData", "ModuleManagerWatchDog.dll"))),
					"666_ModuleManagerWatchDog.dll"),
				new SanityLib.UpdateData(
					"Module Manager",
					System.IO.Path.Combine("ModuleManager",
						System.IO.Path.Combine("PluginData", "ModuleManager.dll")),
					"ModuleManager.dll"),
				new SanityLib.UpdateData(
					"TweakScale's Scale_Redist",
					System.IO.Path.Combine("TweakScale",
						System.IO.Path.Combine("Plugins",
							System.IO.Path.Combine("PluginData", "Scale_Redist.dll"))),
					"999_Scale_Redist.dll")
			};

		private void Start()
		{
			Log.force("Version {0}", ModuleManagerWatchDog.Version.Text);

			try
			{
				// Always check for being the unique Assembly loaded. This will avoid problems in the future.
				String msg = CheckMyself();

				if (null != msg)
					GUI.ShowStopperAlertBox.Show(msg);
			}
			catch (Exception e)
			{
				Log.error(e.ToString());
				GUI.ShowStopperAlertBox.Show(e.ToString());
			}

			try
			{
				List<string> msgs = new List<string>();
				string msg;
				foreach (SanityLib.UpdateData ud in UPDATEABLES)
				{
					msg = SanityLib.UpdateIfNeeded(ud);
					if (null != msg) msgs.Add(msg);
				}

				msg = string.Join("\n\n", msgs.ToArray());
				if (!string.Empty.Equals(msg))
					GUI.ShowRebootTheGame.Show(msg);
			}
			catch (Exception e)
			{
				Log.error(e.ToString());
				GUI.ShowStopperAlertBox.Show(e.ToString());
			}
		}

		internal static bool quitOnDestroy = false;
		private void OnDestroy()
		{
			if (!quitOnDestroy) return;

			// Someone, probably a FatalError, told us to quit the game.
			Log.force("Quitting KSP due an unrecoverable error.");
			UnityEngine.Application.Quit();
		}

		private string CheckMyself()
		{
			// The previous, minimalistic, check was hurting the update process, when a new DLL would be, in fact, in the MMWD's diretory.
			// So we ignore these copies when the effectivelly loaded on is the one named 666_ModuleManagerWatchDog.dll .
 			{
 				IEnumerable<AssemblyLoader.LoadedAssembly> loaded = SanityLib.FetchLoadedAssembliesByName(this.GetType().Assembly.GetName().Name);

 				// Obviously, would be pointless to check for it not being installed! (0 == count). :)
				if (loaded.Count() > 1
					&& !"666_ModuleManagerWatchDog.dll".Equals(System.IO.Path.GetFileName(loaded.First().assembly.Location), StringComparison.InvariantCultureIgnoreCase)
				)
					return ErrorMessage.ERR_MULTIPLE_TOOL;
			}
			return null;
		}
	}
}
