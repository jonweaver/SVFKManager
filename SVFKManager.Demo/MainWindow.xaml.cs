namespace SVFKMgr.Demo
{
    using GO.Utilities.VFK;
    using Microsoft.Win32;
    using SVFKMgr.Demo.Domain;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            SVFKManager = new GO.Utilities.VFK.SVFKManager();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void btnResetIni_Click(object sender, RoutedEventArgs e)
        {
            SVFKManager.ResetIni();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SVFKManager)));
        }

        void btnRestart_Click(object sender, RoutedEventArgs e) => SVFKManager.KillAndRestartSVFK();

        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SVFKManager.SaveButtons();
            MessageBox.Show("Save Complete!");
        }

        void btnShowIni_Click(object sender, RoutedEventArgs e) => Process.Start("notepad.exe",
                                                                                 SVFKManager.SVFKFileInfo.FullName);

        public SVFKManager SVFKManager
        {
            get;
        }
        
        #region Browse Command

        ICommand _browse;

        public ICommand Browse => _browse ?? (_browse = new CommandHandler(param => ExecuteBrowse(param), true));

        void ExecuteBrowse(object button)
        {
            if(button.GetType() != typeof(SVFKButton)) return;

            var btn = (SVFKButton)button;

            var fileDialog = new OpenFileDialog()
            {
                InitialDirectory = File.Exists(btn.FilePath) ? btn.FilePath : @"C:\",
                Multiselect = false,
            };

            var res = fileDialog.ShowDialog();

            if(res == true)
            {
                btn.FilePath = fileDialog.FileName;
            }
        }
        #endregion
    }
}
