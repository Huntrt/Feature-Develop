using System.Collections.Generic;
using UnityEngine;

public class SnakeLine : MonoBehaviour
{
	public Transform head;
	public LineRenderer line;
	public int initialSegment;
	[SerializeField] float spacing;
	public List<Vector2> segments = new List<Vector2>();
	[SerializeField] List<Vector2> segmentPos = new List<Vector2>();
	[SerializeField] EdgeCollider2D collisionCollider, triggerCollider;

	void OnEnable()
	{
		ResetPart();
	}

	public void ResetPart()
	{
		//Renew all the segment
		segments = new List<Vector2>(); segmentPos = new List<Vector2>();
		//Reset collider point
		SetColliderPoint();
		//Add head as the frist segment position
		segmentPos.Add(head.position);
		//Initilize all the segment needed
		for (int i = 0; i < initialSegment; i++)
		{
			segments.Add(Vector2.zero);
			segmentPos.Add(Vector2.zero);
		}
		//Set line position for each segment
		line.positionCount = segments.Count;
	}

	void LateUpdate()
	{
		//Only draw if segment pos for head exist
		if(segmentPos.Count > 0) DrawLine();
	}

	public void DrawLine()
	{
		//Get distance between the head and the first segment
		float dist = Vector2.Distance((Vector2)head.position, segmentPos[0]);
		//While distance still bigger than spacing
		while (dist > spacing)
		{
			//Get the direction from the first segment to head
			Vector2 dir = ((Vector2)head.position - segmentPos[0]).normalized;
			//Insert an position from the first segment spacing toward direction
			segmentPos.Insert(0, segmentPos[0] + dir * spacing);
			//Remove the last segment position
			segmentPos.RemoveAt(segmentPos.Count - 1);
			//Has spacing from distance
			dist -= spacing;
		}
		//Array for segment 3d position
		Vector3[] segments3d = new Vector3[segments.Count];
		//Set the frist segment to be the head's position
		if(segments.Count > 0) {segments[0] = head.position; segments3d[0] = head.position;}
		//Go through all the segment
		for (int b = 1; b < segments.Count; b++)
		{
			//Lerping this segment to it previous segment using progress of distance
			segments[b] = Vector2.Lerp(segmentPos[b], segmentPos[b-1], dist/spacing);
			//Save this segment in 3d position
			segments3d[b] = segments[b];
		}
		//Apply all the segment position to line
		line.SetPositions(segments3d);
		//Set collider point
		SetColliderPoint();
	}

	public void Grow()
	{
		//Grow another part 
		segments.Add(segmentPos[segmentPos.Count-1]);
		segmentPos.Add(segmentPos[segmentPos.Count-1]);
		//Set line position for each segment
		line.positionCount = segments.Count;
		//Draw segment instantly after grow
		DrawLine();
	}

	void SetColliderPoint()
	{
		//Set collision collider point if needed
		if(collisionCollider != null) collisionCollider.SetPoints(segments);
		//Set trigger collider point if needed
		if(triggerCollider != null) triggerCollider.SetPoints(segments);
	}
}
