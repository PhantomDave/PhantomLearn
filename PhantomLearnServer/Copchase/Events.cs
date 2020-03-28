using System;
using CitizenFX.Core;

namespace PhantomLearnServer.Copchase
{
    internal class Events : BaseScript
    {
        public Events()
        {
            EventHandlers["plearn:joincopchase"] += new Action<Player, int>(OnCopChaseJoin);
            
        }

        private static void OnCopChaseJoin([FromSource] Player ply, int id2)
        {
            string id = ply.Handle;
            if (Main.CChaseList.Count < 7 && Main.CopChase.Started == false)
            {

                TriggerClientEvent("plearn:SendClientMessage", 0, 255, 0, "[CopChase]",
                    $"{ply.Name} joined the copchase! {Main.CChaseList.Count+1}/7");
            }
            else
            {
                TriggerClientEvent("plearn:SendClientMessage", 0, 255, 0, "[CopChase]",
                    "The Copchase is full, wait the next one!");
            }

            if (Main.CChaseList.Contains(id))
            {
                Main.CChaseList.Remove(id);

                TriggerClientEvent("plearn:SendClientMessage", 0, 255, 0, "[CopChase]",
                    $"{ply.Name} left the Copchase {Main.CChaseList.Count}/7");
                return;
            }
            Main.CChaseList.Add(id);
            if (Main.CChaseList.Count == 2) Main.StartCopchaseCountdown();
        }
    }
}