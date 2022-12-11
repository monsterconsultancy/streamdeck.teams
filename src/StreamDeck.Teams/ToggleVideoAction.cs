using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using StreamDeckLib;
using StreamDeckLib.Messages;
using System.Threading.Tasks;

using Desktop.Robot;
using Desktop.Robot.Extensions;

namespace StreamDeck.Teams
{
    [ActionUuid(Uuid = "com.monsterconsultancy.teams.ToggleVideoAction")]
    public class ToggleVideoAction : BaseStreamDeckActionWithSettingsModel<Models.CounterSettingsModel>
    {
        private readonly Robot _robot = new();

        public override async Task OnKeyUp(StreamDeckEventPayload args)
        {
            SettingsModel.Counter += 1;
            ActivateApp("Teams");
            
            _robot.CombineKeys(Key.Control, Key.Shift, Key.A);

            //update settings
            await Manager.SetSettingsAsync(args.context, SettingsModel);
        }

        public override async Task OnDidReceiveSettings(StreamDeckEventPayload args)
        {
            await base.OnDidReceiveSettings(args);
            //await Manager.SetTitleAsync(args.context, SettingsModel.Counter.ToString());
        }

        public override async Task OnWillAppear(StreamDeckEventPayload args)
        {
            await base.OnWillAppear(args);
            await Manager.SetTitleAsync(args.context, $"WA \n{DateTime.Now:HH:mm:ss}");
            //await Manager.SetTitleAsync(args.context, SettingsModel.Counter.ToString());
        }

        public override async Task OnApplicationDidLaunchAsync(StreamDeckEventPayload args)
        {
            await base.OnApplicationDidLaunchAsync(args);
            await Manager.SetTitleAsync(args.context, $"L \n{DateTime.Now:HH:mm:ss}");
        }

        public override async Task OnApplicationDidTerminateAsync(StreamDeckEventPayload args)
        {
            await base.OnApplicationDidTerminateAsync(args);
            await Manager.SetTitleAsync(args.context, $"T \n{DateTime.Now:HH:mm:ss}");
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        static bool ActivateApp(string processName)
        {
            Process[] p = Process.GetProcessesByName(processName);

            // Activate the first application we find with this name
            if (p.Length > 0)
            {
                SetForegroundWindow(p[0].MainWindowHandle);
                return true;
            }

            return false;
        }
    }
}