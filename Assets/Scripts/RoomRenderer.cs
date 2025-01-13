using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    class RoomRenderer
    {
        Tilemap Room;

        public RoomRenderer(Tilemap room) => Room = room;

        public void RenderLevel(int[,] gameGrid){
            for (var i = 0; i < gameGrid.GetLength(0); i++) {
                for (var j = 0; j < gameGrid.GetLength(1); j++) {
                    if(gameGrid[i,j] == 0)
                        continue;
                        
                    Room.SetTile(new Vector3Int(i, j, 0), GetTileForPosition(gameGrid, i, j));
                }
            }
        }

        public void RenderRoom(int[,] gameGrid)
        {
            for (var i = 0; i < gameGrid.GetLength(0); i++) {
                for (var j = 0; j < gameGrid.GetLength(1); j++) {
                    Room.SetTile(new Vector3Int(i, j, 0), GetTileForPosition(gameGrid, i, j));
                }
            }
        }

        private Tile GetTileForPosition(int[,] gameGrid, int x, int y)
        {
            var width = gameGrid.GetLength(0);
            var height = gameGrid.GetLength(1);

            if (x >= width || y >= height || gameGrid[x, y] == 0)
                return TileResourceLoader.GetEmptySpace();
            if (gameGrid[x,y] == 1)
                return TileResourceLoader.GetWalkableGroundMiddle();
            if (gameGrid[x, y] == 2)
                return TileResourceLoader.GetEntityPlaceholder();
            
            return TileResourceLoader.GetInvalid();
        }
    }
}
