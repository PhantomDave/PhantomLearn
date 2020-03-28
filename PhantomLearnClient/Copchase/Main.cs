using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace PhantomLearnClient.Copchase
{
    public class Main : BaseScript
    {
        public static List<int> CChaseList = new List<int>();
        public class CopChase
        {
            public static int players;
            public static int fugitive;
        }
        public Main()
        {
            
        }

        public static void EndCopChase(int winner)
        {
            switch (winner)
            {
                case 0:
                {
                    Functions.SendNotification($"The Cops lost {CChaseList.Count} against 1! they are BAD", 0, 0, 0, 0,
                        false);
                    TriggerServerEvent("plearn:EndCopChase", 0);
                    break;
                }
                case 1:
                {
                    Functions.SendNotification($"The Cops won the copchase, well, {CChaseList.Count} against 1.", 0, 0, 0, 0,
                        false);
                    TriggerServerEvent("plearn:EndCopChase", 1);
                    break;
                }
            }
            CChaseList.Clear();
            CopChase.players = 0;
            CopChase.fugitive = 0;

        }
    }
}