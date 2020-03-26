using System;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace PhantomLearnClient.Copchase
{
    public class Main : BaseScript
    {
        public Main()
        {
            EventHandlers["plearn:CreateCopChaseCar"] +=
                new Action<int, float, float, float, float, bool>(CreateChaseCarAsync);
            EventHandlers["plearn:startCopChase"] += new Action<int>(StartCopchase);
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
                GiveWeaponToPed(ply, (uint) GetHashKey("weapon_pistol"), 500, false, true);
                SetPlayerCanDoDriveBy(ply, false);
            }
            else
            {
                var veh = await World.CreateVehicle(841808271, new Vector3(x, y, z), a);
                SetVehicleOnGroundProperly(veh.Handle);
                SetVehicleNumberPlateText(veh.Handle, "FKpoLiSC");
                SetPedIntoVehicle(ply, veh.Handle, -1);
                SetPlayerControl(ply, false, 0);
                SetPlayerCanDoDriveBy(ply, false);
                GiveWeaponToPed(ply, (uint) GetHashKey("weapon_pistol"), 500, false, true);
            }
        }
    }
}