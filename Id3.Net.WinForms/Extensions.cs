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
using System.IO;

using Id3.Frames;

namespace Id3
{
    public static class Extensions
    {
        public static Image GetPicture(this PictureFrame frame)
        {
            if (frame.PictureData == null || frame.PictureData.Length == 0)
                return null;
            var ms = new MemoryStream(frame.PictureData);
            Image image = Image.FromStream(ms);
            return image;
        }

        public static void SetPicture(this PictureFrame frame, Image image)
        {
            if (image == null)
                frame.PictureData = null;
            else
            {
                var ms = new MemoryStream();
                image.Save(ms, image.RawFormat);
                frame.PictureData = new byte[ms.Length];
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(frame.PictureData, 0, frame.PictureData.Length);
            }
        }
    }
}