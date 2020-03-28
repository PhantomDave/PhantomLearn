using System;
using System.IO;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace PhantomLearnServer.Events
{
    public class CommandEvents : BaseScript
    {
        public CommandEvents()
        {
            EventHandlers["plearn:savecommand"] += new Action<string, Vector4, string>(OnSaveCommand);
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

    }
}
