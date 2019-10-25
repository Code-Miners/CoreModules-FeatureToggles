namespace FeatureToggles.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;

    [ConfigurationCollection(typeof(IpAddressesElement), AddItemName = "ipaddress", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class IpAddressesElementCollection : ConfigurationElementCollection, IEnumerable<IpAddressesElement>
    {
        /// <summary>
        /// Gets or sets a url element from the collection by index
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>The url element</returns>
        public IpAddressesElement this[int index]
        {
            get => BaseGet(index) as IpAddressesElement;
            set
            {
                if (Count <= 0)
                {
                    BaseAdd(index, value);
                    return;
                }

                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        /// <summary>
        /// Iterator for returning url elements from the collection
        /// </summary>
        /// <returns>A url element</returns>
        public new IEnumerator<IpAddressesElement> GetEnumerator()
        {
            int count = Count;
            for (int i = 0; i < count; i++)
            {
                yield return BaseGet(i) as IpAddressesElement;
            }
        }

        /// <summary>
        /// Required protected method for returning a configuration element compatible with this
        /// collection
        /// </summary>
        /// <returns>The configuration element</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new IpAddressesElement();
        }

        /// <summary>
        /// Gets the configuration element key from the provided element
        /// </summary>
        /// <param name="element">The configuration element base class</param>
        /// <returns>The actual instance required</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IpAddressesElement)element).Value;
        }
    }
}