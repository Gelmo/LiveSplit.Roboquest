using LiveSplit.ComponentUtil;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace LiveSplit.RoboquestTimer
{
    public class Component : IComponent
    {
        private Settings settings;
        private Watchers _Watchers;

        public string ComponentName => "Roboquest Timer";

        public float PaddingTop => 0;
        public float PaddingLeft => 0;
        public float PaddingBottom => 0;
        public float PaddingRight => 0;

        public float VerticalHeight => 0;
        public float MinimumWidth => 0;
        public float HorizontalWidth => 0;
        public float MinimumHeight => 0;

        public IDictionary<string, Action> ContextMenuControls => null;

        private System.Diagnostics.Process process;

        private TimerModel _timer = new TimerModel();
        public event EventHandler TimerStart;
        public event EventHandler TimerReset;
        public event EventHandler TimerSplit;

        public Component(LiveSplitState state)
        {
            settings = new Settings();
            settings.HandleDestroyed += SettingsUpdated;
            SettingsUpdated(null, null);

            _timer.CurrentState = state;
            TimerStart += LSTimer_start;
            TimerReset += LSTimer_reset;
            TimerSplit += LSTimer_split;
        }

        private void SettingsUpdated(object sender, EventArgs e)
        {
            _Watchers = new Watchers(settings);
        }

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {
        }

        class Watchers : MemoryWatcherList
        {
            public MemoryWatcher<int> LastLevel { get; }
            public MemoryWatcher<int> GameLevel { get; }
            public MemoryWatcher<int> PlayerLevel { get; }
            public MemoryWatcher<float> GameTime { get; }
            public MemoryWatcher<float> GameTimeOnLevelStart { get; }
            public MemoryWatcher<float> TotalRunTime { get; }
            public MemoryWatcher<bool> BGameTimePaused { get; }
            public MemoryWatcher<bool> BIsDead { get; }
            public MemoryWatcher<bool> BCurrentlyFightingBoss { get; }

            public Watchers(Settings settings)
            {
                if (settings.RQVersion == "Steam")
                {
                    LastLevel = new MemoryWatcher<int>(new DeepPointer(0x04B427F0, 0x120, 0x3D0)) { Name = "LastLevel" };
                    GameLevel = new MemoryWatcher<int>(new DeepPointer(0x04B427F0, 0x120, 0x3D8)) { Name = "GameLevel" };
                    PlayerLevel = new MemoryWatcher<int>(new DeepPointer(0x04B427F0, 0x120, 0x5A0)) { Name = "PlayerLevel" };
                    GameTime = new MemoryWatcher<float>(new DeepPointer(0x04B427F0, 0x120, 0x858)) { Name = "GameTime" };
                    GameTimeOnLevelStart = new MemoryWatcher<float>(new DeepPointer(0x04B427F0, 0x120, 0x85C)) { Name = "GameTimeOnLevelStart" };
                    TotalRunTime = new MemoryWatcher<float>(new DeepPointer(0x04B427F0, 0x120, 0x860)) { Name = "TotalRunTime" };
                    BGameTimePaused = new MemoryWatcher<bool>(new DeepPointer(0x04B427F0, 0x120, 0x864)) { Name = "BGameTimePaused" };
                    BIsDead = new MemoryWatcher<bool>(new DeepPointer(0x04B427F0, 0x180, 0x38, 0x0, 0x30, 0x260, 0x88A)) { Name = "BIsDead" };
                    BCurrentlyFightingBoss = new MemoryWatcher<bool>(new DeepPointer(0x04B427F0, 0x180, 0x38, 0x0, 0x30, 0x260, 0x4EF9)) { Name = "BCurrentlyFightingBoss" };
                }
                else
                {
                    LastLevel = null;
                    GameLevel = null;
                    PlayerLevel = null;
                    GameTime = null;
                    GameTimeOnLevelStart = null;
                    TotalRunTime = null;
                    BGameTimePaused = null;
                    BIsDead = null;
                    BCurrentlyFightingBoss = null;
                }
            }
        }

        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            if (process != null && !process.HasExited && settings.RQVersion != null &&
                string.Equals(process.ProcessName, settings.ProcessName, StringComparison.OrdinalIgnoreCase))
            {
                _Watchers.LastLevel.Update(process);
                _Watchers.GameLevel.Update(process);
                _Watchers.PlayerLevel.Update(process);
                _Watchers.GameTime.Update(process);
                _Watchers.GameTimeOnLevelStart.Update(process);
                _Watchers.TotalRunTime.Update(process);
                _Watchers.BGameTimePaused.Update(process);
                _Watchers.BIsDead.Update(process);
                _Watchers.BCurrentlyFightingBoss.Update(process);

                state.SetGameTime(TimeSpan.FromSeconds(_Watchers.GameTime.Current));

                // If the in-game time is running and the in-game time was previously 0, start the timer
                if (_Watchers.GameTime.Current > 0 && _Watchers.GameTime.Old == 0)
                {
                    TimerStart?.Invoke(this, EventArgs.Empty);
                    state.SetGameTime(TimeSpan.FromSeconds(_Watchers.GameTime.Current));
                }

                // If the in-game timer is set to 0 and ResetGame is enabled, reset the timer. This occurs when restarting the run in-game, when you leave the Game Over screen, or when you go to Basecamp
                if (settings.ResetGame == true && _Watchers.GameTime.Current == 0 && _Watchers.GameTime.Old == 0)
                {
                    TimerReset?.Invoke(this, EventArgs.Empty);
                }

                // If the player has died and ResetDeath is enabled, reset the timer
                if (settings.ResetDeath == true && _Watchers.BIsDead.Current && !_Watchers.BIsDead.Old)
                {
                    TimerReset?.Invoke(this, EventArgs.Empty);
                }

                // If the game has updated TotalRunTime and the player has not died, split. This should only occur on the final split
                if (_Watchers.TotalRunTime.Current > 0 && _Watchers.TotalRunTime.Old == 0 && !_Watchers.BIsDead.Current)
                {
                    TimerSplit?.Invoke(this, EventArgs.Empty);
                }
                // Otherwise, if the current level differs from the level in the previous loop, split
                else if (_Watchers.GameLevel.Current != _Watchers.GameLevel.Old)
                {
                    TimerSplit?.Invoke(this, EventArgs.Empty);
                }

                if (invalidator != null)
                {
                    invalidator.Invalidate(0, 0, width, height);
                }
            }
            else
            {
                process = System.Diagnostics.Process.GetProcessesByName(settings.ProcessName).FirstOrDefault();
            }
        }

        public System.Windows.Forms.Control GetSettingsControl(LayoutMode mode)
        {
            return settings;
        }

        public void SetSettings(System.Xml.XmlNode settings)
        {
            this.settings.SetSettings(settings);
        }

        public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
        {
            return settings.GetSettings(document);
        }

        public int GetSettingsHashCode()
        {
            return settings.GetSettingsHashCode();
        }

        protected virtual void Dispose(bool disposing)
        {
            settings.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void LSTimer_start(object sender, EventArgs e)
        {
            _timer.Start();
        }

        void LSTimer_split(object sender, EventArgs e)
        {
            _timer.Split();
        }

        void LSTimer_reset(object sender, EventArgs e)
        {
            _timer.Reset();
        }
    }
}
