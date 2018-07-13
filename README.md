﻿﻿﻿# SVFKManagerLibrary for managing SVFK button on Okuma OSP controlUses [martinusso's](https://github.com/martinusso) [ini.net](https://github.com/martinusso/ini.net) to read and write the Okuma SVFKA-USR.INI file. ## Usage```csharp                 var SVFKManager = new SVFKManager();                        //Get the list of button objects            var buttons = SVFKManager.Buttons;            //Find the first available button            var firstAvailBtn = SVFKManager.GetFirstAvailableButton();            if(firstAvailBtn != null)            {                //Update the button                firstAvailBtn.FilePath = @"C:/System32/notepad.exe";                firstAvailBtn.DisplayName = "Notepad";            }            //Save the file            SVFKManager.SaveButtons();            //Get the ini FileInfo            var fileInfo = SVFKManager.SVFKFileInfo;            //Initialize the file to it's default value            SVFKManager.ResetIni();```