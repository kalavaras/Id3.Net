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
using System.IO;
using System.Net;

using Id3.Frames;

namespace Id3.Info.LyrDb
{
    public sealed class LyrDbInfoProvider : InfoProvider
    {
        protected override Id3Tag[] GetTagInfo(Id3Tag tag)
        {
            const string SearchLyricsUrlFormat = "http://webservices.lyrdb.com/lookup.php?q={0}|{1}&for=match&agent=ID3.NET/1.0";
            const string RetrieveLyricsUrlFormat = "http://webservices.lyrdb.com/getlyr.php?q={0}";

            string searchResult = GetWebResponse(SearchLyricsUrlFormat, Utility.UrlEncode(tag.Artists.Value[0]), Utility.UrlEncode(tag.Title.Value));
            if (string.IsNullOrEmpty(searchResult))
                return Id3Tag.Empty;
            string[] lineParts = searchResult.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            string id = lineParts[0];
            if (string.IsNullOrEmpty(id))
                return Id3Tag.Empty;

            string lyricsResult = GetWebResponse(RetrieveLyricsUrlFormat, id);
            string lyrics = lyricsResult.Replace("\x000D\x000A", Environment.NewLine).Replace("\x000A", Environment.NewLine);

            var result = new Id3Tag();
            result.Lyrics.Add(new LyricsFrame {
                Lyrics = lyrics
            });
            return new[] { result };
        }

        protected override InfoProviderProperties GetProperties()
        {
            var properties = new InfoProviderProperties("LyrDB", "http://www.lyrdb.com/", "http://www.lyrdb.com/services/lws-tech.php");
            properties.RequiredInputs.Add<ArtistsFrame>();
            properties.RequiredInputs.Add<TitleFrame>();
            properties.AvailableOutputs.Add<LyricsFrame>();
            return properties;
        }

        private static string GetWebResponse(string urlFormat, params object[] args)
        {
            string url = string.Format(urlFormat, args);
            var request = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            var response = (HttpWebResponse)request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                if (responseStream == null)
                    return null;
                using (var reader = new StreamReader(responseStream))
                    return reader.ReadToEnd();
            }
        }
    }
}