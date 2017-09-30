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
using System.Drawing;
using System.IO;

using Id3.Frames;
using Id3.Info;

using WMPLib;

namespace Id3.Info.WindowsMedia
{
    public sealed class WindowsMediaInfoProvider : InfoProvider
    {
        protected override Id3Tag[] GetTagInfo(Id3Tag tag)
        {
            var windowsMediaPlayer = new WindowsMediaPlayerClass();
            try
            {
                IWMPPlaylist albumContents = windowsMediaPlayer.mediaCollection.getByAlbum(tag.Album.Value);
                IWMPMedia albumTrack = null;
                for (int contextIdx = 0; contextIdx < albumContents.count; contextIdx++)
                {
                    string artistContent = albumContents.get_Item(contextIdx).getItemInfo("Artist");
                    if (string.Compare(artistContent, string.Join("/", tag.Artists.Value), StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        albumTrack = albumContents.get_Item(contextIdx);
                        break;
                    }
                }

                if (albumTrack != null)
                {
                    string collectionId = albumTrack.getItemInfo("WM/WMCollectionID");
                    var largePicture = new PictureFrame();
                    largePicture.SetPicture(
                        Image.FromFile(string.Format("{0}\\AlbumArt_{1}_Large.jpg", Path.GetDirectoryName(albumTrack.sourceURL),
                            collectionId)));
                    var smallPicture = new PictureFrame();
                    smallPicture.SetPicture(
                        Image.FromFile(string.Format("{0}\\AlbumArt_{1}_Small.jpg", Path.GetDirectoryName(albumTrack.sourceURL),
                            collectionId)));

                    var result = new Id3Tag();
                    result.Pictures.Add(largePicture);
                    result.Pictures.Add(smallPicture);
                    return new[] { result };
                }

                return Id3Tag.Empty;
            }
            catch
            {
                return Id3Tag.Empty;
            }
            finally
            {
                windowsMediaPlayer.close();
            }
        }

        protected override InfoProviderProperties GetProperties()
        {
            var properties = new InfoProviderProperties("Windows Media Player", "http://www.microsoft.com/windows/windowsmedia/default.mspx",
                null);
            properties.AvailableOutputs.Add<PictureFrame>();
            properties.RequiredInputs.Add<AlbumFrame>();
            properties.RequiredInputs.Add<ArtistsFrame>();
            return properties;
        }
    }
}