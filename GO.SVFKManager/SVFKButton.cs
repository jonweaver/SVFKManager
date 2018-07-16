namespace GO.Utilities.VFK
{
    using GO.Utilities.VFK.Enumerations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    public class SVFKButton : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        string _filePath;
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                if (_filePath == value)
                    return;
                _filePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilePath)));
            }
        }
        string _displayName;
        /// <summary>
        /// The button caption (use '/n' for new line)
        /// </summary>
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                if (_displayName == value)
                    return;
                _displayName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
            }
        }
        string _arguments;
        public string Arguments
        {
            get
            {
                return _arguments;
            }
            set
            {
                if (_arguments == value)
                    return;
                _arguments = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Arguments)));
            }
        }
        VKeyStartMode _startMode;
        public VKeyStartMode StartMode
        {
            get
            {
                return _startMode;
            }
            set
            {
                if (_startMode == value)
                    return;
                _startMode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartMode)));
            }
        }
        
    }
}
