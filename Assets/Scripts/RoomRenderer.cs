using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    class RoomRenderer
    {
        Tilemap Room;

        public RoomRenderer(Tilemap room) => Room = room;

        public void RenderLevel(int[,] gameGrid){
            Room.ClearAllTiles();
            
            for (var i = 0; i < gameGrid.GetLength(0); i++) {
                for (var j = 0; j < gameGrid.GetLength(1); j++) {
                    if(gameGrid[i,j] == 0 
                    || gameGrid[i,j] == 3 
                    || gameGrid[i,j] == 2
                    || gameGrid[i,j] == 4
                    || gameGrid[i,j] == 6)
                        continue;
                    
                    Room.SetTile(new Vector3Int(i, j, 0), GetTileForPosition(gameGrid, i, j));
                }
            }
        }

        public void RenderLadders(int[,] gameGrid){
            Room.ClearAllTiles();
            for (var i = 0; i < gameGrid.GetLength(0); i++) {
                for (var j = 0; j < gameGrid.GetLength(1); j++) {
                    if(gameGrid[i,j] == 4 || gameGrid[i,j] == 6)
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

            return gameGrid[x, y] switch {
                1 => TileResourceLoader.GetWalkableGroundMiddle(),
                2 => TileResourceLoader.GetEntityPlaceholder(),
                3 => TileResourceLoader.GetSpawnPoint(),
                4 => TileResourceLoader.GetWallLeft(),
                5 => TileResourceLoader.GetPlatformMiddle(),
                6 => TileResourceLoader.GetPlatformMiddle(),
                _ => TileResourceLoader.GetInvalid()
            };
        }
    }
}
