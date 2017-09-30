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
using System.Linq;

using Id3.Frames;

using Xunit;

namespace Id3.Info.Tests
{
    public abstract class InfoProviderTestsBase
    {
        private readonly Id3Tag _tag;
        private InfoProvider _provider;

        protected InfoProviderTestsBase()
        {
            _tag = new Id3Tag();
        }

        protected void AssertValidResult(Id3Tag[] result)
        {
            Assert.NotEmpty(result);

            Id3Tag checkTag = result[0];
            Assert.NotEmpty(checkTag);
            foreach (Type frameType in Provider.Properties.AvailableOutputs)
            {
                Id3Frame frame = checkTag.FirstOrDefault(f => f.GetType() == frameType && f.IsAssigned);
                Assert.True(frame != null);
            }
        }

        protected abstract InfoProvider CreateProvider();

        protected InfoProvider Provider
        {
            get { return _provider ?? (_provider = CreateProvider()); }
        }

        protected Id3Tag Tag
        {
            get { return _tag; }
        }
    }
}