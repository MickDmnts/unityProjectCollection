namespace htm.core
{
    public class EntryInfoPacket
    {
        public string parentTile;

        public string ldPath;
        public string hdPath;

        public double lat;
        public double lon;

        public int tileType;
        public double tileRes;

        public string[] scriptToAdd;

        public string customLayerPrefix;
        public int customDetailLevel;
        public int customLayerType;
    }
}