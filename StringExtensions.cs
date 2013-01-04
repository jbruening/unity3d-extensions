using System.Text;

namespace UEx
{
    public static class StringExtensions
    {
        /// <summary>
        /// Insert the specified character into the string every n characters
        /// </summary>
        /// <param name="input"></param>
        /// <param name="insertCharacter"></param>
        /// <param name="n"></param>
        /// <param name="charsInserted"></param>
        /// <returns></returns>
        public static string InsertCharEveryNChars(this string input, char insertCharacter, int n, out int charsInserted)
        {
            charsInserted = 0;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (i % n == 0)
                {
                    sb.Append(insertCharacter);
                    ++charsInserted;
                }
                if (input[i] == insertCharacter)
                    ++charsInserted;
                sb.Append(input[i]);
            }
            return sb.ToString();
        }
    }
}
