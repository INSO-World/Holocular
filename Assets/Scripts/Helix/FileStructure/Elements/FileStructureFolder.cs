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
            (file as FileStructureFile).authorSignature = commit.signature;

            if (helixCommitFileRelation.dBCommitsFilesStore.stats.additions >= Helix.maxAdditions)
            {
                Helix.maxAdditions = helixCommitFileRelation.dBCommitsFilesStore.stats.additions;
            }

            if (helixCommitFileRelation.dBCommitsFilesStore.stats.deletions >= Helix.maxDeletions)
            {
                Helix.maxDeletions = helixCommitFileRelation.dBCommitsFilesStore.stats.deletions;
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
        Dictionary<string, HelixConnectionTree> fileHelixConnectiontreeDictionary)
    {
        Vector3 pos = parent.position;
        GameObject folderObject = new GameObject(name);
        folderObject.transform.position = pos;
        folderObject.transform.parent = parent;

        int elementsInFolder = folderContent.ToList().Count();

        float angleBetweenElements = (2 * Mathf.PI) / elementsInFolder;
        Main.actionQueue.Enqueue(() =>
        {
            GameObject connection = new GameObject("Connection");
            connection.transform.parent = parent;
            connection.transform.position = new Vector3(pos.x, pos.y, pos.z);
            connection.AddComponent<LineRenderer>();

            LineRenderer lr = connection.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            lr.SetColors(ringColor, ringColor);
            lr.SetWidth(0.1f, 0.1f);

            if (elementsInFolder == 1)
            {
                lr.positionCount = 2;
                lr.SetPosition(0, new Vector3(pos.x, pos.y, pos.z));
                lr.SetPosition(1, new Vector3(pos.x + r, pos.y, pos.z));
            }
            else
            {
                lr.positionCount = elementsInFolder;
                lr.loop = true;

                for (int i = 0; i < elementsInFolder; i++)
                {
                    float x = r * Mathf.Cos(i * angleBetweenElements);
                    float y = r * Mathf.Sin(i * angleBetweenElements);

                    lr.SetPosition(i, new Vector3(pos.x + x, pos.y + y, pos.z));

                    IFileStructureElement element = folderContent.Values.ElementAt(i);

                    if (element.Changed)
                    {
                        if (element is FileStructureFolder)
                        {
                            GameObject helixElementObject = new GameObject(element.Name);
                            helixElementObject.transform.parent = folderObject.transform;
                            helixElementObject.transform.position = new Vector3(pos.x + x, pos.y + y, pos.z);
                            (element as FileStructureFolder).Draw(helixElementObject.transform, r / 2, Color.Lerp(ringColor, ringEndColor, colorShiftFactor), ringEndColor, colorShiftFactor, branchName, fileHelixConnectiontreeDictionary);

                        }
                        else if (element is FileStructureFile)
                        {

                            GameObject helixElementObject = new GameObject(element.Name);
                            helixElementObject.transform.parent = folderObject.transform;
                            helixElementObject.transform.position = new Vector3(pos.x + x, pos.y + y, pos.z);
                            string fullFilePath = (element as FileStructureFile).fullPath;

                            GameObject changedFileObject = Instantiate(Main.sChangedFile, helixElementObject.transform);

                            changedFileObject.name = fullFilePath;
                            FileController fileController = changedFileObject.GetComponent<FileController>();
                            fileController.authorSighnature = (element as FileStructureFile).authorSignature;

                            float additionFactor = (float)(element as FileStructureFile).helixCommitFileRelation.dBCommitsFilesStore.stats.additions / Helix.maxAdditions;
                            float deletionFactor = (float)(element as FileStructureFile).helixCommitFileRelation.dBCommitsFilesStore.stats.deletions / Helix.maxDeletions;

                            float changeFactor = (float)((element as FileStructureFile).helixCommitFileRelation.dBCommitsFilesStore.stats.additions + (element as FileStructureFile).helixCommitFileRelation.dBCommitsFilesStore.stats.deletions) / (Helix.maxAdditions + Helix.maxDeletions);

                            if (!fileHelixConnectiontreeDictionary.ContainsKey(fullFilePath))
                            {
                                HelixConnectionTree connectionTree = new HelixConnectionTree(fullFilePath + "-Connections", Main.sBranchTreeMaterial);
                                connectionTree.addDualPoint(branchName, helixElementObject.transform.position, null, null, additionFactor, deletionFactor);
                                fileHelixConnectiontreeDictionary.Add(fullFilePath, connectionTree);
                            }
                            else
                            {
                                fileHelixConnectiontreeDictionary[fullFilePath].addDualPoint(branchName, helixElementObject.transform.position, null, null, additionFactor, deletionFactor);
                            }
                        }
                    }

                }
            }
        });
        return folderObject.transform;
    }
}

