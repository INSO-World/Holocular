using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Commit : MonoBehaviour
{
    public Commit(Vector3 pos)
    {
        GameObject commit = new GameObject("Commit");
        commit.transform.position = pos;
        Instantiate(Main.sFile, commit.transform);
        RuntimeDebug.Log("Commit created: ");
    }
}
