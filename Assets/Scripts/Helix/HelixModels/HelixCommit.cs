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

    GameObject commitObject;

    FileStructure fileStructure;

    public HelixCommit(int id,DBCommit dbCommit,HelixBranch branch)
    {
        dBCommitStore = dbCommit;
        helixBranchStore = branch;
        idStore = id;
        fileStructure = new FileStructure();
    }

    public GameObject GenerateCommit(Dictionary<string, List<HelixComitFileRelation>> commitsFiles,
        Dictionary<string, HelixFile> files,
        Dictionary<string, HelixComitFileRelation> projectFiles)
    {
        commitObject = new GameObject("Commit["+idStore+"]: " + dBCommitStore.sha);
        commitObject.transform.position = new Vector3(helixBranchStore.position.x, helixBranchStore.position.y, idStore * 4);
        Instantiate(Main.sCommit, commitObject.transform);

        BuildFileStructure(commitsFiles, files, projectFiles);

        RuntimeDebug.Log("Commit created: " + dBCommitStore.sha);
        return commitObject;
    }



    private void BuildFileStructure(Dictionary<string, List<HelixComitFileRelation>> commitsFiles,
        Dictionary<string, HelixFile> files,
        Dictionary<string, HelixComitFileRelation> projectFiles)
    {
        if (commitsFiles.ContainsKey(dBCommitStore._id))
        {
            List<HelixComitFileRelation> helixComitFileRelations = commitsFiles[dBCommitStore._id];
            Dictionary<String, HelixComitFileRelation> changedFiles = new Dictionary<string, HelixComitFileRelation>();
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


            foreach (HelixComitFileRelation helixComitFileRelation in helixComitFileRelations)
            {
                HelixFile file = files[helixComitFileRelation.dBCommitsFilesStore.from != null ?
                    helixComitFileRelation.dBCommitsFilesStore.from :
                    helixComitFileRelation.dBCommitsFilesStore._from];
                changedFiles.Add(file.dBFileStore.path, helixComitFileRelation);
            }

            //Add all historic files to File structure
            foreach (HelixComitFileRelation helixComitFileRelation in projectFiles.Values)
            {
                fileCount++;

                HelixFile file = helixComitFileRelation.dBCommitsFilesStore.from != null ?
                    files[helixComitFileRelation.dBCommitsFilesStore.from] :
                    files[helixComitFileRelation.dBCommitsFilesStore._from];

                fileStructure.AddFilePathToFileStructure(file.dBFileStore.path, changedFiles.ContainsKey(file.dBFileStore.path));
            }

            //Add new files to File structure
            foreach (HelixComitFileRelation helixComitFileRelation in helixComitFileRelations)
            {
                fileCount++;
                HelixFile file = helixComitFileRelation.dBCommitsFilesStore.from != null ?
                    files[helixComitFileRelation.dBCommitsFilesStore.from] :
                    files[helixComitFileRelation.dBCommitsFilesStore._from];

                if (!projectFiles.ContainsKey(file.dBFileStore.path))
                {
                    fileStructure.AddFilePathToFileStructure(file.dBFileStore.path, true);
                    projectFiles.Add(file.dBFileStore.path, helixComitFileRelation);
                }
            }
        }
    }

    public void ConnectCommit(HelixConnectionTree connectionTree,
    Dictionary<string, GameObject> shaCommitsRelation)
    {
        connectionTree.addPoint(helixBranchStore.dBBranchStore.branch, commitObject.transform.position, dBCommitStore.parents == "" ? null : dBCommitStore.parents.Split(","), shaCommitsRelation);
    }

    public void DrawHelixRing(Dictionary<string, HelixConnectionTree> fileHelixConnectiontreeDictionary)
    {
        fileStructure.DrawHelixRing(commitObject.transform,helixBranchStore.dBBranchStore.branch, fileHelixConnectiontreeDictionary);
    }

}
