using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTerrain
{

	public static int GetMaterialIndex(RaycastHit hit)
	{
		Mesh m = hit.collider.gameObject.GetComponent<MeshFilter>().mesh;
		int[] triangle = new int[]
		{
			m.triangles[hit.triangleIndex * 3 + 0],
			m.triangles[hit.triangleIndex * 3 + 1],
			m.triangles[hit.triangleIndex * 3 + 2]
		};
		for (int i = 0; i < m.subMeshCount; ++i)
		{
			int[] triangles = m.GetTriangles(i);
			for (int j = 0; j < triangles.Length; j += 3)
			{
				if (triangles[j + 0] == triangle[0] &&
					triangles[j + 1] == triangle[1] &&
					triangles[j + 2] == triangle[2])
					return i;
			}
		}
		return -1;
	}
}
