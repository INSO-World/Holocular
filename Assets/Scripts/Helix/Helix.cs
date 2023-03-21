using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helix : MonoBehaviour
{
    Dictionary<string,HelixCommit> commits = new Dictionary<string, HelixCommit>(); //Key: sha

    Dictionary<string, HelixBranch> branches = new Dictionary<string, HelixBranch>(); //Key: branch name

    Dictionary<string, List<HelixComitFileRelation>> commitsFiles = new Dictionary<string, List<HelixComitFileRelation>>(); //Key: to = commit id

    Dictionary<string, HelixFile> files = new Dictionary<string, HelixFile>(); // Key: id

    Dictionary<string, HelixComitFileRelation> projectFiles = new Dictionary<string, HelixComitFileRelation>(); //key: path

    HelixConnectionTree commitConnectionTree = new HelixConnectionTree("Commits-Connections",Color.gray);

    Dictionary<string, HelixConnectionTree> fileHelixConnectiontreeDictionary = new Dictionary<string, HelixConnectionTree>();

    public static int changesGenerated = 0;

    public Helix()
    {

        for (int i = 0; i < Main.branches.branches.Length; i++)
        {
            if (!branches.ContainsKey(Main.branches.branches[i].branch))
            {
                branches.Add(Main.branches.branches[i].branch, new HelixBranch(Main.branches.branches[i], Main.branches.branches.Length));
            }
        }

        Array.Sort(Main.commits.commits, (a, b) => DateTime.Parse(a.date).CompareTo(DateTime.Parse(b.date)));

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
        Dictionary<string, GameObject> shaCommitsRelation = new Dictionary<string, GameObject>();
        foreach (HelixCommit commit in commits.Values)
        {
            cCount++;
            shaCommitsRelation.Add(commit.dBCommitStore.sha, commit.GenerateCommit(commitsFiles, files, projectFiles));
        }

        foreach (HelixCommit commit in commits.Values)
        {
            commit.ConnectCommit(commitConnectionTree, shaCommitsRelation);
            commit.DrawHelixRing(fileHelixConnectiontreeDictionary);
        }
        RuntimeDebug.Log("Changes generated: " + changesGenerated);

        RuntimeDebug.Log("Helix created successfull");
    }
}
