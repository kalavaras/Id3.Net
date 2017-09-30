using System;
using System.IO;
using System.Net;
using System.Xml;

namespace Id3.Info.LyricWiki
{
    internal sealed class LyricWiki2
    {
        internal string GetLyrics(string artistName, string songName)
        {
            if (artistName == null)
                throw new ArgumentNullException("artistName");

            const string UrlFormat = @"http://lyricwiki.org/api.php?func=getSong&artist={0}&song={1}&fmt=xml";

            artistName = Utility.UrlEncode(artistName.Replace(' ', '_'));
            if (!string.IsNullOrEmpty(songName))
                songName = Utility.UrlEncode(songName.Replace(' ', '_'));

            string url = string.Format(UrlFormat, artistName, songName);
            var request = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
                return null;
            using (Stream responseStream = response.GetResponseStream())
            {
                if (responseStream == null)
                    return null;

                var xdoc = new XmlDocument();
                xdoc.Load(responseStream);
                if (xdoc.DocumentElement == null)
                    return null;
                XmlNode lyricsNode = xdoc.SelectSingleNode("LyricsResult/lyrics");
                if (lyricsNode == null)
                    return null;
                string lyrics = lyricsNode.InnerText;
                return string.IsNullOrEmpty(lyrics) || lyrics.Equals("Not found", StringComparison.OrdinalIgnoreCase)
                    ? null : lyrics.Replace("\n", Environment.NewLine);
            }
        }
    }
}