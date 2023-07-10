namespace Headtrip.GameServer.Models.OverworldInstance
{
    public class GetChannelsParams
    {
        public Guid SessionId { get; set; }
        public string ZoneName { get; set; } = null!;

    }
}
