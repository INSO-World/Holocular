using System;
using UnityEngine;

public class Utils
{
    public static float EaseOut(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * (value * value * value + 1) + start;
    }

    public static void GUIDrawSprite(Rect rect, Sprite sprite)
    {
        Rect spriteRect = sprite.rect;
        Texture2D tex = sprite.texture;
        GUI.DrawTextureWithTexCoords(rect, tex, new Rect(spriteRect.x / tex.width, spriteRect.y / tex.height, spriteRect.width / tex.width, spriteRect.height / tex.height));
    }

    public static int CalculateOwnedLines(DBOwnershipHunk[] ownershipHunks)
    {
        int ownedLines = 0;

        foreach (DBOwnershipHunk ownershipHunk in ownershipHunks)
        {
            foreach (string line in ownershipHunk.lines)
            {
                int from = int.Parse(line.Split(",")[0]);
                int to = int.Parse(line.Split(",")[1]);
                ownedLines += (to - from);
            }
        }
        return ownedLines;
    }

}