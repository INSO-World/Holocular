using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class FileStructure :MonoBehaviour
{
	IFileStructureElement root = new FileStructureFolder();

	int maxDepht = 1;


	public FileStructure()
	{
		root.Name = "root";
    }

	public void AddFilePathToFileStructure(string path,bool changedInThisCommit)
	{
		string[] splittedPath = path.Split('/');
		maxDepht = splittedPath.Length;
        (root as FileStructureFolder).AddElement(splittedPath, changedInThisCommit);
    }

	public void DrawHelixRing(Transform commit)
	{
		Transform rootFolderObject = (root as FileStructureFolder).Draw(commit.transform, maxDepht * Main.helixReferenceRadius *4,new Color(0,1,1,1),new Color(0,1,1,0),0.75f);
		rootFolderObject.parent = commit;
    }
}

