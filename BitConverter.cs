using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UEx
{
    public class BitConverter
    {
        /// <summary>
        /// copy the bytes of the specified int into the buffer
        /// </summary>
        /// <param name="value"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static unsafe void CopyBytes(int value, byte[] buffer, int offset)
        {
            // Here should be a range check. For example:
            if (offset + sizeof(int) > buffer.Length) throw new IndexOutOfRangeException();

            fixed (byte* numPtr = &buffer[offset])
                *(int*) numPtr = value;
        }
        /// <summary>
        /// copy the bytes of the specified float into the buffer
        /// </summary>
        /// <param name="value"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        public static unsafe void CopyBytes(float value, byte[] buffer, int offset)
        {
            // Here should be a range check. For example:
            if (offset + sizeof(float) > buffer.Length) throw new IndexOutOfRangeException();

            fixed (byte* numPtr = &buffer[offset])
                *(float*)numPtr = value;
        }
    }
}
