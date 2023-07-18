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


    // Start is called before the first frame update
    void Start()
    {
        showAuthorsColorsSwitch = new Switch(new Rect(0, 0, 40, 20), switchBackground, switchKnob);
        showAuthorsPaletteSwitch = new Switch(new Rect(0, 40, 40, 20), switchBackground, switchKnob);

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnGUI()
    {
        GUI.skin = uiStyle;

        settingsWindowRect = GUI.Window(0, settingsWindowRect, SettingsWindow, "");


    }

    void SettingsWindow(int windowID)
    {
        GUI.Label(new Rect(0, 0, 200, 20), "Settings", uiStyle.GetStyle("windowBar"));
        /*if (GUI.Button(new Rect(10, 30, 150, 20), "New"))
        {
            Commit commit = new Commit(new Vector3(0, 0, Main.commitCount*2));
            Main.commitCount++;
        }*/

        if (GUI.Button(new Rect(10, 30, 150, 20), "Open Database"))
        {
            OpenFolder();
        }
        GUI.BeginGroup(new Rect(10, 50, 200, 200));
        GUI.Label(new Rect(0, 0, 200, 20), "Commits: " + Statistics.commitsDrawn + "/" + (Main.commits == null ? "0" : Main.commits.commits.Length));
        GUI.Label(new Rect(0, 20, 200, 20), "Branches: " + (Main.branches == null ? "0" : Main.branches.branches.Length));
        GUI.Label(new Rect(0, 40, 200, 20), "Files: " + (Main.files == null ? "0" : Main.files.files.Length));
        GUI.Label(new Rect(0, 60, 200, 20), "Commits-Files: " + (Main.commits == null ? "0" : Main.commitsFiles.commitsFiles.Length));
        GUI.Label(new Rect(0, 80, 200, 20), "Stakeholders: " + (Main.stakeholders == null ? "0" : Main.stakeholders.stakeholders.Length));
        GUI.EndGroup();

        GUI.BeginGroup(new Rect(10, 180, 200, 200));
        GlobalSettings.showAuthorColors = showAuthorsColorsSwitch.render(GlobalSettings.showAuthorColors);
        GUI.Label(new Rect(50, 0, 160, 40), "Show Committer\nColors (c)");
        GlobalSettings.showAuthorPalette = showAuthorsPaletteSwitch.render(GlobalSettings.showAuthorPalette);
        GUI.Label(new Rect(50, 40, 160, 40), "Show Committer\nPalette (p)");
        GUI.EndGroup();


        GUI.DragWindow(new Rect(0, 0, 200, 20));
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
