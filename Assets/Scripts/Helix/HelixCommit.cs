using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HelixCommit : MonoBehaviour
{
    public HelixCommit(int id,DBCommit dbCommit,HelixBranch branch)
    {
        GameObject commit = new GameObject("Commit"+id);
        commit.transform.position = new Vector3(branch.position.x, branch.position.y, id * 2);
        Instantiate(Main.sFile, commit.transform);
        RuntimeDebug.Log("Commit created: " + dbCommit.sha);
    }
}
