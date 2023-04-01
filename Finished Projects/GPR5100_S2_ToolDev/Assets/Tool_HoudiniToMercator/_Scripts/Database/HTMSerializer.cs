namespace htm.core
{
    using Newtonsoft.Json;

    public static class HTMSerializer
    {
        public static string SerializeEntryInfoPacket(EntryInfoPacket packet)
        {
            string jsonStr = string.Empty;

            jsonStr = JsonConvert.SerializeObject(packet, Formatting.Indented);

            return jsonStr;
        }

        public static EntryInfoPacket DeserializeEntryInfoPacket(string jsonStr)
        {
            EntryInfoPacket packet = JsonConvert.DeserializeObject<EntryInfoPacket>(jsonStr);

            return packet;
        }
    }
}