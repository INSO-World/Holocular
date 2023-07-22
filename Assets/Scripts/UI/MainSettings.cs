using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;
using UnityEditor;
using UnityEngine;

public class MainSettings : MonoBehaviour
{
    public GUISkin uiStyle;
    Rect settingsWindowRect = new Rect(10, 10, 200, Screen.height / 2);

    public GUIStyle switchBackground;
    public GUIStyle switchKnob;

    Switch showAuthorsColorsSwitch;
    Switch showAuthorsPaletteSwitch;
    WindowBar windowBar;

    public string commitDistanceMultiplicatorTmp;

    // Start is called before the first frame update
    void Start()
    {
        showAuthorsColorsSwitch = new Switch(new Rect(0, 0, 40, 20), switchBackground, switchKnob);
        showAuthorsPaletteSwitch = new Switch(new Rect(0, 40, 40, 20), switchBackground, switchKnob);
        windowBar = new WindowBar("Settings", uiStyle);

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
        if (GUI.Button(new Rect(10, 40, 180, 20), "Open Database"))
        {
            OpenFolder();
        }

        if (GUI.Button(new Rect(10, 65, 180, 20), "Delete Helix"))
        {
            Main.helix.DeleteHelix();
        }

        if (GUI.Button(new Rect(10, 90, 180, 20), "Stop Helix Generation"))
        {
            Main.helix.StopThreads();
        }

        GUI.BeginGroup(new Rect(10, 120, 200, 200));
        GUI.Label(new Rect(0, 0, 200, 20), "Commits: " + Statistics.commitsDrawn + "/" + (Main.commits == null ? "0" : Main.commits.commits.Length));
        GUI.Label(new Rect(0, 20, 200, 20), "Branches: " + (Main.branches == null ? "0" : Main.branches.branches.Length));
        GUI.Label(new Rect(0, 40, 200, 20), "Files: " + (Main.files == null ? "0" : Main.files.files.Length));
        GUI.Label(new Rect(0, 60, 200, 20), "Commits-Files: " + (Main.commits == null ? "0" : Main.commitsFiles.commitsFiles.Length));
        GUI.Label(new Rect(0, 80, 200, 20), "Stakeholders: " + (Main.stakeholders == null ? "0" : Main.stakeholders.stakeholders.Length));
        GUI.EndGroup();

        GUI.BeginGroup(new Rect(10, 240, 200, 200));
        GlobalSettings.showAuthorColors = showAuthorsColorsSwitch.render(GlobalSettings.showAuthorColors);
        GUI.Label(new Rect(50, 0, 160, 40), "Show Committer\nColors (c)");


        GUI.EndGroup();


        GUI.BeginGroup(new Rect(10, 290, 200, 200));
        GUI.Label(new Rect(0, 0, 200, 20), "Commit Distance Multiplicator");

        commitDistanceMultiplicatorTmp = GUI.TextField(new Rect(0, 20, 180, 20), commitDistanceMultiplicatorTmp);
        int num;
        if (int.TryParse(commitDistanceMultiplicatorTmp, out num))
        {
            GlobalSettings.commitDistanceMultiplicator = num;
        }

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
