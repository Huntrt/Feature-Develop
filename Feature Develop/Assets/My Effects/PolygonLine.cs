using UnityEngine;
using System.Collections.Generic;

public static class PolygonLine
{
    public static void Draw(LineRenderer line, int vertexCount, float size, EdgeCollider2D collider = null)
	{
		//The last vertex to connect
		vertexCount++;
		//List of all vertices vector in 2d & 3d with an additional vertex
		List<Vector2> vertices2 = new List<Vector2>(); Vector3[] vertices3 = new Vector3[vertexCount];
		//Go through all the vertices
		for (int v = 0; v < vertexCount; v++)
		{
			//Convert this split vertex degree to radians
			float radians = Mathf.Deg2Rad * (v * 360 / (vertexCount-1));
			//Get the position for this vertex using radiant has get toward size given
			vertices2.Add(new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * size);
			//Save this vertex as 2d
			vertices3[v] = vertices2[v];
		}
		//Set given line position count
		line.positionCount = vertexCount;
		//Set all line position to be vertices
		line.SetPositions(vertices3);
		//Add vertex to edge collider if it exist
		if(collider != null) collider.SetPoints(vertices2);
	}
}