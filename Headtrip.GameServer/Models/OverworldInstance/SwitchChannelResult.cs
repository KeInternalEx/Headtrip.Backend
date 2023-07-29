using Headtrip.Objects.Instance;

namespace Headtrip.GameServer.Models.OverworldInstance
{
    public class SwitchChannelResult
    {
        public Channel Channel { get; set; } = null!;
        public int NumberOfPlayers { get; set; }
    }
}
