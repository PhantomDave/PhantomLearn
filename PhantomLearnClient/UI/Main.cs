using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;

namespace PhantomLearnClient.UI
{
    public class MainMenu : BaseScript
    {
        private readonly MenuPool _menuPool;

        private readonly List<Vector3> _teleports = new List<Vector3>
        {
            new Vector3(93.56f, -1939.94f, 20.70f), // Goto LS
            new Vector3(-1619.058f, -3100.323f, 13.94475f), //LAX
            new Vector3(-1493.161f, -1388.568f, 2.147076f), // Santa Maria Beach
            new Vector3(1098.831f, 3023.568f, 40.53411f), // Goto Sandy Shores Airfield
            new Vector3(722.8278f, 4165f, 40.75159f), // Goto Lake
            new Vector3(501.2086f, 5596.275f, 795.7725f) // Goto Chilliad
        };

        private readonly List<string> _teleportNames = new List<string>
        {
            "Teleport to Groove Street",
            "Teleport to Los Santos Internetional Airport",
            "Teleport to Santa Maria Beach",
            "Teleport to Sandy Shores Airfield",
            "Teleport to the Alamo Sea",
            "Teleport to Mount Chilliad"
        };

        public MainMenu()
        {
            _menuPool = new MenuPool();
            var mainmenu = new UIMenu("Choose your menù", "Select something!");
            _menuPool.Add(mainmenu);
            AddTeleportMenu(mainmenu);
            AddWeaponMenu(mainmenu);
            _menuPool.RefreshIndex();
            _menuPool.MouseEdgeEnabled = false;

            Tick += async () =>
            {
                _menuPool.ProcessMenus();
                if (Game.IsControlJustPressed(0, Control.SelectCharacterFranklin) && !_menuPool.IsAnyMenuOpen()) // Our menu on/off switch
                    mainmenu.Visible = !mainmenu.Visible;
                    
            };
        }

        public void AddWeaponMenu(UIMenu menu)
        {
            var submenu = _menuPool.AddSubMenu(menu, "Weapons", "Select a weapon from the list");
            foreach (WeaponHash wep in Enum.GetValues(typeof(WeaponHash)))
            {

                var hash = Game.GetGXTEntry(Weapon.GetDisplayNameFromHash(wep));
                if (hash.Length == 0 || hash.Contains("Non valido")) continue;
                if(API.IsWeaponValid((uint)wep)) submenu.AddItem(new UIMenuItem($"{hash}", $"{(uint)wep}"));
            }

            submenu.OnItemSelect += (sender, item, index) =>
            {
                var wep = Convert.ToUInt32(item.Description);
                API.GiveWeaponToPed(API.GetPlayerPed(-1), wep, 100, false, true);
                Functions.SendNotification($"You were given a {item.Text} with 100 shots", 0, 0, 0, 0, false);
            };
        }

        public void AddTeleportMenu(UIMenu menu)
        {
            var submenu = _menuPool.AddSubMenu(menu, "Teleports", "Select the avaiable teleports");
            foreach (var i in _teleportNames) submenu.AddItem(new UIMenuItem(i));

            submenu.OnItemSelect += (sender, item, index) => { Game.PlayerPed.Position = _teleports[index]; };
        }
    }
}