using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class HelixCommit : MonoBehaviour
{

    public DBCommit dBCommitStore;

    public HelixBranch helixBranchStore;

    public int idStore;

    Vector3 commitPosition;

    GameObject commitObject;

    FileStructure fileStructure;


    public HelixCommit(int id, DBCommit dbCommit, HelixBranch branch)
    {
        dBCommitStore = dbCommit;
        helixBranchStore = branch;
        idStore = id;
        fileStructure = new FileStructure();
        commitPosition = new Vector3(branch.position.x, branch.position.y, id * 4);
    }

    public void DrawCommit(Dictionary<string, List<HelixCommitFileRelation>> commitsFiles,
        Dictionary<string, HelixFile> files,
        Dictionary<string, HelixCommitFileRelation> projectFiles)
    {
        Main.actionQueue.Enqueue(() =>
        {
            commitObject = new GameObject("Commit[" + idStore + "]: " + dBCommitStore.sha);
            commitObject.transform.position = commitPosition;
            Instantiate(Main.sCommit, commitObject.transform);
            Statistics.commitsDrawn++;
        });

        BuildFileStructure(commitsFiles, files, projectFiles);
        RuntimeDebug.Log("Commit created: " + dBCommitStore.sha);


    }

    public Vector3 GetCommitPosition()
    {
        return commitPosition;
    }

    private void BuildFileStructure(Dictionary<string, List<HelixCommitFileRelation>> commitsFiles,
        Dictionary<string, HelixFile> files,
        Dictionary<string, HelixCommitFileRelation> projectFiles)
    {
        if (commitsFiles.ContainsKey(dBCommitStore._id))
        {
            List<HelixCommitFileRelation> helixComitFileRelations = commitsFiles[dBCommitStore._id];
            Dictionary<String, HelixCommitFileRelation> changedFiles = new Dictionary<string, HelixCommitFileRelation>();
            helixComitFileRelations.Sort((r1, r2) =>
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


            foreach (HelixCommitFileRelation helixCommitFileRelation in helixComitFileRelations)
            {
                HelixFile file = files[helixCommitFileRelation.dBCommitsFilesStore.from != null ?
                    helixCommitFileRelation.dBCommitsFilesStore.from :
                    helixCommitFileRelation.dBCommitsFilesStore._from];
                changedFiles.Add(file.dBFileStore.path, helixCommitFileRelation);
            }

            //Add all historic files to File structure
            foreach (HelixCommitFileRelation helixCommitFileRelation in projectFiles.Values)
            {
                fileCount++;

                HelixFile file = helixCommitFileRelation.dBCommitsFilesStore.from != null ?
                    files[helixCommitFileRelation.dBCommitsFilesStore.from] :
                    files[helixCommitFileRelation.dBCommitsFilesStore._from];

                fileStructure.AddFilePathToFileStructure(dBCommitStore, file.dBFileStore.path, changedFiles.ContainsKey(file.dBFileStore.path), helixCommitFileRelation);
            }

            //Add new files to File structure
            foreach (HelixCommitFileRelation helixCommitFileRelation in helixComitFileRelations)
            {
                fileCount++;
                HelixFile file = helixCommitFileRelation.dBCommitsFilesStore.from != null ?
                    files[helixCommitFileRelation.dBCommitsFilesStore.from] :
                    files[helixCommitFileRelation.dBCommitsFilesStore._from];

                if (!projectFiles.ContainsKey(file.dBFileStore.path))
                {
                    fileStructure.AddFilePathToFileStructure(dBCommitStore, file.dBFileStore.path, true, helixCommitFileRelation);
                    projectFiles.Add(file.dBFileStore.path, helixCommitFileRelation);
                }
            }
        }
    }

    public void ConnectCommit(HelixConnectionTree connectionTree,
    Dictionary<string, HelixCommit> shaCommitsRelation)
    {
        Main.actionQueue.Enqueue(() =>
        {
            connectionTree.addPoint(helixBranchStore.dBBranchStore.branch, commitObject.transform.position, dBCommitStore.parents == "" ? null : dBCommitStore.parents.Split(","), shaCommitsRelation, 0.0f, 0.5f);
        });
    }

    public void DrawHelixRing(Dictionary<string, HelixConnectionTree> fileHelixConnectiontreeDictionary)
    {
        Main.actionQueue.Enqueue(() =>
        {
            fileStructure.DrawHelixRing(commitObject.transform, helixBranchStore.dBBranchStore.branch, fileHelixConnectiontreeDictionary);

        });
    }
}
