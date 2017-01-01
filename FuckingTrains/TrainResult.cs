using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuckingTrains.LDBService;

namespace FuckingTrains
{
    public class TrainResult
    {
        private FuckingTrainStates _fuckingTrainState;

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

        public FuckingTrainStates FuckingTrainState
        {
            get { return _fuckingTrainState; }
            set
            {
                _fuckingTrainState = value;
                FuckingTrainStateDescription = _fuckingTrainState.ToString();
            }
        }
        public string FuckingTrainStateDescription { get; private set; }

        public string FuckingPlatform { get; }

        public string ShowerOfBastards { get; }

        public string FuckingDelayReason { get; }

        public string FuckingCancellationReason { get; }

        public string StandardTimeOfFuckingDeparture { get; set; }
        public string EstimatedTimeOfFuckingDeparture { get; }

        public string FuckingFrom { get; set; }
        public string FuckingTo { get; set; }
        public List<string> Messages { get; private set; } = new List<string>();
        public int? FuckingDelayInMinutes { get; set; }
        public TrainResult[] AllFuckingServices { get; set; }


        private static string FormatTrain(TrainResult train)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(train.StandardTimeOfFuckingDeparture)
                && !string.IsNullOrEmpty(train.FuckingFrom) 
                && !string.IsNullOrEmpty(train.FuckingTo))
            {
                sb.AppendFormat("The {0} from {1} to {2} is ", train.StandardTimeOfFuckingDeparture, train.FuckingFrom, train.FuckingTo);
            }


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
                    sb.Append("YOU NEED A FUCKING CRYSTAL BALL");
                    break;
                case FuckingTrainStates.TheFuckingServiceIsDownOrSomething:
                    sb.Append("THE FUCKING SERVICE IS DOWN OR SOMETHING");
                    break;
                case FuckingTrainStates.NoFuckingTrains:
                    sb.Append("NO FUCKING TRAINS");
                    break;
                case FuckingTrainStates.IDontFuckingKnow:
                    sb.Append("I DONT FUCKING KNOW");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(train.FuckingTrainState.ToString());
            }

            if (!string.IsNullOrEmpty(train.FuckingPlatform))
            {
                sb.AppendFormat(" from fucking platform: {0}", train.FuckingPlatform);
            }
            if (!string.IsNullOrEmpty(train.ShowerOfBastards))
            {
                sb.AppendFormat(" (shower of bastards: {0})", train.ShowerOfBastards).AppendLine();
            }

            if (train.AllFuckingServices!=null)
            {
                foreach (var service in train.AllFuckingServices)
                {
                    sb.AppendLine(FormatTrain(service));
                }
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return FormatTrain(this);
        }
    }
}