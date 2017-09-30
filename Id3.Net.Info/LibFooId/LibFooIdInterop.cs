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
using System.Runtime.InteropServices;

namespace Id3.Info.Foosic
{
    internal static class LibFooIdInterop
    {
        [DllImport(Library, EntryPoint = "fp_init")]
        internal static extern IntPtr Init(int sampleRate, int channels);

        [DllImport(Library, EntryPoint = "fp_free")]
        internal static extern void Free(IntPtr fid);

        [DllImport(Library, EntryPoint = "fp_feed_short")]
        internal static extern int FeedShort(IntPtr fid, ref short data, int size);

        [DllImport(Library, EntryPoint = "fp_feed_float")]
        internal static extern int FeedFloat(IntPtr fid, ref float data, int size);

        [DllImport(Library, EntryPoint = "fp_getsize")]
        internal static extern int GetSize(IntPtr fid);

        [DllImport(Library, EntryPoint = "fp_getversion")]
        internal static extern int GetVersion(IntPtr fid);

        [DllImport(Library, EntryPoint = "fp_calculate")]
        internal static extern int Calculate(IntPtr fid, int songlen, byte[] buff);

        private const string Library = "FooID.dll";
    }
}