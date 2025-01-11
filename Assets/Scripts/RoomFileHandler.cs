using System;
using System.IO;
using System.Linq;

namespace Assets.Scripts
{
    public class RoomFileHandler
    {
        private const string RoomDir = "rooms";
        private void EnsureRoomFolderExists() => FileHandler.CreateDir("rooms");

        public RoomFileHandler()
        {
            EnsureRoomFolderExists();
        }

        public void SaveRoom(Room room, string roomName = null)
        {
            var fileName = roomName ?? $"room-{DateTime.Now.ToString("dd-MM-yy")}";
            fileName += ".json";
            var relPath = Path.Combine(RoomDir, fileName);
            FileHandler.SaveToJSON(room, relPath);
        }

        public Room LoadRoom(string roomName)
        {
            var path = roomName;
            if (!Path.HasExtension(path))
                path += ".json";

            return FileHandler.ReadFromJSON<Room>(Path.Combine(RoomDir, path));
        }

        public string[] FetchRooms() => FileHandler.GetFiles(RoomDir).Select(x => x.Split("\\").Last().Split(".").First()).ToArray();
    }
}
