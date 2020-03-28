using System;
using System.Dynamic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace PhantomLearnClient.Copchase
{
    public class Events : BaseScript
    {
        public Events()
        {
            EventHandlers["plearn:CreateCopChaseCar"] +=
                new Action<int, float, float, float, float, bool>(CreateChaseCarAsync);
            EventHandlers["plearn:startCopChase"] += new Action<int>(StartCopchase);
            EventHandlers["plearn:CChaseOnPlayerKilled"] +=
                new Action<int, int, ExpandoObject>(CopChaseOnPlayerKilled);
            EventHandlers["pLearn:CChaseOnPlayerDied"] += new Action<int, int, dynamic>(CopChaseOnPlayerDied);
        }

        private static void CopChaseOnPlayerDied(int ply, int ped, dynamic deathcoords)
        {
            Main.CopChase.players--;
            if (Main.CopChase.players == 1 && Main.CopChase.fugitive != ply)
            {
                TriggerServerEvent("plearn:EndCopChase", ply, 1);
                Main.EndCopChase(0);
            }
            if (Main.CopChase.fugitive == ply)
            {
                TriggerServerEvent("plearn:EndCopChase", ply, 0);
                Main.EndCopChase(1);
            }
            Functions.SendNotification(
                $"{ply} died by the hand of nobody and lost the copchase!", 0, 0, 0, 0, false);
        }

        private static void CopChaseOnPlayerKilled(int ply, int killerid, ExpandoObject deathdata)
        {
            Main.CopChase.players--;
            if (Main.CopChase.players == 1 && Main.CopChase.fugitive != ply)
            {
                TriggerServerEvent("plearn:EndCopChase", ply, 1);
                Main.EndCopChase(0);
            }
            if (Main.CopChase.fugitive == ply)
            {
                TriggerServerEvent("plearn:EndCopChase", ply, 0);
                Main.EndCopChase(1);
            }
            Functions.SendNotification(
                $"{ply} died by the hand of {GetPlayerName(killerid)} and lost the copchase!", 0, 0, 0, 0, false);

        }

        private static void StartCopchase(int ply)
        {
            SetPlayerControl(ply, true, 0);
            Functions.SendClientMessage(0, 0, 255, "[Copchase]", "The Copchase started, Good Luck");
        }

        private static async void CreateChaseCarAsync(int ped, float x, float y, float z, float a, bool isCrim)
        {
            var ply = GetPlayerPed(ped);

            if (isCrim == false)
            {
                var veh = await World.CreateVehicle(2046537925, new Vector3(x, y, z), a);
                SetVehicleOnGroundProperly(veh.Handle);
                SetVehicleNumberPlateText(veh.Handle, "poLiSCe");
                SetPedIntoVehicle(ply, veh.Handle, -1);
                SetPlayerControl(ply, false, 0);
                GiveWeaponToPed(ply, (uint)GetHashKey("weapon_pistol"), 500, false, true);
                SetPlayerCanDoDriveBy(ply, false);
                Main.CopChase.players++;
            }
            else
            {
                var veh = await World.CreateVehicle(841808271, new Vector3(x, y, z), a);
                SetVehicleOnGroundProperly(veh.Handle);
                SetVehicleNumberPlateText(veh.Handle, "FKpoLiSC");
                SetPedIntoVehicle(ply, veh.Handle, -1);
                SetPlayerControl(ply, false, 0);
                SetPlayerCanDoDriveBy(ply, false);
                GiveWeaponToPed(ply, (uint)GetHashKey("weapon_pistol"), 500, false, true);
                Main.CopChase.players++;
                Main.CopChase.fugitive = ply;
            }
        }
    }
}
