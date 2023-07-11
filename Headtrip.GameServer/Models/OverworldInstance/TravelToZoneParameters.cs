using System.ComponentModel.DataAnnotations;

namespace Headtrip.GameServer.Models.OverworldInstance
{
    public class TravelToZoneParameters
    {

        [Required]
        public string ZoneName { get; set; } = null!;

        public double PlayerCoordsX;
        public double PlayerCoordsY;
        public double PlayerCoordsZ;


    }
}
