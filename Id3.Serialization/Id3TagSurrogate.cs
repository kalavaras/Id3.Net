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
using System.Runtime.Serialization;

using Id3.Frames;

namespace Id3.Serialization
{
    internal sealed class Id3TagSurrogate : ISerializationSurrogate
    {
        void ISerializationSurrogate.GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var tag = (Id3Tag)obj;

            info.AddValue("MajorVersion", tag.MajorVersion);
            info.AddValue("MinorVersion", tag.MinorVersion);

            int frameCount = tag.Frames.Count;
            info.AddValue("FrameCount", frameCount);
            for (int i = 0; i < frameCount; i++)
            {
                Id3Frame frame = tag.Frames[i];
                info.AddValue("Frame" + i + "Type", frame.GetType().AssemblyQualifiedName);
                info.AddValue("Frame" + i, frame);
            }
        }

        object ISerializationSurrogate.SetObjectData(object obj, SerializationInfo info, StreamingContext context,
            ISurrogateSelector selector)
        {
            var tag = (Id3Tag)obj;

            tag.MajorVersion = info.GetInt32("MajorVersion");
            tag.MinorVersion = info.GetInt32("MinorVersion");

            int frameCount = info.GetInt32("FrameCount");
            for (int i = 0; i < frameCount; i++)
            {
                string frameTypeName = info.GetString("Frame" + i + "Type");
                Type frameType = Type.GetType(frameTypeName, true, false);
                var frame = info.GetValue("Frame" + i, frameType) as Id3Frame;
                if (frame != null)
                    tag.Frames.Add(frame);
            }

            return tag;
        }
    }
}