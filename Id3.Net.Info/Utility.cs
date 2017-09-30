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

using System.Drawing;
using System.Net;

using Id3.Frames;

namespace Id3.Info
{
    public static partial class Utility
    {
        public static PictureFrame GetPicture(string url)
        {
            using (var webClient = new WebClient())
            {
                byte[] pictureData = webClient.DownloadData(url);

                var pictureFrame = new PictureFrame {
                    PictureData = pictureData,
                    MimeType = webClient.ResponseHeaders["content-type"],
                    PictureType = PictureType.Other
                };

                //using (Image image = pictureFrame.GetPicture())
                //{
                //    if (image.Height == 1 && image.Width == 1)
                //        return null;
                //}

                return pictureFrame;
            }
        }

    }
}