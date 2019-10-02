using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using ES.Domain.Models;
using ES.Http.Extensions;
using ES.Infrastructure.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ES.Controllers
{
    /// <summary>
    /// PlayersController.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="configuration">Configuration Properties</param>
        /// <param name="jsonSerializer">Json Serializer</param>
        /// <param name="httpClientFactory">Http Client Factory</param>
        public PlayersController(
            IConfiguration configuration,
            IJsonSerializer jsonSerializer,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _jsonSerializer = jsonSerializer;
            _httpClientFactory = httpClientFactory;

        }

        /// <summary>
        /// GET: api/Players
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Players()
        {
            var result = await RetrievePlayers();

            return Ok(result.Players.OrderBy(Player => Player.Id));
        }

        /// <summary>
        /// GET: api/Players/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetPlayer(int id)
        {
            var result = await RetrievePlayers();

            var player = result.Players.FirstOrDefault(Player => Player.Id == id);

            if (player is null)
            {
                return NotFound("User Not Found");
            }

            return Ok(player);
        }

        /// <summary>
        /// DELETE: api/Players/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var result = await RetrievePlayers();

            var player = result.Players.FirstOrDefault(Player => Player.Id == id);

            if (player is null)
            {
                return NotFound("User Not Found");
            }

            result.Players.ToList().Remove(player);

            return Ok(player);
        }

        #region "Private Helpers"

        private async Task<PlayersList> RetrievePlayers()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var uri = $"{_configuration.GetValue<string>("Server:BaseAddress")}headtohead.json";
                var request = new HttpRequestMessage()
                     .Get()
                     .ForUrl(uri);

                var response = await httpClient.SendAsync(request, CancellationToken.None);
                return await response.Content.ReadAsAsync<PlayersList>();
            }
            catch (System.Exception ex)
            {
                throw;
            }
           
        }
        
        #endregion
    }
}
