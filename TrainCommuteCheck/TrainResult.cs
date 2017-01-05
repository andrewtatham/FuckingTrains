using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrainCommuteCheck.LiveDepartureBoards;

namespace TrainCommuteCheck
{
    public class TrainResult
    {
        private TrainStatus _trainState;

        public TrainResult()
        {
        }

        public TrainResult(ServiceItem1 train)
        {
            StandardTimeOfDeparture = train.std;
            EstimatedTimeOfDeparture = train.etd;
            From = string.Join(" ", train.origin?.Select(o => o.locationName));
            To = string.Join(" ", train.destination?.Select(d => d.locationName));
            Platform = train.platform;
            ServiceProvider = train.@operator;
            CancellationReason = train.cancelReason;
            DelayReason = train.delayReason;

            var isCancelled = train.isCancelled;
            var isDelayed = !train.isCancelled && (
                train.etd.ToUpperInvariant() != "ON TIME"
                || !string.IsNullOrWhiteSpace(train.delayReason)
                );
            var isOnTime = !train.isCancelled
                           && train.etd.ToUpperInvariant() == "ON TIME"
                           && string.IsNullOrWhiteSpace(train.delayReason);

            if (isOnTime)
            {
                TrainState = TrainStatus.OnTime;
            }
            else if (isDelayed)
            {
                TrainState = TrainStatus.Delayed;
            }
            else if (isCancelled)
            {
                TrainState = TrainStatus.Cancelled;
            }
            else
            {
                TrainState = TrainStatus.Unknown;
            }
        }

        public TrainStatus TrainState
        {
            get { return _trainState; }
            set
            {
                _trainState = value;
                TrainStateDescription = _trainState.ToString();
            }
        }

        public string TrainStateDescription { get; private set; }

        public string Platform { get; }

        public string ServiceProvider { get; }

        public string DelayReason { get; }

        public string CancellationReason { get; }

        public string StandardTimeOfDeparture { get; set; }
        public string EstimatedTimeOfDeparture { get; }

        public string From { get; set; }
        public string To { get; set; }
        public List<string> Messages { get; private set; } = new List<string>();
        public int? DelayInMinutes { get; set; }
        public TrainResult[] AllServices { get; set; }


        private static string FormatTrain(TrainResult train)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(train.StandardTimeOfDeparture)
                && !string.IsNullOrEmpty(train.From)
                && !string.IsNullOrEmpty(train.To))
            {
                sb.AppendFormat("The {0} from {1} to {2} is ", train.StandardTimeOfDeparture, train.From, train.To);
            }


            switch (train.TrainState)
            {
                case TrainStatus.OnTime:
                    sb.Append("ON TIME");
                    break;
                case TrainStatus.Delayed:
                    sb.AppendFormat("DELAYED {0}", train.EstimatedTimeOfDeparture);
                    if (!string.IsNullOrWhiteSpace(train.DelayReason))
                    {
                        sb.AppendFormat(" DUE TO {0}", train.DelayReason);
                    }
                    break;
                case TrainStatus.Cancelled:
                    sb.Append("CANCELLED");
                    if (!string.IsNullOrWhiteSpace(train.CancellationReason))
                    {
                        sb.AppendFormat(" DUE TO {0}", train.CancellationReason);
                    }
                    break;
                case TrainStatus.TooFarAhead:
                    sb.Append("YOU NEED A  CRYSTAL BALL");
                    break;
                case TrainStatus.ServiceDown:
                    sb.Append("THE SERVICE IS DOWN OR SOMETHING");
                    break;
                case TrainStatus.NoTrains:
                    sb.Append("NO TRAINS");
                    break;
                case TrainStatus.Unknown:
                    sb.Append("I DONT KNOW");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(train.TrainState.ToString());
            }

            if (!string.IsNullOrEmpty(train.Platform))
            {
                sb.AppendFormat(" from platform: {0}", train.Platform);
            }
            if (!string.IsNullOrEmpty(train.ServiceProvider))
            {
                sb.AppendFormat(" ({0})", train.ServiceProvider).AppendLine();
            }

            if (train.AllServices != null)
            {
                foreach (var service in train.AllServices)
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