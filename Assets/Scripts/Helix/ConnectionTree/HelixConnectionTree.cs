using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;

public class HelixConnectionTree : MonoBehaviour
{
	/*
	 * Key: Branchname
	 * Value: Linerenderer for Branch
	 */
	Dictionary<string, LineRenderer> branchLines = new Dictionary<string, LineRenderer>();

	GameObject connectionTree;
    Color color;

	public HelixConnectionTree(string name,Color color){
		connectionTree = new GameObject(name);
        this.color = color;
	}

	public void addPoint(string branchName, Vector3 position, string[] parentShas, Dictionary<string, GameObject> shaObjectDictionary)
	{
		if (!branchLines.ContainsKey(branchName))
		{
			GameObject connection = new GameObject(branchName);
			connection.transform.parent = connectionTree.transform;
            connection.transform.position = position;
            connection.AddComponent<LineRenderer>();
            LineRenderer lr = connection.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            lr.SetColors(color, color);
            lr.SetWidth(0.2f, 0.2f);

			if (parentShas == null)
			{
                lr.positionCount = 1;
                lr.SetPosition(0, position);
			}
			else
			{
                lr.positionCount = 2;
                lr.SetPosition(0, shaObjectDictionary[parentShas[0]].transform.position);
                lr.SetPosition(1, position);
            }

            branchLines.Add(branchName, lr);
        }
		else
		{ 

			if (parentShas !=null && parentShas.Length >= 1)
			{
				foreach (string parentSha in parentShas)
				{
                    GameObject connection = new GameObject(branchName);
                    connection.transform.parent = connectionTree.transform;
                    connection.transform.position = position;
                    connection.AddComponent<LineRenderer>();
                    LineRenderer lr = connection.GetComponent<LineRenderer>();
                    lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
                    lr.SetColors(color,color);
                    lr.SetWidth(0.2f, 0.2f);
                    lr.SetPosition(0, shaObjectDictionary[parentSha].transform.position);
                    lr.SetPosition(1, position);
                }
            }
			else
			{
                LineRenderer lr = branchLines[branchName];
                lr.positionCount = lr.positionCount + 1;
                lr.SetPosition(lr.positionCount - 1, position);
            }
        }
    }
}

