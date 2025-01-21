using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Loader;
using RemoteAdmin;

namespace Scp559.Utilities;

public static class StartupChecks
{
    public static bool CheckForMapEditorReborn()
    {
        return File.Exists(Path.Combine(Paths.Plugins, "MapEditorReborn.dll"));
    }

    private static bool IsJadeLibInstalled()
    {
        return File.Exists(Path.Combine(Paths.Dependencies, "JadeLib.dll"));
    }

    public static void UnRegisterIncompatibilities()
    {
        if (!IsJadeLibInstalled()) 
            return;

        Assembly jadeLibDep = Loader.Dependencies.FirstOrDefault(x => x.GetName().Name == "JadeLib");

        if (jadeLibDep == null)
            return;
        
        Type commandInterfaceType = typeof(ICommand);
        
        Type roomPointType = jadeLibDep.GetTypes().FirstOrDefault(t => t.Name == "RoomPoint");

        if (roomPointType is null || !commandInterfaceType.IsAssignableFrom(roomPointType)) 
            return;
        
        ICommand roomPointAsCommand = (ICommand)Activator.CreateInstance(roomPointType);
            
        CommandProcessor.RemoteAdminCommandHandler.UnregisterCommand(roomPointAsCommand);
    }
}