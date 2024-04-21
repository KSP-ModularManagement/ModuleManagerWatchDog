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
using SIO = System.IO;

namespace WatchDog.ModuleManager
{
	public class Globals
	{
		private static Globals INSTANCE = null;
		public static Globals Instance => INSTANCE ?? (INSTANCE = new Globals());

		private const string NODE = "ModuleManagerWatchDog";
		private const string DATAFILE = NODE + ".cfg";
		private readonly string ROOT = SIO.Path.Combine(KSPUtil.ApplicationRootPath, "PluginData");

		internal readonly bool IsValid;
		private bool dirty = false;

		private bool prefersMyFork = false;
		public bool PrefersMyFork {
			get => this.prefersMyFork;
			internal set
			{
				this.dirty = true;
				this.prefersMyFork = value;
			}
		}

		private Globals()
		{
			string path = SIO.Path.Combine(ROOT, DATAFILE);
			this.dirty = false;
			bool r = true;
			try
			{
				ConfigNode cn = ConfigNode.Load(path).GetNode(NODE);
				r &= cn.TryGetValue("PrefersMyFork", ref this.prefersMyFork);
			}
			catch (Exception)
			{
				this.prefersMyFork = false;
				r = false;
			}
			this.IsValid = r;
		}

		internal void Save()
		{
			if (!this.dirty) return;
			string path = SIO.Path.Combine(ROOT, DATAFILE);
			Log.dbg("Writing {0}", path);
			try
			{
				ConfigNode cn = new ConfigNode(NODE);
				ConfigNode ccn = cn.AddNode(NODE);
				ccn.SetValue("PrefersMyFork", this.prefersMyFork, true);
				cn.Save(path);
				INSTANCE = null;
			}
			catch (Exception e)
			{
				Log.error("Could not write {0} due {1}!", path, e.Message);
			}
		}
	}
}
