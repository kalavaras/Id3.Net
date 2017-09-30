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

using System.Collections.Generic;
using System.Web.Services.Protocols;

using Id3.Frames;
using Id3.Info;

namespace Id3.Info.Amazon
{
    public sealed class AmazonInfoProvider : InfoProvider
    {
        protected override Id3Tag[] GetTagInfo(Id3Tag tag)
        {
            var request = new ItemSearchRequest {
                SearchIndex = "Music",
                Keywords = tag.Artists.Value + " " + tag.Title.Value,
                ResponseGroup = new[] { "Small", "Images" }
            };

            var itemSearch = new ItemSearch {
                AWSAccessKeyId = "1YY0CZVHA1BX3BEV5H82",
                AssociateTag = "knanamusin-20",
                Request = new[] { request }
            };

            ItemSearchResponse response;
            var service = new AWSECommerceService();
            try
            {
                response = service.ItemSearch(itemSearch);
            }
            catch (SoapException)
            {
                return Id3Tag.Empty;
            }
            finally
            {
                service.Dispose();
            }

            if (response == null || response.Items == null || response.Items.Length == 0)
                return Id3Tag.Empty;
            Item[] items = response.Items[0].Item;
            if (items == null || items.Length == 0)
                return Id3Tag.Empty;

            var results = new List<Id3Tag>(items.Length);
            foreach (Item item in items)
            {
                var result = new Id3Tag();
                result.Album.Value = item.ItemAttributes.Title;
                IList<string> artists;
                if (item.ItemAttributes.Artist == null || item.ItemAttributes.Artist.Length == 0)
                    artists = tag.Artists.Value;
                else
                    artists = item.ItemAttributes.Artist;
                foreach (string artist in artists)
                    result.Artists.Value.Add(artist);
                result.Publisher.Value = item.ItemAttributes.Manufacturer;
                result.PaymentUrl.Url = item.DetailPageURL;

                if (item.LargeImage != null)
                {
                    PictureFrame picture = Utility.GetPicture(item.LargeImage.URL);
                    if (picture != null)
                        result.Pictures.Add(picture);
                }

                if (item.MediumImage != null)
                {
                    PictureFrame picture = Utility.GetPicture(item.MediumImage.URL);
                    if (picture != null)
                        result.Pictures.Add(picture);
                }

                if (item.SmallImage != null)
                {
                    PictureFrame picture = Utility.GetPicture(item.SmallImage.URL);
                    if (picture != null)
                        result.Pictures.Add(picture);
                }

                results.Add(result);
            }

            return results.ToArray();
        }

        protected override InfoProviderProperties GetProperties()
        {
            var availableOutputs = new[] {
                typeof(AlbumFrame), typeof(ArtistsFrame), typeof(PaymentUrlFrame), typeof(PictureFrame),
                typeof(PublisherFrame)
            };
            var requiredInputs = new[] { typeof(TitleFrame), typeof(ArtistsFrame) };

            var properties = new InfoProviderProperties("Amazon", "http://www.amazon.com/music", null);
            properties.AvailableOutputs.AddMultiple(availableOutputs);
            properties.RequiredInputs.AddMultiple(requiredInputs);
            return properties;
        }
    }
}