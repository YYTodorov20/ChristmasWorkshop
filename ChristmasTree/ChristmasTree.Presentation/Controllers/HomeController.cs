using System.Text.Json;
using System.Web;
using ChristmasTree.Models;
using ChristmasTree.Services;
using ChristmasTree.Services.Factory;
using ChristmasTree.Services.Services;
using ChristmasTree.Services.Token;
using Microsoft.AspNetCore.Mvc;

namespace ChristmasTree.Presentation.Controllers;

    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> logger;
        private readonly LightService lightService;
        private readonly TokenAccessor tokenAccessor;

        public HomeController(
            ILogger<HomeController> logger,
            LightService lightService,
            TokenAccessor tokenAccessor)
        {
            this.logger = logger;
            this.lightService = lightService;
            this.tokenAccessor = tokenAccessor;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            return JsonSerializer.Serialize(await this.lightService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostViewModel model)
        {
            var ct = this.tokenAccessor.GetChristmasToken();

            if (string.IsNullOrEmpty(ct))
            {
                this.logger.LogError("No christmas token provided");
                return this.BadRequest();
            }

            if (model.desc != null)
            {
                model.desc = HttpUtility.HtmlEncode("\u200e" + model.desc);
                await this.lightService.AddAsync(model.desc, ct!);
                return this.Ok();
            }

            return this.BadRequest();
        }
    }