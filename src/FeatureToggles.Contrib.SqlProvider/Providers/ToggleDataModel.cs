//-----------------------------------------------------------------------
// <copyright file="ToggleDataModel.cs" company="Code Miners Limited">
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
    public class ToggleDataModel
    {
        private readonly bool empty;

        public bool DefaultState { get; }

        public string UserRoles { get; }

        public string IpAddress { get; }

        public string UserId { get; }

        public static ToggleDataModel Empty => new ToggleDataModel();

        private ToggleDataModel()
        {
            empty = true;
        }

        public ToggleDataModel(bool defaultState, string userRoles, string ipAddress, string userId)
        {
            DefaultState = defaultState;
            UserRoles = userRoles;
            IpAddress = ipAddress;
            UserId = userId;
        }

        public static bool IsNullOrEmpty(ToggleDataModel model)
        {
            if (model == null)
            {
                return true;
            }

            return model.empty;
        }
    }
}
