using System.ComponentModel.DataAnnotations;

namespace Headtrip.GameServer.Models.OverworldInstance
{
    public class SwitchChannelParameters
    {

        [Required]
        public Guid ChannelId { get; set; }

    }
}
