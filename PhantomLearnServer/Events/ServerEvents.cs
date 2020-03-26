using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using CitizenFX.Core;
using CitizenFX.Core.Native;

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
            EventHandlers["plearn:savecommand"] += new Action<string, Vector4, string>(OnSaveCommand);
            EventHandlers["plearn:joincopchase"] += new Action<Player, int>(OnCopChaseJoin);
        }

        private static void OnCopChaseJoin([FromSource] Player ply, int id)
        {


            if (Main.CChaseList.Contains((id)))
            {
                Main.CChaseList.Remove(id);

                TriggerClientEvent("plearn:SendClientMessage", 0, 255, 0, "[CopChase]",
                    $"{ply.Name} left the Copchase {Main.CChaseList.Count}/7");
                return;
            }



            if (Main.CChaseList.Count < 7 && Copchase.Main.CopChase.Started == false)
            {
                Main.CChaseList.Add(id);

                TriggerClientEvent("plearn:SendClientMessage", 0, 255, 0, "[CopChase]",
                    $"{ply.Name} joined the copchase! {Main.CChaseList.Count}/7");
            }
            else
            {
                TriggerClientEvent("plearn:SendClientMessage", 0, 255, 0, "[CopChase]",
                    "The Copchase is full, wait the next one!");
            }

            if (Main.CChaseList.Count == 2) Copchase.Main.StartCopchaseCountdown();
        }

        private static void OnSaveCommand(string sender, Vector4 pos, string comment)
        {
            var path = API.GetResourcePath(API.GetCurrentResourceName()) + "\\savedpos.txt";

            if (File.Exists(path))
            {
                Main.Log("File Exists!");
            }
            else if (!File.Exists(path))
            {
                Main.Log($"Dosen't Exists {path}!");
                try
                {
                    File.Create(path);
                }
                catch (Exception ex)
                {
                    Main.Log($"{ex.Message}");
                    return;
                }

                Main.Log($"Il file è stato creato {path}!");
            }

            try
            {
                using (var sw = new StreamWriter(path, true))
                {
                    sw.WriteLine($"{sender}, saved the position: Vector4: {pos}. // {comment}");
                }
            }
            catch (Exception ex)
            {
                Main.Log($"{ex.Message}");
            }
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
            //Vector3 coords = new Vector3((float)deathcords[0], (float)deathcords[1], (float)deathcords[2]);
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
                if (data.Key == "killertype") killertype = (int) data.Value;
                if (data.Key == "weaponHash") weaponhash = (uint) data.Value;
                if (data.Key == "killerInVeh") isinVeh = (bool) data.Value;
                if (data.Key == "killerpos") deathCoords = data.Value as List<dynamic>;
                if (isinVeh)
                    if (data.Key == "killerVehName")
                        killedfrom = (string) data.Value;
            }

            var deathcoords = new Vector3((float) deathCoords[0], (float) deathCoords[1], (float) deathCoords[2]);

            var killer = API.GetPlayerFromIndex(killerid);
            TriggerClientEvent("plearn:sendNotification",
                $"{ply.Name} was killed by {killer} someone at {deathcoords}.");
        }
    }
}