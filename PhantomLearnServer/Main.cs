using CitizenFX.Core;
using System;
using System.Collections.Generic;


namespace PhantomLearnServer
{
    public class Main : BaseScript
    {
        public static List<Player> PList = new List<Player>();

        public Main()
        {
            //DateBase.Init();
        }
        public static void Log(string text)
        {
            Console.WriteLine("[Phantom-Logs] " + text);
        }
    }
}
