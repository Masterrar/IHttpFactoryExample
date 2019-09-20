using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IHttpFactoryExample.Controllers
{
    //Askona Home: netstat -n | select-string -pattern "40.87.145.144"
    //Habr       : netstat -n | select-string -pattern "178.248.237.68"
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IHttpClientFactory Factory { get; }
        public ValuesController(IHttpClientFactory factory)
        {
            Factory = factory;
        }
        // GET api/values
        [HttpGet]
        public JsonResult GetHttpUsing()
        {
            var listString = new List<string>();

            for (int i = 0; i < 10; i++)
            {
                using (var client = new HttpClient())
                {
                    client.GetStringAsync("https://home.askona.ru/basket").Wait();
                    listString.Add("https://home.askona.ru/basket " + DateTime.UtcNow);

                    client.GetStringAsync("https://home.askona.ru/delivery").Wait();
                    listString.Add("https://home.askona.ru/delivery " + DateTime.UtcNow);

                    client.GetStringAsync("https://habr.com").Wait();
                    listString.Add("https://habr.com " + DateTime.UtcNow);
                }
            }

            return new JsonResult(listString);
        }

        [HttpGet]
        public JsonResult GetHttpFactory()
        {
            var listString = new List<string>();

            
            for (int i = 0; i < 10; i++)
            {
                var client = Factory.CreateClient();

                client.GetStringAsync("https://home.askona.ru/basket").Wait();
                listString.Add("https://home.askona.ru/basket " + DateTime.UtcNow);

                client.GetStringAsync("https://home.askona.ru/delivery").Wait();
                listString.Add("https://home.askona.ru/delivery " + DateTime.UtcNow);

                client.GetStringAsync("https://habr.com").Wait();
                    listString.Add("https://habr.com " + DateTime.UtcNow);
            }

            return new JsonResult(listString);
        }
    }
}
