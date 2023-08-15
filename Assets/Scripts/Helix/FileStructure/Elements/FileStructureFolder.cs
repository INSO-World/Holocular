using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class FileStructureFolder : MonoBehaviour, IFileStructureElement
{
    private string name = "";
    private bool changed = false;

    Dictionary<string, IFileStructureElement> folderContent = new Dictionary<string, IFileStructureElement>();

    public FileStructureFolder()
    {
    }

    string IFileStructureElement.Name { get { return name; } set { name = value; } }

    bool IFileStructureElement.Changed { get { return changed; } set { changed = value; } }


    public void AddElement(DBCommit commit, string fullPath, string[] pathParts, bool changedInThisCommit, HelixCommitFileRelation helixCommitFileRelation)
    {
        string currentPathPart = pathParts[0];
        if (pathParts.Length > 1)
        {
            string[] remainingPathParts = pathParts.Skip(1).ToArray();

            if (folderContent.ContainsKey(currentPathPart))
            {
                (folderContent[currentPathPart] as FileStructureFolder).AddElement(commit, fullPath, remainingPathParts, changedInThisCommit, helixCommitFileRelation);
                if (changedInThisCommit)
                {
                    folderContent[currentPathPart].Changed = changedInThisCommit;
                }
            }
            else
            {
                IFileStructureElement folder = new FileStructureFolder();
                folder.Name = currentPathPart;
                (folder as FileStructureFolder).AddElement(commit, fullPath, remainingPathParts, changedInThisCommit, helixCommitFileRelation);
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
            GameObject ring = new GameObject("Ring");
            ring.transform.parent = parent;
            ring.transform.localPosition = new Vector3(0, 0, 0);
            ring.AddComponent<LineRenderer>();

            LineRenderer lr = ring.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            lr.SetColors(ringColor, ringColor);
            lr.SetWidth(0.1f, 0.1f);
            lr.useWorldSpace = false;

            if (elementsInFolder == 1)
            {
                lr.positionCount = 2;
                lr.SetPosition(0, new Vector3(0, 0, 0));
                lr.SetPosition(1, new Vector3(r, 0, 0));
            }
            else
            {
                lr.positionCount = elementsInFolder;
                lr.loop = true;

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
                            fileController.commit = (element as FileStructureFile).commit;
                            fileController.Init();

                            float additionFactor = (float)(element as FileStructureFile).helixCommitFileRelation.dBCommitsFilesStore.stats.additions / Main.helix.maxAdditions;
                            float deletionFactor = (float)(element as FileStructureFile).helixCommitFileRelation.dBCommitsFilesStore.stats.deletions / Main.helix.maxDeletions;

                            float changeFactor = (float)((element as FileStructureFile).helixCommitFileRelation.dBCommitsFilesStore.stats.additions + (element as FileStructureFile).helixCommitFileRelation.dBCommitsFilesStore.stats.deletions) / (Main.helix.maxAdditions + Main.helix.maxDeletions);

                            if (!fileHelixConnectiontreeDictionary.ContainsKey(fullFilePath))
                            {
                                HelixConnectionTree connectionTree = new HelixConnectionTree(fullFilePath + "-Connections", Main.sBranchTreeMaterial, Main.helix.helixObject);
                                connectionTree.addDualPoint(branchName, fullFilePath, commit, commit.dBCommitStore.parents == "" ? null : commit.dBCommitStore.parents.Split(","), newOffsetPos, changeFactor, changeFactor, shaCommitsRelation);
                                fileHelixConnectiontreeDictionary.Add(fullFilePath, connectionTree);
                            }
                            else
                            {
                                fileHelixConnectiontreeDictionary[fullFilePath].addDualPoint(branchName, fullFilePath, commit, commit.dBCommitStore.parents == "" ? null : commit.dBCommitStore.parents.Split(","), newOffsetPos, changeFactor, changeFactor, shaCommitsRelation);
                            }
                        }
                    }

                }
            }
        });
        return folderObject.transform;
    }
}

