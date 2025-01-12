using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    public static class TileResourceLoader
    {
        private const string prefix = "Foozle_2DT0008_GreenValley_Tileset_Pixel_Art";

        public static Tile GetInvalid() => GetTileByNumber(70);
        public static Tile GetEmptySpace() => GetTileByNumber(6);
        public static Tile GetWalkableGroundMiddle() => GetTileByNumber(1);
        public static Tile GetWalkableGroundLeft() => GetTileByNumber(0);
        public static Tile GetWalkableGroundRight() => GetTileByNumber(2);
        public static Tile GetWallLeft() => GetTileByNumber(21);
        public static Tile GetInternalGround() => GetTileByNumber(22);
        public static Tile GetWallRight() => GetTileByNumber(23);
        public static Tile GetCeilingLeft() => GetTileByNumber(46);
        public static Tile GetCeilingMiddle() => GetTileByNumber(47);
        public static Tile GetCeilingRight() => GetTileByNumber(48);
        public static Tile GetPlatformLeft() => GetTileByNumber(59);
        public static Tile GetPlatformMiddle() => GetTileByNumber(60);
        public static Tile GetPlatformRight() => GetTileByNumber(61);
        public static Tile GetEntityPlaceholder() => GetTileByNumber(55);

        private static Tile GetTileByNumber(int tileNum) => Resources.Load($"Tiles/{prefix}_{tileNum}", typeof(Tile)) as Tile;
    }
}
