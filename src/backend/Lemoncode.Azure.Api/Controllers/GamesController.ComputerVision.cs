﻿using Lemoncode.Azure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.EntityFrameworkCore;

namespace Lemoncode.Azure.Api.Controllers
{
    public partial class GamesController : ControllerBase
    {
        [HttpGet("{id}/Screenshots/{screenshotId}/Analyze")]
        [ProducesResponseType(typeof(ImageAnalysis), 200)]
        public async Task<IActionResult> GetScreenshotAnalysis([FromRoute] int id, [FromRoute] int screenshotId)
        {
            log.LogInformation($"GAMES - Getting Screenshot for game with id {id}");

            try
            {
                var game = context.Game.Include(i => i.Screenshots).FirstOrDefault(i => i.Id == id);
                if (game is null)
                {
                    return NotFound("Game not found");
                }

                var screenshot = game.Screenshots.FirstOrDefault(x => x.Id == screenshotId);
                if (screenshot is null)
                {
                    return NotFound("Screenshot not found");
                }

                log.LogInformation($"GAMES - Get Screenshot analysis successfully");
                var analysis = await computerVisionService.AnalyzeImageAsync(screenshot.Url);
                return Ok(analysis);

            }
            catch (Exception ex)
            {
                log.LogError($"GAMES - {ex.Message}");
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}/Screenshots/{screenshotId}/Tags")]
        [ProducesResponseType(typeof(TagResult), 200)]
        public async Task<IActionResult> GetScreenshotTags([FromRoute] int id, [FromRoute] int screenshotId)
        {
            log.LogInformation($"GAMES - Getting Screenshot for game with id {id}");

            try
            {
                var game = context.Game.Include(i => i.Screenshots).FirstOrDefault(i => i.Id == id);
                if (game is null)
                {
                    return NotFound("Game not found");
                }

                var screenshot = game.Screenshots.FirstOrDefault(x => x.Id == screenshotId);
                if (screenshot is null)
                {
                    return NotFound("Screenshot not found");
                }

                log.LogInformation($"GAMES - Get Screenshot adultInfo successfully");
                var tags = await computerVisionService.GetTagsAsync(screenshot.Url);
                return Ok(tags);

            }
            catch (Exception ex)
            {
                log.LogError($"GAMES - {ex.Message}");
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}/Screenshots/{screenshotId}/Describe")]
        [ProducesResponseType(typeof(ImageDescription), 200)]
        public async Task<IActionResult> GetScreenshotDescribe([FromRoute] int id, [FromRoute] int screenshotId)
        {
            log.LogInformation($"GAMES - Getting Screenshot for game with id {id}");

            try
            {
                var game = context.Game.Include(i => i.Screenshots).FirstOrDefault(i => i.Id == id);
                if (game is null)
                {
                    return NotFound("Game not found");
                }

                var screenshot = game.Screenshots.FirstOrDefault(x => x.Id == screenshotId);
                if (screenshot is null)
                {
                    return NotFound("Screenshot not found");
                }

                log.LogInformation($"GAMES - Get Screenshot describe successfully");
                var description = await computerVisionService.DescribeAsync(screenshot.Url);
                return Ok(description);

            }
            catch (Exception ex)
            {
                log.LogError($"GAMES - {ex.Message}");
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}/Screenshots/{screenshotId}/AdultInformation")]
        [ProducesResponseType(typeof(AdultInfo), 200)]
        public async Task<IActionResult> GetScreenshotAdultInformation([FromRoute] int id, [FromRoute] int screenshotId)
        {
            log.LogInformation($"GAMES - Getting Screenshot for game with id {id}");

            try
            {
                var game = context.Game.Include(i => i.Screenshots).FirstOrDefault(i => i.Id == id);
                if (game is null)
                {
                    return NotFound("Game not found");
                }

                var screenshot = game.Screenshots.FirstOrDefault(x => x.Id == screenshotId);
                if (screenshot is null)
                {
                    return NotFound("Screenshot not found");
                }

                log.LogInformation($"GAMES - Get Screenshot adult information successfully");
                var adultInfo = await computerVisionService.GetAdultInfoAsync(screenshot.Url);
                return Ok(adultInfo);

            }
            catch (Exception ex)
            {
                log.LogError($"GAMES - {ex.Message}");
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}/Screenshots/{screenshotId}/Thumbnail")]
        [ProducesResponseType(typeof(FileBase64), 200)]
        public async Task<IActionResult> GetScreenshotThumbnail([FromRoute] int id, [FromRoute] int screenshotId,
                                                                [FromQuery] int width, [FromQuery] int height, [FromQuery] bool smartCropping)
        {
            log.LogInformation($"GAMES - Getting Screenshot for game with id {id}");

            try
            {
                var game = context.Game.Include(i => i.Screenshots).FirstOrDefault(i => i.Id == id);
                if (game is null)
                {
                    return NotFound("Game not found");
                }

                var screenshot = game.Screenshots.FirstOrDefault(x => x.Id == screenshotId);
                if (screenshot is null)
                {
                    return NotFound("Screenshot not found");
                }

                log.LogInformation($"GAMES - Get Screenshot thumbnail successfully");
                var stream = await computerVisionService.GetThumbnailAsync(screenshot.Url, width, height, smartCropping);

                if (stream is not null)
                {
                    using MemoryStream ms = new();
                    await stream.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    return Ok(new FileBase64 (){ Content = (Convert.ToBase64String(fileBytes)) });
                }

                return NotFound();

            }
            catch (Exception ex)
            {
                log.LogError($"GAMES - {ex.Message}");
                return BadRequest(ex.Message);
            }

        }
    }
}
