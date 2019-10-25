//-----------------------------------------------------------------------
// <copyright file="ConfigurationTests.cs" company="Code Miners Limited">
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

namespace ToggleTests
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using FeatureToggles.Configuration;
    using NUnit.Framework;

    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ConfigurationTests
    {
        [Test]
        public void LoadSectionConfigTest()
        {
            ToggleConfigurationSection section = ConfigurationManager.GetSection("ToggleConfiguration") as ToggleConfigurationSection;
            
            Assert.IsNotNull(section);
        }

        [Test]
        public void ValidateSettingsTest()
        {
           ToggleConfigurationSection section = ConfigurationManager.GetSection("ToggleConfiguration") as ToggleConfigurationSection;

           ToggleElement toggle = section.Toggles[0];

           Assert.IsNotNull(toggle);

           Assert.AreEqual("CacheInheritableDatasource", toggle.Name);
           Assert.IsTrue(toggle.Users.Count > 0);
           Assert.IsTrue(toggle.Roles.Count > 0);
           Assert.IsTrue(toggle.IpAddresses.Count > 0);
        }

        [Test]
        public void ValidateUsersTest()
        {
           ToggleConfigurationSection section = ConfigurationManager.GetSection("ToggleConfiguration") as ToggleConfigurationSection;

           ToggleElement toggle = section.Toggles[0];

           Assert.IsNotNull(toggle);

           Assert.AreEqual("CacheInheritableDatasource", toggle.Name);
           Assert.IsTrue(toggle.Users.Count > 0);
           
           Assert.AreEqual("abcd", toggle.Users[0].Name);
        }

        [Test]
        public void ValidateRolesTest()
        {
           ToggleConfigurationSection section = ConfigurationManager.GetSection("ToggleConfiguration") as ToggleConfigurationSection;

           ToggleElement toggle = section.Toggles[0];

           Assert.IsNotNull(toggle);

           Assert.AreEqual("CacheInheritableDatasource", toggle.Name);
           Assert.IsTrue(toggle.Roles.Count > 0);
           
           Assert.AreEqual("Staff", toggle.Roles[0].Name);
        }

        [Test]
        public void ValidateIpAddressesTest()
        {
           ToggleConfigurationSection section = ConfigurationManager.GetSection("ToggleConfiguration") as ToggleConfigurationSection;

           ToggleElement toggle = section.Toggles[0];

           Assert.IsNotNull(toggle);

           Assert.AreEqual("CacheInheritableDatasource", toggle.Name);
           Assert.IsTrue(toggle.IpAddresses.Count > 0);
           
           Assert.AreEqual("127.0.0.1/28", toggle.IpAddresses[0].Value);
        }
    }
}
