using Headtrip.Objects.Instance;

namespace Headtrip.GameServer.Models.OverworldInstance
{
    public class GetChannelsResult
    {
        public Zone Zone { get; set; } = null!;
        public List<Channel> Channels { get; set; } = null!;
    }
}
