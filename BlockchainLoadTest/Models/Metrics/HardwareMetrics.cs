using System;
namespace BlockchainLoadTest.Models.Metrics
{
    public class HardwareMetrics
    {
        public Decimal CPU { get; set; }

        public Decimal Memory { get; set; }

        public Decimal IO { get; set; }

        public HardwareMetrics()
        {
        }
    }
}