using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PruebaSummit.Controllers
//    public class DataObject
//{
//    public string Name { get; set; }
//}
{
    [Route("api/[controller]")]
    [ApiController]
    public class SummitController : ControllerBase
    {
        // GET: api/<SummitController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SummitController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SummitController>
        [HttpPost]
        public string Post([FromBody] JsonElement jsonElement)
        {
            JsonElement Valor = jsonElement.GetProperty("input");
            string ValorNumerico = Valor.GetString();
            HttpClient client = new HttpClient();

            bool isNumber = int.TryParse(ValorNumerico, out int numericValue);
            if (isNumber)
            {
                string URL = "https://api.coinbase.com/v2/exchange-rates?currency=USD";
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Add(
           new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                HttpResponseMessage response = client.GetAsync(URL).Result;
                if (response.IsSuccessStatusCode)
                {

                    var dataObjects = response.Content.ReadAsStringAsync().Result;
                    var jsonObject = JObject.Parse(dataObjects);
                    var data = (JObject)jsonObject["data"];
                    // var currency = (string)data["currency"];
                    var rates = (JObject)data["rates"];
                    double cop = (double)rates["COP"];


                    return (cop * numericValue).ToString();
                }

            }
            else
            {
                string URL= "https://api.dictionaryapi.dev/api/v2/entries/en/";

                 URL = string.Concat(URL, ValorNumerico);
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(URL).Result;
                if (response.IsSuccessStatusCode)
                {

                    var dataObjects = response.Content.ReadAsStringAsync().Result;
                   

                   // var jsonObject = JObject.Parse(dataObjects);
                  

                    return dataObjects.ToString();
                }

                   
            }
            return "0";
        }

        // PUT api/<SummitController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SummitController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
