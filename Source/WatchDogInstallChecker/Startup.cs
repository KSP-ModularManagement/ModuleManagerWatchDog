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

namespace WatchDog.InstallChecker
{
	[KSPAddon(KSPAddon.Startup.Instantly, true)]
	internal class Startup : MonoBehaviour
	{
		private const string MYSELF_NAME			= "Module Manager WatchDog Main Assembly";
		private readonly string MYSELF_SOURCE_FN	= System.IO.Path.Combine("ModuleManagerWatchDog",
														System.IO.Path.Combine("Plugins",
															System.IO.Path.Combine("PluginData", "ModuleManagerWatchDog.dll")
														)
													);
		private const string MYSELF_TARGET_FN		= "666_ModuleManagerWatchDog.dll";

		private const string MML_NAME				= "Module Manager";
		private readonly string MML_SOURCE_FN		= System.IO.Path.Combine("ModuleManager",
														System.IO.Path.Combine("PluginData", "ModuleManager.dll")
													);
		private const string MML_TARGET_FN			= "ModuleManager.dll";

		private const string SCALE_NAME				= "TweakScale's Scale_Redist";
		private readonly string SCALE_SOURCE_FN		= System.IO.Path.Combine("TweakScale",
														System.IO.Path.Combine("Plugins",
															System.IO.Path.Combine("PluginData", "Scale_Redist.dll")
														)
													);
		private const string SCALE_TARGET_FN		= "999_Scale_Redist.dll";

		private void Start()
		{
			Log.force("Version {0}", ModuleManagerWatchDog.Version.Text);

			try
			{
				// Always check for being the unique Assembly loaded. This will avoid problems in the future.
				String msg = CheckMyself();

				if ( null != msg )
					GUI.ShowStopperAlertBox.Show(msg);
			}
			catch (Exception e)
			{
				Log.error(e.ToString());
				GUI.ShowStopperAlertBox.Show(e.ToString());
			}

			try
			{
				List<String> msgs = new List<string>();

				String msg = SanityLib.UpdateIfNeeded(MYSELF_NAME, MYSELF_SOURCE_FN, MYSELF_TARGET_FN);
				if (null != msg) msgs.Add(msg);

				msg = SanityLib.UpdateIfNeeded(MML_NAME, MML_SOURCE_FN, MML_TARGET_FN);
				if (null != msg) msgs.Add(msg);

				msg = SanityLib.UpdateIfNeeded(SCALE_NAME, SCALE_SOURCE_FN, SCALE_TARGET_FN);
				if (null != msg) msgs.Add(msg);

				msg = string.Join("\n\n", msgs.ToArray());
				if ( !string.Empty.Equals(msg) )
					GUI.ShowRebootTheGame.Show(msg);
			}
			catch (Exception e)
			{
				Log.error(e.ToString());
				GUI.ShowStopperAlertBox.Show(e.ToString());
			}
		}

		private const string ERR_MULTIPLE_TOOL = "There're more than one WatchDog Install Checker on this KSP installment! Please delete all but the one on GameData/ModuleManagerWatchDog/Plugins !";

		private string CheckMyself()
		{
			IEnumerable<AssemblyLoader.LoadedAssembly> loaded = SanityLib.FetchLoadedAssembliesByName("WatchDogInstallChecker");

			// Obviously, would be pointless to check for it not being installed! (0 == count). :)
			if (1 != loaded.Count()) return ERR_MULTIPLE_TOOL;
			return null;
		}
	}
}
