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
using UnityEngine;
using KSPe.UI;

namespace KSPe.Common.Dialogs
{
	public class ShowStopperAlertBox : AbstractDialog
	{
		private static readonly string aMSG = "close KSP, then fix the problem described above";

		private static readonly string MSG = @"{0}

This is a Show Stopper problem. You should click OK to {1}.

If you choose to continue running KSP (by clicking Cancel), your saves may be unrecoverably corrupted, even if it seems to be working correctly.

Make back-ups now! You may find the S.A.V.E mod helpful for this.";

		public static void Show(string errorMessage)
		{
			Show(errorMessage, aMSG, Application.Quit);
		}

		public static void Show(string errorMessage, string actionMessage, Action lambda)
		{
			GameObject go = new GameObject("KSPe.Common.Diallgs.ShowStopperAlertBox");
			MessageBox dlg = go.AddComponent<MessageBox>();

			//GUIStyle win = new GUIStyle(HighLogic.Skin.window)
			GUIStyle win = new GUIStyle("Window")
			{
				fontSize = 26,
				fontStyle = FontStyle.Bold,
				alignment = TextAnchor.UpperCenter,
				wordWrap = false
			};
			win.normal.textColor = Color.red;
			win.border.top = 0;
			win.padding.top = -5;
			SetWindowBackground(win);
			win.active.background =	win.focused.background = win.normal.background;

			GUIStyle text = new GUIStyle("Label")
			{
				fontSize = 18,
				fontStyle = FontStyle.Normal,
				alignment = TextAnchor.MiddleLeft,
				wordWrap = true
			};
			text.normal.textColor = Color.white;
			text.padding.top = 8;
			text.padding.bottom = text.padding.top;
			text.padding.left = text.padding.top;
			text.padding.right = text.padding.top;
			SetTextBackground(text);

			dlg.Show(
				"Houston, we have a problem!",
				String.Format(MSG, errorMessage, actionMessage),
				lambda,
				win, text
			);
		}
	}
}
