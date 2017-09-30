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
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Xml;

using Id3.Frames;

namespace Id3.Info.Discogs
{
    public sealed class DiscogsInfoProvider : InfoProvider
    {
        protected override Id3Tag[] GetTagInfo(Id3Tag tag)
        {
            string artists = string.Join("/", tag.Artists.Value);
            XmlDocument artistDoc = CallService(GetArtistFormat, Utility.EncodeSearchTermsForUrl(artists), ApiKey);
            if (artistDoc == null)
                return Id3Tag.Empty;

            var results = new List<Id3Tag>();
            List<Id3Frame> artistPictureFrames = GetArtistPictures(artistDoc);
            List<Id3Frame> artistUrlFrames = GetArtistUrls(artistDoc);

            IEnumerable<string> releaseIds = GetReleaseIds(artistDoc, tag.Album.IsAssigned ? tag.Album.Value : null);
            foreach (string releaseId in releaseIds)
            {
                XmlDocument releaseDoc = CallService(GetReleaseFormat, releaseId, ApiKey);
                if (releaseDoc == null)
                    continue;
                foreach (Id3Tag result in GetMatchingTitleTags(releaseDoc, tag.Title.Value))
                {
                    AddReleaseDetails(releaseDoc, result);
                    foreach (PictureFrame artistPictureFrame in artistPictureFrames)
                        result.Pictures.Add(artistPictureFrame);
                    foreach (ArtistUrlFrame artistUrlFrame in artistUrlFrames)
                        result.ArtistUrls.Add(artistUrlFrame);
                    results.Add(result);
                }
            }
            return results.ToArray();
        }

        private static List<Id3Frame> GetArtistPictures(XmlNode xdoc)
        {
            var pictureFrames = new List<Id3Frame>();
            XmlNodeList imageNodes = xdoc.SelectNodes("resp/artist/images/image");
            if (imageNodes == null)
                return pictureFrames;
            foreach (XmlNode imageNode in imageNodes)
            {
                if (imageNode.Attributes == null)
                    continue;
                PictureFrame pictureFrame = Utility.GetPicture(imageNode.Attributes["uri"].Value);
                if (pictureFrame != null)
                {
                    pictureFrame.PictureType = PictureType.ArtistOrPerformer;
                    pictureFrames.Add(pictureFrame);
                }
            }
            return pictureFrames;
        }

        private static List<Id3Frame> GetArtistUrls(XmlNode xdoc)
        {
            var urlFrames = new List<Id3Frame>();
            XmlNodeList urlNodes = xdoc.SelectNodes("resp/artist/urls/url");
            if (urlNodes == null)
                return urlFrames;
            foreach (XmlNode urlNode in urlNodes)
            {
                var urlFrame = new ArtistUrlFrame {
                    Url = urlNode.InnerText
                };
                urlFrames.Add(urlFrame);
            }
            return urlFrames;
        }

        private IEnumerable<string> GetReleaseIds(XmlNode xdoc, string albumName)
        {
            XmlNodeList releaseNodes = xdoc.SelectNodes("resp/artist/releases/release");
            if (releaseNodes == null)
                return new string[0];

            var releaseIds = new List<string>(releaseNodes.Count);
            foreach (XmlNode releaseNode in releaseNodes)
            {
                if (releaseNode.Attributes == null)
                    continue;

                string status = releaseNode.Attributes["status"].Value;
                string type = releaseNode.Attributes["type"].Value;
                if (status != "Accepted" || type != "Main")
                    continue;
                XmlNode releaseTitleNode = releaseNode.SelectSingleNode("title");
                if (releaseTitleNode == null)
                    continue;
                if (albumName == null || Utility.TolerantEquals(albumName, releaseTitleNode.InnerText, Inputs.MatchingTolerance))
                    releaseIds.Add(releaseNode.Attributes["id"].Value);
                //TODO: Remove this hard-coding and make it configurable
                if (releaseIds.Count == 5)
                    break;
            }
            return releaseIds.ToArray();
        }

        private IEnumerable<Id3Tag> GetMatchingTitleTags(XmlNode xdoc, string title)
        {
            XmlNodeList titleNodes = xdoc.SelectNodes("resp/release/tracklist/track/title");
            if (titleNodes == null)
                yield break;
            foreach (XmlNode titleNode in titleNodes)
            {
                if (!Utility.TolerantEquals(title, titleNode.InnerText, Inputs.MatchingTolerance))
                    continue;
                var tag = new Id3Tag();
                tag.Title.Value = titleNode.InnerText;
                tag.Track.Value = int.Parse(GetXmlChildInnerText(titleNode.ParentNode, "position"));
                yield return tag;
            }
        }

        private static string GetXmlChildInnerText(XmlNode node, string childName)
        {
            XmlNode childNode = node[childName];
            return childNode != null ? childNode.InnerText : null;
        }

        private static void AddReleaseDetails(XmlNode xdoc, Id3Tag tag)
        {
            XmlNode rootNode = xdoc.SelectSingleNode("resp/release");

            XmlNode titleNode = rootNode["title"];
            if (titleNode != null)
                tag.Album.Value = titleNode.InnerText;

            XmlNodeList artistNodes = rootNode.SelectNodes("artists/artist");
            if (artistNodes != null)
            {
                foreach (XmlNode artistNode in artistNodes)
                    tag.Artists.Value.Add(artistNode.InnerText);
            }

            //TODO: Genre

            XmlNode publisherNode = rootNode.SelectSingleNode("labels/label");
            if (publisherNode != null && publisherNode.Attributes != null)
                tag.Publisher.Value = publisherNode.Attributes["name"].Value;

            XmlNode yearNode = rootNode["released"];
            if (yearNode != null)
                tag.Year.Value = int.Parse(yearNode.InnerText);
        }

        protected override InfoProviderProperties GetProperties()
        {
            var properties = new InfoProviderProperties("Discogs", "http://www.discogs.com", "http://www.discogs.com/users/api_key");
            properties.RequiredInputs.AddMultiple(typeof(TitleFrame), typeof(ArtistsFrame));
            properties.OptionalInputs.Add(typeof(AlbumFrame));
            properties.AvailableOutputs.AddMultiple(typeof(AlbumFrame), typeof(ArtistsFrame), typeof(ArtistUrlFrame), //typeof(BandFrame),
                //TODO: typeof(CountryFrame),
                typeof(GenreFrame), //TODO: typeof(InvolvedPeopleFrame),
                typeof(PictureFrame), typeof(PublisherFrame), typeof(TrackFrame), typeof(YearFrame));
            return properties;
        }

        private static XmlDocument CallService(string serviceFormat, params object[] args)
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");

                byte[] response = webClient.DownloadData(String.Format(serviceFormat, args));
                string responseXml;
                if (webClient.ResponseHeaders[HttpResponseHeader.ContentEncoding] == "gzip")
                {
                    byte[] uncompressedResponse = DecompressGZipData(response);
                    responseXml = Encoding.UTF8.GetString(uncompressedResponse);
                } else
                    responseXml = Encoding.UTF8.GetString(response);

                var xdoc = new XmlDocument();
                xdoc.LoadXml(responseXml);
                return xdoc;
            }
        }

        private static byte[] DecompressGZipData(byte[] compressedData)
        {
            const int BufferSize = 4096;

            using (var compressedStream = new MemoryStream(compressedData))
            using (var decompressor = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var decompressedStream = new MemoryStream())
            {
                long size = 0;
                var buffer = new byte[BufferSize];
                int readLength;
                do
                {
                    readLength = decompressor.Read(buffer, 0, BufferSize);
                    if (readLength > 0)
                    {
                        size += readLength;
                        decompressedStream.SetLength(size);
                        decompressedStream.Write(buffer, 0, readLength);
                    }
                } while (readLength == BufferSize);
                return decompressedStream.ToArray();
            }
        }

        private const string GetArtistFormat = @"http://www.discogs.com/artist/{0}?f=xml&api_key={1}";
        private const string GetReleaseFormat = @"http://www.discogs.com/release/{0}?f=xml&api_key={1}";
        private const string ApiKey = "96f9c4dca9";
    }
}