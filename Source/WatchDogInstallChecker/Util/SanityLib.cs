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

using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace WatchDog.InstallChecker
{
	public static class SanityLib
	{
		/**
		 * If you are interested only on assemblies that were properly loaded by KSP, this is the one you want.
		 */
		public static IEnumerable<AssemblyLoader.LoadedAssembly> FetchLoadedAssembliesByName(string assemblyName)
		{ 
			return from a in AssemblyLoader.loadedAssemblies
					let ass = a.assembly
					where assemblyName == ass.GetName().Name
					orderby a.path ascending
					select a
				;
		}

		internal static string UpdateIfNeeded(string name, string sourceFilename, string targetFilename)
		{
			sourceFilename = Path.Combine(SanityLib.CalcGameData(), sourceFilename);
			targetFilename = Path.Combine(SanityLib.CalcGameData(), targetFilename);
			if (File.Exists(sourceFilename))
			{
				if (File.Exists(targetFilename))
				{
					{ 
						System.Diagnostics.FileVersionInfo sourceVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(sourceFilename);
						System.Diagnostics.FileVersionInfo targetVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(targetFilename);

						bool sane = sourceVersionInfo.ProductName.Equals(targetVersionInfo.ProductName);
						sane &= sourceVersionInfo.LegalCopyright.Equals(targetVersionInfo.LegalCopyright);
						sane &= sourceVersionInfo.LegalTrademarks.Equals(targetVersionInfo.LegalTrademarks);
						sane &= sourceVersionInfo.CompanyName.Equals(targetVersionInfo.CompanyName);

						if (!sane)
						{
							Log.info("File {0} is not compatible with {1}. This is going to cause trouble, replacing it!", targetFilename, name);
							File.Delete(targetFilename);	// Remove the file and update it no matter what!
							return SanityLib.Update(name, sourceFilename, targetFilename);
						}
					}
					{
						System.Reflection.Assembly sourceAsm = System.Reflection.Assembly.LoadFile(sourceFilename);
						System.Reflection.Assembly targetAsm = System.Reflection.Assembly.LoadFile(targetFilename);
						if (!sourceAsm.GetName().Version.Equals(targetAsm.GetName().Version))
							return SanityLib.Update(name, sourceFilename, targetFilename);
						else
						{
							File.Delete(sourceFilename);
							return null;
						}
					}
				}
				else return SanityLib.Update(name, sourceFilename, targetFilename);
			}
			// Nothing to do. If this is an error, someone else will yell about.
			return null;
		}

		private static string Update(string name, string sourceFilename, string targetFilename)
		{
			try
			{
				File.Copy(sourceFilename, targetFilename);
				File.Delete(sourceFilename);
				return string.Format("{0} was updated.", name);
			}
			catch (Exception e)
			{
				Log.error("Error while installing {0}", sourceFilename); // the Exception is logged on the caller's handler
				throw e;
			}
		}

		private static string GAMEDATA = null;
		private static string CalcGameData()
		{
			if (null != GAMEDATA) return GAMEDATA;
			System.Reflection.Assembly asm = System.Reflection.Assembly.GetAssembly(typeof(UnityEngine.MonoBehaviour));
			string path = Path.GetDirectoryName(asm.Location);
			string candidate = Path.Combine(path, "GameData");
			try
			{
				while (!Directory.Exists(candidate))
				{
					path = Path.GetDirectoryName(path);
					candidate = Path.Combine(path, "GameData");
				}
				Log.dbg("GameData found on {0}", GAMEDATA);
				return (GAMEDATA = candidate);
			}
			catch (Exception e)
			{
				Log.error("Error while looking for the GameData! : {0}", e.ToString());
				throw e;
			}
		}
	}
}
