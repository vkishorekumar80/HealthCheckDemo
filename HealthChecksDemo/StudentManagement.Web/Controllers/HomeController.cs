using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StudentManagement.Core.Model;
using StudentManagement.Web.Models;

namespace StudentManagement.Web.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration) {
            _logger = logger;
            this.configuration = configuration;
        }

        public async Task<IActionResult> Index() {
            var http = new HttpClient();
            var url = $"{configuration["StudentManagementUrl"]}/school";
            var response = await http.GetAsync(url);

            var jsonString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<List<Student>>(jsonString);

            //IEnumerable<Student> list = new List<Student>();


            //if(x.IsSuccessStatusCode) {
            //    var rlist = await JsonSerializer.DeserializeAsync<IEnumerable<Student>>(await x.Content.ReadAsStreamAsync());

            //}

            return View(result);
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
