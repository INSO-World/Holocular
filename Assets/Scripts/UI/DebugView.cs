using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugView : MonoBehaviour
{
    public GUIStyle debugBackgroud;
    public GUIStyle logOutputStyle;
    int logOutputLines = 10;

    public Vector2 logScrollPosition = Vector2.zero;


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
        if (Main.debugMode)
        {
            GUI.BeginGroup(new Rect(Screen.width - 400, 0, 400, Screen.height), debugBackgroud);
            GUI.Label(new Rect(0, 0, 400, 20), "Move Speed (Mouse wheel): " + Main.moveSpeed + "m/s");
            GUI.Label(new Rect(0, 20, 400, 20), "Mouse Sensitivity (alt + Mouse wheel): " + Main.mouseSensitivity);
            GUI.Label(new Rect(0, 60, 400, 20), "FPS: " + ((int)(1f / Time.unscaledDeltaTime)));

            GUI.Label(new Rect(0, 100, 400, 20), "Threads:");
            GUI.Label(new Rect(0, 120, 400, 20), "Create Structure Thread: " + Main.helix.createStructureThreadState);
            GUI.Label(new Rect(0, 140, 400, 20), "Draw Structure Thread: " + Main.helix.drawStructureThreadState);




            LogScrollView();

            GUI.EndGroup();
        }
        else
        {
            GUI.Label(new Rect(Screen.width - 200, 0, 200, 20), "F3 for debug info.");
        }
    }


    private void LogScrollView()
    {
        List<Log> logs = RuntimeDebug.getLogs();
        logScrollPosition = GUI.BeginScrollView(new Rect(0, Screen.height - 20 * logOutputLines, 400, 20 * logOutputLines), logScrollPosition, new Rect(0, 0, 380, 20 * logs.Count));

        for (int i = 0; i < logs.Count; i++)
        {
            GUI.Label(new Rect(0, 20 * (logs.Count - i), 600, 20), "[" + logs[i].time.ToLongTimeString() + "]: " + logs[i].message);
        }
        GUI.EndScrollView();
    }
}
