using System;
using System.Web.Http;
using TrainCommuteCheck;

namespace TrainCommuteCheckWebApi.Controllers
{
    public class TrainCheckController : ApiController
    {
        [HttpGet]
        public IHttpActionResult IsMyTrainOnTime()
        {
            var train = Trains.IsMyTrainOnTime();
            Console.WriteLine(train.ToString());
            return Ok(train);
        }
    }
}