using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;
using UnityEditor;
using UnityEngine;

public class MainSettings : MonoBehaviour
{
    public GUISkin uiStyle;
    public Rect settingsWindowRect = new Rect(10, 10, 200, Screen.height / 2);
    public GUIStyle WindowBar;


    // Start is called before the first frame update
    void Start()
    {

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
        GUI.Label(new Rect(0, 0, 200, 20), "Settings", WindowBar);
        /*if (GUI.Button(new Rect(10, 30, 150, 20), "New"))
        {
            Commit commit = new Commit(new Vector3(0, 0, Main.commitCount*2));
            Main.commitCount++;
        }*/

        if (GUI.Button(new Rect(10, 30, 150, 20), "Open Database"))
        {
            OpenFolder();
        }

        GUI.Label(new Rect(10, 50, 200, 20), "Commits: " + Statistics.commitsDrawn + "/" + (Main.commits == null ? "0" : Main.commits.commits.Length));
        GUI.Label(new Rect(10, 70, 200, 20), "Branches: " + (Main.branches == null ? "0" : Main.branches.branches.Length));
        GUI.Label(new Rect(10, 90, 200, 20), "Files: " + (Main.files == null ? "0" : Main.files.files.Length));
        GUI.Label(new Rect(10, 110, 200, 20), "Commits-Files: " + (Main.commits == null ? "0" : Main.commitsFiles.commitsFiles.Length));


        GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
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
