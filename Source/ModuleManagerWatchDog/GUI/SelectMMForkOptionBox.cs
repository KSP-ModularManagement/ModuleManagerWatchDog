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

namespace WatchDog.ModuleManager.GUI.Dialogs
{
	internal static class SelectMMForkOptionBox
	{
		private readonly static string AMSG = "{0}\n\nKSP will close once you make a choice, so the fix takes effect.";
		internal static void Show(KSPe.UI.OptionDialogBox.Option[] options)
		{
			KSPe.UI.OptionDialogBox.Show(
				ErrorMessage.Conflict.TITLE,
				string.Format(AMSG, ErrorMessage.Conflict.ERR_MSG),
				options, true
			);
			Log.detail("\"{0}\" was displayed about : {1}", ErrorMessage.Conflict.TITLE, ErrorMessage.Conflict.ERR_MSG.Replace("\n"," "));
		}
	}
}
