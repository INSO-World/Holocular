using System;
using System.Collections.Generic;
using UnityEngine;

public class HelixCommit : MonoBehaviour
{

    public DBCommit dBCommitStore;

    public HelixBranch helixBranchStore;

    public int idStore;

    Vector3 commitPositionLinear;
    Vector3 commitPositionTime;

    public GameObject commitObject;

    FileStructure fileStructure;

    public string[] parents;
    public string signature;

    public HelixCommit(int id, DBCommit dbCommit, HelixBranch branch)
    {
        dBCommitStore = dbCommit;
        helixBranchStore = branch;
        idStore = id;
        fileStructure = new FileStructure();
        parents = GetParents();
        signature = GetSignature();

        commitPositionLinear = new Vector3(branch.position.x, branch.position.y, id);
        float timestamp = DateTime.Parse(dbCommit.date).Ticks / 10000 / 1000 / 60 / 60 / 24;//10000 ticks, 1000 ms, 60 sec, 60 min, 24 h
        if (Main.helix.firstTimestamp < 0f)
        {
            Main.helix.firstTimestamp = timestamp;
        }
        commitPositionTime = new Vector3(helixBranchStore.position.x, helixBranchStore.position.y, timestamp - Main.helix.firstTimestamp);
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
            CommitController commitController = commitObject.AddComponent<CommitController>();
            commitController.positionLinear = commitPositionLinear;
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
                HelixFile file1 = files[r1.dBCommitsFilesStore.to != null ?
                    r1.dBCommitsFilesStore.to :
                    r1.dBCommitsFilesStore._to];
                HelixFile file2 = files[r2.dBCommitsFilesStore.to != null ?
                    r2.dBCommitsFilesStore.to :
                    r2.dBCommitsFilesStore._to];
                return file1.dBFileStore.path.CompareTo(file2.dBFileStore.path);
            });
            int fileCount = 0;


            foreach (HelixCommitFileRelation helixCommitFileRelation in helixCommitFileRelations)
            {
                HelixFile file = files[helixCommitFileRelation.dBCommitsFilesStore.to != null ?
                    helixCommitFileRelation.dBCommitsFilesStore.to :
                    helixCommitFileRelation.dBCommitsFilesStore._to];
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

                HelixFile file = helixCommitFileRelation.dBCommitsFilesStore.to != null ?
                    files[helixCommitFileRelation.dBCommitsFilesStore.to] :
                    files[helixCommitFileRelation.dBCommitsFilesStore._to];

                fileStructure.AddFilePathToFileStructure(dBCommitStore, file.dBFileStore.path, changedFiles.ContainsKey(file.dBFileStore.path), helixCommitFileRelation);
            }
        }
    }

    public void ConnectCommit(HelixConnectionTree connectionTree,
        Dictionary<string, HelixCommit> shaCommitsRelation)
    {
        Debug.Log("Parents: " + parents.Length);
        Main.actionQueue.Enqueue(() =>
        {
            connectionTree.addPoint(helixBranchStore.dBBranchStore.branch, "commits", this, parents, new Vector3(0, 0, 0), 0.0f, 0.5f, shaCommitsRelation);
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

    public string[] GetParents()
    {
        List<string> parents = new List<string>();
        if (dBCommitStore.parents != null && dBCommitStore.parents != "")
        {
            foreach (string parent in dBCommitStore.parents.Split(","))
            {
                parents.Add(parent);
            }
        }
        else if (Main.helix.commitCommitRelations.Count > 0 && Main.helix.commitCommitRelations.ContainsKey(dBCommitStore._id))
        {
            foreach (HelixCommitCommitRelation parent in Main.helix.commitCommitRelations[dBCommitStore._id])
            {
                parents.Add(parent.dbCommitStore.sha);
            }
        }
        return parents.ToArray();
    }

    public string GetSignature()
    {
        string signature = "";

        if (dBCommitStore.signature != null && dBCommitStore.signature != "")
        {
            signature = dBCommitStore.signature;
        }
        else if (Main.helix.commitStakeholderRelations.Count > 0 && Main.helix.commitStakeholderRelations.ContainsKey(dBCommitStore._id))
        {
            if (Main.helix.commitStakeholderRelations[dBCommitStore._id].dBCommitStakeholderStore.to != null && Main.helix.stakeholdersID.ContainsKey(Main.helix.commitStakeholderRelations[dBCommitStore._id].dBCommitStakeholderStore.to))
            {
                signature = Main.helix.stakeholdersID[Main.helix.commitStakeholderRelations[dBCommitStore._id].dBCommitStakeholderStore.to].dBStakeholderStore.gitSignature;
            }
            else if (Main.helix.commitStakeholderRelations[dBCommitStore._id].dBCommitStakeholderStore._to != null && Main.helix.stakeholdersID.ContainsKey(Main.helix.commitStakeholderRelations[dBCommitStore._id].dBCommitStakeholderStore._to))
            {
                signature = Main.helix.stakeholdersID[Main.helix.commitStakeholderRelations[dBCommitStore._id].dBCommitStakeholderStore._to].dBStakeholderStore.gitSignature;
            }
        }

        return signature;
    }
}
