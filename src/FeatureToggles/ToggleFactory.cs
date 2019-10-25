//-----------------------------------------------------------------------
// <copyright file="ToggleFactory.cs" company="Code Miners Limited">
//  Copyright (c) 2019 Code Miners Limited
//   
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//  GNU Lesser General Public License for more details.
//  
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.If not, see<https://www.gnu.org/licenses/>.
// </copyright>
//-----------------------------------------------------------------------

namespace FeatureToggles
{
    using Configuration;
    using Models;
    using Providers;

    public class ToggleFactory : IToggleFactory
    {
        private IToggleConfiguration Configuration { get; }

        private IToggleDataProvider DataProvider { get; }

        public ToggleFactory(IToggleConfiguration configuration, IToggleDataProvider dataProvider)
        {
            Configuration = configuration;
            DataProvider = dataProvider;
        }

        public Toggle Get(string name)
        {
            if (!Configuration.SystemEnabled)
            {
                return new Toggle(name, Configuration.DefaultValue);
            }

            Toggle data = DataProvider.GetFlag(name);
            if (data == null)
            {
                return Toggle.Empty;
            }

            return data;
        }
        public Toggle Get(string name, ToggleData userData)
        {
            if (!Configuration.SystemEnabled)
            {
                return new Toggle(name, Configuration.DefaultValue);
            }

            if (userData == null)
            {
                return Get(name);
            }

            Toggle data = DataProvider.GetFlag(name, userData);
            if (data == null)
            {
                return Toggle.Empty;
            }

            return data;
        }

        public Toggle Get<T>() where T: ToggleId
        {
            string name = typeof(T).Name;
            if (!Configuration.SystemEnabled)
            {
                return new Toggle(name, Configuration.DefaultValue);
            }

            Toggle data = DataProvider.GetFlag(name);
            if (data == null)
            {
                return Toggle.Empty;
            }

            return data;
        }

        public Toggle Get<T>(ToggleData userData) where T: ToggleId
        {
            string name = typeof(T).Name;
            if (!Configuration.SystemEnabled)
            {
                return new Toggle(name, Configuration.DefaultValue);
            }

            if (userData == null)
            {
                return Get<T>();
            }

            Toggle data = DataProvider.GetFlag(name, userData);
            if (data == null)
            {
                return Toggle.Empty;
            }

            return data;
        }
    }
}
