using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentManagement.Api.Data;
using StudentManagement.Core.Model;

namespace StudentManagement.Api.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class SchoolController : ControllerBase {

        private readonly ILogger<SchoolController> _logger;
        private readonly SchoolContext schoolContext;

        public SchoolController(ILogger<SchoolController> logger, SchoolContext schoolContext) {
            _logger = logger;
            this.schoolContext = schoolContext;
        }

        [HttpGet]
        public IEnumerable<Student> Get() {
            return schoolContext.Students;
        }
    }
}
