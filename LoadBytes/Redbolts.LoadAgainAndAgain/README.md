#LoadAgainAndAgain

Simple example showing reloading of assemblies. Note they're still in memory and can't be unloaded without restarting revit. Also you have to be careful with potential 'dll hell' as Suzanne Cook called it as the assemblies are in a different AppDomain context to normally loaded revit assemblies.