using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugView : MonoBehaviour
{
    public GUIStyle debugBackgroud;
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
            GUI.BeginGroup(new Rect(Screen.width - 400, 0, 400, Screen.height),debugBackgroud);
            List<string> logs = RuntimeDebug.getLogs();
            GUI.Label(new Rect(0, 0, 400, 20), "Move Speed (Mouse wheel): "+Main.moveSpeed+ "m/s");
            GUI.Label(new Rect(0, 20, 400, 20), "Move Speed (alt + Mouse wheel): " + Main.mouseSensitivity);

            GUI.Label(new Rect(0, Screen.height - 20 * 11, 400, 20), "Last Logs:");
            for (int i = 1; i <= Mathf.Min(logs.Count, 10); i++)
            {
                GUI.Label(new Rect(0, Screen.height - 20 * i, 400, 20), logs[logs.Count - i]);
            }
            GUI.EndGroup();
        }
        else
        {
            GUI.Label(new Rect(Screen.width - 200,0, 200, 20), "F3 for debug info.");
        }
    }
}
