using Steamworks;

namespace Fishy.Models.Packets
{
    public class LetterPacket(SteamId to, SteamId from, string header, string body, string closing, string user) : FPacket
    {
        public override string Type { get; set; } = "letter_received";
        public string To { get; set; } = to.Value.ToString();
        public Letter Data { get; set; } = new Letter(to, from, header, body, closing, user);

        public override Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> data = Data.GetType()
               .GetProperties()
               .ToDictionary(p => p.Name.ToLower(), p => p.GetValue(this.Data) ?? "");

            Dictionary<string, object> letter = GetType()
               .GetProperties()
               .ToDictionary(p => p.Name.ToLower(), p => p.GetValue(this) ?? "");

            letter["data"] = data;

            return letter;
        }
    }
    public class Letter
    {
        public string To { get; set; } = String.Empty;
        public string From { get; set; } = String.Empty;
        public string Header { get; set; } = String.Empty;
        public string Body { get; set; } = String.Empty;
        public string Closing { get; set; } = String.Empty;
        public string User { get; set; } = String.Empty;
        public double Letter_ID { get; set; } = new Random().Next();
        public Dictionary<int, object> Items { get; set; } = [];
        public Letter(SteamId to, SteamId from, string header, string body, string closing, string user)
        {
            To = to.Value.ToString();
            From = from.Value.ToString();
            Header = header;
            Body = body;
            Closing = closing;
            User = user;
            Letter_ID = new Random().Next();
            Items = [];
        }
        public Letter() { }
    }
}
