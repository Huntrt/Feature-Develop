using UnityEngine;

public class PolygonLine_Testing : MonoBehaviour
{
	[SerializeField] LineRenderer line;
	[SerializeField] float size;
	[SerializeField] int vertexCount;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.UpArrow)) vertexCount++;
		if(Input.GetKeyDown(KeyCode.DownArrow)) vertexCount--;
		PolygonLine.Draw(line, vertexCount, size);
	}
}
