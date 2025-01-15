using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using static TMPro.TMP_Dropdown;

namespace Assets.Scripts
{
    public class RoomEditorManager : MonoBehaviour
    {
        public Tilemap Tilemap;

        public TMP_Dropdown roomSelector;

        public TMP_Dropdown modeSelector;

        public TMP_InputField designationInput;

        RoomRenderer roomRenderer;

        RoomFileHandler fileHandler;

        EditMode currentMode = EditMode.Clear;

        private int[,] Grid;
        
        string[] modes = new string[] {
            "Clear",
            "Ground",
            "Entity",
            "Player Spawn",
            "Ladder",
            "One-Way Platform",
            "Ladder & Platform"
        };

        void Start()
        {
            fileHandler = new RoomFileHandler();

            modeSelector.options.Clear();
            modeSelector.AddOptions(modes.ToList());
            modeSelector.value = 0;

            LoadRoomNames();
            LoadRoom();
        }

        private void LoadRoomNames(){
            roomSelector.ClearOptions();
            var rooms = fileHandler.FetchRooms().Select(x => new OptionData(x)).ToList();
            roomSelector.AddOptions(rooms);
        }

        public void SetGridValue(int x, int y)
        {
            var val = GetValue(currentMode);

            if (Grid[x, y] == val)
                return;

            Grid[x, y] = val;

            roomRenderer.RenderRoom(Grid);
        }

        private int GetValue(EditMode mode) =>
            mode switch
            {
                EditMode.Clear => 0,
                EditMode.Ground => 1,
                EditMode.Entity => 2,
                EditMode.PlayerSpawn => 3,
                EditMode.Ladder => 4,
                EditMode.OneWayPlatform => 5,
                EditMode.LadderAndPlatform => 6,
                _ => -1,
            };

        public void ChangeSelectionMode() =>
            currentMode = modeSelector.value switch {
                0 => EditMode.Clear,
                1 => EditMode.Ground,
                2 => EditMode.Entity,
                3 => EditMode.PlayerSpawn,
                4 => EditMode.Ladder,
                5 => EditMode.OneWayPlatform,
                6 => EditMode.LadderAndPlatform,
                _ => EditMode.Ground
            };

        public void LoadRoom(){
            if(roomSelector.options.Count == 0 || roomSelector.options[roomSelector.value].text == "") {
                Grid = new int[10,10];
            }else {
                var selectedValue = roomSelector.options[roomSelector.value].text;
                var room = fileHandler.LoadRoom(selectedValue);
                Grid = room.Grid;
                designationInput.text = $"{room.Designation}";
            }

            roomRenderer = new RoomRenderer(Tilemap);
            roomRenderer.RenderRoom(Grid);
        }

        public void SaveRoom(string roomName)
        {
            var room = new Room
            {
                Grid = Grid,
                Height = 10,
                Width = 10,
                Designation = int.Parse(designationInput.text)
            };
            fileHandler.SaveRoom(room, roomName);
            
            LoadRoomNames();
        }

        public void SaveNewRoom(){
            var regex = new Regex(@"\d+");
            
            var newName = fileHandler.FetchRooms().Last();
            var num = int.Parse(regex.Match(newName).Value) + 1;
            var numStr = $"{num}".PadLeft(4, '0');
            SaveRoom($"room{numStr}");
        }

        public void OverwriteRoom(){
            var selectedValue = roomSelector.options[roomSelector.value].text;
            SaveRoom(selectedValue);
        }

        private enum EditMode
        {
            Clear,
            Ground,
            Entity,
            PlayerSpawn,
            Ladder,
            OneWayPlatform,
            LadderAndPlatform
        }
    }
}
