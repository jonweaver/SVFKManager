namespace SVFKManager.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using GO.Utilities.VFK;
    using System.IO;
    using SVFKManager.Tests.Properties;
    using System.Text;
    using System.Linq;
    using Ini.Net;
    using GO.Utilities.VFK.Enumerations;

    [TestClass]
    public class SVFKManagerTests
    {
        private string _testFilePath;
        private SVFKManager _svfkManager;
        public SVFKManagerTests()
        {

            _testFilePath = "TEST.ini";
            File.WriteAllText(_testFilePath, Resources.DefaultIni, Encoding.ASCII);

        }

        [TestInitialize()]
        public void Initialize()
        {
            _svfkManager = new SVFKManager(_testFilePath);
            
        }

        [TestCleanup()]
        public void Cleanup()
        {
            File.Delete(this._testFilePath);
        }

        [TestMethod]
        public void TestUsingTestFile()
        {
            Assert.AreEqual(_testFilePath, _svfkManager.SVFKFileInfo.Name);
        }

        [TestMethod]
        public void TestIniFileShouldExist()
        {
            bool fileExists = File.Exists(this._testFilePath);
            Assert.IsTrue(fileExists);
        }

        [TestMethod]
        public void TestButtonListNotNull()
        {
            var btnList = _svfkManager.Buttons;

            Assert.IsNotNull(btnList);
        }

        [TestMethod]
        public void TestButtonListLength()
        {
            var btnList = _svfkManager.Buttons;

            Assert.AreEqual(btnList.Count, 7);
        }

        [TestMethod]
        public void TestButtonsSaveFilePath()
        {
            var btn = _svfkManager.Buttons.First();

            btn.FilePath = @"C:/System32/notepad.exe";

            _svfkManager.SaveButtons();

            //Read ini file to check save
            var iniFile = new IniFile(_testFilePath);

            var suppPath = iniFile.ReadString("SuppPath", "data1");

            Assert.AreEqual(suppPath, btn.FilePath);

        }

        [TestMethod]
        public void TestButtonsSaveDisplayName()
        {
            var btn = _svfkManager.Buttons.First();

            btn.DisplayName = "notepad";

            _svfkManager.SaveButtons();

            //Read ini file to check save
            var iniFile = new IniFile(_testFilePath);

            var suppCap = iniFile.ReadString("SuppCap", "data1");

            Assert.AreEqual(suppCap, btn.DisplayName);

        }

        [TestMethod]
        public void TestButtonsSaveArguments()
        {
            var btn = _svfkManager.Buttons.First();

            btn.Arguments = @"/?";

            _svfkManager.SaveButtons();

            //Read ini file to check save
            var iniFile = new IniFile(_testFilePath);

            var suppNum = iniFile.ReadString("SuppNum", "data1");

            Assert.AreEqual(suppNum, btn.Arguments);

        }

        [TestMethod]
        public void TestButtonsSaveStartMode()
        {
            var btn = _svfkManager.Buttons.First();

            btn.StartMode = VKeyStartMode.MultiInstance;

            _svfkManager.SaveButtons();

            //Read ini file to check save
            var iniFile = new IniFile(_testFilePath);

           int suppStartMode = int.TryParse(iniFile.ReadString("Supp-Start-Mode", "data1"), out suppStartMode)
                       ? suppStartMode
                       : 0;
            
            Assert.AreEqual(suppStartMode, (int)VKeyStartMode.MultiInstance);
        }

        [TestMethod]
        public void TestInitializeFile()
        {

            var btn = _svfkManager.Buttons.First();

            var fp = @"C:/System32/notepad.exe";

            btn.FilePath = fp;

            _svfkManager.SaveButtons();

            _svfkManager.RefreshButtons();

            btn = _svfkManager.Buttons.First();

            Assert.AreEqual(fp, btn.FilePath);

            _svfkManager.ResetIni();

            btn = _svfkManager.Buttons.First();

            Assert.AreNotEqual(btn.FilePath, fp);

        }

        [TestMethod]
        public void TestRefreshButtons()
        {

            File.WriteAllText(_testFilePath, Resources.FullIni, Encoding.ASCII);
            _svfkManager.RefreshButtons();

            var path = _svfkManager.Buttons.First().FilePath;

            Assert.AreNotEqual(path, string.Empty);

            File.WriteAllText(_testFilePath, Resources.DefaultIni, Encoding.ASCII);
            _svfkManager.RefreshButtons();

            Assert.AreNotEqual(path, _svfkManager.Buttons.First().FilePath);

        }


        [TestMethod]
        public void TestGetFirstAvailableIsNullWhenFull()
        {

            File.WriteAllText(_testFilePath, Resources.FullIni, Encoding.ASCII);
            _svfkManager.RefreshButtons();

            var firstAvailable =_svfkManager.GetFirstAvailableButton();

            Assert.IsNull(firstAvailable);

            _svfkManager.ResetIni();
        }

        [TestMethod]
        public void TestGetFirstAvailable()
        {
            _svfkManager.ResetIni();
            var firstAvailable = _svfkManager.GetFirstAvailableButton();

            Assert.IsNotNull(firstAvailable);
        }
    }
}
