
//-----------------------------------------------------------------------
// <copyright file="ToggleTests.cs" company="Code Miners Limited">
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
    using System.Diagnostics.CodeAnalysis;
    using FeatureToggles;
    using FeatureToggles.Configuration;
    using FeatureToggles.Providers;
    using Moq;
    using NUnit.Framework;
    using TestModels;

    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class StrongTypeTests
    {
        [Test]
        public void TestStrongToggleEnabled()
        {
            Mock<IToggleDataProvider> dataProvider = new Mock<IToggleDataProvider>(MockBehavior.Strict);
            dataProvider.Setup(x => x.GetFlag("StrongToggle"))
                .Returns(new Toggle("StrongToggle", true));
            
            ToggleFactory factory = new ToggleFactory(GetEnabledConfiguration(), dataProvider.Object);

            Toggle t = factory.Get("StrongToggle");

            Assert.IsFalse(t == Toggle.Empty);
            Assert.IsFalse(Toggle.IsNullOrEmpty(t));
            Assert.AreEqual("StrongToggle", t.Name);
            Assert.IsTrue(t.IsEnabled);
        }

        [Test]
        public void FromConfigTest()
        {
            ToggleFactory factory = new ToggleFactory(new AppConfigurationProvider(), new AppConfigDataProvider());

            Toggle toggle = factory.Get<StrongToggleId>();

            Assert.IsFalse(toggle == Toggle.Empty);
            Assert.IsFalse(Toggle.IsNullOrEmpty(toggle));
            Assert.AreEqual("StrongToggleId", toggle.Name);
            Assert.IsTrue(toggle.IsEnabled);
        }


        private IToggleConfiguration GetEnabledConfiguration()
        {
            Mock<IToggleConfiguration> config = new Mock<IToggleConfiguration>(MockBehavior.Strict);
            config.SetupGet(x => x.SystemEnabled)
                .Returns(true);

            config.SetupGet(x => x.DefaultValue)
                .Returns(false);

            return config.Object;
        }
    }
}
