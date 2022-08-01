using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static Mesh CreateMeshFromCells(ref Array2D<Cell> cells, int cellTypesNb, float unitSize = 1.0f)
    {
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        List<Vector3> newVertices = new List<Vector3>();
        List<Vector2> newUVs = new List<Vector2>();

        mesh.subMeshCount = cellTypesNb;
        List<int>[] triangles = new List<int>[cellTypesNb];
        for (int i = 0; i < cellTypesNb; i++)
            triangles[i] = new List<int>();

        int rows = cells.GetWidth();
        int columns = cells.GetHeight();

        for (int i = 0; i <= rows; i++)
        {
            for (int j = 0; j <= columns; j++)
            {
                if (!cells.TryGet(i, j, out Cell cell))
                    continue;

                AddQuad(Matrix4x4.TRS(
                    new Vector3(i * unitSize, cell.height, j * unitSize),
                    Quaternion.LookRotation(Vector3.up),
                    new Vector3(unitSize, unitSize, 1)
                    ), ref newVertices, ref newUVs, ref triangles[cell.id]);

                if (i - 1 < 0) { }
                else if (cells.TryGet(i - 1, j, out Cell outCell) && outCell.height < cell.height)
                {
                    float height = Mathf.Abs(cell.height - outCell.height);
                    AddQuad(Matrix4x4.TRS(
                    new Vector3((i - .5f) * unitSize, cell.height - height * 0.5f, j * unitSize),
                    Quaternion.LookRotation(Vector3.left),
                    new Vector3(unitSize, height, 1)
                ), ref newVertices, ref newUVs, ref triangles[cell.id]);
                }

                if (j + 1 > columns) { }
                else if (cells.TryGet(i, j + 1, out Cell outCell) && outCell.height < cell.height)
                {
                    float height = Mathf.Abs(cell.height - outCell.height);
                    AddQuad(Matrix4x4.TRS(
                    new Vector3(i * unitSize, cell.height - height * 0.5f, (j + .5f) * unitSize),
                    Quaternion.LookRotation(Vector3.forward),
                    new Vector3(unitSize, height, 1)
                ), ref newVertices, ref newUVs, ref triangles[cell.id]);
                }

                if (j - 1 < 0) { }
                else if (cells.TryGet(i, j - 1, out Cell outCell) && outCell.height < cell.height)
                {
                    float height = Mathf.Abs(cell.height - outCell.height);
                    AddQuad(Matrix4x4.TRS(
                    new Vector3(i * unitSize, cell.height - height * 0.5f, (j - .5f) * unitSize),
                    Quaternion.LookRotation(Vector3.back),
                    new Vector3(unitSize, height, 1)
                ), ref newVertices, ref newUVs, ref triangles[cell.id]);
                }

                if (i + 1 > rows) { }
                else if (cells.TryGet(i + 1, j, out Cell outCell) && outCell.height < cell.height)
                {
                    float height = Mathf.Abs(cell.height - outCell.height);
                    AddQuad(Matrix4x4.TRS(
                    new Vector3((i + .5f) * unitSize, cell.height - height * 0.5f, j * unitSize),
                    Quaternion.LookRotation(Vector3.right),
                    new Vector3(unitSize, height, 1)
                ), ref newVertices, ref newUVs, ref triangles[cell.id]);
                }
            }
        }

        mesh.vertices = newVertices.ToArray();
        mesh.uv = newUVs.ToArray();

        for (int i = 0; i < cellTypesNb; i++)
        {
            mesh.SetTriangles(triangles[i].ToArray(), i);
        }

        mesh.RecalculateNormals();

        return mesh;
    }

private static void AddQuad(Matrix4x4 matrix, ref List<Vector3> newVertices, ref List<Vector2> newUVs, ref List<int> newTriangles)
{
    int index = newVertices.Count;

    // corners before transforming
    Vector3 vert1 = new Vector3(-.5f, -.5f, 0);
    Vector3 vert2 = new Vector3(-.5f, .5f, 0);
    Vector3 vert3 = new Vector3(.5f, .5f, 0);
    Vector3 vert4 = new Vector3(.5f, -.5f, 0);

    newVertices.Add(matrix.MultiplyPoint3x4(vert1));
    newVertices.Add(matrix.MultiplyPoint3x4(vert2));
    newVertices.Add(matrix.MultiplyPoint3x4(vert3));
    newVertices.Add(matrix.MultiplyPoint3x4(vert4));

    newUVs.Add(new Vector2(1, 0));
    newUVs.Add(new Vector2(1, 1));
    newUVs.Add(new Vector2(0, 1));
    newUVs.Add(new Vector2(0, 0));

    newTriangles.Add(index + 2);
    newTriangles.Add(index + 1);
    newTriangles.Add(index);

    newTriangles.Add(index + 3);
    newTriangles.Add(index + 2);
    newTriangles.Add(index);
}
}
