using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class MapDisplay : MonoBehaviour
{
	MeshFilter meshFilter;
	MeshRenderer meshRenderer;
	MeshCollider meshCollider;

	private void Awake()
    {
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
		meshCollider = GetComponent<MeshCollider>();
	}

	public void DrawMesh(Mesh mesh, Material[] materials)
	{
		meshFilter.sharedMesh = mesh;
		meshCollider.sharedMesh = mesh;
		meshRenderer.sharedMaterials = materials;

		GameInstance.Instance.MapGenerated();
	}
}
