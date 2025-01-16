using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Assets.Scripts {
    public class LevelGenerator : MonoBehaviour {

        public Grid gameGrid;

        public Tilemap levelMap;
        public Tilemap ladderMap;

        public GameObject playerPrefab;

        RoomRenderer roomRenderer;
        RoomRenderer ladderRenderer;
        RoomFileHandler fileHandler;
        GameObject player;

        public TMP_Text label00;
        public TMP_Text label01;
        public TMP_Text label02;
        public TMP_Text label03;
        public TMP_Text label10;
        public TMP_Text label11;
        public TMP_Text label12;
        public TMP_Text label13;
        public TMP_Text label20;
        public TMP_Text label21;
        public TMP_Text label22;
        public TMP_Text label23;
        public TMP_Text label30;
        public TMP_Text label31;
        public TMP_Text label32;
        public TMP_Text label33;

        Room[][] roomsByType;

        Room[] offPathRooms;

        void Start(){
            ladderRenderer = new RoomRenderer(ladderMap);
            roomRenderer = new RoomRenderer(levelMap);
            fileHandler = new RoomFileHandler();

            var rooms = fileHandler.FetchRooms().Select(x => fileHandler.LoadRoom(x));

            roomsByType = new Room[7][];
            foreach (int i in Enumerable.Range(0,7)){
                roomsByType[i] = rooms.Where(x => x.Designation == i).ToArray();
            }

            offPathRooms = rooms.Where(x => x.Designation != 5 && x.Designation != 6).ToArray();

            Generate();
        }

        public void Generate(){
            Destroy(player);
            var map = GenerateTopLevelMap();
            UpdateDisplay(map);
            var levelGrid = new int[40,40];
            (int x, int y) playerPos = (-1,-1); 

            for(var i = 3; i >= 0; i--){
                for(var j = 0; j < map.GetLength(1); j++){
                    var roomType = map[i,j];
                    Debug.Log($"Getting room type: {roomType}");

                    Room room;
                    
                    if(roomType == 0){
                        var nextVal = (int)Math.Floor(Random.value * offPathRooms.Length);
                        room = offPathRooms[nextVal];
                    }else{
                        var nextVal = (int)Math.Floor(Random.value * roomsByType[roomType].Length);
                        room = roomsByType[roomType][nextVal];
                    }
                    
                    for(var p = 0; p < room.Grid.GetLength(0); p++){
                        for(var q = 0; q < room.Grid.GetLength(1); q++){
                            var currValue = room.Grid[p, 9-q];
                            levelGrid[j*10 + p, 39-(i*10 + q)] = currValue; 
                            if(currValue == 3)
                                playerPos = (j*10 + p, 39-(i*10 + q));
                        }
                    }
                }
            }

            if(playerPos == (-1, -1))
                throw new Exception("Player position never assigned");

            var coords = gameGrid.CellToWorld(new Vector3Int(playerPos.x, playerPos.y, 0));

            coords.x = coords.x + 5;
            coords.y = coords.y + 2;
            
            ladderRenderer.RenderLadders(levelGrid);
            roomRenderer.RenderLevel(levelGrid);
            player = Instantiate(playerPrefab, coords, Quaternion.identity);
        }

        void UpdateDisplay(int[,] topLevelMap){
            label00.text = topLevelMap[0,0].ToString();
            label01.text = topLevelMap[0,1].ToString();
            label02.text = topLevelMap[0,2].ToString();
            label03.text = topLevelMap[0,3].ToString();
            label10.text = topLevelMap[1,0].ToString();
            label11.text = topLevelMap[1,1].ToString();
            label12.text = topLevelMap[1,2].ToString();
            label13.text = topLevelMap[1,3].ToString();
            label20.text = topLevelMap[2,0].ToString();
            label21.text = topLevelMap[2,1].ToString();
            label22.text = topLevelMap[2,2].ToString();
            label23.text = topLevelMap[2,3].ToString();
            label30.text = topLevelMap[3,0].ToString();
            label31.text = topLevelMap[3,1].ToString();
            label32.text = topLevelMap[3,2].ToString();
            label33.text = topLevelMap[3,3].ToString();
        }

        public int[,] GenerateCanaryMap(){
            int[,] fuckthis = new int[4,4];
            fuckthis[0,0] = 2;
            fuckthis[1,1] = 1;
            fuckthis[2,2] = 1;
            fuckthis[3,3] = 1;

            return fuckthis;
        }

        public int[,] GenerateTopLevelMap() {
            int[,] rooms = new int[4,4];
            var currX = (int)Math.Ceiling(Random.value * 4) - 1;
            var currY = 0;

            rooms[currY, currX] = 6;

            int direction;

            if (currX == 0){
                direction = 1;
            } else if (currX == 3){
                direction = -1;
            } else {
                direction = Random.value > 0.5 ? 1 : -1; 
            }

            while(currY < 4){
                if(shouldDrop(rooms[currY, currX], currX, direction)){
                    // exit case
                    if(currY == 3){
                        rooms[currY, currX] = 5;
                        return rooms;
                    }
                    if(colliding(currX, direction)){
                        // drop n' reverse case
                        rooms[currY, currX] = 2;
                        currY++;
                        rooms[currY, currX] = 3;
                        direction *= -1;
                        continue;
                    }

                    currY++;
                    rooms[currY, currX] = 3;
                    direction = Random.value > 0.5 ? 1 : -1;
                }

                currX += direction;
                rooms[currY, currX] = Random.value < 0.33 ? 2 : 1;
            }

            return rooms;
        }

        bool shouldDrop(int roomType, int xPos, int direction) =>
            roomType == 2 || colliding(xPos, direction);

        bool colliding(int xPos, int direction) =>
            (xPos == 0 && direction == -1) ||
            (xPos == 3 && direction == 1);
    }
}