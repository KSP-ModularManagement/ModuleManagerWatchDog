# Module Manager Watch Dog :: Changes

* 2021-0907: 1.0.1.0 (LisiasT) for KSP >= 1.2.2
	+ Promoted to Release
	+ Revamping the Redist checks, promoting modularity and selective deployment
	+ Adding checks for KSP 1.12.x due changes on how DLLs are loaded.
		- Prevents MM and MM /L forks from stomping each other toes. 
* 2021-0705: 1.0.0.0 BETA (LisiasT) for KSP >= 1.2.2
	+ Added rules to be enforced for:
		- Scale Redist not having duplicated DLLs, being present when needed and on the correct place  
		- Interstellar Redist not having duplicated DLLs, being present when needed and on the correct place  
	+ Allowing the rules to be waived or enforced on the running KSP version at user's discretion (by a patch on a ConfigNode file)
		- Including the Module Manager ones. 
	+ Allowing the thing to run on KSP >= 1.2.2, as this thingy can be useful on many KSP versions now.
