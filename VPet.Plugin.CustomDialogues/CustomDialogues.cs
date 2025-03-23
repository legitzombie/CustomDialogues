using System.Windows;
using System.Windows.Controls;
using LinePutScript.Localization.WPF;
using VPet.Plugin.CustomDialogues.Config;
using VPet.Plugin.CustomDialogues.Core;
using VPet.Plugin.CustomDialogues.UI;
using VPet.Plugin.CustomDialogues.Utilities;
using VPet_Simulator.Windows.Interface;

namespace VPet.Plugin.CustomDialogues
{
    public class CustomDialogues : MainPlugin
    {
        private TimerManager GameTalkTimer;


        public static Setting setting = new Setting();

        public CustomDialogues(IMainWindow mainwin) : base(mainwin) { }

        public override void LoadPlugin()
        {

            setting.Load();

            GameTalkTimer = new TimerManager(setting.Name, setting.DelayTimer, setting.ChanceTalk);


            InitializeTimers();
            AddSettingsMenu();
        }

        private void InitializeTimers()
        {
            GameTalkTimer.AddOrUpdateTimer(() =>
                DialogueManager.HandleDialogue(MW, GameTalkTimer));
        }

        private void AddSettingsMenu()
        {
            var menuItem = new MenuItem
            {
                Header = "CustomDialogues".Translate(),
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            menuItem.Click += (s, e) => UIManager.Display();
            MW.Main.ToolBar.MenuMODConfig.Items.Add(menuItem);
        }

        public override string PluginName => "CustomDialogues";
    }
}
