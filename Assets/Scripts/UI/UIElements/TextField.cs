using System;
using UnityEngine;
using UnityEngine.UI;

public class TextField
{
    public static string render(Rect rect, string state, GUIStyle textFieldText, GUIStyle textFieldBack, GUIStyle textFieldBorder)
    {
        GUI.Label(rect, "", textFieldBorder);
        GUI.Label(new Rect(rect.x + 1, rect.y + 1, rect.width - 2, rect.height - 2), "", textFieldBack);
        return GUI.TextField(new Rect(rect.x + 2, rect.y + 2, rect.width - 4, rect.height - 4), state, textFieldText);
    }

}

