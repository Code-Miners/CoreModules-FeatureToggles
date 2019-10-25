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
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using Configuration;
    using Models;
    using Util;

    public class AppConfigDataProvider : IToggleDataProvider
    {
        private static readonly ToggleConfigurationSection Configuration = ConfigurationManager.GetSection("ToggleConfiguration") as ToggleConfigurationSection;

        private readonly Dictionary<string, ToggleElement> toggles = new Dictionary<string, ToggleElement>();

        private ILog Logger { get; }

        public AppConfigDataProvider()
        {
            Logger = new TraceLogger();
            Initialise();
        }

        public AppConfigDataProvider(ILog logger)
        {
            Logger = logger;
            Initialise();
        }

        private void Initialise()
        {
            Logger.Debug("Initialising toggles from app config");

            foreach (ToggleElement element in Configuration.Toggles)
            {
                if (!toggles.ContainsKey(element.Name))
                {
                    toggles.Add(element.Name, element);
                }
            }
        }

        public Toggle GetFlag(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            { 
                Logger.Error("Fetching flag with no name provided");
                return Toggle.Empty;
            }

            Logger.Debug("Fetching toggle: " + name);

            if (!toggles.ContainsKey(name))
            {
                return Toggle.Empty;
            }

            ToggleElement element = toggles[name];

            Toggle toggle = new Toggle(element.Name, element.Enabled);

            return toggle;
        }

        public Toggle GetFlag(string name, ToggleData userData)
        {
            if (!toggles.ContainsKey(name))
            {
                Logger.Error("Fetching flag with no name provided");
                return Toggle.Empty;
            }

            Logger.Debug("Fetching toggle (with user data): " + name);

            ToggleElement element = toggles[name];

            if (!string.IsNullOrWhiteSpace(userData.UserRoles))
            {
                List<string> roles = element.Roles.Select(x => x.Name).ToList();
                bool found = false;
                foreach (string role in userData.UserRoles.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (roles.Contains(role))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return new Toggle(name, false);
                }
            }

            if (!string.IsNullOrWhiteSpace(userData.UserId))
            {
                List<string> users = element.Users.Select(x => x.Name).ToList();

                if (!users.Contains(userData.UserId))
                {
                    return new Toggle(name, false);
                }
            }

            if (!string.IsNullOrWhiteSpace(userData.IpAddress))
            {
                if (!IPAddress.TryParse(userData.IpAddress, out IPAddress candidate))
                {
                    return new Toggle(name, false);
                }

                List<string> addresses = element.IpAddresses.Select(x => x.Value).ToList();
                bool found = false;
                foreach (string address in addresses)
                {
                    if (string.IsNullOrWhiteSpace(address))
                    {
                        continue;
                    }

                    IPAddressRange range = IPAddressRange.FromCidrAddress(address);
                    if (range == IPAddressRange.Empty)
                    {
                        continue;
                    }

                    if (range.IPInRange(candidate))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return new Toggle(name, false);
                }
            }

            return new Toggle(name, true);
        }
    }
}
