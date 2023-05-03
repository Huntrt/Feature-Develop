using UnityEngine;

public class CurvePath_Testing : MonoBehaviour
{
	[SerializeField] Rigidbody2D ship;
	[SerializeField] float speed, stray;
    public Vector2 start, end, curve;
	public float offset;
	[SerializeField] Vector2[] paths;
	int nextPath;
	Vector2 shipDir;

	void Start()
	{
		ship.position = paths[0];
	}

	void FixedUpdate()
	{
		//Make ship move toward current diraction with set speed
		ship.velocity = shipDir * speed;
		//Get the distance from ship current position to it next path
		float dist = Vector2.Distance(ship.position, paths[nextPath]);
		//If distance has stray enough from next path
		if(dist <= stray)
		{
			//Go to the next path
			nextPath++;
			//Go back to the first path if out of path to go next
			if(nextPath > paths.Length-1) {nextPath = 0; ship.position = paths[0];}
			//Set ship direction using it current position toward the naxt path
			shipDir = paths[nextPath] - ship.position;
			//Make ship rotate accordingly to it direction
			ship.transform.up = shipDir;
		}
		//Reset back to the current path if stray away too much from it next path
		if(dist >= stray * 5) ship.position = paths[nextPath-1];
	}
}