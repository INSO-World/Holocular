using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;
using UnityEditor;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public GUISkin uiStyle;

    public GUIStyle settingsIcon;
    public GUIStyle authorListIcon;
    public GUIStyle debugViewIcon;

    WindowButton settingsButton;
    WindowButton authorListButton;
    WindowButton debugViewButton;
    int windowCount = 3;


    int padding = 10;
    int buttonSize = Screen.height / 20;

    // Start is called before the first frame update
    void Start()
    {
        settingsButton = new WindowButton(new Rect(padding, padding, buttonSize, buttonSize), settingsIcon, uiStyle.GetStyle("windowButtonActive"));
        authorListButton = new WindowButton(new Rect(2 * padding + buttonSize, padding, buttonSize, buttonSize), authorListIcon, uiStyle.GetStyle("windowButtonActive"));
        debugViewButton = new WindowButton(new Rect(3 * padding + 2 * buttonSize, padding, buttonSize, buttonSize), debugViewIcon, uiStyle.GetStyle("windowButtonActive"));
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void OnGUI()
    {
        GUI.skin = uiStyle;

        GUI.Label(new Rect(0, Screen.height - 2 * padding - buttonSize, windowCount * (padding + buttonSize) + padding, 2 * padding + buttonSize), "", uiStyle.GetStyle("windowDock"));
        GUI.BeginGroup(new Rect(0, Screen.height - 2 * padding - buttonSize, windowCount * (padding + buttonSize) + padding, 2 * padding + buttonSize));
        GlobalSettings.showSettings = settingsButton.render(GlobalSettings.showSettings);
        GlobalSettings.showAuthorPalette = authorListButton.render(GlobalSettings.showAuthorPalette);
        GlobalSettings.debugMode = debugViewButton.render(GlobalSettings.debugMode);
        GUI.EndGroup();
    }
}
