using System;
using UnityEngine;
using UnityEngine.UI;

public class WindowBar
{

    GUISkin uiStyle;
    string name;
    int width;

    public WindowBar(string name, GUISkin uiStyle, int width)
    {
        this.uiStyle = uiStyle;
        this.name = name;
        this.width = width;
    }

    public void render()
    {
        GUI.Label(new Rect(0, 0, width, 30), name, uiStyle.GetStyle("windowBar"));
        GUI.Label(new Rect(5, 5, 20, 20), "", uiStyle.GetStyle("windowMove"));
        GUI.DragWindow(new Rect(0, 0, width, 30));

    }
}