using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class HelixStakeholder
{

    public DBStakeholder dBStakeholderStore;
    public Color colorStore;
    public Color invColorStore;

    public HelixStakeholder(DBStakeholder dBStakeholder, Color color)
    {
        dBStakeholderStore = dBStakeholder;
        colorStore = color;
        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v);
        h += 0.5f;
        if (h > 1)
        {
            h -= 1f;
        }
        invColorStore = Color.HSVToRGB(h, s, v);
    }
}

