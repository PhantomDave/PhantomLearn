using System.Collections.Generic;
using CitizenFX.Core;

namespace PhantomLearnServer.Copchase
{
    public class Main : BaseScript
    {
        public static List<string> CChaseList = new List<string>();
        public class CopChase
        {
            public static bool Joinable { get; set; }
            public static bool Started;
            public static int CopsInChase { get; set; }
            public static int TimeRemaning { get; set; }
        }

        public static async void StartCopchaseCountdown()
        {
            TriggerClientEvent("plearn:SendClientMessage", 0, 255, 0, "[CopChase]",
                "The copchase will start in 30 seconds!");
          
            var i = 30;
            while(i != 0)
            {
                await Delay(1000);
                TriggerClientEvent("plearn:sendNotification", $"The copchase will start in {i} seconds!");

                if (i == 5 && CChaseList.Count > 0)
                {
                    CopChase.Joinable = true;
                    CopChase.Started = false;
                    CopChasePrestart();
                }
                else if(CChaseList.Count < 2)
                {
                    TriggerClientEvent("plearn:SendClientMessage", 255, 0, 0, "[CopChase]",
                        "The Copchase didin't start because there weren't enough players!");
                    return;
                }

                i--;
            }
            StartCopChase();
        }

        private static void StartCopChase()
        {
            CopChase.Started = true;
            CopChase.CopsInChase = CChaseList.Count - 1;
            CopChase.TimeRemaning = 10;
            foreach (var i in CChaseList)
            {
                TriggerClientEvent("plearn:startCopChase", i);
                TriggerClientEvent("plearn:sendNotification", "The Copchase started now!");
            }
        }

        private static void CopChasePrestart()
        {
            for (var i = 0; i < CChaseList.Count; i++)
            {
                TriggerClientEvent("plearn:CreateCopChaseCar", CChaseList[i],
                    CopChaseVehicles[i].X, CopChaseVehicles[i].Y, CopChaseVehicles[i].Z, CopChaseVehicles[i].W, false);

                if (i == CChaseList.Count - 1)
                    TriggerClientEvent("plearn:CreateCopChaseCar", CChaseList[i],
                        CopChaseVehicles[6].X, CopChaseVehicles[6].Y, CopChaseVehicles[6].Z, CopChaseVehicles[6].W,
                        true);
            }
        }

        private static readonly List<Vector4> CopChaseVehicles = new List<Vector4>
        {
            new Vector4(100.1886f, -1949.337f, 20.44338f, 36.35664f), // Police1
            new Vector4(98.82269f, -1940.976f, 20.56043f, 36.36863f), // Police2
            new Vector4(104.3484f, -1934.545f, 20.56008f, 56.46371f), // Police3
            new Vector4(114.3945f, -1938.217f, 20.45716f, 66.25507f), // Police4
            new Vector4(104.3117f, -1940.572f, 20.41029f, 46.99102f), // Police5
            new Vector4(110.8860f, -1944.439f, 20.38387f, 45.79087f), // Police6
            new Vector4(84.84459f, -1923.38f, 20.60431f, 49.42601f) // crim
        };
    }
}