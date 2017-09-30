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

using Id3.Frames;

namespace Id3.Info.LyricWiki
{
    public sealed class LyricWikiInfoProvider : InfoProvider
    {
        protected override Id3Tag[] GetTagInfo(Id3Tag tag)
        {
            var service = new LyricWiki2();
            string lyrics = service.GetLyrics(tag.Artists.Value[0], tag.Title.Value);
            if (string.IsNullOrEmpty(lyrics))
                return Id3Tag.Empty;

            var lyricsFrame = new LyricsFrame {
                Lyrics = lyrics,
            };
            var result = new Id3Tag();
            result.Lyrics.Add(lyricsFrame);
            return new[] { result };
        }

        protected override InfoProviderProperties GetProperties()
        {
            var properties = new InfoProviderProperties("Lyric Wiki", "http://lyricwiki.org", null);
            properties.RequiredInputs.Add<ArtistsFrame>();
            properties.RequiredInputs.Add<TitleFrame>();
            properties.AvailableOutputs.Add<LyricsFrame>();
            return properties;
        }
    }
}