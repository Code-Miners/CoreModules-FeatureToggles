//-----------------------------------------------------------------------
// <copyright file="AppConfigDataProvider.cs" company="Code Miners Limited">
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

namespace FeatureToggles.Providers
{
    using System;
    using System.Configuration;

    public class AppConfigDataProvider : IToggleDataProvider
    {
        public Toggle GetFlag(string name)
        {
            string toggle = string.Concat("ToggleFlag:", name);
            string value = ConfigurationManager.AppSettings.Get(toggle);

            if (string.IsNullOrWhiteSpace(value))
            {
                return Toggle.Empty;
            }

            bool flag = value.Equals("true", StringComparison.InvariantCultureIgnoreCase);
            return new Toggle(name, flag);

        }
    }
}
