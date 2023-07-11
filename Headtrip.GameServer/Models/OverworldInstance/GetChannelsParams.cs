using System.ComponentModel.DataAnnotations;

namespace Headtrip.GameServer.Models.OverworldInstance
{
    public class GetChannelsParams
    {

        [Required]
        public string ZoneName { get; set; } = null!;

    }
}
