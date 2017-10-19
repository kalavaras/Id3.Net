using System;
using System.Collections.Generic;
using System.Text;

namespace Id3.Internal
{
    class ShiftJisDetector
    {
        public static bool Detect(byte[] data, int start, int count)
        {           
            if(count < 2)
            {
                return false;
            }

            for (var i = start; i < start + count - 1; i++)
            {
                byte first = data[i];
                byte second = data[i + 1];

                /* Check first byte is valid shift JIS. */
                if ((first >= 0x81 && first <= 0x84) ||
                    (first >= 0x87 && first <= 0x9f))
                {
                    if (second >= 0x40 && second <= 0x9e)
                    {
                        return true;
                    }
                    else if (second >= 0x9f && second <= 0xfc)
                    {
                        return true;
                    }
                }
                else if (first >= 0xe0 && first <= 0xef)
                {
                    if (second >= 0x40 && second <= 0x9e)
                    {
                        return true;
                    }
                    else if (second >= 0x9f && second <= 0xfc)
                    {
                        return true;
                    }
                }
            }            

            return false;
        }
    }
}
