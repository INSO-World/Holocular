using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helix : MonoBehaviour
{
    List<HelixCommit> commits = new List<HelixCommit>();


    public Helix()
    {
        for (int i = 0; i < Main.commits.commits.Length; i++)
        {
            commits.Add(new HelixCommit(i * 2, Main.commits.commits[i]));

        }
        RuntimeDebug.Log("Helix created successfull");
    }
}
