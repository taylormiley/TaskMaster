using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TaskMaster.Models;
using System.Net;
using TaskMaster.ViewModels;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Authorization;

namespace TaskMaster.Controllers.Api
{
    [Authorize]
    [Route("api/quests")]
    public class QuestController : Controller
    {
        private ILogger<QuestController> _logger;
        private ITaskRepository _repository;

        public QuestController(ITaskRepository repository, ILogger<QuestController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public JsonResult Get()
        {
            var quests = _repository.GetAllIncompleteUserQuests(User.Identity.Name);
            var results = Mapper.Map<IEnumerable<QuestViewModel>>(quests);
            return Json(results);
        }

        [HttpPost("")]
        public JsonResult Post([FromBody]QuestViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newQuest = Mapper.Map<Quest>(vm);
                    newQuest.UserName = User.Identity.Name;

                    // Save to the Database
                    _logger.LogInformation("Attempting to save a new quest");
                    _repository.AddQuest(newQuest);


                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<QuestViewModel>(newQuest));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new quest", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }

        [HttpDelete("")]
        public JsonResult Delete([FromBody]QuestViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var questToDelete = Mapper.Map<Quest>(vm);
                    questToDelete.UserName = User.Identity.Name;

                    // Save to the Database
                    _logger.LogInformation("Attempting to delete a quest");
                    _repository.DeleteQuest(questToDelete);


                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<QuestViewModel>(questToDelete));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete quest", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }
    }
}
