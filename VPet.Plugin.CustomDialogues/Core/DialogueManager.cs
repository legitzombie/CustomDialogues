using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;
using LinePutScript;
using LinePutScript.Converter;
using VPet.Plugin.CustomDialogues.Sys;
using VPet.Plugin.CustomDialogues.UI;
using VPet.Plugin.CustomDialogues.Utilities;
using VPet_Simulator.Windows.Interface;


namespace VPet.Plugin.CustomDialogues.Core
{
    public class DialogueManager
    {

        private static DialogueManager _instance;
        private static readonly object _lock = new object();

        public static DialogueManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new DialogueManager();
                    return _instance;
                }
            }
        }

        private static List<DialogueEntry> dialogues = IOManager.LoadLPS<DialogueEntry>(FolderPath.Get("text"));
        public static DialogueEntry dialogue { get; private set; }
        private static Random random = new Random();
        private static int lastDialogue = 0; // how long since last dialogue

        public static void playEffect(bool play)
        {
            if (!play) return;

            SoundPlayer player = new SoundPlayer(Path.Combine(FolderPath.Get("sound"), dialogue.SoundEffect));
            player.Play();
        }

        public static void Talk(IMainWindow main, TimerManager timer, string msg = null)
        {
                main.Main.Say(msg ?? dialogue?.Dialogue ?? string.Empty);


        }

        public static bool canTalk(int timerElapsed, TimerManager timer)
        {
            return timerElapsed > timer.interval && random.Next(100) < timer.chance;
        }

        public static void PlayDialogue(IMainWindow MW, TimerManager timer)
        {
            AnimationManager.Instance.updateAnimation(MW);
            if (CustomDialogues.setting.Debug && timer.name == "speech") Talk(MW, timer, AnimationManager.Instance.debugMessage());
            else
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    GetRandomDialogue(MW, timer);
                    Talk(MW, timer);
                    if (timer.name == "speech")  playEffect(CustomDialogues.setting.SoundEffect);
                });
            }
        }

        public static void HandleDialogue(IMainWindow MW, TimerManager timer)
        {
            string name = timer.name;
            int delay = timer.interval;
            int chance = timer.chance;


            lastDialogue += delay;

            if (name == "speech") timer.UpdateTimerInterval(name);
            else if (name == "AIspeech") timer.UpdateTimerInterval(name);

            if (canTalk(lastDialogue, timer)) PlayDialogue(MW, timer);
        }


        /// <summary>
        /// Selects a random dialogue entry based on Type, Name, and Mood
        /// </summary>
        public static void GetRandomDialogue(IMainWindow MW, TimerManager timer)
        {

            var type = AnimationManager.Instance.animation.Type.ToString();
            var name = AnimationManager.Instance.animation.Name.ToString();
            var mood = AnimationManager.Instance.animation.ModeType.ToString();

            List<DialogueEntry> filteredDialogues = new List<DialogueEntry>();


                filteredDialogues = dialogues.Where(d =>
                string.Equals(d.Type, type, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(d.Mood, mood, StringComparison.OrdinalIgnoreCase) &&
                (type.Equals("default", StringComparison.OrdinalIgnoreCase) || string.Equals(d.Name, name, StringComparison.OrdinalIgnoreCase))
                ).ToList();
                if (filteredDialogues.Count == 0) { return; }
                dialogue = filteredDialogues[random.Next(filteredDialogues.Count)];


            if (dialogue.Dialogue.Length > 0) lastDialogue = 0;
        }

    }

    public class DialogueEntry
    {
        [Line] public string Type { get; set; }
        [Line] public string Name { get; set; }
        [Line] public string Mood { get; set; }
        [Line] public string Dialogue { get; set; }
        [Line] public string SoundEffect { get; set; }
    }
}
