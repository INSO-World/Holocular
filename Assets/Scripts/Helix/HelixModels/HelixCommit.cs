using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class HelixCommit : MonoBehaviour
{

    public DBCommit dBCommitStore;

    public HelixBranch helixBranchStore;

    public int idStore;

    Vector3 commitPositionLinear;
    Vector3 commitPositionTime;

    public GameObject commitObject;

    FileStructure fileStructure;


    public HelixCommit(int id, DBCommit dbCommit, HelixBranch branch)
    {
        dBCommitStore = dbCommit;
        helixBranchStore = branch;
        idStore = id;
        fileStructure = new FileStructure();
    }

    public void DrawCommit(Dictionary<string, List<HelixCommitFileRelation>> commitsFiles,
        Dictionary<string, HelixFile> files,
        Dictionary<string, HelixCommitFileRelation> projectFiles,
        GameObject parent)
    {
        Main.actionQueue.Enqueue(() =>
        {
            commitObject = new GameObject("Commit[" + idStore + "]: " + dBCommitStore.sha);
            commitObject.transform.parent = parent.transform;
            commitPositionLinear = new Vector3(helixBranchStore.position.x, helixBranchStore.position.y, idStore);
            CommitController commitController = commitObject.AddComponent<CommitController>();
            commitController.positionLinear = commitPositionLinear;

            float timestamp = DateTime.Parse(dBCommitStore.date).Ticks / 10000 / 1000 / 60 / 60 / 24;//10000 ticks, 1000 ms, 60 sec, 60 min, 24 h
            if (Main.helix.firstTimestamp < 0f)
            {
                Main.helix.firstTimestamp = timestamp;
            }
            commitPositionTime = new Vector3(helixBranchStore.position.x, helixBranchStore.position.y, timestamp - Main.helix.firstTimestamp);
            commitController.positionTime = commitPositionTime;

            Instantiate(Main.sCommit, commitObject.transform);
            Statistics.commitsDrawn++;
        });

        BuildFileStructure(commitsFiles, files, projectFiles);
        RuntimeDebug.Log("Commit created: " + dBCommitStore.sha);


    }

    public Vector3 GetCommitPosition()
    {
        if (GlobalSettings.commitPlacementMode)
        {
            return commitPositionTime;
        }
        else
        {
            return commitPositionLinear;
        }
    }

    public Vector3 GetCommitPositionLinear()
    {
        return commitPositionLinear;
    }

    public Vector3 GetCommitPositionTime()
    {
        return commitPositionTime;
    }

    private void BuildFileStructure(Dictionary<string, List<HelixCommitFileRelation>> commitsFiles,
        Dictionary<string, HelixFile> files,
        Dictionary<string, HelixCommitFileRelation> projectFiles)
    {
        if (commitsFiles.ContainsKey(dBCommitStore._id))
        {
            List<HelixCommitFileRelation> helixCommitFileRelations = commitsFiles[dBCommitStore._id];
            Dictionary<String, HelixCommitFileRelation> changedFiles = new Dictionary<string, HelixCommitFileRelation>();
            helixCommitFileRelations.Sort((r1, r2) =>
            {
                HelixFile file1 = files[r1.dBCommitsFilesStore.from != null ?
                    r1.dBCommitsFilesStore.from :
                    r1.dBCommitsFilesStore._from];
                HelixFile file2 = files[r2.dBCommitsFilesStore.from != null ?
                    r2.dBCommitsFilesStore.from :
                    r2.dBCommitsFilesStore._from];
                return file1.dBFileStore.path.CompareTo(file2.dBFileStore.path);
            });
            int fileCount = 0;


            foreach (HelixCommitFileRelation helixCommitFileRelation in helixCommitFileRelations)
            {
                HelixFile file = files[helixCommitFileRelation.dBCommitsFilesStore.from != null ?
                    helixCommitFileRelation.dBCommitsFilesStore.from :
                    helixCommitFileRelation.dBCommitsFilesStore._from];
                changedFiles.Add(file.dBFileStore.path, helixCommitFileRelation);
                if (projectFiles.ContainsKey(file.dBFileStore.path))
                {
                    projectFiles[file.dBFileStore.path] = helixCommitFileRelation;
                }
                else
                {
                    projectFiles.Add(file.dBFileStore.path, helixCommitFileRelation);

                }
            }

            foreach (HelixCommitFileRelation helixCommitFileRelation in projectFiles.Values)
            {
                fileCount++;

                HelixFile file = helixCommitFileRelation.dBCommitsFilesStore.from != null ?
                    files[helixCommitFileRelation.dBCommitsFilesStore.from] :
                    files[helixCommitFileRelation.dBCommitsFilesStore._from];

                fileStructure.AddFilePathToFileStructure(dBCommitStore, file.dBFileStore.path, changedFiles.ContainsKey(file.dBFileStore.path), helixCommitFileRelation);
            }
        }
    }

    public void ConnectCommit(HelixConnectionTree connectionTree,
        Dictionary<string, HelixCommit> shaCommitsRelation)
    {
        Main.actionQueue.Enqueue(() =>
        {
            connectionTree.addPoint(helixBranchStore.dBBranchStore.branch, "commits", this, dBCommitStore.parents == "" ? null : dBCommitStore.parents.Split(","), new Vector3(0, 0, 0), 0.0f, 0.5f);
        });
    }

    public void DrawHelixRing(Dictionary<string, HelixConnectionTree> fileHelixConnectiontreeDictionary,
        Dictionary<string, HelixCommit> shaCommitsRelation)
    {
        Main.actionQueue.Enqueue(() =>
        {
            fileStructure.DrawHelixRing(this, helixBranchStore.dBBranchStore.branch, fileHelixConnectiontreeDictionary, shaCommitsRelation);

        });
    }
}
