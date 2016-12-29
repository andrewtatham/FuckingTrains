using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuckingTrains.LDBService;

namespace FuckingTrains
{
    public class TrainResult
    {
        private readonly ServiceItem train;
        public TrainResult()
        {
        }
        public TrainResult(ServiceItem train)
        {
            this.train = train;

            StandardTimeOfFuckingDeparture = train.std;
            EstimatedTimeOfFuckingDeparture = train.etd;
            FuckingFrom = String.Join(" ", train.origin?.Select(o => o.locationName));
            FuckingTo = String.Join(" ", train.destination?.Select(d => d.locationName));
            FuckingPlatform = train.platform;
            ShowerOfBastards = train.@operator;
            FuckingCancellationReason = train.cancelReason;
            FuckingDelayReason = train.delayReason;

            isFuckingCancelled = train.isCancelled;
            isFuckingDelayed = !train.isCancelled && (
                train.etd.ToUpperInvariant() != "ON TIME"
                || !String.IsNullOrWhiteSpace(train.delayReason)
                );
            isOnTime = !train.isCancelled
                       && train.etd.ToUpperInvariant() == "ON TIME"
                       && String.IsNullOrWhiteSpace(train.delayReason);
        }



        public string FuckingPlatform { get; private set; }

        public string ShowerOfBastards { get; private set; }

        public string FuckingDelayReason { get; private set; }

        public string FuckingCancellationReason { get; private set; }

        public string StandardTimeOfFuckingDeparture { get; private set; }
        public string EstimatedTimeOfFuckingDeparture { get; private set; }

        public bool isOnTime { get; private set; }

        public bool isFuckingDelayed { get; private set; }

        public bool isFuckingCancelled { get; private set; }


        public bool IsTooFuckingFarInTheFuture { get; set; }
        public bool TheFuckingServiceIsDownOrSomething { get; set; }

        public bool NoFuckingServicesAvailable { get; set; }


        public string FuckingFrom { get; set; }
        public string FuckingTo { get; set; }
        public List<string> Messages { get; private set; } = new List<string>();

        private static string FormatTrain(TrainResult train)
        {


            var sb = new StringBuilder();


            sb.AppendFormat("{0} from {1} to {2} is ", train.StandardTimeOfFuckingDeparture, train.FuckingFrom, train.FuckingTo);

            if (train.isFuckingCancelled)
            {
                sb.Append("FUCKING CANCELLED");
                if (!String.IsNullOrWhiteSpace(train.FuckingCancellationReason))
                {
                    sb.AppendFormat(" DUE TO {0}", train.FuckingCancellationReason);
                }
            }
            else if (train.isFuckingDelayed)
            {
                sb.AppendFormat("FUCKING DELAYED {0}", train.EstimatedTimeOfFuckingDeparture);
                if (!String.IsNullOrWhiteSpace(train.FuckingDelayReason))
                {
                    sb.AppendFormat(" DUE TO {0}", train.FuckingDelayReason);
                }
            }
            else if (train.isOnTime)
            {
                sb.Append("ON FUCKING TIME APPARENTLY");
            }
            else
            {
                sb.Append("I DONT FUCKING KNOW");
            }
            if (train.FuckingPlatform != null)
            {
                sb.AppendFormat(" from fucking platform {0}", train.FuckingPlatform);
            }
            sb.AppendFormat(" (fucking {0})", train.ShowerOfBastards).AppendLine();
            return sb.ToString();
        }

        public override string ToString()
        {
            return FormatTrain(this);
        }
    }
}
