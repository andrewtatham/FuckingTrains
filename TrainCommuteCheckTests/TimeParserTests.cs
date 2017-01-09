using NUnit.Framework;
using TrainCommuteCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainCommuteCheckTests
{
    [TestFixture]
    public class TimeParserTests
    {
        private readonly TimeParser _t = new TimeParser("23:58");

        [Test]
        public void Hours() => Assert.AreEqual(23, _t.H);

        [Test]
        public void Minutes() => Assert.AreEqual(58, _t.M);

        [Test]
        public void Time() => Assert.AreEqual("23:58", _t.Time);

        [Test]
        public void GreaterThan() => Assert.IsTrue(_t > DateTime.Today.Date.Add(new TimeSpan(23, 57, 0)));

        [Test]
        public void EqualTo() => Assert.IsTrue(_t == DateTime.Today.Date.Add(new TimeSpan(23, 58, 0)));

        [Test]
        public void LessThan() => Assert.IsTrue(_t < DateTime.Today.Date.Add( new TimeSpan(23, 59, 0)));
    }
}