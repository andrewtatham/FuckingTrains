using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuckingTrains;
using NUnit.Framework;

namespace FuckingTrainsTests
{
    [TestFixture]
    public class MonitorSettingsTests
    {
        [Test]
        public void Monitor_Commute_Outbound_GetCrons()
        {
            var actual = Journeys.Commute.Outbound.Monitor.GetCrons();
            var expected = new[]
            {
                "0 20-59/5 6 ? * MON-FRI",
                "0 0-40/5 7 ? * MON-FRI"
            }; // Every 5 mins 6:20 to 7:40 MON-FRI
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void Monitor_Commute_Outbound_GetOffCrons()
        {
            var actual = Journeys.Commute.Outbound.Monitor.GetOffCrons();
            var expected = new[]
            {
                "0 42 7 ? * MON-FRI" // 07:42 MON-FRI
            };
            CollectionAssert.AreEquivalent(expected, actual);
        }
        [Test]
        public void Monitor_Commute_Inbound_GetCrons()
        {
            var actual = Journeys.Commute.Inbound.Monitor.GetCrons();
            var expected = new[]
            {
                "0 15-38/5 16 ? * MON-FRI", // Every 5 mins 16:15 to 16:38 MON-FRI
            };
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void Monitor_Commute_Inbound_GetOffCrons()
        {
            var actual = Journeys.Commute.Inbound.Monitor.GetOffCrons();
            var expected = new[]
            {
                "0 40 16 ? * MON-FRI" // 16:40 MON-FRI
            };
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}
