using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;


namespace PhantomLearnClient.Commands
{
    public class Client : BaseScript
    {
        public Client()
        {
            RegisterCommand("unfreeze",
                new Action<int, List<object>, string>((source, args, raw) =>
                {
                    SetPlayerControl(GetPlayerPed(-1), true, 0);
                }), false);
            RegisterCommand("car", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                string model;
                if (args.Count > 0)
                {
                    model = args[0].ToString();
                }
                else
                {
                    Functions.SendClientMessage(255, 0, 0, "[CarSpawner]", "You need to input a Model Name!");
                    return;
                }

                var hash = (uint) GetHashKey(model);

                if (!IsModelInCdimage(hash) || !IsModelAVehicle(hash))
                {
                    Functions.SendClientMessage(255, 0, 0, "[CarSpawner]", "Good Choice, but it does not exists!");
                    return;
                }

                var veh = await World.CreateVehicle(model, Game.PlayerPed.Position, Game.PlayerPed.Heading);


                Game.PlayerPed.SetIntoVehicle(veh, VehicleSeat.Driver);
                SetModelAsNoLongerNeeded(hash);
                Functions.SendClientMessage(0, 255, 0, "[CarSpawner]",
                    $"Good Choice, Enjoy your new {veh.DisplayName}");
            }), false);

            RegisterCommand("gotocoords", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (args.Count < 3)
                    Functions.SendClientMessage(255, 0, 0, "[GotoCoords]", "Usage: /gotocoords <X>, <Y>, <Z>");


                var pos = new Vector3(Convert.ToSingle(args[0]), Convert.ToSingle(args[1]), Convert.ToSingle(args[2]));

                Game.PlayerPed.Position = pos;
            }), false);

            RegisterCommand("goto", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (args.Count == 0)
                {
                    Functions.SendClientMessage(255, 0, 0, "[ServerMessage]", "Usage: /goto <playerid>");
                    return;
                }


                var otherid = GetPlayerPed(Convert.ToInt32(args[0]));

                if (!IsPedAPlayer(otherid))
                {
                    Functions.SendClientMessage(255, 0, 0, "[ServerMessage]",
                        "The player you choose dosen't exsists");
                    return;
                }

                var pos = GetEntityCoords(otherid, true);


                Functions.SendClientMessage(0, 0, 0, "[DEBUG]", $"{args[0]}, {otherid}, {pos}");

                Game.PlayerPed.Position = pos;
            }), false);

            RegisterCommand("copchase", new Action<int, List<object>, string>((source, args, raw) =>
            {
                Functions.SendClientMessage(255, 211, 0, "[CopChase]", $"{GetPlayerName(source)} joined the copchase!");

                TriggerServerEvent("plearn:joincopchase");
            }), false);

            RegisterCommand("wantedlevel", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (args.Count == 0)
                {
                    Functions.SendClientMessage(0, 0, 255, "[WantedLevel]", "Wanted Level Set to 0");
                    ClearPlayerWantedLevel(source);
                    return;
                }

                var level = Convert.ToInt32(args[0]);

                Functions.SendClientMessage(0, 0, 255, "[WantedLevel]", $"Wanted Level Set to {level}");

                SetPlayerWantedLevel(source, level, false);
            }), false);

            RegisterCommand("save", new Action<int, List<object>, string>((source, args, raw) =>
            {
                Functions.SendClientMessage(0, 0, 255, "[DEBUG]",
                    $"{GetPlayerName(source)}, saved the position: VECTOR3: {Game.PlayerPed.Position} Heading: {Game.PlayerPed.Heading}. // {args[0]}");

                TriggerServerEvent("plearn:Log",
                    $"{GetPlayerName(source)}, saved the position: VECTOR3: {Game.PlayerPed.Position} Heading: {Game.PlayerPed.Heading}. // {args[0]}");

                TriggerServerEvent("plearn:savecommand", GetPlayerName(source),
                    new Vector4(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z,
                        Game.PlayerPed.Heading), " " + args[0]);
            }), false);

            RegisterCommand("gotols", new Action<int, List<object>, string>((source, args, raw) =>
            {
                Game.PlayerPed.Position = new Vector3(93.56f, -1939.94f, 20.70f);
                Functions.SendClientMessage(0, 144, 144, "[ServerCommands]", "You have been teleported to LS");
            }), false);

            RegisterCommand("wep", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (args.Count == 0 || args.Count == 1)
                {
                    Functions.SendClientMessage(255, 0, 0, "[Weapons]", "Usage: /wep <weapon_name> <ammo>");
                    return;
                }

                var weapon = "weapon_" + args[0];

                var hash = (uint) GetHashKey(weapon);

                if (!IsWeaponValid(hash))
                {
                    Functions.SendClientMessage(255, 0, 0, "[Weapons]",
                        "Invalid weapon name, valid names: pistol, smg, snspistol");
                    return;
                }

                var ammoCount = Convert.ToInt32(args[1]);

                GiveWeaponToPed(GetPlayerPed(-1), hash, ammoCount, false, false);

                Functions.SendClientMessage(0, 255, 0, "[Weapons]",
                    $"Enjoy this {weapon} and it's {ammoCount} bullets");
            }), false);
        }
    }
}