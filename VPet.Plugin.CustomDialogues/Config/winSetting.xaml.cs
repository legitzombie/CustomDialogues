using System.Windows;
using VPet.Plugin.CustomDialogues.Config;

namespace VPet.Plugin.CustomDialogues
{
    /// <summary>
    /// Interaction logic for winSetting.xaml
    /// </summary>
    public partial class winSetting : Window
    {
        public Setting UserSettings { get; set; }

        public winSetting()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            UserSettings = CustomDialogues.setting;

            UserSettings.Load();

            DataContext = UserSettings;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            UserSettings.Save();
            Close();
        }
    }
}
