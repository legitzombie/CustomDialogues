using VPet_Simulator.Core;
using VPet_Simulator.Windows.Interface;

namespace VPet.Plugin.CustomDialogues.Core
{
    public class AnimationManager
    {

        private static AnimationManager _instance;
        private static readonly object _lock = new object();

        public static AnimationManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new AnimationManager();
                    return _instance;
                }
            }
        }

        public GraphInfo animation { get; private set; } = new GraphInfo();

        public void updateAnimation(IMainWindow main)
        {
            animation = main.Main.DisplayType;
        }
        public string debugMessage()
        {
            return $"Animation Type: {animation.Type} \n Animation Name: {animation.Name} \n Animation Mood: {animation.ModeType} \n Animation Animat: {animation.Animat}";
        }
    }
}
