using System.Numerics;

namespace Fishy.Models.Packets
{
    public class ChalkPacket : FPacket
    {
        public override string Type { get; set; } = "chalk_packet";
        public int Canvas_ID { get; set; } = 0;
        public Dictionary<int, object> Data { get; set; }

        public ChalkPacket(int Canvas, Dictionary<Vector2, int> CanvasData)
        {
            Canvas_ID = Canvas;
            Data = new Dictionary<int, object>();

            int i = 0;

            foreach (var entry in CanvasData)
            {
                var innerData = new Dictionary<int, object>
                {
                    { 0, entry.Key },
                    { 1, entry.Value }
                };

                Data[i] = innerData;

                i++;
            }
        }
    }
}