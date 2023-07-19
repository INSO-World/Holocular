using System;
using UnityEngine;
using UnityEngine.UI;

public class WindowButton
{
    Rect rect;
    GUIStyle icon;
    GUIStyle activeIndicator;

    public WindowButton(Rect rect, GUIStyle icon, GUIStyle activeIndicator)
    {
        this.rect = rect;
        this.icon = icon;
        this.activeIndicator = activeIndicator;
    }

    public bool render(bool state)
    {
        if (GUI.Button(rect, "", icon))
        {
            state = !state;
        }

        if (state)
        {
            GUI.Label(rect, "", activeIndicator);
        }

        return state;
    }

}

