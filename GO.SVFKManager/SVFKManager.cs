namespace GO.Utilities.VFK
{
    using Ini.Net;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using GO.Utilities.VFK.Enumerations;
    using GO.Utilities.VFK.Properties;

    public class SVFKManager
    {
        List<SVFKButton> _buttons;

        readonly IniFile _iniFile;
        readonly string _iniPath;
        public SVFKManager(string iniPath = @"C:\PBU-DAT\SVFKA-USR.INI")
        {
            _iniPath = iniPath;

            if (!SVFKFileInfo.Exists)
            {
                throw new FileNotFoundException($"Unable to locate SVFKA ini file @ {_iniPath}");
            }

            _iniFile = new IniFile(_iniPath);

            _buttons = LoadButtons();
        }

        List<SVFKButton> LoadButtons()
        {

            
            var rtnList = new List<SVFKButton>();


            for(var i = 1; i < 8; i++)
            {
                var errMsgSb = new StringBuilder();
                try
                {
                    if(!_iniFile.SectionExists("SuppPath"))
                    {
                        errMsgSb.AppendLine("File Corrupt SuppPath does not exist");
                    }
                    if(!_iniFile.SectionExists("SuppCap"))
                    {
                        errMsgSb.AppendLine("File Corrupt SuppCap does not exist");
                    }
                    if(!_iniFile.SectionExists("SuppNum"))
                    {
                        errMsgSb.AppendLine("File Corrupt SuppNum does not exist");
                    }
                    if(!_iniFile.SectionExists("Supp-Start-Mode"))
                    {
                        errMsgSb.AppendLine("File Corrupt Supp-Start-Mode does not exist");
                    }
                    if(!_iniFile.SectionExists("Supp-Def-Mode"))
                    {
                        errMsgSb.AppendLine("File Corrupt Supp-Def-Mode does not exist");
                    }

                    if(errMsgSb.Length > 0)
                    {
                        throw new FormatException(errMsgSb.ToString());
                    }

                    var item = $"data{i}";
                    var supPath = _iniFile.ReadString("SuppPath", item);
                    var suppCap = _iniFile.ReadString("SuppCap", item);
                    var suppNum = _iniFile.ReadString("SuppNum", item);
                    int suppStartMode = int.TryParse(_iniFile.ReadString("Supp-Start-Mode", item), out suppStartMode)
                        ? suppStartMode
                        : 0;
                    int keyIndex = int.TryParse(_iniFile.ReadString("Supp-Def-Mode", item), out keyIndex) ? keyIndex : 0;

                    rtnList.Add(new SVFKButton()
                    {
                        FilePath = supPath,
                        DisplayName = suppCap,
                        Arguments = suppNum,
                        StartMode = (VKeyStartMode)suppStartMode
                    });
                }

                catch(Exception ex)
                {
                    throw new Exception($"Problem reading SVFK record index {i}. Call ResetIni() to load the factory default.",
                                        ex);
                }
            }

            return rtnList;
        }

        /// <summary>
        /// Reads the ini and finds a button without a SuppPath defined
        /// </summary>
        /// <returns>returns an avaiable SVFK if one is avaiable otherwise null</returns>
        public SVFKButton GetFirstAvailableButton() => _buttons.FirstOrDefault(b => b.FilePath == string.Empty);

        /// <summary>
        /// Changes to the SVFK will not show up unit you restart SVFK.exe
        /// Calling this will Kill the SVFK process and restart it.  
        /// Notice! this will cause a harmless level D alarm to show up that can be cleared by reset
        /// </summary>
        public void KillAndRestartSVFK()
        {
            var SVKFA = Process.GetProcessesByName("SVFKA");
            if(SVKFA.Any())
            {
                SVKFA[0].Kill();
            }
            if(File.Exists(@"C:\OSP-P\V-FKEY\SVFKA.EXE"))
            {
                Process.Start(@"C:\OSP-P\V-FKEY\SVFKA.EXE");
            }
        }

        /// <summary>
        /// Loads default SVFKA-USR.INI to C:\PBU-DAT\SVFKA-USR.INI and saves old one as *.Corrupt
        /// </summary>
        public void ResetIni()
        {
            try
            {
                if(SVFKFileInfo.Exists)
                {
                    File.Copy(_iniPath, Path.Combine(SVFKFileInfo.Directory.FullName, "SVFKA-USR.INI.CORRUPT"), true);
                    File.WriteAllText(_iniPath, Resources.DefaultIni, Encoding.ASCII);
                    _buttons = LoadButtons();
                }
            } catch(Exception ex)
            {
                throw new Exception("Problem creating resetting ini file.", ex);
            }
        }

        public void SaveButtons()
        {
            if(_buttons.Count < 7)
            {
                throw new IndexOutOfRangeException("The number of buttons currently in the list is out of range.");
            }

            for(var i = 0; i < 7; i++)
            {
                try
                {
                    var item = $"data{i + 1}";
                    _iniFile.WriteString("SuppPath", item, _buttons[i].FilePath);
                    _iniFile.WriteString("SuppCap", item, _buttons[i].DisplayName);
                    _iniFile.WriteString("SuppNum", item, _buttons[i].Arguments);
                    _iniFile.WriteString("Supp-Start-Mode", item, ((int)_buttons[i].StartMode).ToString());
                } catch(Exception ex)
                {
                    throw new Exception($"Problem saving buttons @ index {i}", ex);
                }
            }
        }

       /// <summary>
       /// Reloads the button list from disk
       /// </summary>
        public void RefreshButtons()
        {
            _buttons = LoadButtons();
        }

        public ReadOnlyCollection<SVFKButton> Buttons => new ReadOnlyCollection<SVFKButton>(_buttons);

        /// <summary>
        /// FileInfo for SVFKA-USR.INI. Use this to backup the file if desired.
        /// </summary>

      public FileInfo SVFKFileInfo
        {
            get
            {
                return new FileInfo(_iniPath);
            }
           
        }
       
    }
}
