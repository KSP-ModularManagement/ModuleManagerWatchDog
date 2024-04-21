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
using System;
using UnityEngine;
using UGUI = UnityEngine.GUI;

namespace KSPe.UI
{
	internal class OptionDialogBox : KSPe.Common.Dialogs.AbstractDialog
	{
		public class Option
		{
			public readonly string text;
			public readonly Action lambda;
			public readonly bool closeAfterClick;

			public Option(string text, Action lambda, bool closeAfterClick)
			{
				this.text = text;
				this.lambda = lambda;
				this.closeAfterClick = closeAfterClick;
			}
		}

		private class Dialog : MonoBehaviour
		{
			private string title;
			private string msg;
			private Option[] options;
			private GUIStyle win_style;
			private GUIStyle text_style;
			private int window_id;
			private Rect windowRect;
			private bool noCancel = false;

			public void Show(string title, string msg, Option[] options)
			{
				Show(title, msg, options, null, null);
			}

			public void Show(string title, string msg, Option[] options, GUIStyle win_style, GUIStyle text_style, bool noCancel = false)
			{
				this.title = title;
				this.msg = msg;
				this.options = options;
				this.win_style = win_style;
				this.text_style = text_style;
				this.noCancel = noCancel;
				this.window_id = (int)System.DateTime.Now.Ticks;

				this.windowRect = this.calculateWindow();
			}

			private Rect calculateWindow()
			{
				const int maxWidth = 640;
				const int maxHeight = 480;

				int width = Mathf.Min(maxWidth, Screen.width - 20);
				int height = Mathf.Min(maxHeight, Screen.height - 20);

				return new Rect(
					(Screen.width - width) / 2, (Screen.height - height) / 2,
					width, height
				);

			}

			private void OnGUI()
			{
				this.windowRect = this.win_style is null
					? UGUI.ModalWindow(this.window_id, this.windowRect, WindowFunc, this.title)
					: UGUI.ModalWindow(this.window_id, this.windowRect, WindowFunc, this.title, this.win_style);
			}

			private void WindowFunc(int windowID)
			{
				const int border = 10;
				const int height = 25;
				const int spacing = 10;

				int i = height * (this.options.Length + (this.noCancel ? 0 : 1));
				Rect l = new Rect(
						border, border + spacing,
						this.windowRect.width - border * 2, this.windowRect.height - border * 2 - height - spacing - i
					);

				if (this.text_style is null)
					UGUI.Label(l, this.msg);
				else
					UGUI.Label(l, this.msg, this.text_style);

				foreach (Option o in this.options)
				{
					Rect b = new Rect(
						border,
						this.windowRect.height - border - i,
						this.windowRect.width - border,
						height);
					i -= height;
					string text = o.closeAfterClick ? string.Format("{0}.", o.text) : o.text;
					if (UGUI.Button(b, o.text))
					{
						o.lambda();
						if (o.closeAfterClick) Destroy(this.gameObject);
					}
				}
				if (!this.noCancel)
				{
					Rect b = new Rect(
						border,
						this.windowRect.height - border - i,
						this.windowRect.width - border,
						height);
					if (UGUI.Button(b, "Cancel")) Destroy(this.gameObject);
				}
			}
		}

		public static void Show(string tittle, string msg, Option[] options, bool noCancel)
		{
			GameObject go = new GameObject(typeof(Dialog).FullName);
			Dialog dlg = go.AddComponent<Dialog>();
			//GUIStyle win = new GUIStyle(HighLogic.Skin.window)
			GUIStyle win = new GUIStyle("Window")
			{
				fontSize = 26,
				fontStyle = FontStyle.Bold,
				alignment = TextAnchor.UpperCenter,
				wordWrap = false
			};
			win.normal.textColor = Color.white;
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
			dlg.Show(tittle, msg, options, win, text, noCancel);
		}
	}
}
