using System.Collections;
using System.Collections.Generic;
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
        GUI.Label(new Rect(0, 0, 200, 20),"Settings", WindowBar);
        if (GUI.Button(new Rect(10, 30, 150, 20), "New"))
        {
            Commit commit = new Commit(new Vector3(0, 0, Main.commitCount*2));
            Main.commitCount++;
        }

        GUI.Label(new Rect(10, 50, 200, 20), "Commits: " + Main.commitCount);

        GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }
}
