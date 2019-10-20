//-----------------------------------------------------------------------
// <copyright file="ToggleFactoryTests.cs" company="Code Miners Limited">
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
    using System;
    using FeatureToggles;
    using FeatureToggles.Configuration;
    using FeatureToggles.Providers;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ToggleFactoryTests
    {
        [Test]
        public void GetMissingToggleTest()
        {
            Mock<IToggleConfiguration> config = new Mock<IToggleConfiguration>(MockBehavior.Strict);
            config.SetupGet(x => x.SystemEnabled)
                .Returns(true);

            config.SetupGet(x => x.DefaultValue)
                .Returns(false);

            Mock<IToggleDataProvider> dataProvider = new Mock<IToggleDataProvider>(MockBehavior.Strict);
            dataProvider.Setup(x => x.GetFlag(It.IsAny<string>()))
                .Returns((Toggle) null);

            ToggleFactory factory = new ToggleFactory(config.Object, dataProvider.Object);

            Toggle t = factory.Get("test");

            Assert.IsNotNull(t, "Toggle should never be null");
            Assert.IsTrue(Toggle.IsNullOrEmpty(t));
            Assert.IsFalse(t.IsEnabled, "Toggle should return the configured value of true");
        }

        [Test]
        public void GetToggleTest()
        {
            Mock<IToggleConfiguration> config = new Mock<IToggleConfiguration>(MockBehavior.Strict);
            config.SetupGet(x => x.SystemEnabled)
                .Returns(true);

            config.SetupGet(x => x.DefaultValue)
                .Returns(false);

            Mock<IToggleDataProvider> dataProvider = new Mock<IToggleDataProvider>(MockBehavior.Strict);
            dataProvider.Setup(x => x.GetFlag(It.IsAny<string>()))
                .Returns(new Toggle("test", true));

            ToggleFactory factory = new ToggleFactory(config.Object, dataProvider.Object);

            Toggle t = factory.Get("test");

            Assert.IsTrue(t.IsEnabled, "Toggle should return the configured value of true");
        }

        [Test]
        public void GetDefaultValueTest()
        {
            Mock<IToggleConfiguration> config = new Mock<IToggleConfiguration>(MockBehavior.Strict);
            config.SetupGet(x => x.SystemEnabled)
                .Returns(false);

            config.SetupGet(x => x.DefaultValue)
                .Returns(true);

            Mock<IToggleDataProvider> dataProvider = new Mock<IToggleDataProvider>(MockBehavior.Strict);
            dataProvider.Setup(x => x.GetFlag(It.IsAny<string>()))
                .Returns(new Toggle("test", false));

            ToggleFactory factory = new ToggleFactory(config.Object, dataProvider.Object);

            Toggle t = factory.Get("test");

            Assert.IsTrue(t.IsEnabled, "Toggle should return the default value of true");
        }
    }
}
