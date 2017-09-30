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
using System.ComponentModel;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace Id3.Info.ChartLyrics
{
    [WebServiceBinding(Name = "apiv1Soap", Namespace = "http://api.chartlyrics.com/")]
    public sealed class LyricsService : SoapHttpClientProtocol
    {
        private SendOrPostCallback SearchLyricOperationCompleted;

        private SendOrPostCallback SearchLyricTextOperationCompleted;

        private SendOrPostCallback GetLyricOperationCompleted;

        private SendOrPostCallback AddLyricOperationCompleted;

        private SendOrPostCallback SearchLyricDirectOperationCompleted;

        private bool _useDefaultCredentialsSetExplicitly;

        public LyricsService()
        {
            Url = "http://api.chartlyrics.com/apiv1.asmx";
            if (IsLocalFileSystemWebService(Url))
            {
                UseDefaultCredentials = true;
                _useDefaultCredentialsSetExplicitly = false;
            } else
                _useDefaultCredentialsSetExplicitly = true;
        }

        public new string Url
        {
            get { return base.Url; }
            set
            {
                if (((IsLocalFileSystemWebService(base.Url) && (_useDefaultCredentialsSetExplicitly == false)) &&
                    (IsLocalFileSystemWebService(value) == false)))
                    base.UseDefaultCredentials = false;
                base.Url = value;
            }
        }

        public new bool UseDefaultCredentials
        {
            get { return base.UseDefaultCredentials; }
            set
            {
                base.UseDefaultCredentials = value;
                _useDefaultCredentialsSetExplicitly = true;
            }
        }

        public event SearchLyricCompletedEventHandler SearchLyricCompleted;

        public event SearchLyricTextCompletedEventHandler SearchLyricTextCompleted;

        public event GetLyricCompletedEventHandler GetLyricCompleted;

        public event AddLyricCompletedEventHandler AddLyricCompleted;

        public event SearchLyricDirectCompletedEventHandler SearchLyricDirectCompleted;

        [SoapDocumentMethod("http://api.chartlyrics.com/SearchLyric", RequestNamespace = "http://api.chartlyrics.com/",
            ResponseNamespace = "http://api.chartlyrics.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public SearchLyricResult[] SearchLyric(string artist, string song)
        {
            object[] results = Invoke("SearchLyric", new object[] { artist, song });
            return ((SearchLyricResult[])(results[0]));
        }

        public void SearchLyricAsync(string artist, string song)
        {
            SearchLyricAsync(artist, song, null);
        }

        public void SearchLyricAsync(string artist, string song, object userState)
        {
            if ((SearchLyricOperationCompleted == null))
                SearchLyricOperationCompleted = OnSearchLyricOperationCompleted;
            InvokeAsync("SearchLyric", new object[] { artist, song }, SearchLyricOperationCompleted, userState);
        }

        private void OnSearchLyricOperationCompleted(object arg)
        {
            if ((SearchLyricCompleted == null))
                return;

            var invokeArgs = ((InvokeCompletedEventArgs)(arg));
            SearchLyricCompleted(this,
                new SearchLyricCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }

        [SoapDocumentMethod("http://api.chartlyrics.com/SearchLyricText", RequestNamespace = "http://api.chartlyrics.com/",
            ResponseNamespace = "http://api.chartlyrics.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public SearchLyricResult[] SearchLyricText(string lyricText)
        {
            object[] results = Invoke("SearchLyricText", new object[] { lyricText });
            return ((SearchLyricResult[])(results[0]));
        }

        public void SearchLyricTextAsync(string lyricText)
        {
            SearchLyricTextAsync(lyricText, null);
        }

        public void SearchLyricTextAsync(string lyricText, object userState)
        {
            if ((SearchLyricTextOperationCompleted == null))
                SearchLyricTextOperationCompleted = OnSearchLyricTextOperationCompleted;
            InvokeAsync("SearchLyricText", new object[] { lyricText }, SearchLyricTextOperationCompleted, userState);
        }

        private void OnSearchLyricTextOperationCompleted(object arg)
        {
            if ((SearchLyricTextCompleted == null))
                return;

            var invokeArgs = ((InvokeCompletedEventArgs)(arg));
            SearchLyricTextCompleted(this,
                new SearchLyricTextCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }

        [SoapDocumentMethod("http://api.chartlyrics.com/GetLyric", RequestNamespace = "http://api.chartlyrics.com/",
            ResponseNamespace = "http://api.chartlyrics.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public GetLyricResult GetLyric(int lyricId, string lyricCheckSum)
        {
            object[] results = Invoke("GetLyric", new object[] { lyricId, lyricCheckSum });
            return ((GetLyricResult)(results[0]));
        }

        public void GetLyricAsync(int lyricId, string lyricCheckSum)
        {
            GetLyricAsync(lyricId, lyricCheckSum, null);
        }

        public void GetLyricAsync(int lyricId, string lyricCheckSum, object userState)
        {
            if ((GetLyricOperationCompleted == null))
                GetLyricOperationCompleted = OnGetLyricOperationCompleted;
            InvokeAsync("GetLyric", new object[] { lyricId, lyricCheckSum }, GetLyricOperationCompleted, userState);
        }

        private void OnGetLyricOperationCompleted(object arg)
        {
            if ((GetLyricCompleted == null))
                return;

            var invokeArgs = ((InvokeCompletedEventArgs)(arg));
            GetLyricCompleted(this,
                new GetLyricCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }

        [SoapDocumentMethod("http://api.chartlyrics.com/AddLyric", RequestNamespace = "http://api.chartlyrics.com/",
            ResponseNamespace = "http://api.chartlyrics.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public string AddLyric(int trackId, string trackCheckSum, string lyric, string email)
        {
            object[] results = Invoke("AddLyric", new object[] { trackId, trackCheckSum, lyric, email });
            return ((string)(results[0]));
        }

        public void AddLyricAsync(int trackId, string trackCheckSum, string lyric, string email)
        {
            AddLyricAsync(trackId, trackCheckSum, lyric, email, null);
        }

        public void AddLyricAsync(int trackId, string trackCheckSum, string lyric, string email, object userState)
        {
            if ((AddLyricOperationCompleted == null))
                AddLyricOperationCompleted = OnAddLyricOperationCompleted;
            InvokeAsync("AddLyric", new object[] { trackId, trackCheckSum, lyric, email }, AddLyricOperationCompleted, userState);
        }

        private void OnAddLyricOperationCompleted(object arg)
        {
            if ((AddLyricCompleted == null))
                return;

            var invokeArgs = ((InvokeCompletedEventArgs)(arg));
            AddLyricCompleted(this,
                new AddLyricCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }

        [SoapDocumentMethod("http://api.chartlyrics.com/SearchLyricDirect", RequestNamespace = "http://api.chartlyrics.com/",
            ResponseNamespace = "http://api.chartlyrics.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public GetLyricResult SearchLyricDirect(string artist, string song)
        {
            object[] results = Invoke("SearchLyricDirect", new object[] { artist, song });
            return ((GetLyricResult)(results[0]));
        }

        public void SearchLyricDirectAsync(string artist, string song)
        {
            SearchLyricDirectAsync(artist, song, null);
        }

        public void SearchLyricDirectAsync(string artist, string song, object userState)
        {
            if ((SearchLyricDirectOperationCompleted == null))
                SearchLyricDirectOperationCompleted = OnSearchLyricDirectOperationCompleted;
            InvokeAsync("SearchLyricDirect", new object[] { artist, song }, SearchLyricDirectOperationCompleted, userState);
        }

        private void OnSearchLyricDirectOperationCompleted(object arg)
        {
            if ((SearchLyricDirectCompleted == null))
                return;

            var invokeArgs = ((InvokeCompletedEventArgs)(arg));
            SearchLyricDirectCompleted(this,
                new SearchLyricDirectCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }

        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }

        private static bool IsLocalFileSystemWebService(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;
            var wsUri = new Uri(url);
            if (((wsUri.Port >= 1024) && (string.Compare(wsUri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0)))
                return true;
            return false;
        }
    }

    [Serializable]
    [XmlType(Namespace = "http://api.chartlyrics.com/")]
    public class SearchLyricResult
    {
        public string TrackChecksum { get; set; }
        public int TrackId { get; set; }
        public string LyricChecksum { get; set; }
        public int LyricId { get; set; }
        public string SongUrl { get; set; }
        public string ArtistUrl { get; set; }
        public string Artist { get; set; }
        public string Song { get; set; }
        public int SongRank { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://api.chartlyrics.com/")]
    public class GetLyricResult
    {
        public string TrackChecksum { get; set; }
        public int TrackId { get; set; }
        public string LyricChecksum { get; set; }
        public int LyricId { get; set; }
        public string LyricSong { get; set; }
        public string LyricArtist { get; set; }
        public string LyricUrl { get; set; }
        public string LyricCovertArtUrl { get; set; }
        public int LyricRank { get; set; }
        public string LyricCorrectUrl { get; set; }
        public string Lyric { get; set; }
    }

    public delegate void SearchLyricCompletedEventHandler(object sender, SearchLyricCompletedEventArgs e);

    public class SearchLyricCompletedEventArgs : AsyncCompletedEventArgs
    {
        private readonly object[] _results;

        internal SearchLyricCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
            : base(exception, cancelled, userState)
        {
            _results = results;
        }

        public SearchLyricResult[] Result
        {
            get
            {
                RaiseExceptionIfNecessary();
                return ((SearchLyricResult[])(_results[0]));
            }
        }
    }

    public delegate void SearchLyricTextCompletedEventHandler(object sender, SearchLyricTextCompletedEventArgs e);

    public class SearchLyricTextCompletedEventArgs : AsyncCompletedEventArgs
    {
        private readonly object[] _results;

        internal SearchLyricTextCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
            : base(exception, cancelled, userState)
        {
            _results = results;
        }

        public SearchLyricResult[] Result
        {
            get
            {
                RaiseExceptionIfNecessary();
                return ((SearchLyricResult[])(_results[0]));
            }
        }
    }

    public delegate void GetLyricCompletedEventHandler(object sender, GetLyricCompletedEventArgs e);

    public class GetLyricCompletedEventArgs : AsyncCompletedEventArgs
    {
        private readonly object[] _results;

        internal GetLyricCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
            : base(exception, cancelled, userState)
        {
            _results = results;
        }

        public GetLyricResult Result
        {
            get
            {
                RaiseExceptionIfNecessary();
                return ((GetLyricResult)(_results[0]));
            }
        }
    }

    public delegate void AddLyricCompletedEventHandler(object sender, AddLyricCompletedEventArgs e);

    public class AddLyricCompletedEventArgs : AsyncCompletedEventArgs
    {
        private readonly object[] _results;

        internal AddLyricCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
            : base(exception, cancelled, userState)
        {
            _results = results;
        }

        public string Result
        {
            get
            {
                RaiseExceptionIfNecessary();
                return ((string)(_results[0]));
            }
        }
    }

    public delegate void SearchLyricDirectCompletedEventHandler(object sender, SearchLyricDirectCompletedEventArgs e);

    public class SearchLyricDirectCompletedEventArgs : AsyncCompletedEventArgs
    {
        private readonly object[] _results;

        internal SearchLyricDirectCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
            : base(exception, cancelled, userState)
        {
            _results = results;
        }

        public GetLyricResult Result
        {
            get
            {
                RaiseExceptionIfNecessary();
                return ((GetLyricResult)(_results[0]));
            }
        }
    }
}
