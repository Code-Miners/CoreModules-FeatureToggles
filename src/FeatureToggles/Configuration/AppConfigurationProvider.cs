

namespace FeatureToggles.Configuration
{
    using System;
    using System.Configuration;

    public class AppConfigurationProvider : IToggleConfiguration
    {
        public bool SystemEnabled
        {
            get
            {
                string value = ConfigurationManager.AppSettings.Get("Toggle:Enabled");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return false;
                }

                return value.Equals("true", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public bool DefaultValue
        {
            get
            {
                string value = ConfigurationManager.AppSettings.Get("Toggle:DefaultValue");

                if (string.IsNullOrWhiteSpace(value))
                {
                    return false;
                }

                return value.Equals("true", StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }
}
