using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class HelixConnectionTree : MonoBehaviour
{
    /*
	 * Key: Branchname
	 * Value: Linerenderer for Branch
	 */
    Dictionary<string, Mesh> branchLines = new Dictionary<string, Mesh>();
    Dictionary<string, List<Vector3>> branchPositions = new Dictionary<string, List<Vector3>>();

    GameObject connectionTree;

    Material material;

    public HelixConnectionTree(string name, Material material)
    {
        connectionTree = new GameObject(name);
        this.material = material;
    }

    public void addPoint(string branchName, Vector3 position, string[] parentShas, Dictionary<string, HelixCommit> commits, float uvColorFactor, float lineThickness)
    {
        if (!branchLines.ContainsKey(branchName))
        {

            Mesh instantiatedMesh = CreateConnectionAndInstantateMesh(branchName, position);

            if (parentShas == null)
            {
                AddVertex(instantiatedMesh, branchName, position, uvColorFactor, lineThickness);
            }
            else
            {
                AddVertex(instantiatedMesh, branchName, commits[parentShas[0]].GetCommitPosition(), uvColorFactor, lineThickness);
                AddVertex(instantiatedMesh, branchName, position, uvColorFactor, lineThickness);
            }
            branchLines.Add(branchName, instantiatedMesh);
        }
        else
        {
            if (parentShas != null && parentShas.Length >= 1)
            {
                foreach (string parentSha in parentShas)
                {
                    Mesh instantiatedMesh = CreateConnectionAndInstantateMesh(branchName + '-' + parentSha, position);

                    AddVertex(instantiatedMesh, branchName + '-' + parentSha, commits[parentSha].GetCommitPosition(), uvColorFactor, lineThickness);
                    AddVertex(instantiatedMesh, branchName + '-' + parentSha, position, uvColorFactor, lineThickness);
                }
            }
            else
            {
                Mesh branchMesh = branchLines[branchName];
                AddVertex(branchMesh, branchName, position, uvColorFactor, lineThickness);
            }
        }
    }

    public void addDualPoint(string branchName, Vector3 position, string[] parentShas, Dictionary<string, HelixCommit> commits, float line1Thickness, float line2Thickness)
    {
        if (!branchLines.ContainsKey(branchName))
        {

            Mesh instantiatedMesh = CreateConnectionAndInstantateMesh(branchName, position);

            if (parentShas == null)
            {
                AddDualVertex(instantiatedMesh, branchName, position, line1Thickness, line2Thickness);
            }
            else
            {
                AddDualVertex(instantiatedMesh, branchName, commits[parentShas[0]].GetCommitPosition(), line1Thickness, line2Thickness);
                AddDualVertex(instantiatedMesh, branchName, position, line1Thickness, line2Thickness);
            }
            branchLines.Add(branchName, instantiatedMesh);
        }
        else
        {
            if (parentShas != null && parentShas.Length >= 1)
            {
                foreach (string parentSha in parentShas)
                {
                    Mesh instantiatedMesh = CreateConnectionAndInstantateMesh(branchName + '-' + parentSha, position);

                    AddDualVertex(instantiatedMesh, branchName + '-' + parentSha, commits[parentSha].GetCommitPosition(), line1Thickness, line2Thickness);
                    AddDualVertex(instantiatedMesh, branchName + '-' + parentSha, position, line1Thickness, line2Thickness);
                }
            }
            else
            {
                Mesh branchMesh = branchLines[branchName];
                AddDualVertex(branchMesh, branchName, position, line1Thickness, line2Thickness);
            }
        }
    }

    private Mesh CreateConnectionAndInstantateMesh(string branchName, Vector3 position)
    {
        GameObject connection = new GameObject(branchName);
        connection.transform.parent = connectionTree.transform;
        connection.transform.position = new Vector3(0, 0, 0);
        MeshRenderer mr = connection.AddComponent<MeshRenderer>();
        MeshFilter mf = connection.AddComponent<MeshFilter>();
        mr.material = material;
        Mesh instantiatedMesh = Instantiate(InitMesh());
        mf.sharedMesh = instantiatedMesh;

        if (!branchPositions.ContainsKey(branchName))
        {
            List<Vector3> positions = new List<Vector3>();
            positions.Add(position);
            branchPositions.Add(branchName, positions);

        }

        return instantiatedMesh;
    }


    private Mesh InitMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[0];
        Vector2[] uv = new Vector2[0];
        int[] triangles = new int[0];
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        return mesh;
    }

    private void AddDualVertex(Mesh branchMesh, string branchName, Vector3 position, float line1Thickness, float line2Thickness)
    {
        Vector3 lasPos = branchPositions[branchName].Last();
        AddVertexToMesh(branchMesh, lasPos, position, 0.75f, line1Thickness * 4);
        AddVertexToMesh(branchMesh, lasPos, position, 0.25f, -line2Thickness * 4);
        branchPositions[branchName].Add(position);
    }

    private void AddVertex(Mesh branchMesh, string branchName, Vector3 position, float uvColorFactor, float lineThickness)
    {
        Vector3 lasPos = branchPositions[branchName].Last();
        AddVertexToMesh(branchMesh, lasPos, position, uvColorFactor, lineThickness);
        branchPositions[branchName].Add(position);
    }

    private void AddVertexToMesh(Mesh branchMesh, Vector3 lastPos, Vector3 position, float uvColorFactor, float lineThickness)
    {
        int vertexCount = branchMesh.vertexCount;
        List<Vector3> vertices = new List<Vector3>(branchMesh.vertices);
        List<Vector2> uv = new List<Vector2>(branchMesh.uv);
        List<int> triangles = new List<int>(branchMesh.triangles);

        vertices.Add(lastPos);
        vertices.Add(lastPos + Vector3.up * lineThickness);

        vertices.Add(position);
        vertices.Add(position + Vector3.up * lineThickness);

        uv.Add(new Vector2(uvColorFactor, 0.75f));
        uv.Add(new Vector2(uvColorFactor, 0.75f));

        uv.Add(new Vector2(uvColorFactor, 0.75f));
        uv.Add(new Vector2(uvColorFactor, 0.75f));

        triangles.Add(vertexCount + 1);
        triangles.Add(vertexCount + 2);
        triangles.Add(vertexCount + 3);
        triangles.Add(vertexCount + 1);
        triangles.Add(vertexCount + 3);
        triangles.Add(vertexCount + 2);

        triangles.Add(vertexCount);
        triangles.Add(vertexCount + 1);
        triangles.Add(vertexCount + 2);
        triangles.Add(vertexCount);
        triangles.Add(vertexCount + 2);
        triangles.Add(vertexCount + 1);

        branchMesh.vertices = vertices.ToArray();
        branchMesh.uv = uv.ToArray();
        branchMesh.triangles = triangles.ToArray();
    }
}

