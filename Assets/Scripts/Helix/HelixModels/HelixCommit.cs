using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HelixCommit : MonoBehaviour
{
    public DBCommit dBCommitStore;

    public HelixBranch helixBranchStore;

    public int idStore;


    public HelixCommit(int id,DBCommit dbCommit,HelixBranch branch)
    {
        dBCommitStore = dbCommit;
        helixBranchStore = branch;
        idStore = id;

    }

    public void GenerateCommit(Dictionary<String, List<HelixComitFileRelation>> commitsFiles, Dictionary<String, HelixFile> files,Dictionary<String, HelixComitFileRelation> projectFiles)
    {
        GameObject commitObject = new GameObject("Commit" + idStore);
        commitObject.transform.position = new Vector3(helixBranchStore.position.x, helixBranchStore.position.y, idStore * 4);
        Instantiate(Main.sCommit, commitObject.transform);
        if (commitsFiles.ContainsKey(dBCommitStore._id))
        {
            FileStructure fileStructure = new FileStructure();
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
                    helixComitFileRelation.dBCommitsFilesStore.from:
                    helixComitFileRelation.dBCommitsFilesStore._from];
                changedFiles.Add(file.dBFileStore.path, helixComitFileRelation);
            }

            //Add all historic files to File structure
            foreach (HelixComitFileRelation helixComitFileRelation in projectFiles.Values)
            {
                fileCount++;

                HelixFile file = helixComitFileRelation.dBCommitsFilesStore.from!=null?
                    files[helixComitFileRelation.dBCommitsFilesStore.from]:
                    files[helixComitFileRelation.dBCommitsFilesStore._from];

                fileStructure.AddFilePathToFileStructure(file.dBFileStore.path, changedFiles.ContainsKey(file.dBFileStore.path));
                /*file.fileObject = new GameObject("File: " + file.dBFileStore.path);
                file.fileObject.transform.parent = commitObject.transform;
                file.fileObject.transform.localPosition = new Vector3(fileCount * 2, file.dBFileStore.path.Split("/").Length, 0);
                if (!changedFiles.ContainsKey(file.dBFileStore.path))
                {
                    //Helix.changesGenerated++;
                    //Instantiate(Main.sFile, file.fileObject.transform);
                }
                else
                {
                    Helix.changesGenerated++;
                    Instantiate(Main.sChangedFile, file.fileObject.transform);
                }*/
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
                    /*
                    GameObject fileObject = new GameObject("File: " + file.dBFileStore.path);
                    fileObject.transform.parent = commitObject.transform;
                    fileObject.transform.localPosition = new Vector3(fileCount * 2, file.dBFileStore.path.Split("/").Length, 0);
                    Helix.changesGenerated++;
                    Instantiate(Main.sChangedFile, fileObject.transform);
                    projectFiles.Add(file.dBFileStore.path, helixComitFileRelation);
                    */
                }
            }

            fileStructure.DrawHelixRing(commitObject.transform);
        }
        RuntimeDebug.Log("Commit created: " + dBCommitStore.sha);
    }

}
