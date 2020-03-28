using System;
using System.Collections.Generic;
using System.Dynamic;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace PhantomLearnClient.Events
{
    public class ClientEvents : BaseScript
    {
        public ClientEvents()
        {
            EventHandlers["baseevents:onPlayerKilled"] += new Action<int, ExpandoObject>(OnPlayerKilled);
            EventHandlers["baseevents:onPlayerDied"] += new Action<int, List<dynamic>>(OnPlayerDied);
            EventHandlers["playerSpawned"] += new Action<ExpandoObject>(OnPlayerSpawn);
        }

        private static void OnPlayerSpawn(ExpandoObject obj)
        {
            API.SetCanAttackFriendly(API.GetPlayerPed(-1), true, false);
            API.NetworkSetFriendlyFireOption(true);
            Game.PlayerPed.Position = new Vector3(93.56f, -1939.94f, 20.70f);
        }

        private static void OnPlayerKilled(int killerid, ExpandoObject deathData)
        {
           if(Copchase.Main.CChaseList.Contains(API.GetPlayerPed(-1))) TriggerEvent("plearn:CChaseOnPlayerKilled", API.GetPlayerPed(-1), killerid, deathData);
        }

        public static void OnPlayerDied(int ped, dynamic deathCoords)
        {
            if (Copchase.Main.CChaseList.Contains(API.GetPlayerPed(-1))) TriggerEvent("plearn:CChaseOnPlayerDied", API.GetPlayerPed(-1), ped, deathCoords);
        }
    }
}