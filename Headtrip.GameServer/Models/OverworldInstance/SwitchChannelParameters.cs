using System.ComponentModel.DataAnnotations;

namespace Headtrip.GameServer.Models.OverworldInstance
{
    public class SwitchChannelParameters
    {
        [Required]
        public Guid SessionId { get; set; }

        [Required]
        public Guid ChannelId { get; set; }

    }
}
