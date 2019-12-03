//-----------------------------------------------------------------------
// <copyright file="SqlDataProvider.cs" company="Code Miners Limited">
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

namespace FeatureToggles.Contrib.SqlProvider.Providers
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using Configuration;
    using FeatureToggles.Providers;
    using Models;
    using Util;

    public class SqlDataProvider : IToggleDataProvider
    {
        private IToggleConfiguration Configuration { get; }

        private ILog Logger { get; }

        public SqlDataProvider(IToggleConfiguration configuration, ILog logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        [ExcludeFromCodeCoverage]
        public Toggle GetFlag(string name)
        {
            Toggle flag;
            try
            {
                using (SqlConnection conn = new SqlConnection("ToggleFlagConnection"))
                {
                    conn.Open();

                    using (SqlCommand reader = new SqlCommand("GetFlag", conn))
                    {
                        reader.CommandType = CommandType.StoredProcedure;
                        reader.Parameters.AddWithValue("@Name", name);
                        reader.Parameters.AddWithValue("@Environment", Configuration.Environment);

                        object result = reader.ExecuteScalar();

                        if (result == null)
                        {
                            flag = Toggle.Empty;
                        }
                        else
                        {
                            bool enabled = (bool) result;
                            flag = new Toggle(name, enabled);
                        }
                    }
                }

                return flag;
            }
            catch (Exception ex)
            {
                Logger.Error("Error loading toggle by name", ex);
            }

            return Toggle.Empty;
        }

        [ExcludeFromCodeCoverage]
        public Toggle GetFlag(string name, ToggleData userData)
        {
            ToggleDataModel model = ToggleDataModel.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection("ToggleFlagConnection"))
                {
                    conn.Open();

                    using (SqlCommand reader = new SqlCommand("GetFlagWithChecks", conn))
                    {
                        reader.CommandType = CommandType.StoredProcedure;
                        reader.Parameters.AddWithValue("@Name", name);
                        reader.Parameters.AddWithValue("@Environment", Configuration.Environment);

                        using (SqlDataReader result =
                            reader.ExecuteReader(CommandBehavior.SingleRow | CommandBehavior.SingleResult))
                        {
                            while (result.Read())
                            {
                                string userRoles = result.GetString(result.GetOrdinal("roles"));
                                string ipAddress = result.GetString(result.GetOrdinal("ipAdddress"));
                                string userId = result.GetString(result.GetOrdinal("userId"));
                                bool defaultState = result.GetBoolean(result.GetOrdinal("state"));

                                model = new ToggleDataModel(defaultState, userRoles, ipAddress, userId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
                Logger.Error("Error loading toggle by name (with user data)", ex);
                return Toggle.Empty;
            }

            // If the system is happy the flag is ena
            if (ToggleDataModel.IsNullOrEmpty(model))
            {
                return Toggle.Empty;
            }

            return BuildToggle(name, userData, model);
        }

        public Toggle BuildToggle(string name, ToggleData userData, ToggleDataModel model)
        {
            if (ToggleDataModel.IsNullOrEmpty(model))
            {
                return new Toggle(name, false);
            }

            if (userData == null)
            {
                return new Toggle(name, model.DefaultState);
            }

            if (!string.IsNullOrWhiteSpace(userData.UserRoles) && !string.IsNullOrWhiteSpace(model.UserRoles))
            {
                string[] roles = model.UserRoles.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);
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

            if (!string.IsNullOrWhiteSpace(userData.UserId) && !string.IsNullOrWhiteSpace(model.UserId))
            {
                string[] users = model.UserId.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);

                if (!users.Contains(userData.UserId))
                {
                    return new Toggle(name, false);
                }
            }

            if (!string.IsNullOrWhiteSpace(userData.IpAddress) && !string.IsNullOrWhiteSpace(model.IpAddress))
            {
                if (!IPAddress.TryParse(userData.IpAddress, out IPAddress candidate))
                {
                    return new Toggle(name, false);
                }

                string[] addresses = model.IpAddress.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
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