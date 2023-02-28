using UnityEngine;

public static class VertexCircle
{
    public static void Draw(LineRenderer line, int vertexCount, float size)
	{
		//Split 360 degree with vertices count
		float degSplit = 360/vertexCount;
		//List of all vertices vector in 2d & 3d with an additional vertex
		Vector2[] vertices = new Vector2[vertexCount+1]; Vector3[] vertices3d = new Vector3[vertexCount+1];
		//Go through all the vertices
		for (int v = 0; v < vertexCount; v++)
		{
			//Convert this degree split to radians
			float radians = (degSplit * v) * Mathf.Deg2Rad;
			//Get the position for this vertex using radiant has get toward size given
			vertices[v] = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * size;
			//Save this vertex as 2d
			vertices3d[v] = vertices[v];
		}
		//Connect the last vertex to the first vertex
		vertices[vertexCount] = vertices[0]; vertices3d[vertexCount] = vertices3d[0];
		//Set given line position count
		line.positionCount = vertexCount+1;
		//Set all line position to be vertices
		line.SetPositions(vertices3d);
	}
}