using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Helix : MonoBehaviour
{
    public GameObject helixObject;

    public Dictionary<string, HelixCommit> commits = new Dictionary<string, HelixCommit>(); //Key: sha
    public Dictionary<string, HelixCommit> commitsID = new Dictionary<string, HelixCommit>(); //Key: sha

    public Dictionary<string, HelixBranch> branches = new Dictionary<string, HelixBranch>(); //Key: branch name

    Dictionary<string, List<HelixCommitFileRelation>> commitsFiles = new Dictionary<string, List<HelixCommitFileRelation>>(); //Key: to = commit id

    public Dictionary<string, HelixCommitStakeholderRelation> commitStakeholderRelations = new Dictionary<string, HelixCommitStakeholderRelation>(); //Key: commit id

    public Dictionary<string, List<HelixCommitCommitRelation>> commitCommitRelations = new Dictionary<string, List<HelixCommitCommitRelation>>(); //Key: to = parent commit id

    Dictionary<string, HelixFile> files = new Dictionary<string, HelixFile>(); // Key: id

    Dictionary<string, HelixCommitFileRelation> projectFiles = new Dictionary<string, HelixCommitFileRelation>(); //key: path

    Dictionary<string, List<HelixCommitFileStakeholderRelation>> commitFileStakeholderRelations = new Dictionary<string, List<HelixCommitFileStakeholderRelation>>(); //key: _from (CommitFileRelationID)


    public Dictionary<string, HelixStakeholder> stakeholders = new Dictionary<string, HelixStakeholder>(); // Key: siganture
    public Dictionary<string, HelixStakeholder> stakeholdersID = new Dictionary<string, HelixStakeholder>(); // Key: db id


    HelixConnectionTree commitConnectionTree;

    Dictionary<string, HelixConnectionTree> fileHelixConnectiontreeDictionary = new Dictionary<string, HelixConnectionTree>();

    public bool structureCreated = false;
    public bool structureDrawn = false;


    public int changesGenerated = 0;

    public int maxAdditions = 0;
    public int maxDeletions = 0;

    Thread createStructureThread;
    public ThreadState createStructureThreadState;
    Thread drawStructureThread;
    public ThreadState drawStructureThreadState;

    public float firstTimestamp = -1f;

    public Helix(GameObject helixObjectP)
    {
        helixObject = helixObjectP;

        //Create Connection Tree
        commitConnectionTree = new HelixConnectionTree("Commits-Connections", Main.sCommitTreeMaterial, helixObject);

        //Initialize Threads
        createStructureThread = new Thread(CreateStructure);
        createStructureThreadState = createStructureThread.ThreadState;
        createStructureThread.IsBackground = true;
        drawStructureThread = new Thread(DrawStructure);
        drawStructureThreadState = drawStructureThread.ThreadState;
        createStructureThread.IsBackground = true;
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

        CreateCommitsIDDictionary();

        CreateStakeholdersDictionary();

        CreateCommitsFileStakeholderDictionary();

        CreateCommitsCommitsDictionary();

        CreateCommitsStakeholderDictionary();

        CreateCommitsDictionary();

        CreateFilesDictionary();

    }

    private void CreateCommitsFileStakeholderDictionary()
    {
        for (int i = 0; i < Main.commitsFilesStakeholders.commitsFilesStakeholders.Length; i++)
        {

            if (Main.commitsFilesStakeholders.commitsFilesStakeholders[i].to != null)
            {
                if (!commitFileStakeholderRelations.ContainsKey(Main.commitsFilesStakeholders.commitsFilesStakeholders[i].to))
                {
                    commitFileStakeholderRelations.Add(Main.commitsFilesStakeholders.commitsFilesStakeholders[i].from, new List<HelixCommitFileStakeholderRelation>());
                }
                commitFileStakeholderRelations[Main.commitsFilesStakeholders.commitsFilesStakeholders[i].from].Add(new HelixCommitFileStakeholderRelation(Main.commitsFilesStakeholders.commitsFilesStakeholders[i], stakeholdersID[Main.commitsFilesStakeholders.commitsFilesStakeholders[i].to]));
            }
            else
            {
                if (!commitFileStakeholderRelations.ContainsKey(Main.commitsFilesStakeholders.commitsFilesStakeholders[i]._from))
                {
                    commitFileStakeholderRelations.Add(Main.commitsFilesStakeholders.commitsFilesStakeholders[i]._from, new List<HelixCommitFileStakeholderRelation>());
                }
                commitFileStakeholderRelations[Main.commitsFilesStakeholders.commitsFilesStakeholders[i]._from].Add(new HelixCommitFileStakeholderRelation(Main.commitsFilesStakeholders.commitsFilesStakeholders[i], stakeholdersID[Main.commitsFilesStakeholders.commitsFilesStakeholders[i]._to]));
            }

        }
    }

    private void CreateCommitsCommitsDictionary()
    {
        for (int i = 0; i < Main.commitsCommits.commitsCommits.Length; i++)
        {

            if (Main.commitsCommits.commitsCommits[i].to != null)
            {
                if (!commitCommitRelations.ContainsKey(Main.commitsCommits.commitsCommits[i].to))
                {
                    commitCommitRelations.Add(Main.commitsCommits.commitsCommits[i].to, new List<HelixCommitCommitRelation>());
                }
                commitCommitRelations[Main.commitsCommits.commitsCommits[i].to].Add(new HelixCommitCommitRelation(Main.commitsCommits.commitsCommits[i], commitsID[Main.commitsCommits.commitsCommits[i].from]));
            }
            else
            {
                if (!commitCommitRelations.ContainsKey(Main.commitsCommits.commitsCommits[i]._to))
                {
                    commitCommitRelations.Add(Main.commitsCommits.commitsCommits[i]._to, new List<HelixCommitCommitRelation>());
                }
                commitCommitRelations[Main.commitsCommits.commitsCommits[i]._to].Add(new HelixCommitCommitRelation(Main.commitsCommits.commitsCommits[i], commitsID[Main.commitsCommits.commitsCommits[i]._from]));
            }

        }
    }

    private void CreateCommitsStakeholderDictionary()
    {
        for (int i = 0; i < Main.commitsStakeholders.commitsStakeholders.Length; i++)
        {

            if (Main.commitsStakeholders.commitsStakeholders[i].to != null)
            {

                commitStakeholderRelations.Add(Main.commitsStakeholders.commitsStakeholders[i].to, new HelixCommitStakeholderRelation(Main.commitsStakeholders.commitsStakeholders[i]));
            }
            else
            {
                commitStakeholderRelations.Add(Main.commitsStakeholders.commitsStakeholders[i]._to, new HelixCommitStakeholderRelation(Main.commitsStakeholders.commitsStakeholders[i]));
            }

        }
    }

    private void CreateCommitsIDDictionary()
    {
        Array.Sort(Main.commits.commits, (a, b) => DateTime.Parse(a.date).CompareTo(DateTime.Parse(b.date)));

        for (int i = 0; i < Main.commits.commits.Length; i++)
        {
            commitsID.Add(Main.commits.commits[i]._id, new HelixCommit(i, Main.commits.commits[i], branches[Main.commits.commits[i].branch]));
        }
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
            List<HelixCommitFileStakeholderRelation> tmpCommitFileStakeholderRelationList =
                commitFileStakeholderRelations.ContainsKey(Main.commitsFiles.commitsFiles[i]._id) ?
                commitFileStakeholderRelations[Main.commitsFiles.commitsFiles[i]._id] :
                new List<HelixCommitFileStakeholderRelation>();

            if (Main.commitsFiles.commitsFiles[i].to != null)
            {
                if (!commitsFiles.ContainsKey(Main.commitsFiles.commitsFiles[i].to))
                {
                    commitsFiles.Add(Main.commitsFiles.commitsFiles[i].to, new List<HelixCommitFileRelation>());
                }
                commitsFiles[Main.commitsFiles.commitsFiles[i].to].Add(new HelixCommitFileRelation(Main.commitsFiles.commitsFiles[i], tmpCommitFileStakeholderRelationList));
            }
            else
            {
                if (!commitsFiles.ContainsKey(Main.commitsFiles.commitsFiles[i]._to))
                {
                    commitsFiles.Add(Main.commitsFiles.commitsFiles[i]._to, new List<HelixCommitFileRelation>());
                }
                commitsFiles[Main.commitsFiles.commitsFiles[i]._to].Add(new HelixCommitFileRelation(Main.commitsFiles.commitsFiles[i], tmpCommitFileStakeholderRelationList));

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
            stakeholdersID.Add(Main.stakeholders.stakeholders[i]._id, new HelixStakeholder(Main.stakeholders.stakeholders[i], palette[i]));
        }
    }

    void DrawStructure()
    {
        foreach (HelixCommit commit in commits.Values)
        {
            commit.DrawCommit(commitsFiles, files, projectFiles, helixObject);
            commit.ConnectCommit(commitConnectionTree, commits);
            commit.DrawHelixRing(fileHelixConnectiontreeDictionary, commits);
        }
    }

    public void DeleteHelix()
    {
        foreach (Transform child in helixObject.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void StopThreads()
    {
        createStructureThread.Abort();
        drawStructureThread.Abort();
    }

    public void UpdateConnectionTreeDistance()
    {
        commitConnectionTree.UpdateDistances();
        foreach (HelixConnectionTree connectionTree in fileHelixConnectiontreeDictionary.Values)
        {
            connectionTree.UpdateDistances();
        }
    }
}
