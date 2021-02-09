source ../CONFIG.inc

PACKAGE="ModuleManagerWatchDog"
TARGETDIR="${PACKAGE}"
TARGETBINDIR="$TARGETDIR"
PROJECTSDIR="$PACKAGE/"
VERSIONFILE="ModuleManagerWatchDog.version"

GD_DLLS="ModuleManagerWatchDog"
GD_PRIORITY="666"
DLLS=""


PROJECT_BRANCH=`git branch | grep \* | cut -d ' ' -f2`
if [ "$PROJECT_BRANCH" == "master" ] ; then
	# Release mode
	PROJECT_STATE=""
else
	# Note: Add a leading dash when this value is present!
	PROJECT_STATE=""
fi

VERSION=$( cat $VERSIONFILE | tr '\n' ' ' | sed -n -E 's/^.*?"VERSION\":\{.*"MAJOR":([0-9]+?),.*?"MINOR":([0-9]+?),.*?"PATCH":([0-9]+?),.*?"BUILD":([0-9]+?).*?\}.*$/\1.\2.\3.\4/p' )
