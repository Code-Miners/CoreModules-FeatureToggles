namespace FeatureToggles.Configuration
{
    using System.Configuration;

    /// <summary>
    /// The url element represents the raw Xml config as an object
    /// </summary>
    public class IpAddressesElement : ConfigurationElement
    {
        /// <summary>
        /// The url element value. This will be the fully qualified url for the endpoint
        /// </summary>
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value => this["value"] as string;
    }
}