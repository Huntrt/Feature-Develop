using Pathfinding.AStar;
using UnityEngine;

public class AStar_Testing : MonoBehaviour
{
	Tracer[] tracers;

	void Start()
	{
		//Get all the object with tracer component 
		tracers = GameObject.FindObjectsOfType<Tracer>();
	}

    void Update()
	{
		if(Input.GetMouseButton(0)) for (int t = 0; t < tracers.Length; t++)
		{
			//Set all the tracer destination to be mouse position
			tracers[t].destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
	}
}
