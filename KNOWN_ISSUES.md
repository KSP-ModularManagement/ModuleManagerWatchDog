# Module Manager Watch Dog :: Known Issues

This is a troubleshooting list to help you on fixing your instalment.


## Known detectable problems

If you need help about how to fix your system, drop me a message on [Forum](https://forum.kerbalspaceprogram.com/index.php?/profile/187168-lisias/) or send me a email using the address `support` **at** `lisias` **dot** `net` . Please add "KSP:ModuleManagerWatchDog" as the subject of the email, or my email filter will not detect your email and I may miss it.

### There're more than one MM Watch Dog on this KSP instalment!

Well, somehow you managed to have installed more than one copy of this tool itself!

Check `GameData/` for DLLs with the name "ModuleManagerWatchDog" on it and delete all of them - but the newest one.

### "There's no Module Manager on this KSP instalment!"

Oukey, you need to install Module Manager! :) See the "How-To" below.

###  "There're more than one Module Manager on this KSP instalment!"

Unfortunately, from KSP 1.8.0 and above a fatal bug is affecting Module Manager when more then one MM dll is installed on your system, what renders Module Manager choosing to use the **oldest** version to be used. This caused some problems to TweakScale at least once.

There's no other solution available but to manually **DELETE** all redundant copies of Module Manager, leaving only one on the GameData. 

This was diagnosed on [TweakScale's thread](https://forum.kerbalspaceprogram.com/index.php?/topic/179030-ksp-141-tweakscale-under-lisias-management-24314-2020-0519/&do=findComment&comment=3797945).

## How-To Install Module Manager

You have two options, each one with advantages and drawbacks.

### Option I : Module Manager /L Experimental

This is my personal fork of Module Manager, aiming to fix some non functional bugs while keeping compatibility with the original Module Manager.

* Advantages
	+ Slightly faster, mainly on systems with less memory available and a lot of patches.
	+ Way better logging
	+ Some fixes from small annoyances or bugs, as long they don't break compatibility with the upstream. 
	+ It works on every KSP Version from 1.3.0 to the latest
		- 1.2.2 are currently in Technology Demonstration.
* Disadvantages
	+ It's not supported by most of the Add'On authors
		- To tell you the true, I'm the only one supporting this stunt. :)
	+ Some Add'On authors may dismiss your bug reports by using a non "official" Module Manager.
	+ It has dependencies.

If you are willing to risk your SAS, you can downloaded it from my github:

* [Module Manager /L Experimental](https://github.com/net-lisias-ksp/ModuleManager/releases)
* Dependencies
	+ [KSPe and KSP API Extensions](https://github.com/net-lisias-ksp/KSPAPIExtensions).

### Option II : Official Module Manager

Obviously, you can use the Official Module Manager from Forum.

* Advantages
	+ No risk of having your reports rejected by using non Official versions of something
* Disadvantages
	+ Less conveniency on diagnosing problems using the logs
	+ Some annoying misfeatures
	+ You need to cope with this type of [bad behaviour](https://forum.kerbalspaceprogram.com/index.php?/topic/50533-18x-19x-module-manager-413-november-30th-2019-right-to-ludicrous-speed/&do=findComment&comment=3742972). :(
		- Yeah, it was moderated, so...
			-  ![One](./Screen%20Shot%202020-02-18%20at%2008.59.14.png)
			-  ![Two](./Screen%20Shot%202020-02-18%20at%2008.59.21.png)
	+ Or this kind of [asshole](https://forum.kerbalspaceprogram.com/index.php?/topic/50533-18x-110x-module-manager-414-july-7th-2020-locked-inside-edition/&do=findComment&comment=3816948). 
		- ![](./Screen%20Shot%202020-07-08%20at%2007.47.29.png) 
		- That is a follow up from this [comment](https://github.com/UbioWeldingLtd/UbioWeldContinued/issues/47#issuecomment-655553866).
		- ![](./Screen%20Shot%202020-07-11%20at%2007.29.20.png)
	+ And now, some of his friends is slandering my Add'Ons. :)
		- ![](./Screen%20Shot%202020-07-14%20at%2002.14.51.png) 
	+ No software can be better than the dude that wrote it.
		- Or his friends. 

To download it, go to [Forum](https://forum.kerbalspaceprogram.com/index.php?/topic/50533-*).
