using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace PhantomLearnServer.Events
{
    public class ServerEvents : BaseScript
    {
        public ServerEvents()
        {
            EventHandlers["baseevents:onPlayerKilled"] += new Action<Player, int, ExpandoObject>(OnPlayerKilled);
            EventHandlers["baseevents:onPlayerDied"] += new Action<Player, int, List<dynamic>>(OnPlayerDied);
            EventHandlers["playerConnecting"] += new Action<Player, string, dynamic, dynamic>(OnPlayerConnecting);
            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);
            EventHandlers["plearn:log"] += new Action<string>(OnClientLogs);
        }

        private static void OnClientLogs(string text)
        {
            Main.Log(text);
        }

        private static async void OnPlayerConnecting([FromSource] Player player, string playerName, dynamic setKickReason,
            dynamic deferrals)
        {
            deferrals.defer();

            // mandatory wait!
            await Delay(0);

            var licenseIdentifier = player.Identifiers["license"];

            Main.Log(
                $"A player with the name {playerName} (Identifier: [{licenseIdentifier}]) is connecting to the server.");

            deferrals.update($"Hello {playerName}, your license [{licenseIdentifier}] is being checked");


            deferrals.done();

            Main.PList.Add(player);
        }

        private static void OnPlayerDropped([FromSource] Player player, string reason)
        {
            Debug.WriteLine($"Player {player.Name} dropped (Reason: {reason}).");
            Main.PList.Remove(player);
        }

        private static void OnPlayerDied([FromSource] Player ply, int killerType, List<dynamic> deathcords)
        {
            TriggerClientEvent("plearn:sendNotification", $"{ply.Name} died alone, by himself, how sad.");
        }

        private static void OnPlayerKilled([FromSource] Player ply, int killerid, ExpandoObject deathData)
        {
            int killertype;
            var deathCoords = new List<dynamic>();
            uint weaponhash;
            var isinVeh = false;
            string killedfrom;

            foreach (var data in deathData)
            {
                switch (data.Key)
                {
                    case "killertype":
                        killertype = (int) data.Value;
                        break;
                    case "weaponHash":
                        weaponhash = (uint) data.Value;
                        break;
                    case "killerInVeh":
                        isinVeh = (bool) data.Value;
                        break;
                    case "killerpos":
                        deathCoords = data.Value as List<dynamic>;
                        break;
                }

                if (!isinVeh) continue;
                if (data.Key == "killerVehName")
                    killedfrom = (string) data.Value;
            }

            var deathcoords = new Vector3((float) deathCoords[0], (float) deathCoords[1], (float) deathCoords[2]);

            var killer = API.GetPlayerFromIndex(killerid);
            TriggerClientEvent("plearn:sendNotification",
                killer != null
                    ? (string) $"{ply.Name} was killed by {killer} someone at {deathcoords}."
                    : (string) $"{ply.Name} was killed by someone at {deathcoords}.");
        }
    }
}