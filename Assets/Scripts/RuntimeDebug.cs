using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeDebug : MonoBehaviour
{
    private static GameObject debugLine;
    private static List<string> logs = new List<string>();

    public static void Log(string message)
    {
        Debug.Log(message);
        logs.Add(message);
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        if(debugLine == null)
        {
            debugLine = new GameObject("DebugLine");
        }
        debugLine.transform.position = start;
        if (debugLine.GetComponent<LineRenderer>() == null)
        {
            debugLine.AddComponent<LineRenderer>();
        }
        LineRenderer lr = debugLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(debugLine, duration);
    }

    public static List<string> getLogs()
    {
        return logs;
    }
}
