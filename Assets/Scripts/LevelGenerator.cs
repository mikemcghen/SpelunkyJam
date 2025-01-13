using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts {
    public class LevelGenerator {
        

        public int[,] GenerateMap() {
            Debug.Log("oooohwee I did something");

            int[,] rooms = new int[4,4];
            var currX = (int)Math.Ceiling(Random.value * 4) - 1;
            var currY = 0;

            rooms[currY, currX] = 1;

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