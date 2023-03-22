using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using System;

public class HelixConnectionTree : MonoBehaviour
{
    /*
	 * Key: Branchname
	 * Value: Linerenderer for Branch
	 */
    Dictionary<string, Mesh> branchLines = new Dictionary<string, Mesh>();

    GameObject connectionTree;

    Material material;

    float lineThickness = 0.5f;

    public HelixConnectionTree(string name, Material material)
    {
        connectionTree = new GameObject(name);
        this.material = material;
    }

    public void addPoint(string branchName, Vector3 position, string[] parentShas, Dictionary<string, GameObject> shaObjectDictionary, float uvColorFactor)
    {
        if (!branchLines.ContainsKey(branchName))
        {
            GameObject connection = new GameObject(branchName);
            connection.transform.parent = connectionTree.transform;
            connection.transform.position = new Vector3(0, 0, 0);
            MeshRenderer mr = connection.AddComponent<MeshRenderer>();
            MeshFilter mf = connection.AddComponent<MeshFilter>();
            mr.material = material;

            Mesh instantiatedMesh = Instantiate(InitMesh(position));
            mf.sharedMesh = instantiatedMesh;

            if (parentShas == null)
            {
                AddVertexToMesh(instantiatedMesh, position, uvColorFactor);
            }
            else
            {
                AddVertexToMesh(instantiatedMesh, shaObjectDictionary[parentShas[0]].transform.position, uvColorFactor);
                AddVertexToMesh(instantiatedMesh, position, uvColorFactor);
            }
            branchLines.Add(branchName, instantiatedMesh);
        }
        else
        {
            if (parentShas != null && parentShas.Length >= 1)
            {
                foreach (string parentSha in parentShas)
                {
                    GameObject connection = new GameObject(branchName);
                    connection.transform.parent = connectionTree.transform;
                    connection.transform.position = new Vector3(0, 0, 0);
                    MeshRenderer mr = connection.AddComponent<MeshRenderer>();
                    MeshFilter mf = connection.AddComponent<MeshFilter>();
                    mr.material = material;

                    Mesh instantiatedMesh = Instantiate(InitMesh(position));
                    mf.sharedMesh = instantiatedMesh;

                    AddVertexToMesh(instantiatedMesh, shaObjectDictionary[parentSha].transform.position, uvColorFactor);
                    AddVertexToMesh(instantiatedMesh, position, uvColorFactor);
                }
            }
            else
            {
                Mesh branchMesh = branchLines[branchName];
                AddVertexToMesh(branchMesh, position, uvColorFactor);
            }
        }
    }

    private Mesh InitMesh(Vector3 position)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[1];
        Vector2[] uv = new Vector2[1];
        int[] triangles = new int[0];

        vertices[0] = position;
        uv[0] = new Vector2(0, 0);

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        return mesh;
    }

    private void AddVertexToMesh(Mesh branchMesh, Vector3 position, float uvColorFactor)
    {
        int vertexCount = branchMesh.vertexCount;
        List<Vector3> vertices = new List<Vector3>(branchMesh.vertices);
        List<Vector2> uv = new List<Vector2>(branchMesh.uv);
        List<int> triangles = new List<int>(branchMesh.triangles);

        vertices.Add(position - Vector3.up * lineThickness / 2);
        vertices.Add(position + Vector3.up * lineThickness / 2);
        uv.Add(new Vector2(uvColorFactor, 0f));
        uv.Add(new Vector2(uvColorFactor, 0f));
        triangles.Add(vertexCount - 1);
        triangles.Add(vertexCount);
        triangles.Add(vertexCount + 1);
        triangles.Add(vertexCount - 1);
        triangles.Add(vertexCount + 1);
        triangles.Add(vertexCount);

        triangles.Add(vertexCount - 2);
        triangles.Add(vertexCount - 1);
        triangles.Add(vertexCount);
        triangles.Add(vertexCount - 2);
        triangles.Add(vertexCount);
        triangles.Add(vertexCount - 1);

        branchMesh.vertices = vertices.ToArray();
        branchMesh.uv = uv.ToArray();
        branchMesh.triangles = triangles.ToArray();
    }
}

