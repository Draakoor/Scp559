# SCP-559

Remember the funny cake that makes the people small and change the players voice from the christmas updates? Well, its here in a plugin, for everyone!

## Install Instructions
- Download EXILED (https://github.com/ExMod-Team/EXILED).
- Install MapEditorReborn (https://discord.com/channels/947849283514814486/995694845903524002)
- Download and install the plugin with the default schematic provided in the Release or create your own.
- Place the schematic in the MapEditorReborn schematics folder.

## Configuration
To load correctly the schematic, the plugin needs the SpawnPoints config to be setted up to specific RoomTypes and local positions inside rooms.
You can find all the RoomTypes in the EXILED discord.

To get the local positions, with a plugin you need to do `Room.Get(YourRoomType).GameObject.transform.TransformPoint(x, y, z);`
Or just use the tool I left inside the plugin with a command called roompoint.  
Example:  
`spawn_points:
      Lcz330:
        x: 135.802
        y: 1.278857
        z: 62.9407`  

## Default Model Preview
![image](https://github.com/user-attachments/assets/fef04287-f8fc-4b56-94a6-3759fe68dd63)  

Thanks to [xNexusACS](https://github.com/xNexusACS/SCP-559) and [Kyle's Projekt](https://github.com/Kyle-s-Project/SCP-559) for the original/edited source.
