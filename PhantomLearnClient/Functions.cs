using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace PhantomLearnClient
{
    public class Functions : BaseScript
    {
        public Functions()
        {
            EventHandlers["plearn:sendNotification"] +=
                new Action<string, int, int, int, int>(SendNotificationFromServer);
            EventHandlers["plearn:SendClientMessage"] +=
                new Action<int, int, int, string, string, bool>(SendClientMessage);
        }

        public static void SendClientMessage(int r, int g, int b, string source, string text, bool multiline = true)
        {
            TriggerEvent("chat:addMessage", new
            {
                color = new[] {r, g, b},
                multiline,
                args = new[] {source, text}
            });
        }


        private static void SendNotificationFromServer(string text, int r, int g, int b, int a)
        {
            SendNotification(text, r, g, b, a, true);
        }

        public static int SendNotification(string text, int r, int g, int b, int a, bool centre)
        {
            API.SetNotificationTextEntry("STRING");
            API.AddTextComponentString(text);
            //API.SetNotificationMessage(, name, false, 0, title, subtitle);
            return API.DrawNotification(false, false);
        }
    }
}