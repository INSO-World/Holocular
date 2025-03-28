﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HelixConnectionTree : MonoBehaviour
{
    /*
	 * Key: Branchname
	 * Value: Linerenderer for Branch
	 */
    Dictionary<string, Mesh> branchLines = new Dictionary<string, Mesh>();
    Dictionary<string, List<Position>> branchPositions = new Dictionary<string, List<Position>>();

    GameObject connectionTree;

    Material material;

    public HelixConnectionTree(string name, Material material, GameObject parent)
    {
        connectionTree = new GameObject(name);
        connectionTree.transform.parent = parent.transform;
        this.material = material;
    }

    public void addPoint(string branchName, string fullFileName, HelixCommit commit, string[] parentShas, Vector3 offset, float uvColorFactor, float lineThickness, Dictionary<string, HelixCommit> commits)
    {
        if (!branchPositions.ContainsKey(branchName))
        {
            branchPositions.Add(branchName, new List<Position>());
        }
        if (!branchLines.ContainsKey(branchName))
        {
            Mesh instantiatedMesh = CreateConnectionAndInstantateMesh(branchName);

            foreach (string parentSha in parentShas)
            {
                Position position = new Position(commits[parentSha].GetCommitPositionLinear() + offset, commit.GetCommitPositionLinear() + offset, commits[parentSha].GetCommitPositionTime() + offset, commit.GetCommitPositionTime() + offset, lineThickness);
                branchPositions[branchName].Add(position);
                AddVertex(instantiatedMesh, branchName, position, uvColorFactor);
            }

            branchLines.Add(branchName, instantiatedMesh);
        }
        else
        {
            Mesh branchMesh = branchLines[branchName];
            foreach (string parentSha in parentShas)
            {
                Position lastPos;
                if (branchPositions[branchName].Count > 0)
                {
                    lastPos = branchPositions[branchName][branchPositions[branchName].Count - 1];
                }
                else
                {
                    lastPos = new Position(commits[parentSha].GetCommitPositionLinear() + offset, commits[parentSha].GetCommitPositionTime() + offset);
                }

                Position position = new Position(lastPos.positionLinear, commit.GetCommitPositionLinear() + offset, lastPos.positionTime, commit.GetCommitPositionTime() + offset, lineThickness);
                branchPositions[branchName].Add(position);
                AddVertex(branchMesh, branchName, position, uvColorFactor);
            }
        }
    }

    public void addDualPoint(string branchName, string fullFileName, HelixCommit commit, string[] parentShas, Vector3 offset, float line1Thickness, float line2Thickness, Dictionary<string, HelixCommit> commits)
    {
        if (!branchPositions.ContainsKey(branchName))
        {
            branchPositions.Add(branchName, new List<Position>());
        }
        if (!branchLines.ContainsKey(branchName))
        {
            Mesh instantiatedMesh = CreateConnectionAndInstantateMesh(branchName);

            foreach (string parentSha in parentShas)
            {
                Position position = new Position(commits[parentSha].GetCommitPositionLinear() + offset, commit.GetCommitPositionLinear() + offset, commits[parentSha].GetCommitPositionTime() + offset, commit.GetCommitPositionTime() + offset, line1Thickness, line2Thickness);
                branchPositions[branchName].Add(position);
                AddDualVertex(instantiatedMesh, branchName, position);
            }

            branchLines.Add(branchName, instantiatedMesh);
        }
        else
        {
            Mesh branchMesh = branchLines[branchName];


            foreach (string parentSha in parentShas)
            {
                Position lastPos;
                if (branchPositions[branchName].Count > 0)
                {
                    lastPos = branchPositions[branchName][branchPositions[branchName].Count - 1];
                }
                else
                {
                    lastPos = new Position(commits[parentSha].GetCommitPositionLinear() + offset, commits[parentSha].GetCommitPositionTime() + offset);
                }
                Position position = new Position(lastPos.positionLinear, commit.GetCommitPositionLinear() + offset, lastPos.positionTime, commit.GetCommitPositionTime() + offset, line1Thickness, line2Thickness);
                branchPositions[branchName].Add(position);
                AddDualVertex(branchMesh, branchName, position);
            }

        }
    }

    public void UpdateDistances()
    {
        foreach (string branch in branchPositions.Keys)
        {
            updatePositions(branch, branchPositions[branch]);
        }
    }

    private void updatePositions(string branch, List<Position> branchPositions)
    {
        Mesh branchMesh = branchLines[branch];
        List<Vector3> vertices = new List<Vector3>(branchPositions.Count);

        foreach (Position position in branchPositions)
        {
            if (position.dualPosition)
            {
                Vector3 lastPosition;
                Vector3 currPosition;

                if (GlobalSettings.commitPlacementMode)
                {
                    lastPosition = new Vector3(position.lastPositionTime.x, position.lastPositionTime.y, position.lastPositionTime.z * GlobalSettings.commitDistanceMultiplicator);
                    currPosition = new Vector3(position.positionTime.x, position.positionTime.y, position.positionTime.z * GlobalSettings.commitDistanceMultiplicator);
                }
                else
                {
                    lastPosition = new Vector3(position.lastPositionLinear.x, position.lastPositionLinear.y, position.lastPositionLinear.z * GlobalSettings.commitDistanceMultiplicator);
                    currPosition = new Vector3(position.positionLinear.x, position.positionLinear.y, position.positionLinear.z * GlobalSettings.commitDistanceMultiplicator);
                }
                vertices.Add(lastPosition);
                vertices.Add(lastPosition + Vector3.up * position.lineThickness1 * 4);

                vertices.Add(currPosition);
                vertices.Add(currPosition + Vector3.up * position.lineThickness1 * 4);

                vertices.Add(lastPosition);
                vertices.Add(lastPosition + Vector3.up * -position.lineThickness2 * 4);

                vertices.Add(currPosition);
                vertices.Add(currPosition + Vector3.up * -position.lineThickness2 * 4);
            }
            else
            {
                Vector3 lastPosition;
                Vector3 currPosition;

                if (GlobalSettings.commitPlacementMode)
                {
                    lastPosition = new Vector3(position.lastPositionTime.x, position.lastPositionTime.y, position.lastPositionTime.z * GlobalSettings.commitDistanceMultiplicator);
                    currPosition = new Vector3(position.positionTime.x, position.positionTime.y, position.positionTime.z * GlobalSettings.commitDistanceMultiplicator);
                }
                else
                {
                    lastPosition = new Vector3(position.lastPositionLinear.x, position.lastPositionLinear.y, position.lastPositionLinear.z * GlobalSettings.commitDistanceMultiplicator);
                    currPosition = new Vector3(position.positionLinear.x, position.positionLinear.y, position.positionLinear.z * GlobalSettings.commitDistanceMultiplicator);
                }
                vertices.Add(lastPosition);
                vertices.Add(lastPosition + Vector3.up * position.lineThickness1);

                vertices.Add(currPosition);
                vertices.Add(currPosition + Vector3.up * position.lineThickness1);
            }
        }

        branchMesh.vertices = vertices.ToArray();
    }

    private Mesh CreateConnectionAndInstantateMesh(string branchName)
    {
        GameObject connection = new GameObject(branchName);
        connection.transform.parent = connectionTree.transform;
        connection.transform.localPosition = new Vector3(0, 0, 0);
        SkinnedMeshRenderer mr = connection.AddComponent<SkinnedMeshRenderer>();
        mr.material = material;
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mr.localBounds = new Bounds(new Vector3(0, 0, 5000), new Vector3(10000, 10000, 10000));
        Mesh instantiatedMesh = Instantiate(InitMesh());
        mr.sharedMesh = instantiatedMesh;

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

    private void AddDualVertex(Mesh branchMesh, string branchName, Position position)
    {
        Vector3 lastPosition;
        Vector3 currPosition;

        if (GlobalSettings.commitPlacementMode)
        {
            lastPosition = new Vector3(position.lastPositionTime.x, position.lastPositionTime.y, position.lastPositionTime.z * GlobalSettings.commitDistanceMultiplicator);
            currPosition = new Vector3(position.positionTime.x, position.positionTime.y, position.positionTime.z * GlobalSettings.commitDistanceMultiplicator);
        }
        else
        {
            lastPosition = new Vector3(position.lastPositionLinear.x, position.lastPositionLinear.y, position.lastPositionLinear.z * GlobalSettings.commitDistanceMultiplicator);
            currPosition = new Vector3(position.positionLinear.x, position.positionLinear.y, position.positionLinear.z * GlobalSettings.commitDistanceMultiplicator);
        }

        AddVertexToMesh(branchMesh, lastPosition, currPosition, 0.75f, position.lineThickness1 * 4);
        AddVertexToMesh(branchMesh, lastPosition, currPosition, 0.25f, -position.lineThickness2 * 4);
        branchPositions[branchName].Add(position);
    }

    private void AddVertex(Mesh branchMesh, string branchName, Position position, float uvColorFactor)
    {
        Vector3 lastPosition;
        Vector3 currPosition;

        if (GlobalSettings.commitPlacementMode)
        {
            lastPosition = new Vector3(position.lastPositionTime.x, position.lastPositionTime.y, position.lastPositionTime.z * GlobalSettings.commitDistanceMultiplicator);
            currPosition = new Vector3(position.positionTime.x, position.positionTime.y, position.positionTime.z * GlobalSettings.commitDistanceMultiplicator);
        }
        else
        {
            lastPosition = new Vector3(position.lastPositionLinear.x, position.lastPositionLinear.y, position.lastPositionLinear.z * GlobalSettings.commitDistanceMultiplicator);
            currPosition = new Vector3(position.positionLinear.x, position.positionLinear.y, position.positionLinear.z * GlobalSettings.commitDistanceMultiplicator);
        }

        AddVertexToMesh(branchMesh, lastPosition, currPosition, uvColorFactor, position.lineThickness1);
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

class Position
{
    public Vector3 lastPositionLinear;
    public Vector3 positionLinear;
    public Vector3 lastPositionTime;
    public Vector3 positionTime;
    public float lineThickness1;
    public float lineThickness2;
    public bool dualPosition = false;

    public Position(Vector3 lastPositionLinear,
        Vector3 positionLinear,
        Vector3 lastPositionTime,
        Vector3 positionTime,
        float lineThickness1,
        float lineThickness2)
    {
        this.lastPositionLinear = lastPositionLinear;
        this.positionLinear = positionLinear;
        this.lastPositionTime = lastPositionTime;
        this.positionTime = positionTime;
        this.lineThickness1 = lineThickness1;
        this.lineThickness2 = lineThickness2;
        this.dualPosition = true;
    }

    public Position(Vector3 lastPositionLinear,
        Vector3 positionLinear,
        Vector3 lastPositionTime,
        Vector3 positionTime,
        float lineThickness)
    {
        this.lastPositionLinear = lastPositionLinear;
        this.positionLinear = positionLinear;
        this.lastPositionTime = lastPositionTime;
        this.positionTime = positionTime;
        this.lineThickness1 = lineThickness;
        this.dualPosition = false;
    }

    public Position(Vector3 positionLinear, Vector3 positionTime)
    {
        this.lastPositionLinear = positionLinear;
        this.positionLinear = positionLinear;
        this.lastPositionTime = positionTime;
        this.positionTime = positionTime;
        this.lineThickness1 = 0;
        this.lineThickness2 = 0;
        this.dualPosition = true;
    }
}