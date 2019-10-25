namespace FeatureToggles.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;

    [ConfigurationCollection(typeof(UsersElement), AddItemName = "user", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class UsersElementCollection : ConfigurationElementCollection, IEnumerable<UsersElement>
    {
        /// <summary>
        /// Gets or sets a url element from the collection by index
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>The url element</returns>
        public UsersElement this[int index]
        {
            get => BaseGet(index) as UsersElement;
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
        public new IEnumerator<UsersElement> GetEnumerator()
        {
            int count = Count;
            for (int i = 0; i < count; i++)
            {
                yield return BaseGet(i) as UsersElement;
            }
        }

        /// <summary>
        /// Required protected method for returning a configuration element compatible with this
        /// collection
        /// </summary>
        /// <returns>The configuration element</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new UsersElement();
        }

        /// <summary>
        /// Gets the configuration element key from the provided element
        /// </summary>
        /// <param name="element">The configuration element base class</param>
        /// <returns>The actual instance required</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UsersElement)element).Name;
        }
    }
}