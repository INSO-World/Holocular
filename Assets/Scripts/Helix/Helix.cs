using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helix : MonoBehaviour
{
    Dictionary<String,HelixCommit> commits = new Dictionary<string, HelixCommit>();

    Dictionary<String, HelixBranch> branches = new Dictionary<string, HelixBranch>();


    public Helix()
    {
        for (int i = 0; i < Main.branches.branches.Length; i++)
        {
            branches.Add(Main.branches.branches[i].branch, new HelixBranch(Main.branches.branches[i], Main.branches.branches.Length));

        }

        for (int i = 0; i < Main.commits.commits.Length; i++)
        {
            commits.Add(Main.commits.commits[i].sha,new HelixCommit(i * 2, Main.commits.commits[i], branches[Main.commits.commits[i].branch]));

        }
        RuntimeDebug.Log("Helix created successfull");
    }
}
