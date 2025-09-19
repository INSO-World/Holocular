using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FileStructureFolder : MonoBehaviour, IFileStructureElement
{
    private string name = "";
    private string path = "";
    private bool changed = false;

    Dictionary<string, IFileStructureElement> folderContent = new Dictionary<string, IFileStructureElement>();

    public FileStructureFolder()
    {
    }

    string IFileStructureElement.Name { get { return name; } set { name = value; } }

    bool IFileStructureElement.Changed { get { return changed; } set { changed = value; } }


    public void AddElement(DBCommit commit, string fullPath, string currPath, string[] pathParts, bool changedInThisCommit, HelixCommitFileRelation helixCommitFileRelation)
    {
        string currentPathPart = pathParts[0];
        if (pathParts.Length > 1)
        {
            string[] remainingPathParts = pathParts.Skip(1).ToArray();
            currPath += currentPathPart + "/";
            if (folderContent.ContainsKey(currentPathPart))
            {
                (folderContent[currentPathPart] as FileStructureFolder).AddElement(commit, fullPath, currPath, remainingPathParts, changedInThisCommit, helixCommitFileRelation);
                if (changedInThisCommit)
                {
                    folderContent[currentPathPart].Changed = changedInThisCommit;
                }
            }
            else
            {
                IFileStructureElement folder = new FileStructureFolder();
                folder.Name = currentPathPart;
                (folder as FileStructureFolder).path = currPath;
                (folder as FileStructureFolder).AddElement(commit, fullPath, currPath, remainingPathParts, changedInThisCommit, helixCommitFileRelation);
                folder.Changed = changedInThisCommit;
                folderContent.Add(currentPathPart, folder);
            }
        }
        else
        {
            IFileStructureElement file = new FileStructureFile();
            file.Name = currentPathPart;
            file.Changed = changedInThisCommit;
            (file as FileStructureFile).fullPath = fullPath;
            (file as FileStructureFile).helixCommitFileRelation = helixCommitFileRelation;
            (file as FileStructureFile).commit = commit;
            (file as FileStructureFile).authorSignature = commit.signature;

            if (helixCommitFileRelation.dBCommitsFilesStore.stats.additions > Main.helix.maxAdditions)
            {
                Main.helix.maxAdditions = helixCommitFileRelation.dBCommitsFilesStore.stats.additions;
            }

            if (helixCommitFileRelation.dBCommitsFilesStore.stats.deletions > Main.helix.maxDeletions)
            {
                Main.helix.maxDeletions = helixCommitFileRelation.dBCommitsFilesStore.stats.deletions;
            }


            if ((helixCommitFileRelation.dBCommitsFilesStore.stats.deletions + helixCommitFileRelation.dBCommitsFilesStore.stats.additions) > Main.helix.maxChanges)
            {
                Main.helix.maxChanges = helixCommitFileRelation.dBCommitsFilesStore.stats.deletions + helixCommitFileRelation.dBCommitsFilesStore.stats.additions;
            }


            folderContent.Add(currentPathPart, file);
        }
    }

    public Transform Draw(Transform parent,
        float r,
        Color ringColor,
        Color ringEndColor,
        float colorShiftFactor,
        string branchName,
        HelixCommit commit,
        Vector3 offsetPos,
        Dictionary<string, HelixConnectionTree> fileHelixConnectiontreeDictionary,
        Dictionary<string, HelixCommit> shaCommitsRelation)
    {
        GameObject folderObject = new GameObject(name);
        folderObject.transform.parent = parent;
        folderObject.transform.localPosition = new Vector3(0, 0, 0);

        int elementsInFolder = folderContent.ToList().Count();

        float angleBetweenElements = (2 * Mathf.PI) / elementsInFolder;
        Main.actionQueue.Enqueue(() =>
        {
            GameObject ring = Instantiate(Main.sFolder, parent);
            ring.transform.localPosition = new Vector3(0, 0, 0);
            ring.name = name;
            FolderController folderController = ring.GetComponent<FolderController>();
            folderController.fullPath = path;
            LineRenderer lr = ring.GetComponent<LineRenderer>();


            lr.SetColors(ringColor, ringColor);

            lr.positionCount = elementsInFolder;

            for (int i = 0; i < elementsInFolder; i++)
            {
                float x = r * Mathf.Cos(i * angleBetweenElements);
                float y = r * Mathf.Sin(i * angleBetweenElements);

                lr.SetPosition(i, new Vector3(x, y, 0));

                IFileStructureElement element = folderContent.Values.ElementAt(i);
                Vector3 newOffsetPos = new Vector3(offsetPos.x + x, offsetPos.y + y, 0);
                if (element.Changed)
                {
                    if (element is FileStructureFolder)
                    {
                        GameObject helixElementObject = new GameObject(element.Name);
                        helixElementObject.transform.parent = folderObject.transform;
                        helixElementObject.transform.localPosition = new Vector3(x, y, 0);
                        (element as FileStructureFolder).Draw(helixElementObject.transform,
                            r / 2,
                            Color.Lerp(ringColor, ringEndColor, colorShiftFactor),
                            ringEndColor,
                            colorShiftFactor,
                            branchName,
                            commit,
                            newOffsetPos,
                            fileHelixConnectiontreeDictionary,
                            shaCommitsRelation
                            );

                    }
                    else if (element is FileStructureFile)
                    {
                        GameObject helixElementObject = new GameObject(element.Name);
                        helixElementObject.transform.parent = folderObject.transform;
                        helixElementObject.transform.localPosition = new Vector3(x, y, 0);
                        string fullFilePath = (element as FileStructureFile).fullPath;

                        GameObject changedFileObject = Instantiate(Main.sChangedFile, helixElementObject.transform);

                        changedFileObject.name = fullFilePath;
                        FileController fileController = changedFileObject.GetComponent<FileController>();
                        fileController.fullFilePath = fullFilePath;
                        fileController.fileName = element.Name;
                        fileController.commitFileRelation = (element as FileStructureFile).helixCommitFileRelation.dBCommitsFilesStore;
                        fileController.commitFileStakeholderRelationList = (element as FileStructureFile).helixCommitFileRelation.helixCommitFileStakeholderRelationListStore;
                        fileController.commit = commit;

                        float additionFactor = (float)(element as FileStructureFile).helixCommitFileRelation.dBCommitsFilesStore.stats.additions / (Main.helix.maxAdditions + Main.helix.maxDeletions) * 2;
                        float deletionFactor = (float)(element as FileStructureFile).helixCommitFileRelation.dBCommitsFilesStore.stats.deletions / (Main.helix.maxAdditions + Main.helix.maxDeletions) * 2;
                        
                        if (!fileHelixConnectiontreeDictionary.ContainsKey(fullFilePath))
                        {
                            HelixConnectionTree connectionTree = new HelixConnectionTree(fullFilePath + "-Connections", Main.sBranchTreeMaterial, Main.helix.helixObject);
                            connectionTree.AddDualPoint(branchName, fullFilePath, commit, commit.parents, newOffsetPos, additionFactor, deletionFactor, shaCommitsRelation);
                            fileHelixConnectiontreeDictionary.Add(fullFilePath, connectionTree);
                        }
                        else
                        {
                            fileHelixConnectiontreeDictionary[fullFilePath].AddDualPoint(branchName, fullFilePath, commit, commit.parents, newOffsetPos, additionFactor, deletionFactor, shaCommitsRelation);
                        }
                        HelixParticleSystemRenderer.UpdateElement(commit.dBCommitStore.sha,fullFilePath,new HelixParticleSystemElement(helixElementObject.transform.position, Main.fileDefaultColor));
                    }
                }

            }
        });
        return folderObject.transform;
    }
}

