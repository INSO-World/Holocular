using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Helix : MonoBehaviour
{
    Dictionary<string, HelixCommit> commits = new Dictionary<string, HelixCommit>(); //Key: sha

    Dictionary<string, HelixBranch> branches = new Dictionary<string, HelixBranch>(); //Key: branch name

    Dictionary<string, List<HelixCommitFileRelation>> commitsFiles = new Dictionary<string, List<HelixCommitFileRelation>>(); //Key: to = commit id

    Dictionary<string, HelixFile> files = new Dictionary<string, HelixFile>(); // Key: id

    Dictionary<string, HelixCommitFileRelation> projectFiles = new Dictionary<string, HelixCommitFileRelation>(); //key: path

    public static Dictionary<string, HelixStakeholder> stakeholders = new Dictionary<string, HelixStakeholder>(); // Key: siganture

    HelixConnectionTree commitConnectionTree = new HelixConnectionTree("Commits-Connections", Main.sCommitTreeMaterial);

    Dictionary<string, HelixConnectionTree> fileHelixConnectiontreeDictionary = new Dictionary<string, HelixConnectionTree>();

    public bool structureCreated = false;
    public bool structureDrawn = false;


    public static int changesGenerated = 0;

    public static int maxAdditions = 0;
    public static int maxDeletions = 0;

    Thread createStructureThread;
    public ThreadState createStructureThreadState;
    Thread drawStructureThread;
    public ThreadState drawStructureThreadState;

    public Helix()
    {
        createStructureThread = new Thread(CreateStructure);
        createStructureThreadState = createStructureThread.ThreadState;
        drawStructureThread = new Thread(DrawStructure);
        drawStructureThreadState = drawStructureThread.ThreadState;
    }

    public void GenerateHelix()
    {

        structureCreated = false;
        structureDrawn = false;
        createStructureThread.Start();

    }

    public void CheckUpdate()
    {
        createStructureThreadState = createStructureThread.ThreadState;
        drawStructureThreadState = drawStructureThread.ThreadState;

        if (createStructureThread.ThreadState == ThreadState.Stopped && !structureCreated)
        {
            drawStructureThread.Start();
            structureCreated = true;
        }

        if (drawStructureThread.ThreadState == ThreadState.Stopped && !structureDrawn)
        {

            RuntimeDebug.Log("Changes generated: " + changesGenerated);

            RuntimeDebug.Log("Helix created successfull");
            structureDrawn = true;
        }
    }


    void CreateStructure()
    {
        CreateBranchesDictionary();

        CreateCommitsDictionary();

        CreateFilesDictionary();

        CreateStakeholdersDictionary();
    }


    private void CreateCommitsDictionary()
    {
        Array.Sort(Main.commits.commits, (a, b) => DateTime.Parse(a.date).CompareTo(DateTime.Parse(b.date)));

        for (int i = 0; i < Main.commits.commits.Length; i++)
        {
            commits.Add(Main.commits.commits[i].sha, new HelixCommit(i, Main.commits.commits[i], branches[Main.commits.commits[i].branch]));
        }

        for (int i = 0; i < Main.commitsFiles.commitsFiles.Length; i++)
        {
            if (Main.commitsFiles.commitsFiles[i].to != null)
            {
                if (!commitsFiles.ContainsKey(Main.commitsFiles.commitsFiles[i].to))
                {
                    commitsFiles.Add(Main.commitsFiles.commitsFiles[i].to, new List<HelixCommitFileRelation>());
                }
                commitsFiles[Main.commitsFiles.commitsFiles[i].to].Add(new HelixCommitFileRelation(Main.commitsFiles.commitsFiles[i]));
            }
            else
            {
                if (!commitsFiles.ContainsKey(Main.commitsFiles.commitsFiles[i]._to))
                {
                    commitsFiles.Add(Main.commitsFiles.commitsFiles[i]._to, new List<HelixCommitFileRelation>());
                }
                commitsFiles[Main.commitsFiles.commitsFiles[i]._to].Add(new HelixCommitFileRelation(Main.commitsFiles.commitsFiles[i]));

            }
        }
    }

    private void CreateBranchesDictionary()
    {
        for (int i = 0; i < Main.branches.branches.Length; i++)
        {
            if (!branches.ContainsKey(Main.branches.branches[i].branch))
            {
                branches.Add(Main.branches.branches[i].branch, new HelixBranch(Main.branches.branches[i], Main.branches.branches.Length));
            }
        }
    }

    private void CreateFilesDictionary()
    {
        for (int i = 0; i < Main.files.files.Length; i++)
        {
            files.Add(Main.files.files[i]._id, new HelixFile(Main.files.files[i]));
        }
    }


    private void CreateStakeholdersDictionary()
    {
        Color[] palette = ColorPalette.Generate(Main.stakeholders.stakeholders.Length);
        for (int i = 0; i < Main.stakeholders.stakeholders.Length; i++)
        {
            stakeholders.Add(Main.stakeholders.stakeholders[i].gitSignature, new HelixStakeholder(Main.stakeholders.stakeholders[i], palette[i]));
        }
    }

    void DrawStructure()
    {
        foreach (HelixCommit commit in commits.Values)
        {
            commit.DrawCommit(commitsFiles, files, projectFiles);
            commit.ConnectCommit(commitConnectionTree, commits);
            commit.DrawHelixRing(fileHelixConnectiontreeDictionary);
        }
    }
}
