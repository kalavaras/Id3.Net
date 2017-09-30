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
    public static class SerializationExtensions
    {
        /// <summary>
        ///   Registers all serialization surrogates defined in this assembly with the specified formatter.
        /// </summary>
        /// <typeparam name="TFormatter"> The type of formatter to register for. </typeparam>
        /// <param name="formatter"> The formatter to register for. </param>
        /// <returns> An instance of the formatter. </returns>
        public static TFormatter IncludeId3SerializationSupport<TFormatter>(this TFormatter formatter) where TFormatter : IFormatter
        {
            if (ReferenceEquals(formatter, null))
                throw new ArgumentNullException("formatter");

            var selector = new SurrogateSelector();

            //Id3Tag
            selector.AddSurrogate(typeof(Id3Tag), new StreamingContext(StreamingContextStates.All), new Id3TagSurrogate());

            //TextFrame-derived classes
            selector.AddSurrogate(typeof(AlbumFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());
            selector.AddSurrogate(typeof(BandFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());
            selector.AddSurrogate(typeof(ConductorFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());
            selector.AddSurrogate(typeof(ContentGroupDescriptionFrame), new StreamingContext(StreamingContextStates.All),
                new TextFrameSurrogate());
            selector.AddSurrogate(typeof(CopyrightFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());
            selector.AddSurrogate(typeof(CustomTextFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());
            selector.AddSurrogate(typeof(EncoderFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());
            selector.AddSurrogate(typeof(EncodingSettingsFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());
            selector.AddSurrogate(typeof(FileOwnerFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());
            selector.AddSurrogate(typeof(GenreFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());
            selector.AddSurrogate(typeof(PublisherFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());
            selector.AddSurrogate(typeof(SubtitleFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());
            selector.AddSurrogate(typeof(TitleFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());

            //NumericFrame-derived classes
            selector.AddSurrogate(typeof(BeatsPerMinuteFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());
            selector.AddSurrogate(typeof(YearFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());

            //DateTimeFrame-derived classes
            selector.AddSurrogate(typeof(RecordingDateFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());

            //ListTextFrame-derived classes
            selector.AddSurrogate(typeof(ArtistsFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());
            selector.AddSurrogate(typeof(ComposersFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());
            selector.AddSurrogate(typeof(LyricistsFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());

            //Other TextFrameBase<>-derived classes
            selector.AddSurrogate(typeof(FileTypeFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());
            selector.AddSurrogate(typeof(LengthFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());
            selector.AddSurrogate(typeof(TrackFrame), new StreamingContext(StreamingContextStates.All), new TextFrameSurrogate());

            //UrlLinkFrame-derived classes
            selector.AddSurrogate(typeof(ArtistUrlFrame), new StreamingContext(StreamingContextStates.All), new UrlLinkFrameSurrogate());
            selector.AddSurrogate(typeof(AudioFileUrlFrame), new StreamingContext(StreamingContextStates.All), new UrlLinkFrameSurrogate());
            selector.AddSurrogate(typeof(AudioSourceUrlFrame), new StreamingContext(StreamingContextStates.All), new UrlLinkFrameSurrogate());
            selector.AddSurrogate(typeof(CommercialUrlFrame), new StreamingContext(StreamingContextStates.All), new UrlLinkFrameSurrogate());
            selector.AddSurrogate(typeof(CopyrightUrlFrame), new StreamingContext(StreamingContextStates.All), new UrlLinkFrameSurrogate());
            selector.AddSurrogate(typeof(CustomUrlLinkFrame), new StreamingContext(StreamingContextStates.All), new UrlLinkFrameSurrogate());
            selector.AddSurrogate(typeof(PaymentUrlFrame), new StreamingContext(StreamingContextStates.All), new UrlLinkFrameSurrogate());

            //All other frames
            selector.AddSurrogate(typeof(CommentFrame), new StreamingContext(StreamingContextStates.All), new CommentFrameSurrogate());
            selector.AddSurrogate(typeof(LyricsFrame), new StreamingContext(StreamingContextStates.All), new LyricsFrameSurrogate());
            selector.AddSurrogate(typeof(PictureFrame), new StreamingContext(StreamingContextStates.All), new PictureFrameSurrogate());
            selector.AddSurrogate(typeof(PrivateFrame), new StreamingContext(StreamingContextStates.All), new PrivateFrameSurrogate());
            selector.AddSurrogate(typeof(UnknownFrame), new StreamingContext(StreamingContextStates.All), new UnknownFrameSurrogate());

            formatter.SurrogateSelector = selector;
            return formatter;
        }
    }
}