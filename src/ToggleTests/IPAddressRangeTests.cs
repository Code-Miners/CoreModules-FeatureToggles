//-----------------------------------------------------------------------
// <copyright file="IPAddressRangeTests.cs" company="Code Miners Limited">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using FeatureToggles.Models;
    using NUnit.Framework;

    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class IPAddressRangeTests
    {
        [Test]
        public void ParseCidrBlockTest()
        {
            string ip = "127.10.0.0/28";
            IPAddressRange range = IPAddressRange.FromCidrAddress(ip);

            IPAddress.TryParse("127.10.0.0", out IPAddress lower);
            IPAddress.TryParse("127.10.0.15", out IPAddress higher);

            Assert.IsFalse(IPAddressRange.IsNullOrEmpty(range));
            Assert.AreEqual(lower, range.Lower);
            Assert.AreEqual(higher, range.Upper);
            Console.Error.WriteLine(range.ToString());
        }

        [Test]
        public void Foo()
        {
            string ip = "127.0.0.1/28";
            IPAddressRange range = IPAddressRange.FromCidrAddress(ip);

            Assert.IsFalse(IPAddressRange.IsNullOrEmpty(range));
            Console.Error.WriteLine(range.ToString());
        }

        [Test]
        public void IPAddressInRangeTest()
        {
            string ip = "127.10.0.0/28";
            IPAddressRange range = IPAddressRange.FromCidrAddress(ip);

            IPAddress.TryParse("127.10.0.1", out IPAddress candidate);
            
            Assert.IsTrue(range.IPInRange(candidate));
        }

        [Test]
        public void IPAddressNotInRangeTest()
        {
            string ip = "127.10.0.0/28";
            IPAddressRange range = IPAddressRange.FromCidrAddress(ip);

            IPAddress.TryParse("192.168.0.1", out IPAddress candidate);
            
            Assert.IsFalse(range.IPInRange(candidate));
        }
    }
}
