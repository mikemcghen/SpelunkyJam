using System;
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

        RoomRenderer roomRenderer;

        RoomFileHandler fileHandler;

        EditMode currentMode = EditMode.Ground;

        private int[,] Grid;

        void Start()
        {
            fileHandler = new RoomFileHandler();
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
                EditMode.Entity => 2,
                EditMode.Ground => 1,
                _ => 0,
            };

        public void SetModeClear() => currentMode = EditMode.Clear;

        public void SetModeGround() => currentMode = EditMode.Ground;

        public void SetModeEntity() => currentMode = EditMode.Entity;

        public void LoadRoom(){
            if(roomSelector.options.Count == 0 || roomSelector.options[roomSelector.value].text == "") {
                Grid = new int[10,10];
            }else {
                var selectedValue = roomSelector.options[roomSelector.value].text;
                Grid = fileHandler.LoadRoom(selectedValue).Grid;
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
                Width = 10
            };
            fileHandler.SaveRoom(room, roomName);
            
            LoadRoomNames();
        }

        public void SaveNewRoom(){
            var regex = new Regex(@"\d");
            var newName = fileHandler.FetchRooms().Last();
            var num = int.Parse(regex.Match(newName).Value) + 1;
            SaveRoom($"room{num}");
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
        }
    }
}
