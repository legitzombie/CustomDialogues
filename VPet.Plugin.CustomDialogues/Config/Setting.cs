using System.ComponentModel;
using LinePutScript;
using LinePutScript.Converter;
using VPet.Plugin.CustomDialogues.Core;
using VPet.Plugin.CustomDialogues.Sys;
using VPet.Plugin.CustomDialogues.Utilities;

namespace VPet.Plugin.CustomDialogues.Config
{
    public class Setting
    {
        private static Setting _instance;
        private static readonly object _lock = new object();

        public static Setting Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new Setting();
                    return _instance;
                }
            }
        }

        private int delayTimer = 60000; // loop interval
        private int delayTalk = 5000; // minimum delay between dialogue
        private int chanceTalk = 5; // chance to talk
        private bool debug = false; // debugging
        private bool soundeffect = false; // animation sound effects
        public string Name { set; get; } = "speech";


        [Line]
        public int DelayTimer
        {
            get => delayTimer;
            set { delayTimer = value; OnPropertyChanged(nameof(DelayTimer)); }
        }

        [Line]
        public int DelayTalk
        {
            get => delayTalk;
            set { delayTalk = value; OnPropertyChanged(nameof(DelayTalk)); }
        }

        [Line]
        public int ChanceTalk
        {
            get => chanceTalk;
            set { chanceTalk = value; OnPropertyChanged(nameof(ChanceTalk)); }
        }

        [Line]
        public bool Debug
        {
            get => debug;
            set { debug = value; OnPropertyChanged(nameof(Debug)); }
        }
        [Line]
        public bool SoundEffect
        {
            get => soundeffect;
            set { soundeffect = value; OnPropertyChanged(nameof(SoundEffect)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Loads settings from config.lps
        /// </summary>
        public void Load()
        {
            Setting loadedSettings = IOManager.LoadLPS<Setting>(FolderPath.Get(), "config")[0];
            Set(loadedSettings);
        }

        public void Save()
        {

            IOManager.SaveLPS(CustomDialogues.setting, FolderPath.Get(), "config", false, false);
            CustomDialogues.setting.Load();


        }
        private void Set(Setting loadedSettings)
        {
            DelayTimer = loadedSettings.DelayTimer;
            DelayTalk = loadedSettings.DelayTalk;
            ChanceTalk = loadedSettings.ChanceTalk;
            Debug = loadedSettings.Debug;
            SoundEffect = loadedSettings.SoundEffect;
        }

    }
}

