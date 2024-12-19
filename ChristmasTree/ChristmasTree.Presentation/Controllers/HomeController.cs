using System.Text.Json;
using System.Web;
using ChristmasTree.Models;
using ChristmasTree.Services.Factory;
using Microsoft.AspNetCore.Mvc;

namespace ChristmasTree.Presentation.Controllers;

    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly LightFactory lightFactory;
        private readonly ILogger<HomeController> logger;
        private readonly LightService lightService;

        public HomeController(
            LightFactory lightFactory,
            ILogger<HomeController> logger,
            LightService lightService)
        {
            this.lightFactory = lightFactory;
            this.logger = logger;
            this.lightService = lightService;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            return JsonSerializer.Serialize(await this.lightService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostViewModel model)
        {
            if (!this.Request.Headers.TryGetValue("Christmas-Token", out var ct))
            {
                this.logger.LogError("No christmas token provided");
                return this.BadRequest();
            }

            if (model.desc != null)
            {
                model.desc = HttpUtility.HtmlEncode("\u200e" + model.desc);
                var light = await this.lightFactory.CreateLight(model.desc, ct!);
                await this.lightService.AddAsync(light);
                this.logger.LogInformation($"Created light: {JsonSerializer.Serialize(light)}");
                return this.Ok();
            }

            return this.BadRequest();
        }
    }