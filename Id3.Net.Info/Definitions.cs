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

using Id3.Info;
using Id3.Info.Amazon;
using Id3.Info.Discogs;
using Id3.Info.Filename;
using Id3.Info.LyricWiki;
using Id3.Info.WindowsMedia;

[assembly: InfoProvider(typeof(AmazonInfoProvider))]
[assembly: InfoProvider(typeof(DiscogsInfoProvider))]
[assembly: InfoProvider(typeof(FilenameInfoProvider))]
[assembly: InfoProvider(typeof(LyricWikiInfoProvider))]
[assembly: InfoProvider(typeof(WindowsMediaInfoProvider))]
