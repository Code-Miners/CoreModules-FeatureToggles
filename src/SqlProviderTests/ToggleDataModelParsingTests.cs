//-----------------------------------------------------------------------
// <copyright file="ToggleDataModelParsingTests.cs" company="Code Miners Limited">
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

namespace SqlProviderTests
{
    using FeatureToggles;
    using FeatureToggles.Configuration;
    using FeatureToggles.Contrib.SqlProvider.Providers;
    using FeatureToggles.Models;
    using FeatureToggles.Util;
    using NUnit.Framework;

    [TestFixture]
    public class ToggleDataModelParsingTests
    {
        [Test]
        public void InvalidDataModelTest()
        {
            SqlDataProvider provider = new SqlDataProvider(new AppConfigurationProvider(), new TraceLogger());

            Toggle toggle = provider.BuildToggle("test", null, ToggleDataModel.Empty);

            Assert.IsFalse(toggle.IsEnabled, "Flag should be false");
        }

        [Test]
        public void NullUserDataTest()
        {
            SqlDataProvider provider = new SqlDataProvider(new AppConfigurationProvider(), new TraceLogger());

            Toggle toggle = provider.BuildToggle("test", null, new ToggleDataModel(true, string.Empty, string.Empty, string.Empty));

            Assert.IsTrue(toggle.IsEnabled, "Flag should be enabled by default");
        }

        [Test]
        public void RolesPresentInUserDataTest()
        {
            SqlDataProvider provider = new SqlDataProvider(new AppConfigurationProvider(), new TraceLogger());
            ToggleData userData = new ToggleData(string.Empty, string.Empty, "testingRole", "testingRole");
            ToggleDataModel model = new ToggleDataModel(true, "testingRole|role2", string.Empty, string.Empty);

            Toggle toggle = provider.BuildToggle("test", userData, model);

            Assert.IsTrue(toggle.IsEnabled, "Flag should be enabled");
        }

        [Test]
        public void UserIdPresentInUserDataTest()
        {
            SqlDataProvider provider = new SqlDataProvider(new AppConfigurationProvider(), new TraceLogger());
            ToggleData userData = new ToggleData("12345", string.Empty, "testingRole", "testingRole");
            ToggleDataModel model = new ToggleDataModel(true, string.Empty, string.Empty, "12345|abcd");

            Toggle toggle = provider.BuildToggle("test", userData, model);

            Assert.IsTrue(toggle.IsEnabled, "Flag should be enabled");
        }

        [Test]
        public void IpAddressPresentInUserDataTest()
        {
            SqlDataProvider provider = new SqlDataProvider(new AppConfigurationProvider(), new TraceLogger());
            ToggleData userData = new ToggleData("12345", "127.0.0.1", "testingRole", "testingRole");
            ToggleDataModel model = new ToggleDataModel(true, string.Empty, "127.0.0.0/28", string.Empty);

            Toggle toggle = provider.BuildToggle("test", userData, model);

            Assert.IsTrue(toggle.IsEnabled, "Flag should be enabled");
        }

        [Test]
        public void UserIdAndRolesPresentInUserDataTest()
        {
            SqlDataProvider provider = new SqlDataProvider(new AppConfigurationProvider(), new TraceLogger());
            ToggleData userData = new ToggleData("12345", string.Empty, "testingRole", "testingRole");
            ToggleDataModel model = new ToggleDataModel(true, "testingRole|role2", string.Empty, "12345|abcd");

            Toggle toggle = provider.BuildToggle("test", userData, model);

            Assert.IsTrue(toggle.IsEnabled, "Flag should be enabled");
        }

        [Test]
        public void UserIdAndRolesAndIpAddressPresentInUserDataTest()
        {
            SqlDataProvider provider = new SqlDataProvider(new AppConfigurationProvider(), new TraceLogger());
            ToggleData userData = new ToggleData("12345", "127.0.0.1", "testingRole", "testingRole");
            ToggleDataModel model = new ToggleDataModel(true, "testingRole|role2", "127.0.0.0/28", "12345|abcd");

            Toggle toggle = provider.BuildToggle("test", userData, model);

            Assert.IsTrue(toggle.IsEnabled, "Flag should be enabled");
        }

        [Test]
        public void RolesMissingInUserDataTest()
        {
            SqlDataProvider provider = new SqlDataProvider(new AppConfigurationProvider(), new TraceLogger());
            ToggleData userData = new ToggleData(string.Empty, string.Empty, "user", "admin");
            ToggleDataModel model = new ToggleDataModel(true, "testingRole", string.Empty, string.Empty);

            Toggle toggle = provider.BuildToggle("test", userData, model);

            Assert.IsFalse(toggle.IsEnabled, "Flag should be disabled");
        }

        [Test]
        public void UserIdMissingInUserDataTest()
        {
            SqlDataProvider provider = new SqlDataProvider(new AppConfigurationProvider(), new TraceLogger());
            ToggleData userData = new ToggleData("12345", string.Empty, "user", "admin");
            ToggleDataModel model = new ToggleDataModel(true, string.Empty, string.Empty, "abcd");

            Toggle toggle = provider.BuildToggle("test", userData, model);

            Assert.IsFalse(toggle.IsEnabled, "Flag should be disabled");
        }

        [Test]
        public void RoleAndUserIdMissingInUserDataTest()
        {
            SqlDataProvider provider = new SqlDataProvider(new AppConfigurationProvider(), new TraceLogger());
            ToggleData userData = new ToggleData("12345", string.Empty, "user", "admin");
            ToggleDataModel model = new ToggleDataModel(true, "testingRole", string.Empty, "abcd");

            Toggle toggle = provider.BuildToggle("test", userData, model);

            Assert.IsFalse(toggle.IsEnabled, "Flag should be disabled");
        }

        [Test]
        public void RoleAndUserIdPresentButIpAddressMissingInUserDataTest()
        {
            SqlDataProvider provider = new SqlDataProvider(new AppConfigurationProvider(), new TraceLogger());
            ToggleData userData = new ToggleData("12345", string.Empty, "user", "admin");
            ToggleDataModel model = new ToggleDataModel(true, "testingRole", "127.0.0.1/24", "abcd");

            Toggle toggle = provider.BuildToggle("test", userData, model);

            Assert.IsFalse(toggle.IsEnabled, "Flag should be disabled");
        }
    }
}
