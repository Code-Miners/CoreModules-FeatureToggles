

namespace FeatureToggles.Configuration
{
    using System.Configuration;

    /// <summary>
    /// The xml config section for services
    /// </summary>
    public class ToggleConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Gets the url collection for services
        /// </summary>
        [ConfigurationProperty("toggles", IsRequired = true)]
        public ToggleElementCollection Toggles => this["toggles"] as ToggleElementCollection;
    }
}
