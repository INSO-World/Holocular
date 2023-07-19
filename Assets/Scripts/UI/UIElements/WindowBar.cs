using System;
using UnityEngine;
using UnityEngine.UI;

public class WindowBar
{

    GUISkin uiStyle;
    string name;

    public WindowBar(string name, GUISkin uiStyle)
    {
        this.uiStyle = uiStyle;
        this.name = name;
    }

    public void render()
    {
        GUI.Label(new Rect(0, 0, 200, 30), name, uiStyle.GetStyle("windowBar"));
        GUI.Label(new Rect(5, 5, 20, 20), "", uiStyle.GetStyle("windowMove"));
        GUI.DragWindow(new Rect(0, 0, 200, 30));

    }
}