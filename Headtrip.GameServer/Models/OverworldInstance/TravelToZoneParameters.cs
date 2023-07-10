namespace Headtrip.GameServer.Models.OverworldInstance
{
    public class TravelToZoneParameters
    {
        public Guid SessionId { get; set; }

        public string ZoneName { get; set; } = null!;

        public double PlayerCoordsX;
        public double PlayerCoordsY;
        public double PlayerCoordsZ;


    }
}
