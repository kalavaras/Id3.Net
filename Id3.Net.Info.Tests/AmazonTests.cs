#region --- License & Copyright Notice ---
/*
Copyright (c) 2005-2012 Jeevan James
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

using Id3.Info.Amazon;
using Id3.Tests;

using Xunit;
using Xunit.Extensions;

namespace Id3.Info.Tests
{
    public sealed class AmazonTests : InfoProviderTestsBase
    {
        [Theory, XmlResourceData("Id3.Net.Info.Tests.Data.ValidSongs.xml")]
        public void CheckWithValidInputs(string artist, string title)
        {
            Tag.Artists.Values.Add(artist);
            Tag.Title.Value = title;

            Id3Tag[] result = Provider.GetInfo(Tag, InfoProviderInputs.Default);
            AssertValidResult(result);
        }

        [Theory, XmlResourceData("Id3.Net.Info.Tests.Data.InvalidSongs.xml")]
        public void CheckWithInvalidInputs(string artist, string title)
        {
            Tag.Artists.Values.Add(artist);
            Tag.Title.Value = title;

            Id3Tag[] result = Provider.GetInfo(Tag, InfoProviderInputs.Default);

            Assert.Empty(result);
        }

        [Fact]
        public void CheckWithInsufficientData()
        {
            Assert.Throws<InfoProviderException>(() => Provider.GetInfo(Tag, InfoProviderInputs.Default));
            Assert.Throws<InfoProviderException>(() => Provider.GetInfo(InfoProviderInputs.Default));
        }

        protected override InfoProvider CreateProvider()
        {
            return new AmazonInfoProvider();
        }
    }
}