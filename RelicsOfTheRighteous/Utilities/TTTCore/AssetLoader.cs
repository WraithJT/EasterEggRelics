﻿// WraithJT: Copied and modified from Vek17's TabletopTweaks-Core: https://github.com/Vek17/TabletopTweaks-Core
using System.IO;
using UnityEngine;
using UnityModManagerNet;

namespace RelicsOfTheRighteous.Utilities.TTTCore
{
    public class AssetLoader
    {
        public static Sprite LoadInternal(UnityModManager.ModEntry modContext, string folder, string file)
        {
            return Image2Sprite.Create($"{modContext.Path}Assets{Path.DirectorySeparatorChar}{folder}{Path.DirectorySeparatorChar}{file}");
        }
        public static Sprite LoadInternal(UnityModManager.ModEntry modContext, string folder, string file, int size)
        {
            return Image2Sprite.Create($"{modContext.Path}Assets{Path.DirectorySeparatorChar}{folder}{Path.DirectorySeparatorChar}{file}", size);
        }
        // Loosely based on https://forum.unity.com/threads/generating-sprites-dynamically-from-png-or-jpeg-files-in-c.343735/
        public static class Image2Sprite
        {
            public static string icons_folder = "";
            public static Sprite Create(string filePath, int size = 64)
            {
                var bytes = File.ReadAllBytes(icons_folder + filePath);
                var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
                _ = texture.LoadImage(bytes);
                var sprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0, 0));
                return sprite;
            }
        }
    }
}