namespace FeatureToggles.Configuration
{
    using System.Configuration;

    /// <summary>
    /// The url element represents the raw Xml config as an object
    /// </summary>
    public class ToggleElement : ConfigurationElement
    {
        /// <summary>
        /// Gets the url collection for services
        /// </summary>
        [ConfigurationProperty("roles", IsRequired = false)]
        public RolesElementCollection Roles => this["roles"] as RolesElementCollection;

        /// <summary>
        /// Gets the url collection for services
        /// </summary>
        [ConfigurationProperty("users", IsRequired = false)]
        public UsersElementCollection Users => this["users"] as UsersElementCollection;

        /// <summary>
        /// Gets the url collection for services
        /// </summary>
        [ConfigurationProperty("ipaddresses", IsRequired = false)]
        public IpAddressesElementCollection IpAddresses => this["ipaddresses"] as IpAddressesElementCollection;

        /// <summary>
        /// The unique url element key
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name => this["name"] as string;

        /// <summary>
        /// The url element value. This will be the fully qualified url for the endpoint
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true, DefaultValue = true)]
        public bool Enabled => (bool)this["enabled"];
    }
}