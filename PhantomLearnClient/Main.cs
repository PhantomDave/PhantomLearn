using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.Native;

namespace PhantomLearnClient
{
    public class Main : BaseScript
    {
        public Main()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private static void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

                        
        }
    }
}
