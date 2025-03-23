namespace VPet.Plugin.CustomDialogues.UI
{
    public static class UIManager
    {
        private static winSetting winSetting;
        public static void Display()
        {
            if (winSetting != null)
            {
                winSetting.Activate();
                return;
            }
            else
            {
                winSetting = new winSetting();
                winSetting.Closed += (s, e) => { winSetting = null; };
                winSetting.Show();
            }
        }


    }
}
