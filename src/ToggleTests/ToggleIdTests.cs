//-----------------------------------------------------------------------
// <copyright file="ToggleIdTests.cs" company="Code Miners Limited">
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
    using FeatureToggles;
    using NUnit.Framework;

    [TestFixture]
    public class ToggleIdTests
    {
        [Test]
        public void EqualityTest()
        {
            ToggleId id = new ToggleId("test");
            ToggleId id2 = new ToggleId("test");

            Assert.AreEqual(id, id2, "Toggle ids have the same name so should be equal");
        }

        [Test]
        public void InequalityTest()
        {
            ToggleId id = new ToggleId("test");
            ToggleId id2 = new ToggleId("test2");

            Assert.AreNotEqual(id, id2, "Toggle ids have the different names so should be different");
        }
    }
}
