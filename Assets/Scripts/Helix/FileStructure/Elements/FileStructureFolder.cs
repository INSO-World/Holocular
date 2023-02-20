using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class FileStructureFolder : MonoBehaviour, IFileStructureElement
{
    private string name = "";
    Dictionary<string, IFileStructureElement> folderContent = new Dictionary<string, IFileStructureElement>();

    public FileStructureFolder()
	{
	}

    string IFileStructureElement.Name { get { return name; } set { name = value; } }

    public void AddElement(string[] pathParts, bool changedInThisCommit)
    {
        string currentPathPart = pathParts[0];
        if (pathParts.Length > 1)
        {
            string[] remainingPathParts = pathParts.Skip(1).ToArray();

            if (folderContent.ContainsKey(currentPathPart))
            {
                (folderContent[currentPathPart] as FileStructureFolder).AddElement(remainingPathParts, changedInThisCommit);
            }
            else
            {
                IFileStructureElement folder = new FileStructureFolder();
                folder.Name = currentPathPart;
                (folder as FileStructureFolder).AddElement(remainingPathParts, changedInThisCommit);
                folderContent.Add(currentPathPart, folder);
            }
        }
        else
        {
            IFileStructureElement file = new FileStructureFile();
            file.Name = currentPathPart;
            (file as FileStructureFile).changed = changedInThisCommit;
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

        for (int i = 0; i < elementsInFolder; i++)
        {
            float x = r * Mathf.Cos(i * angleBetweenElements);
            float y = r * Mathf.Sin(i * angleBetweenElements);

            if (elementsInFolder > 1)
            {
                float nextX = r * Mathf.Cos((i + 1) * angleBetweenElements);
                float nextY = r * Mathf.Sin((i + 1) * angleBetweenElements);

                GameObject connection = new GameObject("Connection" + i);
                connection.transform.parent = parent;
                connection.transform.position = new Vector3(pos.x + x, pos.y + y, pos.z);
                connection.AddComponent<LineRenderer>();
                LineRenderer lr = connection.GetComponent<LineRenderer>();
                lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
                lr.SetColors(ringColor, ringColor);
                lr.SetWidth(0.1f, 0.1f);
                lr.SetPosition(0, new Vector3(pos.x + x, pos.y + y, pos.z));
                lr.SetPosition(1, new Vector3(pos.x + nextX, pos.y + nextY, pos.z));

            }
            IFileStructureElement element = folderContent.Values.ElementAt(i);

            if (element is FileStructureFolder)
            {
                GameObject helixElementObject = new GameObject(name);
                helixElementObject.transform.parent = folerObject.transform;
                helixElementObject.transform.position = new Vector3(pos.x + x, pos.y + y, pos.z);
                /*Instantiate(Main.sFile, helixElementObject.transform);*/
                (element as FileStructureFolder).Draw(helixElementObject.transform, r / 2,Color.Lerp(ringColor, ringEndColor, colorShiftFactor),ringEndColor,colorShiftFactor);
            }
            else if(element is FileStructureFile)
            {
                if((element as FileStructureFile).changed)
                {
                    GameObject helixElementObject = new GameObject(name);
                    helixElementObject.transform.parent = folerObject.transform;
                    helixElementObject.transform.position = new Vector3(pos.x + x, pos.y + y, pos.z);
                    Instantiate(Main.sChangedFile, helixElementObject.transform);
                }
            }

        }

        return folerObject.transform;
    }
}

