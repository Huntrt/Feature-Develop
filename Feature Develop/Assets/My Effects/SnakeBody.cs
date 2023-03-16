using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
	public Transform head;
	public GameObject body;
	public int initialSegment;
	[SerializeField] float spacing;
	enum LookAxis {none, up, down, left ,right}
	[SerializeField][Tooltip("The segment behind will look toward the segment infront of it")] LookAxis lookToward;
	public List<Transform> segments = new List<Transform>();
	List<Vector2> segmentPos = new List<Vector2>();

	void OnEnable()
	{
		ResetSegment();
	}

	public void ResetSegment()
	{
		//Destroy all the segemnt except head
		for (int s = 1; s < segments.Count; s++) Destroy(segments[s].gameObject);
		//Renew all the segment list and their position
		segments = new List<Transform>(); segmentPos = new List<Vector2>();
		//Adding the head segment
		segments.Add(head);
		//Add head as the frist segment position
		segmentPos.Add(head.position);
		//Go through how manay segment need to initialy create
		for (int i = 0; i < initialSegment; i++)
		{
			//Use the previous segment pos for this segment
			segmentPos.Add(segmentPos[segmentPos.Count-1]);
			//Create an new segment body
			CreateBodySegment();
		}
	}

	void LateUpdate()
	{
		//Only move if segment pos for head exist
		if(segmentPos.Count > 0) MoveSegment();
	}

	public void MoveSegment()
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
		//Set the frist segment to be the head's position
		if(segments.Count > 0) segments[0] = head.transform;
		//Go through all the segment
		for (int s = 1; s < segments.Count; s++)
		{
			//Lerping this segment to it previous segment using progress of distance
			segments[s].position = Vector2.Lerp(segmentPos[s], segmentPos[s-1], dist/spacing);
			//Get the direction of given index segment look toward segment infront of it
			Vector2 lookDirection = segmentPos[s-1] - segmentPos[s];
			//@ Make given index segment look toward using choosed it axis
			switch((int)lookToward)
			{
				case 1: segments[s].up = lookDirection; break;
				case 2: segments[s].up = -lookDirection; break;
				case 3: segments[s].right = -lookDirection; break;
				case 4: segments[s].right = lookDirection; break;
			}
		}
	}

	public void Grow()
	{
		//Grow another body segment
		CreateBodySegment();
		//Move segment instantly after grow
		MoveSegment();
	}

	public void Shrink()
	{
		//Send an warning when there only head segment left to shrink
		if(segments.Count <= 1) {Debug.LogWarning("Cant shrink snake any further"); return;}
		//Destroy the last body segment
		DestroyBodySegment();
		//Move segment instantly after shrink
		MoveSegment();
	}

	void CreateBodySegment()
	{
		//Create an new body at the last segment position using it own rotation
		GameObject newBody = Instantiate(body, segmentPos[segmentPos.Count-1], body.transform.localRotation);
		//Add the new body to segment
		segments.Add(newBody.transform);
		//Add an position for newly created segment
		segmentPos.Add(segmentPos[segmentPos.Count-1]);
	}

	void DestroyBodySegment()
	{
		//Dont destroy if the last segment no longer exist
		if(segments[segments.Count-1] == null) return;
		//Destroy thr last segment
		Destroy(segments[segments.Count-1].gameObject);
		//Shrink the last segment 
		segments.RemoveAt(segments.Count-1);
		//Remove position of the last segment
		segmentPos.RemoveAt(segmentPos.Count-1);
	}

	void OnDisable()
	{
		//Go through all the segment (except the head) to destroy them
		for (int s = segments.Count - 1; s >= 1 ; s--) DestroyBodySegment();
	}
}
