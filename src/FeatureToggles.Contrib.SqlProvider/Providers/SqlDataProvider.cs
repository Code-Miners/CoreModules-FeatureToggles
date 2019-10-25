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
    using FeatureToggles.Providers;
    using Models;

    public class SqlDataProvider : IToggleDataProvider
    {
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

                        object result = reader.ExecuteScalar();

                        if (result == null)
                        {
                            flag = Toggle.Empty;
                        }
                        else
                        {
                            bool enabled = (bool)result;
                            flag = new Toggle(name, enabled);
                        }
                    }
                }

                return flag;
            }
            catch (Exception)
            {
                
            }

            return Toggle.Empty;
        }

        public Toggle GetFlag(string name, ToggleData userData)
        {
            Toggle flag;
            try
            {
                using (SqlConnection conn = new SqlConnection("ToggleFlagConnection"))
                {
                    conn.Open();

                    using (SqlCommand reader = new SqlCommand("GetFlagWithChecks", conn))
                    {
                        reader.CommandType = CommandType.StoredProcedure;
                        reader.Parameters.AddWithValue("@Name", name);

                        if (!string.IsNullOrWhiteSpace(userData.UserId))
                        {
                            reader.Parameters.AddWithValue("UserId", userData.UserId);
                        }

                        if (!string.IsNullOrWhiteSpace(userData.IpAddress))
                        {
                            reader.Parameters.AddWithValue("IpAddress", userData.IpAddress);
                        }

                        if (!string.IsNullOrWhiteSpace(userData.UserRoles))
                        {
                            reader.Parameters.AddWithValue("UserRoles", userData.UserRoles);
                        }

                        object result = reader.ExecuteScalar();

                        if (result == null)
                        {
                            flag = Toggle.Empty;
                        }
                        else
                        {
                            bool enabled = (bool)result;
                            flag = new Toggle(name, enabled);
                        }
                    }
                }

                return flag;
            }
            catch (Exception)
            {

            }

            return Toggle.Empty;
        }
    }
}
