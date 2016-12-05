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
        public static void IsMyFuckingTrainOnTime(Journey journey, bool useCannedResponse = false)
        {
            var now = DateTime.Now;
            var today = DateTime.Today;
            var tommorrow = today.AddDays(1);

            var journeyType = WhichFuckingJourneyLegAmIOn(journey, now);

            var departureTime = WhenIsTheNextFuckingTrain(journey, journeyType, today, tommorrow);

            string destination;
            string origin;
            WhickFuckingWayAmIGoing(journey, journeyType, out origin, out destination);

            var offset = Convert.ToInt32(Math.Round((departureTime - DateTime.Now).TotalMinutes));
            GetDepartureBoardResponse response;
            if (useCannedResponse)
            {
                response = GetCannedDepartureBoardResponse(origin, destination, departureTime, offset);
            }
            else if (offset > 120)
            {
                Console.WriteLine("YOU NEED A FUCKING CRYSTAL BALL");

                return;

            }
            else
            {
                response = GetRealDepartureBoardResponse(origin, destination, departureTime, offset);
            }
            if (response==null)
            {
                Console.WriteLine("THE FUCKING SERVICE IS DOWN OR SOMETHING");
                return;
            }
            var result = response.GetStationBoardResult;

            Console.WriteLine("[{0}] {1} => [{2}] {3}", result.crs, result.locationName, result.filtercrs, result.filterLocationName);
            if (result.nrccMessages != null)
            {
                foreach (var nrccMessage in result.nrccMessages)
                {
                    Console.WriteLine(" * {0}", nrccMessage.Value);
                }
            }

            if (!result.areServicesAvailable)
            {
                Console.WriteLine("NO FUCKING SERVICES ARE AVAILIABLE");
            }

            if (result?.trainServices != null)
            {
                foreach (var train in result.trainServices)
                {
                    Console.WriteLine(FormatTrain(train));
                }
            }
            else
            {
                Console.WriteLine("NO FUCKING TRAINS");
            }


        }

        private static readonly DataContractSerializer Serializer = new DataContractSerializer(typeof(GetDepartureBoardResponse));

        private static GetDepartureBoardResponse GetRealDepartureBoardResponse(string origin, string destination,
            DateTime departureTime, int offset)
        {
            LDBServiceSoap ldb = new LDBServiceSoapClient();
            var accessToken = new AccessToken()
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
                return (GetDepartureBoardResponse)Serializer.ReadObject(stream);
            }

        }

        private static void WhickFuckingWayAmIGoing(Journey journey, JourneyType journeyType, out string origin, out string destination)
        {

            switch (journeyType)
            {
                case JourneyType.OutboundToday:
                case JourneyType.OutboundTomorrow:
                    origin = journey.Outbound.From;
                    destination = journey.Outbound.To;
                    break;
                case JourneyType.ReturnToday:
                    origin = journey.Inbound.From;
                    destination = journey.Inbound.To;
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

        private static string FormatTrain(ServiceItem train)
        {
            //# Live Departure Boards Web Service (LDBWS / OpenLDBWS)
            //# https://lite.realtime.nationalrail.co.uk/OpenLDBWS/

            //# In the objects detailed above, certain properties were specified to return time values.
            //#  These values will either return absolute times, formatted as a HH:MM string,
            //# or a text string such as (but not limited to) "On time", "No report" or "Cancelled".
            //# These times should be output in the user interface exactly as supplied. In some cases,
            //# the time value may have an asterisk ("*") appended to indicate that the value is "uncertain".

            var sb = new StringBuilder();

            bool isFuckingCancelled = train.isCancelled;

            bool isFuckingDelayed = !train.isCancelled
                                    && (train.etd.ToUpperInvariant() != "ON TIME"
                                        || !String.IsNullOrWhiteSpace(train.delayReason));
            bool isOnTime = !train.isCancelled
                            && train.etd.ToUpperInvariant() == "ON TIME"
                            && String.IsNullOrWhiteSpace(train.delayReason);
            sb.AppendFormat("{0} from {1} to {2} is ",
                train.std,
                String.Join(" ", train.origin?.Select(o => o.locationName)),
                String.Join(" ", train.destination?.Select(d => d.locationName))
                );

            if (isFuckingCancelled)
            {
                sb.Append("FUCKING CANCELLED");
                if (!String.IsNullOrWhiteSpace(train.cancelReason))
                {
                    sb.AppendFormat(" DUE TO {0}", train.cancelReason);
                }
            }
            else if (isFuckingDelayed)
            {
                sb.AppendFormat("FUCKING DELAYED {0}", train.etd);
                if (!String.IsNullOrWhiteSpace(train.delayReason))
                {
                    sb.AppendFormat(" DUE TO {0}", train.delayReason);
                }
            }
            else if (isOnTime)
            {
                sb.Append("ON FUCKING TIME APPARENTLY");
            }
            else
            {
                sb.Append("I DONT FUCKING KNOW");
            }
            if (train.platform != null)
            {
                sb.AppendFormat(" from fucking platform {0}", train.platform);
            }
            sb.AppendFormat(" (fucking {0})", train.@operator).AppendLine();
            return sb.ToString();
        }
    }
}