WindowConfigurator Plugin for Rhino
=========================

![Rhino](https://lh6.googleusercontent.com/-pQtuyrwmcmg/TYtWECHGYNI/AAAAAAAAA7Y/rphjSmq1cuo/s200/Rhino_logo_wire.jpg)

Introduction
---------------------------
WindowConfigurator is a "enigma machine" to let user design a window with simple input based on select window system embeded in the Json file. For test purpose, a rhino pulgin is created with RhinoCommon which will achieve full functionality of the WindowConfigurator. The program will be implement to severl different project requiring a general window design in the future.

Functions
--------------------
InitializeWindow: Initialize a window with width, height, and json file represent each system. The bottom left corner will be located at the origin.
- Input the width
- Input the height
- Select a json file from your folder

AddTransom: Add a transom to the window system with start and end point.
- Select the start point
- Select the end point
- Input the article number
- The program will prompt errors if the start point or end point is outside the window or their Z axis are not equal

AddMullion: Add a mullion to the window system with start and end point.
- Select the start point
- Select the end point
- Input the article number
- The program will prompt errors if the start point or end point is outside the window or their Y axis are not equal

RemoveTransom: Remove a transom in the window system and extends every mullion connected with it
- Select the transom
- The program will prompt errors if the line selected is not horizontal

RemoveMullion: Remove a mullion in the window system and extends every transom connected with it
- Select the mullion
- The program will prompt errors if the line selected is not vertical

Steps:

* Get the source code by downloading everything as a zip or using git

I'm going to need to come up with better instructions on compiling and using the dll, but in a nutshell you should be able to debug into the RhinoCommon source code by:

* rename the shipping RhinoCommon.dll to something like RhinoCommon.dll.original
* place the RhinoCommon.dll and pdb that gets compiled by this project in the Rhino5 system directory

Installation / Configuration
----------------------------
RhinoCommon is written to work under different "modes".
- Assembly running in Rhino: Rhino 5 (and Grasshopper in Rhino 4) ship a precompiled version of RhinoCommon that contains full access to the Rhino SDK. This means highre level functionality of things like intersections or working with the RhinoDoc are supported
- Stand alone assembly accessing OpenNURBS: A special build flavor of RhinoCommon is supported that let's you build RhinoCommon as a .NET layer on top of the C++ OpenNURBS toolkit (www.opennurbs.org)

WindowConfigurator pulgin refer to the Json.net library to load and process the Json file.
- Install the Newtonsoft in the Visual Studio NuGet packages Manager.
- Include Newtonsoft and Newtonsoft.Json in the script.

The WindowConfigurator canbe compiled to the rhp plugin for Rhino.
- Install RhinioCommon following the instuction at https://developer.rhino3d.com/guides/rhinocommon/your-first-plugin-windows/
- Run and compile the WindowConfigurator source code then a rhp plugin will be generated under ~/WindowConfigurator/bin and Rhino should start automatically.
- Find rhp plugin in ~/WindowConfigurator/WindowConfigurator/bin/
- Install the rhp plugin in Rhino PluginManager.


Authors
-------
Specific people to contact about this project:

* Steve Baer - https://github.com/sbaer steve@mcneel.com
* David Rutten - https://github.com/DavidRutten
* Giulio Piacentino - https://github.com/piac


