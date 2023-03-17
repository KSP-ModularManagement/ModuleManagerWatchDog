# Module Manager Watch Dog

A Watch Dog for Module Manager.

## Installation Instructions

To install, place the GameData folder inside your Kerbal Space Program folder:

* **REMOVE ANY OLD VERSIONS OF THE PRODUCT BEFORE INSTALLING**, including any other fork:
	+ Delete `<KSP_ROOT>/GameData/999_MMWD.dll`
* Extract the package's `GameData/` folder into your KSP's as follows:
	+ `<PACKAGE>/GameData/999_MMWD.dll` --> `<KSP_ROOT>/GameData`
		- Overwrite any preexisting file.

The following file layout must be present after installation:

```
<KSP_ROOT>
	[GameData]
		[ModuleManagerWatchDog]
			CHANGE_LOG
			ModuleManagerWatchDog.version
			...
		666_ModuleManagerWatchDog.dll
		ModuleManager.dll
		...
	KSP.log
	PastDatabase.cfg
	...
```


### Dependencies

* None at the moment.
