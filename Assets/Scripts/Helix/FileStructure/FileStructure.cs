using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class FileStructure : MonoBehaviour
{
    IFileStructureElement root = new FileStructureFolder();

    int maxDepht = 1;


    public FileStructure()
    {
        root.Name = "root";
    }

    public void AddFilePathToFileStructure(string path, bool changedInThisCommit, HelixComitFileRelation helixCommitFileRelation)
    {
        string[] splittedPath = path.Split('/');
        maxDepht = splittedPath.Length;
        (root as FileStructureFolder).AddElement(path, splittedPath, changedInThisCommit, helixCommitFileRelation);
    }

    public void DrawHelixRing(Transform commit, string branchName, Dictionary<string, HelixConnectionTree> fileHelixConnectiontreeDictionary)
    {
        Transform rootFolderObject = (root as FileStructureFolder).Draw(commit.transform, maxDepht * Main.helixReferenceRadius * Main.helixeRadiusSpread, new Color(0, 1, 1, 0.5f), new Color(0, 1, 1, 0.25f), 0.75f, branchName, fileHelixConnectiontreeDictionary);
        rootFolderObject.parent = commit;
    }
}

