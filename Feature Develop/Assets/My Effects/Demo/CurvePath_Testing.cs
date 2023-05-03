using UnityEngine;

public class CurvePath_Testing : MonoBehaviour
{
    public Vector2 start, end, curve;
	public float offset;
	[SerializeField] Vector2[] gizmoVertex;

	void Update()
	{
		
	}

	void OnDrawGizmos() 
	{
		//Go through all the vertex needed for gizmo
		for (int v = 0; v < gizmoVertex.Length; v++)
		{
			//Curve this vertex and use it as progress
			gizmoVertex[v] = CurvePath.Curve(start, curve, end, (float)v/(gizmoVertex.Length-1), offset);
			//Green gizmo
			Gizmos.color = Color.green;
			//Dont draw gizmo if this the first vertex
			if(v > 0) Gizmos.DrawLine(gizmoVertex[v], gizmoVertex[v-1]);
		}
	}
}