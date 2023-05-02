using UnityEngine;

public class CurvePath_Testing : MonoBehaviour
{
    public Vector2 start, end, curve;
	public float offset;
	[SerializeField] LineRenderer line;

	void Update()
	{
		for (int l = 0; l < line.positionCount; l++)
		{
			//Set curve for this line position with preset info
			line.SetPosition(l, CurvePath.Curve(start, curve, end, (float)l/(line.positionCount-1), offset));
		}
	}
}