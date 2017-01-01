using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using FuckingTrains.LDBService;

namespace FuckingTrains
{
    public static class Trains
    {
        private static readonly DataContractSerializer Serializer =
            new DataContractSerializer(typeof (GetDepartureBoardResponse));

        public static TrainResult IsMyFuckingTrainOnTime()
        {
            Journey journey = Journeys.Commute;

            var trainResult = new TrainResult();

            var now = DateTime.Now;
            var today = DateTime.Today;
            var tommorrow = today.AddDays(1);

            var journeyLeg = WhichFuckingJourneyLegAmIOn(journey, now);

            string destination;
            string origin;
            string departsAt;
            WhickFuckingWayAmIGoing(journey, journeyLeg, out origin, out destination, out departsAt);
            trainResult.FuckingFrom = origin;
            trainResult.FuckingTo = destination;
            trainResult.StandardTimeOfFuckingDeparture = departsAt;

            var departureTime = WhenIsTheNextFuckingTrain(journey, journeyLeg, today, tommorrow);
            var offset = Convert.ToInt32(Math.Round((departureTime - DateTime.Now).TotalMinutes));
            GetDepartureBoardResponse response;
            if (offset > 120)
            {
                trainResult.FuckingTrainState = FuckingTrainStates.YouNeedAFuckingCrystalBall;
                return trainResult;
            }
            else
            {
                response = GetRealDepartureBoardResponse(origin, destination, departureTime, offset);
            }

            if (response == null)
            {
                trainResult.FuckingTrainState = FuckingTrainStates.TheFuckingServiceIsDownOrSomething;
                return trainResult;
            }

            var result = response.GetStationBoardResult;

            trainResult.FuckingFrom = string.Format("[{0}] {1}", result.crs, result.locationName);
            trainResult.FuckingTo = string.Format("[{0}] {1}", result.filtercrs, result.filterLocationName);

            if (result.nrccMessages != null)
            {
                foreach (var nrccMessage in result.nrccMessages)
                {
                    trainResult.Messages.Add(nrccMessage.Value);
                }
            }

            if (result.areServicesAvailable && result.trainServices != null)
            {
                trainResult.AllFuckingServices = result.trainServices.Select(train => new TrainResult(train)).ToArray();
                var t = FindMyFuckingTrain(result.trainServices, departureTime);
                if (t == null)
                {
                    trainResult.FuckingTrainState = FuckingTrainStates.IDontFuckingKnow;
                }
                else
                {
                    return t;
                }
            }
            else
            { 
                trainResult.FuckingTrainState = FuckingTrainStates.NoFuckingTrains;
            }
            return trainResult;
        }

        private static TrainResult FindMyFuckingTrain(ServiceItem[] trainServices, DateTime departureTime)
        {
            var train = trainServices.SingleOrDefault(t => IsTheSameFuckingTime(t, departureTime));
            if (train != null)
            {
                return new TrainResult(train);
            }
            return null;
        }

        private static bool IsTheSameFuckingTime(ServiceItem trainService, DateTime departureTime)
        {
            var timeComponents = trainService.std.Split(':').Select(s =>Convert.ToInt32(s)).ToArray();
            var hour = timeComponents[0];
            var minute = timeComponents[1];
            return hour == departureTime.Hour && minute == departureTime.Minute;
        }

        private static GetDepartureBoardResponse GetRealDepartureBoardResponse(string origin, string destination,
            DateTime departureTime, int offset)
        {
            //# Live Departure Boards Web Service (LDBWS / OpenLDBWS)
            //# https://lite.realtime.nationalrail.co.uk/OpenLDBWS/

            //# In the objects detailed above, certain properties were specified to return time values.
            //#  These values will either return absolute times, formatted as a HH:MM string,
            //# or a text string such as (but not limited to) "On time", "No report" or "Cancelled".
            //# These times should be output in the user interface exactly as supplied. In some cases,
            //# the time value may have an asterisk ("*") appended to indicate that the value is "uncertain".

            LDBServiceSoap ldb = new LDBServiceSoapClient();
            var accessToken = new AccessToken
            {
                TokenValue = "4708e677-42fc-4d32-99ba-d348279fd1e4"
            };


            Console.WriteLine("{0} => {1} @ {2}", origin, destination, departureTime);

            var request = new GetDepartureBoardRequest(accessToken, 20, origin, destination, FilterType.to, offset, 120);
            GetDepartureBoardResponse response = null;
            try
            {
                response = ldb.GetDepartureBoard(request);
                using (var stream = new XmlTextWriter("last_response.xml", Encoding.Unicode))
                {
                    Serializer.WriteObject(stream, response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }

        private static GetDepartureBoardResponse GetCannedDepartureBoardResponse(string origin, string destination,
            DateTime departureTime, int offset)
        {
            using (var stream = new XmlTextReader("last_response.xml"))
            {
                return (GetDepartureBoardResponse) Serializer.ReadObject(stream);
            }
        }

        private static void WhickFuckingWayAmIGoing(Journey journey, JourneyType journeyType, out string origin, out string destination, out string departsAt)
        {
            switch (journeyType)
            {
                case JourneyType.OutboundToday:
                case JourneyType.OutboundTomorrow:
                    origin = journey.Outbound.From;
                    destination = journey.Outbound.To;
                    departsAt = journey.Outbound.Time;
                    break;
                case JourneyType.ReturnToday:
                    origin = journey.Inbound.From;
                    destination = journey.Inbound.To;
                    departsAt = journey.Inbound.Time;
                    break;
                default:
                    throw new Exception();
            }
        }

        private static DateTime WhenIsTheNextFuckingTrain(Journey journey, JourneyType journeyType, DateTime today,
            DateTime tommorrow)
        {
            DateTime departureTime;

            switch (journeyType)
            {
                case JourneyType.OutboundToday:
                    departureTime = new DateTime(today.Year, today.Month, today.Day,
                        journey.Outbound.H, journey.Outbound.M, 0);
                    break;
                case JourneyType.ReturnToday:
                    departureTime = new DateTime(today.Year, today.Month, today.Day,
                        journey.Inbound.H, journey.Inbound.M, 0);
                    break;
                case JourneyType.OutboundTomorrow:
                    departureTime = new DateTime(tommorrow.Year, tommorrow.Month, tommorrow.Day,
                        journey.Outbound.H, journey.Outbound.M, 0);
                    break;
                default:
                    throw new Exception();
            }
            return departureTime;
        }

        private static JourneyType WhichFuckingJourneyLegAmIOn(Journey journey, DateTime now)
        {
            JourneyType journeyType;
            if (now.Hour <= journey.Outbound.H + 1)
            {
                journeyType = JourneyType.OutboundToday;
            }
            else if (now.Hour <= journey.Inbound.H + 1)
            {
                journeyType = JourneyType.ReturnToday;
            }
            else
            {
                journeyType = JourneyType.OutboundTomorrow;
            }
            return journeyType;
        }
    }
}