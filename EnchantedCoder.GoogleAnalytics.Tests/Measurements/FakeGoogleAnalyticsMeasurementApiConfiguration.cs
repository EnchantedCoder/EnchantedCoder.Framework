using System;
using System.Collections.Generic;
using System.Text;
using EnchantedCoder.GoogleAnalytics.Measurements;

namespace EnchantedCoder.GoogleAnalytics.Tests.Measurements
{
    internal class FakeGoogleAnalyticsMeasurementApiConfiguration : IGoogleAnalyticsMeasurementApiConfiguration
    {
        public string MeasurementEndpointUrl => "fakegaendpoint";

        public string GoogleAnalyticsTrackingId => "UA-FAKE";
    }
}
