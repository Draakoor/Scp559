# Build and install

Target: SCP:SL 14.2.7, EXILED 9.14.2, LabAPI 1.1.7, ProjectMER 2026.7.6.1.

Use .NET SDK and a directory containing the matching server Managed assemblies, EXILED assemblies, ProjectMER.dll and AudioPlayerApi.dll. No game assemblies are redistributed.

```powershell
dotnet build "Scp559/Scp559.csproj" -c Release -p:SLReferences=C:/path/to/references
```

Copy `Scp559.dll` to `EXILED/Plugins`. Copy the bundled MER schematic folder to `LabAPI/configs/ProjectMER/Schematics`. Restart the server. Back up existing files first.

This release replaces old MapEditorReborn API calls with current ProjectMER and cleans up spawned objects and temporary effects at round transitions.
