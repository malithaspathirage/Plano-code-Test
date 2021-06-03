using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Plano.BL.Repository;
using Plano.Data.Interface;
using Plano.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace Plano.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonDetailsController : ControllerBase
    {
        private readonly PersonService _personService;
        private readonly IRepository<Person> _Person;
        private readonly ILogger<PersonDetailsController> _logger;
        private readonly IStringLocalizer<PersonDetailsController> _localizer;

        public PersonDetailsController(IRepository<Person> Person, PersonService ProductService, ILogger<PersonDetailsController> logger, IStringLocalizer<PersonDetailsController> localize)
        {
            _personService = ProductService;
            _Person = Person;
            _logger = logger;
            _localizer = localize;
        }
        //Add Person
        [HttpPost("AddPerson")]
        public async Task<Object> AddPerson(string FullName, string Language)
        {
            bool result;
            try
            {
                var neutralCulture = Thread.CurrentThread.CurrentCulture.Name;
                if (!neutralCulture.Equals(Language, StringComparison.OrdinalIgnoreCase)) //Validate Language
                {
                    _logger.LogInformation("Not valid Language accordance to current CultureInfo -" + Language + ", current CultureInfo -" + neutralCulture);
                    result = false;
                }
                else
                {
                    await _personService.AddPerson(new Person()
                    {
                        FullName = FullName,
                        Language = Language
                    });
                    result = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " - " + ex.StackTrace);
                result = false;
            }

            return JsonConvert.SerializeObject(new { IsSuccess = result }, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }
            );
        }

        //GET Person By UserId
        [HttpGet("GetPersonByUserId")]
        public Object GetPersonByUserId(int UserId)
        {
            var data = _personService.GetPersonByUserId(UserId);
            var message = _localizer["Message"].Value;

            var json = JsonConvert.SerializeObject(new { FullName = data.FullName, Message = message }, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                }
            );
            return json;
        }


    }
}
