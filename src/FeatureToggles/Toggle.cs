//-----------------------------------------------------------------------
// <copyright file="Toggle.cs" company="Code Miners Limited">
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
    using System;

    public class Toggle : IEquatable<Toggle>
    {
        private readonly bool empty;

        public string Name { get; protected set; }

        public bool IsEnabled { get; }

        public static Toggle Empty => new Toggle();

        private Toggle()
        {
            Name = string.Empty;
            IsEnabled = false;
            empty = true;
        }

        public Toggle(string name, bool enabled)
        {
            Name = name;
            IsEnabled = enabled;
            empty = false;
        }

        public static bool IsNullOrEmpty(Toggle toggle)
        {
            if (toggle == null)
            {
                return true;
            }

            return toggle.empty;
        }

        public bool Equals(Toggle other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return empty == other.empty && string.Equals(Name, other.Name) && IsEnabled == other.IsEnabled;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Toggle) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(Toggle left, Toggle right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Toggle left, Toggle right)
        {
            return !Equals(left, right);
        }
    }

    
}