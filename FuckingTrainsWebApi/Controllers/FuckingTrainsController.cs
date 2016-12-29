using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using FuckingTrains;
using Swashbuckle.Swagger.Annotations;

namespace FuckingTrainsWebApi.Controllers
{
    public class FuckingTrainsController : ApiController
    {
        public IHttpActionResult IsMyFuckingTrainOnTime()
        {
            var train = Trains.IsMyFuckingTrainOnTime(Journeys.Commute);
            return Ok(train);
        }
    }
}