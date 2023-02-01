using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Pathfinding.AStar
{
public class Tracer : MonoBehaviour
{
	public Vector2 destination; public System.Action onDestinationReached;
    public float speed;
	public Rotation rotation; [System.Serializable] public class Rotation
	{
		public enum Mode {None, Up, Down, Left, Right} public Mode mode;
		public float speed;
		[HideInInspector] public float progress;
		[HideInInspector] public Vector2 target, origin;
	}
	public TracingConfig tracingConfig; [System.Serializable] public class TracingConfig
	{
		[Tooltip("How much does destination need to move to retrace")] public float sensitivity = 0.2f;
		[Tooltip("The delay between each tracing")] public float repeat = .1f;
		[Tooltip("The distance between unnecessary waypoint for removal")] public float cleanup;
	}
	[HideInInspector] public int targetWaypoint;
	public Vector2[] waypoints;
	public Rigidbody2D rb;
	//? Event call whenever get an new waypoint along with it index and position
	public System.Action<int,Vector2> onNextWaypoint;

	public Preview preview; [System.Serializable] public class Preview
	{
		public bool enable;
		public Color color;
		public float waypointSize, lineSize;
	}

	void Start() 
	{
		UpdateTracing();
	}

	void Update()
	{
		//Rotate toward waypoint if using an rotation mode
		if(rotation.mode != Rotation.Mode.None) RotatingTowardWaypoint();
	}

	Vector2 oldDestPos; void UpdateTracing()
	{
		//Get threshold of destination by multiply sensitivity with itself
		float destThreshold = tracingConfig.sensitivity * tracingConfig.sensitivity;
		//If the destination distance has move away from it old position are bigger then threshold
		if(((Vector2)destination - oldDestPos).sqrMagnitude > destThreshold)
		{
			//Request an new trace from this object to destination while listening to that callback
			TraceRequestor.Requesting(new Request(transform.position, destination, OnTraced));
		}
		//Update the old destination position
		oldDestPos = destination;
		//Reapting update trace 
		Invoke("UpdateTracing", tracingConfig.repeat);
	}

	void OnTraced(Vector2[] tracePath, bool success)
	{
		//If has successfully trace
		if(success) 
		{
			//The waypoints are now the given traced path
			waypoints = tracePath;
			//If needed to cleanup waypoint
			if(tracingConfig.cleanup > 0) CleaningWaypoint();
			//Reseting to follow waypoints
			StopCoroutine("FollowWaypoints"); StartCoroutine("FollowWaypoints");
		}
	}

	void CleaningWaypoint()
	{
		//Final list of waypoint that has been clean
		List<Vector2> cleaned = new List<Vector2>();
		//The next waypoint of current
		Vector2 next;
		//Go through all the waypoints
		for (int w = 0; w < waypoints.Length; w++)
		{
			//Get this waypoint
			Vector2 cur = waypoints[w];
			//Alway add the first and last waypoint
			if(w == 0 || w == waypoints.Length-1) {cleaned.Add(cur); continue;}
			//If the next waypoint still in list then get it if not then complete clean
			if(w+1 < waypoints.Length) next = waypoints[w+1]; else break;
			//If the next waypoint are far enough from current then current has been cleaned
			if(Vector2.Distance(cur, next) > tracingConfig.cleanup) {cleaned.Add(cur);}
		}
		//Cleaning waypoints
		waypoints = cleaned.ToArray();
	}

	IEnumerator FollowWaypoints()
	{
		///First waypoint
		//Start from trace 0 as waypoint
		Vector2 curWaypoint = waypoints[0];
		//Reset the waypoint goal
		targetWaypoint = 0; NextWaypoint();

		while (true)
		{
			//if tracer has reach current waypoint
			if((Vector2)transform.position == curWaypoint)
			{
				//Set goal as next waypoint
				targetWaypoint++; 
				//If has go through all the waypoint then no longer need to follow
				if(targetWaypoint >= waypoints.Length) yield break;
				//Set current waypoint to the goal
				curWaypoint = waypoints[targetWaypoint];
				//Go to the next waypoint
				NextWaypoint();
			}

			///Move from this object toward current way point with speed
			transform.position =Vector2.MoveTowards(transform.position, curWaypoint, speed * Time.deltaTime);
			yield return null;
		}
	}

	void NextWaypoint()
	{
		//If this is the last waypoint
		if(targetWaypoint >= waypoints.Length)
		{
			//Has reached destination
			onDestinationReached?.Invoke(); 
			//Has no waypoint left to go next
			return;
		}
		//If using any rotation mode then refresh them
		if(rotation.mode != Rotation.Mode.None) RefreshWaypointRotation();
		//Has go onto the next waypoint
		onNextWaypoint?.Invoke(targetWaypoint, waypoints[targetWaypoint]);
	}

	void RefreshWaypointRotation()
	{
		//Reset the rotation progress
		rotation.progress = 0;
		//Get the rotation target from waypoint to the tracer 
		rotation.target = (waypoints[targetWaypoint] - (Vector2)transform.position).normalized;
		//If rotation mode are DOWN or LEFT
		if((int)rotation.mode == 2 || (int)rotation.mode == 3) 
		{
			//Flip the target rotation
			rotation.target = -rotation.target;
		}
		//Get the origin base on rotation mode has choosed
		switch((int)rotation.mode)
		{
			case 1: rotation.origin = (Vector2)transform.up; break;
			case 2: rotation.origin = -(Vector2)transform.up; break;
			case 3: rotation.origin = -(Vector2)transform.right; break;
			case 4: rotation.origin = (Vector2)transform.right; break;
		}
	}

	void RotatingTowardWaypoint()
	{
		//Return if has progress through all rotation
		if(rotation.progress >= 1) return;
		//Begin rotation progress
		rotation.progress += rotation.speed * Time.deltaTime;
		//Rotate from origin to target base on progress 
		transform.up = Vector2.Lerp(rotation.origin, rotation.target, rotation.progress);
	}

	void OnDrawGizmos() 
	{
		//Only draw gizmo when there an path and needed
		if(waypoints != null) if(preview.enable)
		{
			for (int t = targetWaypoint; t < waypoints.Length; t++)
			{
				Gizmos.color = preview.color;
				Gizmos.DrawCube(waypoints[t], Vector3.one * preview.waypointSize);

				if(t == targetWaypoint)
				{
					Gizmos.DrawLine(transform.position, waypoints[t]);
				}
				else
				{
					Gizmos.DrawLine(waypoints[t-1], waypoints[t]);
				}
			}
		}
	}
}
}