using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helix : MonoBehaviour
{
    Dictionary<String,HelixCommit> commits = new Dictionary<string, HelixCommit>(); //Key: sha

    Dictionary<String, HelixBranch> branches = new Dictionary<string, HelixBranch>(); //Key: branch name

    Dictionary<String, List<HelixComitFileRelation>> commitsFiles = new Dictionary<string, List<HelixComitFileRelation>>(); //Key: to = commit id

    Dictionary<String, HelixFile> files = new Dictionary<string, HelixFile>(); // Key: id

    Dictionary<String, HelixComitFileRelation> projectFiles = new Dictionary<string, HelixComitFileRelation>(); //key: path

    public static int changesGenerated = 0;

    public Helix()
    {
        for (int i = 0; i < Main.branches.branches.Length; i++)
        {
            branches.Add(Main.branches.branches[i].branch, new HelixBranch(Main.branches.branches[i], Main.branches.branches.Length));
        }

        for (int i = 0; i < Main.commits.commits.Length; i++)
        {
            commits.Add(Main.commits.commits[i].sha,new HelixCommit(i, Main.commits.commits[i], branches[Main.commits.commits[i].branch]));

        }

        for (int i = 0; i < Main.commitsFiles.commitsFiles.Length; i++)
        {
            if (Main.commitsFiles.commitsFiles[i].to != null)
            {
                if (!commitsFiles.ContainsKey(Main.commitsFiles.commitsFiles[i].to))
                {
                    commitsFiles.Add(Main.commitsFiles.commitsFiles[i].to, new List<HelixComitFileRelation>());
                }
                commitsFiles[Main.commitsFiles.commitsFiles[i].to].Add(new HelixComitFileRelation(Main.commitsFiles.commitsFiles[i]));
            }
            else
            {
                if (!commitsFiles.ContainsKey(Main.commitsFiles.commitsFiles[i]._to))
                {
                    commitsFiles.Add(Main.commitsFiles.commitsFiles[i]._to, new List<HelixComitFileRelation>());
                }
                commitsFiles[Main.commitsFiles.commitsFiles[i]._to].Add(new HelixComitFileRelation(Main.commitsFiles.commitsFiles[i]));

            }
        }

        for (int i = 0; i < Main.files.files.Length; i++)
        {
            files.Add(Main.files.files[i]._id, new HelixFile(Main.files.files[i]));
        }
        int cCount = 0;
        foreach (HelixCommit commit in commits.Values)
        {
            cCount++;
            if (cCount > 150)
            {
                break;
            }
            commit.GenerateCommit(commitsFiles, files, projectFiles);
        }
        RuntimeDebug.Log("Changes generated: " + changesGenerated);

        RuntimeDebug.Log("Helix created successfull");
    }
}
