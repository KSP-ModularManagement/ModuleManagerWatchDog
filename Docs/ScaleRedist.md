# Watch Dog for Scale Redist dll :: Known Issues

This is a troubleshooting list to help you on fixing your installment.

If you need further help, please visit [Forum's TweakScale Thread](https://forum.kerbalspaceprogram.com/index.php?/topic/179030-*/).

Alternatively, you can reach the [TweakScale's General Support Issue](https://github.com/net-lisias-ksp/TweakScale/issues/92) if you have a Github account and prefer using it.

## Known detectable problems

### There're more than one `Scale Redist Watch Dog` on this KSP installment!

Well, somehow you managed to have installed more than one copy of this tool itself!

Check `GameData/` for DLLs with the name `WatchDogForScaleRedist` on it and delete all of them - but the one inside `GameData/ModuleManagerWatchDog/`.

### "There's no Scale Redist dll on this KSP installment, besides you having installed known DLL(s) that need it"

***\*\*\*Work in Progress \*\*\****


### "There're more than one Scale Redist dll on this KSP installment!"

Unfortunately, from KSP 1.8.0 and above a potential fatal bug is affecting every DLL when more then one copy if it is installed on your system, causing many troubles from using older, buggy versions of such DLL to hanging KSP itself.

There's no other solution available but to manually **DELETE** all redundant copies of the DLL, leaving only the one on the `GameData/`. 

***\*\*\*Work in Progress \*\*\****


### "Scale Redist dll *must be* directly on GameData (and not inside any subfolder) and *should* be named `999_Scale_Redist.dll`"


***\*\*\*Work in Progress \*\*\****

