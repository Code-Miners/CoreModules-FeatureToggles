namespace FeatureToggles.Configuration
{
    using System.Configuration;

    /// <summary>
    /// The url element represents the raw Xml config as an object
    /// </summary>
    public class RoleElement : ConfigurationElement
    {
        /// <summary>
        /// The unique url element key
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name => this["name"] as string;
    }
}