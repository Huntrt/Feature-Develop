using UnityEngine;

public class VertexCircle_Testing : MonoBehaviour
{
	[SerializeField] LineRenderer line;
	[SerializeField] float size;
	[SerializeField] int vertexCount;

	void Update()
	{
		VertexCircle.Draw(line, vertexCount, size);
	}
}
