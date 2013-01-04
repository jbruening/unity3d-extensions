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
        public static void ClearMipMapBorders(this Texture2D texture, Color clearColor)
        {
            var mipCount = texture.mipmapCount;


            // In general case, mip level size is mipWidth=max(1,width>>miplevel) and similarly for height. 


            var width = texture.width;
            // tint each mip level
            for (var mip = 1; mip < mipCount; ++mip)
            {
                var mipWidth = Mathf.Max(1, width >> mip);
                if (mipWidth <= 2) continue; //don't change mip levels below 2x2
                var cols = new Color[mipWidth]; //instantiates a new color array. default color is completely clear.
                for (var i = 0; i < cols.Length; ++i)
                {
                    cols[i] = clearColor;
                }
                texture.SetPixels(0, 0, mipWidth, 1, cols, mip); //set the top edge colors
                texture.SetPixels(0, 0, 1, mipWidth, cols, mip); //set the left edge colors
                texture.SetPixels(mipWidth - 1, 0, 1, mipWidth, cols, mip); //set the bottom edge colors
                texture.SetPixels(0, mipWidth - 1, mipWidth, 1, cols, mip); //set the right edge colors
            }

            // actually apply all SetPixels, don't recalculate mip levels
            texture.Apply(false);
        }

        /// <summary>
        /// sets a 1 pixel border of the texture on all mipmap levels to clear white
        /// </summary>
        /// <param name="texture"></param>
        public static void ClearMipMapBorders(this Texture2D texture)
        {
            var clear = new Color(1, 1, 1, 0);
            ClearMipMapBorders(texture, clear);
        }
    }
}
