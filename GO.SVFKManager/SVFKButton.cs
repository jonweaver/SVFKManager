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
        public string FilePath { get; set; }
        /// <summary>
        /// The button caption (use '/n' for new line)
        /// </summary>
        public string DisplayName { get; set; }
        public string Arguments { get; set; }
        public VKeyStartMode StartMode { get; set; }
    }
}
