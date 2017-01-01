using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using FuckingTrains;
using Swashbuckle.Swagger.Annotations;

namespace FuckingTrainsWebApi.Controllers
{
    public class FuckingTrainsController : ApiController
    {
        [HttpGet]
        public IHttpActionResult IsMyFuckingTrainOnTime()
        {
            var train = Trains.IsMyFuckingTrainOnTime();
            Console.WriteLine(train.ToString());
            return Ok(train);
        }
    }
}