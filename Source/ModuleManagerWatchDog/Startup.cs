/*
	This file is part of Module Manager Watch Dog
	(C) 2020-21 Lisias T : http://lisias.net <support@lisias.net>

	Module Manager Watch Dog is licensed as follows:

	* SKL 1.0 : https://ksp.lisias.net/SKL-1_0.txt

	And you are allowed to choose the License that better suit your needs.

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
				int kspMajor = Versioning.version_major;
				int kspMinor = Versioning.version_minor;

				{
					String msg = CheckMyself();
					if ( null != msg )
						GUI.ShowStopperAlertBox.Show(msg);
				}

				// On KSP < 1.8, Module Manager works as expected. Do not check.
				if (kspMajor <= 1 && kspMinor < 8) return;

				{
					String msg = CheckModuleManager();
					if ( null != msg )
						GUI.ShowStopperAlertBox.Show(msg);
					else
						Log.info("Module Manager is good to go.");
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
			IEnumerable<AssemblyLoader.LoadedAssembly> loaded =
				from a in AssemblyLoader.loadedAssemblies
					let ass = a.assembly
					where "ModuleManagerWatchDog" == ass.GetName().Name
					orderby a.path ascending
					select a;

			if (1 != loaded.Count()) return "There're more than one MM Watch Dog on this KSP instalment! Please delete all but the one you intend to use!";
			return null;
		}

		private String CheckModuleManager()
		{
			IEnumerable<AssemblyLoader.LoadedAssembly> loaded =
				from a in AssemblyLoader.loadedAssemblies
					let ass = a.assembly
					where "ModuleManager" == ass.GetName().Name
					orderby a.path ascending
					select a;

			if (0 == loaded.Count()) return "There's no Module Manager on this KSP instalment! You need to install Module Manager!";
			if (1 != loaded.Count()) return "There're more than one Module Manager on this KSP instalment! Please delete all but the one you intend to use!";
			return null;
		}
	}
}
