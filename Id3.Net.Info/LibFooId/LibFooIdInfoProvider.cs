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

using Id3.Frames;
using Id3.Info;

// http://bazaar.launchpad.net/~tsmithe/what-tune/trunk/files/toby%40cheetah-20080114183856-8axnltijxmzi25o5?file_id=lib-20071023150321-rqujqhmi6byf2aow-1

namespace Id3.Info.Foosic
{
    public sealed class FoosicInfoProvider : InfoProvider
    {
        protected override Id3Tag[] GetTagInfo(Id3Tag tag)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override InfoProviderProperties GetProperties()
        {
            var properties = new InfoProviderProperties("Foosic LibFooId", "http://www.foosic.com", null) {
                RequiresStream = true
            };
            properties.AvailableOutputs.AddMultiple(typeof(AlbumFrame), typeof(ArtistsFrame), typeof(TitleFrame));
            return properties;
        }
    }
}