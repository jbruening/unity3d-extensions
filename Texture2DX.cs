using UnityEngine;

namespace UEx
{
    public static class Texture2DX
    {
        /// <summary>
        /// sets a 1 pixel border of the texture on all mipmap levels to the clear color
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="clearColor"> </param>
        /// <param name="makeNoLongerReadable"> </param>
        public static void ClearMipMapBorders(this Texture2D texture, Color clearColor, bool makeNoLongerReadable = false)
        {
            var mipCount = texture.mipmapCount;


            // In general case, mip level size is mipWidth=max(1,width>>miplevel) and similarly for height. 


            var width = texture.width;
            var height = texture.height;
            // tint each mip level
            for (var mip = 1; mip < mipCount; ++mip)
            {
                var mipWidth = Mathf.Max(1, width >> mip);
                var mipHeight = Mathf.Max(1, height >> mip);
                if (mipWidth <= 2) continue; //don't change mip levels below 2x2
                var xCols = new Color[mipWidth];
                var yCols = new Color[mipHeight];
                if (clearColor != default(Color)) //speedup.
                {
                    for (var x = 0; x < xCols.Length; ++x)
                    {
                        xCols[x] = clearColor;
                    }
                    for (var y = 0; y < yCols.Length; ++y)
                    {
                        yCols[y] = clearColor;
                    }
                }
                texture.SetPixels(0, 0, mipWidth, 1, xCols, mip); //set the top edge colors
                texture.SetPixels(0, 0, 1, mipHeight, yCols, mip); //set the left edge colors
                texture.SetPixels(mipWidth - 1, 0, 1, mipWidth, xCols, mip); //set the bottom edge colors
                texture.SetPixels(0, mipWidth - 1, mipHeight, 1, yCols, mip); //set the right edge colors
            }

            // actually apply all SetPixels, don't recalculate mip levels
            texture.Apply(false, makeNoLongerReadable);
        }

        /// <summary>
        /// sets a 1 pixel border of the texture on all mipmap levels to clear white
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="makeNoLongerReadable"></param>
        public static void ClearMipMapBorders(this Texture2D texture, bool makeNoLongerReadable = false)
        {
            var clear = new Color(1, 1, 1, 0);
            ClearMipMapBorders(texture, clear, makeNoLongerReadable);
        }
    }
}
