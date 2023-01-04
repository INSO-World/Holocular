using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HelixCommit : MonoBehaviour
{
    public HelixCommit(int id,DBCommit dbCommit)
    {
        GameObject commit = new GameObject("Commit");
        commit.transform.position = new Vector3(0, 0, id * 2);
        Instantiate(Main.sFile, commit.transform);
        RuntimeDebug.Log("Commit created: " + dbCommit.sha);
    }
}
