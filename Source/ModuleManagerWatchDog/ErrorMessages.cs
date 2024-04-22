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
namespace WatchDog.ModuleManager
{
	internal static class ErrorMessage
	{
		public static readonly string ERR_MMWD_DUPLICATED = "There're more than one MM Watch Dog on this KSP installment! Please delete all but the one you intend to use!";
		public static readonly string ERR_MMWD_WRONGPLACE = "666_ModuleManagerWatchDog.dll <b>must be</b> directly on GameData and not inside any subfolder (i.e., it must be in the same place ModuleManager.dll is). Please move 666_ModuleManagerWatchDog.dll directly into GameData.";
		public static readonly string ERR_MM_ABSENT = "There's no Module Manager on this KSP installment! You need to install Module Manager!";
		public static readonly string ERR_MM_WRONGPLACE = "ModuleManager.dll <b>must be</b> directly on GameData and not inside any subfolder. Please move ModuleManager.dll directly into GameData.";
		public static readonly string ERR_MM_DOPPELGANGER = "There're more than one Module Manager on this KSP installment! Please delete all but the one you intend to use!";
		public static readonly string ERR_MM_FORUMDELETED = "You had selected MM/L as preferred Module Manager, but somehow other forks were installed. They were automatically removed.";
		public static readonly string ERR_MM_MYFORKDELETED = "You had selected Forum's as preferred Module Manager, but somehow other forks were installed. They were automatically removed.";
		public static readonly string ERR_MM_MISSING_DEPENDENCIES = "You have MM installed, but not its dependencies. If using MM/L, you need to install KSPe too!"; 

		internal static class Conflict
		{
			public static readonly string TITLE = "Houston, we have a problem!";
			public static readonly string ERR_MSG = "There're conflicting Module Manager versions on your instalment!\n\nYou need to choose the fork you want to use, the other one will be removed. Futurely, undully installed Module Managers will be automatically removed, respecting your choice.";
			public static readonly string URL = "https://ksp.lisias.net/add-ons/ModuleManager/WatchDog/FAQ/ConflictingMM";
			public static readonly string OPTION_READ_MORE = "Would you like to know more? (a web page will be opened)";
			public static readonly string OPTION_MML = "Keep MM/L.";
			public static readonly string OPTION_MMF = "Keep MM/Forum.";
		}
	}
}
