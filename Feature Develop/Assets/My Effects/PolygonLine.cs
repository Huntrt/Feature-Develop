using UnityEngine;

public static class PolygonLine
{
    public static void Draw(LineRenderer line, int vertexCount, float size)
	{
		//The last vertex to connect
		vertexCount++;
		//List of all vertices vector in 2d & 3d with an additional vertex
		Vector2[] vertices = new Vector2[vertexCount]; Vector3[] vertices3d = new Vector3[vertexCount];
		//Go through all the vertices
		for (int v = 0; v < vertexCount; v++)
		{
			//Convert this split vertex degree to radians
			float radians = Mathf.Deg2Rad * (v * 360 / (vertexCount-1));
			//Get the position for this vertex using radiant has get toward size given
			vertices[v] = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * size;
			//Save this vertex as 2d
			vertices3d[v] = vertices[v];
		}
		//Set given line position count
		line.positionCount = vertexCount;
		//Set all line position to be vertices
		line.SetPositions(vertices3d);
	}
}