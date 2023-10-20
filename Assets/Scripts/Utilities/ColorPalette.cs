using System;
using UnityEngine;

public class ColorPalette
{
    public static Color[] Generate(int amount)
    {
        Color[] palette = new Color[amount];

        for (int i = 0; i < amount; i++)
        {
            palette[i] = Color.HSVToRGB(1f / amount * i, 1f, 1f);
        }

        return palette;
    }
}

