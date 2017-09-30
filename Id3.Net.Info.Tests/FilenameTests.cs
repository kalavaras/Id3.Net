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

using System;
using System.Collections.Generic;

using Id3.Info.Filename;

using Xunit;
using Xunit.Extensions;

namespace Id3.Info.Tests
{
    public sealed class FilenameTests : InfoProviderTestsBase
    {
        [Theory]
        [PropertyData("SuccessFilenames")]
        public void DefaultFormatSuccess(string filename)
        {
            InfoProviderInputs inputs = InfoProviderInputs.Create(filename);
            Id3Tag[] tags = Provider.GetInfo(inputs);
            AssertValidResult(tags);
        }

        [Theory]
        [PropertyData("FailureFilenames")]
        public void DefaultFormatFailure(string filename)
        {
            InfoProviderInputs inputs = InfoProviderInputs.Create(filename);
            Assert.Throws<Exception>(() => Provider.GetInfo(inputs));
        }

        public static IEnumerable<object[]> SuccessFilenames
        {
            get
            {
                yield return new object[] { "Metallica - Enter sandman.mp3" };
                yield return new object[] { "Cheap Trick - Mighty wings - Top Gun.mp3" };
            }
        }

        public static IEnumerable<object[]> FailureFilenames
        {
            get
            {
                yield return new object[] { string.Empty };
                yield return new object[] { null };
            }
        }

        protected override InfoProvider CreateProvider()
        {
            return new FilenameInfoProvider();
        }
    }
}
