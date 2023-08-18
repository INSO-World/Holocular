using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;
using UnityEditor;
using UnityEngine;

public class MainSettings : MonoBehaviour
{
    public GUISkin uiStyle;
    Rect settingsWindowRect = new Rect(10, 10, 300, 800);

    public GUIStyle switchBackground;
    public GUIStyle switchKnob;

    Switch showAuthorsColorsSwitch;
    Switch showBranchColorsSwitch;
    Switch showOwnershipColorsSwitch;
    Switch commitPlacementModeSwitch;
    Switch showFolderRingsSwitch;
    WindowBar windowBar;

    public string commitDistanceMultiplicatorTmp;

    // Start is called before the first frame update
    void Start()
    {
        showAuthorsColorsSwitch = new Switch(new Rect(0, 30, 40, 20), switchBackground, switchKnob);
        showBranchColorsSwitch = new Switch(new Rect(0, 70, 40, 20), switchBackground, switchKnob);
        showOwnershipColorsSwitch = new Switch(new Rect(0, 110, 40, 20), switchBackground, switchKnob);
        commitPlacementModeSwitch = new Switch(new Rect(0, 70, 40, 20), switchBackground, switchKnob);
        showFolderRingsSwitch = new Switch(new Rect(0, 110, 40, 20), switchBackground, switchKnob);
        windowBar = new WindowBar("Settings", uiStyle, 300);

        commitDistanceMultiplicatorTmp = "" + GlobalSettings.commitDistanceMultiplicator;
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void OnGUI()
    {
        GUI.skin = uiStyle;
        if (GlobalSettings.showSettings)
        {
            settingsWindowRect = GUI.Window(0, settingsWindowRect, SettingsWindow, "");
        }
    }

    void SettingsWindow(int windowID)
    {
        windowBar.render();
        if (GUI.Button(new Rect(10, 40, 280, 20), "Open Database"))
        {
            OpenFolder();
        }

        if (GUI.Button(new Rect(10, 65, 280, 20), "Delete Helix"))
        {
            Main.helix.DeleteHelix();
        }

        if (GUI.Button(new Rect(10, 90, 280, 20), "Stop Helix Generation"))
        {
            Main.helix.StopThreads();
        }

        GUI.BeginGroup(new Rect(10, 120, 300, 200));
        GUI.Label(new Rect(0, 0, 300, 20), "Stats", uiStyle.GetStyle("headline"));
        GUI.Label(new Rect(0, 30, 300, 20), "Commits: " + Statistics.commitsDrawn + "/" + (Main.commits == null ? "0" : Main.commits.commits.Length));
        GUI.Label(new Rect(0, 50, 300, 20), "Branches: " + (Main.branches == null ? "0" : Main.branches.branches.Length));
        GUI.Label(new Rect(0, 70, 300, 20), "Files: " + (Main.files == null ? "0" : Main.files.files.Length));
        GUI.Label(new Rect(0, 90, 300, 20), "Commits-Files: " + (Main.commitsFiles == null ? "0" : Main.commitsFiles.commitsFiles.Length));
        GUI.Label(new Rect(0, 110, 300, 20), "Commits-Files-Stakeholders: " + (Main.commitsFilesStakeholders == null ? "0" : Main.commitsFilesStakeholders.commitsFilesStakeholders.Length));
        GUI.Label(new Rect(0, 130, 300, 20), "Stakeholders: " + (Main.stakeholders == null ? "0" : Main.stakeholders.stakeholders.Length));
        GUI.EndGroup();

        GUI.BeginGroup(new Rect(10, 300, 200, 200));
        GUI.Label(new Rect(0, 0, 200, 20), "Highlighting", uiStyle.GetStyle("headline"));
        GlobalSettings.showAuthorColors = showAuthorsColorsSwitch.render(GlobalSettings.showAuthorColors);
        GUI.Label(new Rect(50, 30, 120, 40), "Show Committer\nColors (c)");

        GlobalSettings.showBranchColors = showBranchColorsSwitch.render(GlobalSettings.showBranchColors);
        GUI.Label(new Rect(50, 70, 120, 40), "Show Branch\nColors (b)");

        GlobalSettings.showOwnershipColors = showOwnershipColorsSwitch.render(GlobalSettings.showOwnershipColors);
        GUI.Label(new Rect(50, 110, 120, 40), "Show Ownership\nColors (o)");

        GUI.Label(new Rect(0, 150, 200, 20), "Highlight Path:");
        GlobalSettings.folderSearch = TextField.render(new Rect(0, 170, 200, 20), GlobalSettings.folderSearch, uiStyle.GetStyle("textFieldText"), uiStyle.GetStyle("textFieldBack"), uiStyle.GetStyle("textFieldBorder"));

        GUI.EndGroup();


        GUI.BeginGroup(new Rect(10, 520, 200, 200));
        GUI.Label(new Rect(0, 0, 200, 20), "Visuals", uiStyle.GetStyle("headline"));

        GUI.Label(new Rect(0, 30, 200, 20), "Distance Factor:");
        GlobalSettings.commitDistanceMultiplicator = Slider.render(new Rect(0, 50, 180, 20), GlobalSettings.commitDistanceMultiplicator, 1f, 9f, uiStyle.GetStyle("sliderEmpty"), uiStyle.GetStyle("sliderFilled"), uiStyle.GetStyle("sliderKnob"));

        GlobalSettings.commitPlacementMode = commitPlacementModeSwitch.render(GlobalSettings.commitPlacementMode);
        GUI.Label(new Rect(50, 70, 160, 40), "Commit Distribution\nLinar/By Date");

        GlobalSettings.showFolderRings = showFolderRingsSwitch.render(GlobalSettings.showFolderRings);
        GUI.Label(new Rect(50, 110, 120, 40), "Show Folder\nRings (f)");

        GUI.Label(new Rect(0, 150, 200, 20), "File Size:");
        GlobalSettings.fileSize = Slider.render(new Rect(0, 170, 180, 20), GlobalSettings.fileSize, 0.1f, 15f, uiStyle.GetStyle("sliderEmpty"), uiStyle.GetStyle("sliderFilled"), uiStyle.GetStyle("sliderKnob"));


        GUI.EndGroup();
    }

    private void OpenFolder()
    {
        FileBrowser.ShowLoadDialog((paths) =>
        {
            RuntimeDebug.Log("Trying to Open: " + paths[0]);
            if (DatabaseLoader.checkFoolderIfValid(paths[0]))
            {
                RuntimeDebug.Log("Path valid");
                if (DatabaseLoader.importDatabase(paths[0]))
                {
                    RuntimeDebug.Log("Database imported Successfull");
                    Main.helix = new Helix(GameObject.Find("Helix"));
                    Main.helix.GenerateHelix();
                }
            }
            else
            {
                RuntimeDebug.Log("Path Invalid. Selected Folder not a Binocular DB");
            }
        }, () => { Debug.Log("Canceled"); }, FileBrowser.PickMode.Folders, false, null, null, "Select Folder", "Select");
    }

}
