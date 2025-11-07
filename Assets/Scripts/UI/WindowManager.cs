using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class WindowManager : MonoBehaviour
{
    private UnityAction updateUiStyleListener;

    
    public GUIStyle parametersIcon;
    public GUIStyle settingsIcon;
    public GUIStyle authorListIcon;
    public GUIStyle debugViewIcon;
    public GUIStyle fileInfoIcon;
    public GUIStyle fileCompareIcon;
    public GUIStyle branchListIcon;

    WindowButton parametersButton;
    WindowButton settingsButton;
    WindowButton authorListButton;
    WindowButton debugViewButton;
    WindowButton fileInfoButton;
    WindowButton fileCompareButton;
    WindowButton branchListButton;
    int windowCount = 7;


    int padding = 5;
    int buttonSize = Screen.height / 20;

    // Start is called before the first frame update
    void Start()
    {
        parametersButton = new WindowButton(new Rect(padding, padding, buttonSize, buttonSize), parametersIcon, UiSkinManger.sUiStyle.GetStyle("windowButtonActive"));
        authorListButton = new WindowButton(new Rect(2 * padding + buttonSize, padding, buttonSize, buttonSize), authorListIcon, UiSkinManger.sUiStyle.GetStyle("windowButtonActive"));
        branchListButton = new WindowButton(new Rect(3 * padding + 2 * buttonSize, padding, buttonSize, buttonSize), branchListIcon, UiSkinManger.sUiStyle.GetStyle("windowButtonActive"));
        fileInfoButton = new WindowButton(new Rect(4 * padding + 3 * buttonSize, padding, buttonSize, buttonSize), fileInfoIcon, UiSkinManger.sUiStyle.GetStyle("windowButtonActive"));
        fileCompareButton = new WindowButton(new Rect(5 * padding + 4 * buttonSize, padding, buttonSize, buttonSize), fileCompareIcon, UiSkinManger.sUiStyle.GetStyle("windowButtonActive"));
        settingsButton = new WindowButton(new Rect(6 * padding + 5 * buttonSize, padding, buttonSize, buttonSize), settingsIcon, UiSkinManger.sUiStyle.GetStyle("windowButtonActive"));
        debugViewButton = new WindowButton(new Rect(7 * padding + 6 * buttonSize, padding, buttonSize, buttonSize), debugViewIcon, UiSkinManger.sUiStyle.GetStyle("windowButtonActive"));
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void OnGUI()
    {
        GUI.skin = UiSkinManger.sUiStyle;

        GUI.Label(new Rect(0, Screen.height - 2 * padding - buttonSize, windowCount * (padding + buttonSize) + padding, 2 * padding + buttonSize), "", UiSkinManger.sUiStyle.GetStyle("windowDock"));
        GUI.BeginGroup(new Rect(0, Screen.height - 2 * padding - buttonSize, windowCount * (padding + buttonSize) + padding, 2 * padding + buttonSize));
        GlobalSettings.showParameters = parametersButton.render(GlobalSettings.showParameters);
        GlobalSettings.showAuthorPalette = authorListButton.render(GlobalSettings.showAuthorPalette);
        GlobalSettings.showBranchPalette = branchListButton.render(GlobalSettings.showBranchPalette);
        GlobalSettings.showFileInfo = fileInfoButton.render(GlobalSettings.showFileInfo);
        GlobalSettings.showFileCompare = fileCompareButton.render(GlobalSettings.showFileCompare);
        GlobalSettings.showSettings = settingsButton.render(GlobalSettings.showSettings);
        GlobalSettings.debugMode = debugViewButton.render(GlobalSettings.debugMode);
        GUI.EndGroup();
    }
}
