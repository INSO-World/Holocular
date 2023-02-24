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


    public void AddElement(string[] pathParts, bool changedInThisCommit)
    {
        string currentPathPart = pathParts[0];
        if (pathParts.Length > 1)
        {
            string[] remainingPathParts = pathParts.Skip(1).ToArray();

            if (folderContent.ContainsKey(currentPathPart))
            {
                (folderContent[currentPathPart] as FileStructureFolder).AddElement(remainingPathParts, changedInThisCommit);
                if (changedInThisCommit)
                {
                    folderContent[currentPathPart].Changed = changedInThisCommit;
                }
            }
            else
            {
                IFileStructureElement folder = new FileStructureFolder();
                folder.Name = currentPathPart;
                (folder as FileStructureFolder).AddElement(remainingPathParts, changedInThisCommit);
                folder.Changed = changedInThisCommit;
                folderContent.Add(currentPathPart, folder);
            }
        }
        else
        {
            IFileStructureElement file = new FileStructureFile();
            file.Name = currentPathPart;
            file.Changed = changedInThisCommit;
            folderContent.Add(currentPathPart, file);
        }
    }

    public Transform Draw(Transform parent,float r,Color ringColor,Color ringEndColor,float colorShiftFactor)
    {
        Vector3 pos = parent.position;
        GameObject folerObject = new GameObject(name);
        folerObject.transform.position = pos;
        folerObject.transform.parent = parent;

        int elementsInFolder = folderContent.ToList().Count();

        float angleBetweenElements = (2 * Mathf.PI) / elementsInFolder;
        GameObject connection = new GameObject("Connection");
        connection.transform.parent = parent;
        connection.transform.position = new Vector3(pos.x, pos.y, pos.z);
        connection.AddComponent<LineRenderer>();
        LineRenderer lr = connection.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lr.SetColors(ringColor, ringColor);
        lr.SetWidth(0.1f, 0.1f);
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
                    helixElementObject.transform.parent = folerObject.transform;
                    helixElementObject.transform.position = new Vector3(pos.x + x, pos.y + y, pos.z);
                    /*Instantiate(Main.sFile, helixElementObject.transform);*/
                    (element as FileStructureFolder).Draw(helixElementObject.transform, r / 2, Color.Lerp(ringColor, ringEndColor, colorShiftFactor), ringEndColor, colorShiftFactor);

                }
                else if (element is FileStructureFile)
                {

                    GameObject helixElementObject = new GameObject(element.Name);
                    helixElementObject.transform.parent = folerObject.transform;
                    helixElementObject.transform.position = new Vector3(pos.x + x, pos.y + y, pos.z);
                    Instantiate(Main.sChangedFile, helixElementObject.transform);
                }
            }

        }

        /*float xLast = r * Mathf.Cos(elementsInFolder * angleBetweenElements);
        float yLast = r * Mathf.Sin(elementsInFolder * angleBetweenElements);

        lr.SetPosition(elementsInFolder, new Vector3(pos.x + xLast, pos.y + yLast, pos.z));*/



        return folerObject.transform;
    }
}

