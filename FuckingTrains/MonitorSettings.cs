using System;
using System.Collections.Generic;
using System.Linq;

namespace FuckingTrains
{
    public class MonitorSettings
    {
        public string Days { get; set; }
        public FuckingTime From { get; set; }
        public FuckingTime To { get; set; }
        public int Every { get; set; }
        public FuckingTime Off { get; set; }

        public string[] GetCrons()
        {
            if (From.H == To.H)
            {
                return new[] {$"0 {From.M}-{To.M}/{Every} {From.H} ? * {Days}"};
            }
            else
            {
                List<string> crons = new List<string>();
                int n = To.H - From.H;
                for (int i = 0; i <= n; i++)
                {
                    int h = From.H + i;
                    if (h == From.H)
                    {
                        crons.Add($"0 {From.M}-59/{Every} {h} ? * {Days}");
                    }
                    else if (h == To.H)
                    {
                        crons.Add($"0 0-{To.M}/{Every} {h} ? * {Days}");
                    }
                    else
                    {
                        crons.Add($"0 0/{Every} {h} ? * {Days}");
                    }

                }
                return crons.ToArray();

            }
        }

        public string[] GetOffCrons()
        {
            return new[]
            {
                $"0 {Off.M} {Off.H} ? * {Days}"
            };
        }
    }
}