using Headtrip.GameServer.Models;
using Headtrip.GameServer.Models.OverworldInstance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Headtrip.GameServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OverworldInstance : ControllerBase
    {
        // Travel to Zone
        // Switch Channel
        // Get Channels

        [HttpPost]
        public async Task<ActionResult<TravelToZoneResult>> TravelToZone(
            [FromBody] TravelToZoneParameters Params)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(new TravelToZoneResult
            {
                Zone = null,
                Channel = null
            });
        }

        [HttpPost]
        public async Task<ActionResult<SwitchChannelResult>> SwitchChannel(
            [FromBody] SwitchChannelParameters Params)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            return Ok(new SwitchChannelResult
            {
                Channel = null
            });
        }


        [HttpGet]
        public async Task<ActionResult<GetChannelsResult>> GetChannels(
            [FromBody] GetChannelsParams Params)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(new GetChannelsResult
            {
                Zone = null,
                Channels = null
            });
        }

  
    }
}
