using System;
using UnityEngine;
using UnityEngine.UI;

public class Window
{

    GUISkin uiStyle;
    string name;
    int width;
    int height;

    public Window(string name, GUISkin uiStyle, int width, int height)
    {
        this.uiStyle = uiStyle;
        this.name = name;
        this.width = width;
        this.height = height;
    }

    public void render()
    {
        GUI.Label(new Rect(0, 0, width, 30), "", uiStyle.GetStyle("windowBar"));
        GUI.Label(new Rect(0, 30, width, height - 30), "", uiStyle.GetStyle("windowBack"));
        GUI.DrawTexture(new Rect(30, 8, 150, 14), uiStyle.GetStyle("windowBarTextBack").normal.background);
        GUI.DrawTexture(new Rect(30, 22, 150, 2), uiStyle.GetStyle("windowBarLine").normal.background);
        GUI.Label(new Rect(30, 0, width - 30, 30), name, uiStyle.GetStyle("windowBarText"));
        GUI.Label(new Rect(5, 5, 20, 20), "", uiStyle.GetStyle("windowMove"));
        GUI.DragWindow(new Rect(0, 0, width, 30));

    }
}