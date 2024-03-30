/*
	This file is part of Watch Dog for Intestellar Redist, a component from Module Manager Watch Dog
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
namespace WatchDog.InterstellarRedist
{
	public static class ErrorMessage
	{
		public static readonly string ERR_MULTIPLE_TOOL	= "There're more than one Interstellar Redist Watch Dog on this KSP installment! Please delete all but the one on GameData/ModuleManagerWatchDog/Plugins !";
		public static readonly string ERR_MISSING_DLL	= "There's no Interstellar Redist dll on this KSP installment, besides you having installed known DLL(s) that need it!!";
		public static readonly string ERR_MULTIPLE_DLL	= "There're more than one Interstellar Redist dll on this KSP installment! Please delete all but the GameData/Interstellar_Redist.dll one!";
		public static readonly string ERR_MISPLACED_DLL	= "Interstellar Redist dll <b>must be</b> directly on GameData and not inside any subfolder. Please move Interstellar_Redist.dll directly into GameData.";
	}
}
