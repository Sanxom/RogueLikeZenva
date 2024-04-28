using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTextureGenerator
{
    public static Texture2D Generate(bool [,] map, Vector2 playerRoom)
    {
        Texture2D tex = new Texture2D(map.GetLength(0), map.GetLength(1));
        tex.filterMode = FilterMode.Point;

        Color[] pixels = new Color[map.GetLength(0) * map.GetLength(1)];

        for (int i = 0; i < pixels.Length; ++i)
        {
            int x = i % map.GetLength(0);
            int y = Mathf.FloorToInt(i / map.GetLength(1));

            if(playerRoom == new Vector2(x, y))
            {
                pixels[i] = Color.green;
            }
            else
            {
                pixels[i] = map[x, y] == true ? Color.white : Color.clear;
            }
        }

        tex.SetPixels(pixels);
        tex.Apply();

        return tex;
    }
}
