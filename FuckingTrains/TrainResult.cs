using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuckingTrains.LDBService;

namespace FuckingTrains
{
    public class TrainResult
    {
        public TrainResult()
        {
        }

        public TrainResult(ServiceItem train)
        {
  

            StandardTimeOfFuckingDeparture = train.std;
            EstimatedTimeOfFuckingDeparture = train.etd;
            FuckingFrom = string.Join(" ", train.origin?.Select(o => o.locationName));
            FuckingTo = string.Join(" ", train.destination?.Select(d => d.locationName));
            FuckingPlatform = train.platform;
            ShowerOfBastards = train.@operator;
            FuckingCancellationReason = train.cancelReason;
            FuckingDelayReason = train.delayReason;

            var isFuckingCancelled = train.isCancelled;
            var isFuckingDelayed = !train.isCancelled && (
                train.etd.ToUpperInvariant() != "ON TIME"
                || !string.IsNullOrWhiteSpace(train.delayReason)
                );
            var isOnTime = !train.isCancelled
                           && train.etd.ToUpperInvariant() == "ON TIME"
                           && string.IsNullOrWhiteSpace(train.delayReason);

            if (isOnTime)
            {
                FuckingTrainState = FuckingTrainStates.OnFuckingTimeApparently;
            }
            else if (isFuckingDelayed)
            {
                FuckingTrainState = FuckingTrainStates.FuckingDelayed;
            }
            else if (isFuckingCancelled)
            {
                FuckingTrainState = FuckingTrainStates.FuckingCancelled;
            }
            else
            {
                FuckingTrainState = FuckingTrainStates.IDontFuckingKnow;
            }
        }

        public FuckingTrainStates FuckingTrainState { get; set; }


        public string FuckingPlatform { get; }

        public string ShowerOfBastards { get; }

        public string FuckingDelayReason { get; }

        public string FuckingCancellationReason { get; }

        public string StandardTimeOfFuckingDeparture { get; }
        public string EstimatedTimeOfFuckingDeparture { get; }

        public string FuckingFrom { get; set; }
        public string FuckingTo { get; set; }
        public List<string> Messages { get; private set; } = new List<string>();
        public int? FuckingDelayInMinutes { get; set; }

        private static string FormatTrain(TrainResult train)
        {
            var sb = new StringBuilder();


            sb.AppendFormat("{0} from {1} to {2} is ", train.StandardTimeOfFuckingDeparture, train.FuckingFrom,
                train.FuckingTo);

            switch (train.FuckingTrainState)
            {
                case FuckingTrainStates.OnFuckingTimeApparently:
                    sb.Append("ON FUCKING TIME APPARENTLY");
                    break;
                case FuckingTrainStates.FuckingDelayed:
                    sb.AppendFormat("FUCKING DELAYED {0}", train.EstimatedTimeOfFuckingDeparture);
                    if (!string.IsNullOrWhiteSpace(train.FuckingDelayReason))
                    {
                        sb.AppendFormat(" DUE TO {0}", train.FuckingDelayReason);
                    }
                    break;
                case FuckingTrainStates.FuckingCancelled:
                    sb.Append("FUCKING CANCELLED");
                    if (!string.IsNullOrWhiteSpace(train.FuckingCancellationReason))
                    {
                        sb.AppendFormat(" DUE TO {0}", train.FuckingCancellationReason);
                    }
                    break;
                case FuckingTrainStates.YouNeedAFuckingCrystalBall:
                    break;
                case FuckingTrainStates.TheFuckingServiceIsDownOrSomething:
                    break;
                case FuckingTrainStates.NoFuckingTrains:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("I DONT FUCKING KNOW");
            }

            if (train.FuckingPlatform != null)
            {
                sb.AppendFormat(" FROM FUCKING PLATFORM {0}", train.FuckingPlatform);
            }
            sb.AppendFormat(" (FUCKING {0})", train.ShowerOfBastards).AppendLine();
            return sb.ToString();
        }

        public override string ToString()
        {
            return FormatTrain(this);
        }
    }
}