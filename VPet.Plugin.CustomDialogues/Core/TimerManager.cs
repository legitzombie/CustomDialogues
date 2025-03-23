using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;

namespace VPet.Plugin.CustomDialogues.Core
{
    public class TimerManager
    {
        private Dictionary<string, Timer> timers = new Dictionary<string, Timer>();
        private Dictionary<string, ElapsedEventHandler> eventHandlers = new Dictionary<string, ElapsedEventHandler>();

        public TimerManager(string name, int delay, int chance)
        {
            this.name = name;
            interval = delay;
            this.chance = chance;
        }

        private void updateData()
        {
                interval = CustomDialogues.setting.DelayTimer; chance = CustomDialogues.setting.ChanceTalk;
        }

        public string name { get; set; }
        public int interval { get; set; }
        public int chance { get; set; }

        public void AddOrUpdateTimer(Action callback)
        {


            if (timers.ContainsKey(name))
            {
                UpdateTimerInterval(name);
                return;
            }

            Timer timer = new Timer(interval)
            {
                AutoReset = true, // Ensure it repeats
                Enabled = true
            };

            ElapsedEventHandler handler = (sender, e) =>
            {
                try
                {
                    callback();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Timer callback error: {ex.Message}");
                }
            };

            timer.Elapsed += handler;
            eventHandlers[name] = handler;
            timers[name] = timer;

        }

        public void UpdateTimerInterval(string name)
        {
            updateData();
            if (timers.TryGetValue(name, out Timer timer))
            {
                timer.Stop();

                timer.Start(); 
            }
        }

        public void RemoveTimer(string name)
        {
            if (timers.TryGetValue(name, out Timer timer))
            {
                if (eventHandlers.TryGetValue(name, out ElapsedEventHandler handler))
                {
                    timer.Elapsed -= handler;
                    eventHandlers.Remove(name);
                }

                timer.Stop();
                timer.Dispose();
                timers.Remove(name);
            }
        }

        public void StopAllTimers()
        {
            foreach (var kvp in timers)
            {
                kvp.Value.Stop();
                kvp.Value.Dispose();
            }
            timers.Clear();
            eventHandlers.Clear();
        }
    }
}
