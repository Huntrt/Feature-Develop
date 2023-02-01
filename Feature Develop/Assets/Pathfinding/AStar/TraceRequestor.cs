using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;

namespace Pathfinding.AStar
{
public class TraceRequestor : MonoBehaviour
{
	Queue<Result> results = new Queue<Result>();

	[SerializeField] AStar aStar;

	//Make the class accessible for static
	static TraceRequestor i; void Awake() {i = this;}

	void Update()
	{
		//If there still result in queue
		if(results.Count > 0)
		{
			//Lock the thread
			lock(results)
			{
				//Go through all the result in queue
				for (int r = 0; r < results.Count; r++)
				{
					//Take the first result
					Result result = results.Dequeue();
					//Callback the result has take
					result.callback(result.trace, result.success);
				}
			}
		}
	}

    public static void Requesting(Request request)
	{
		//Create an new thresh to find path and listen to when it path are finished
		ThreadStart threadStart = delegate {i.aStar.FindPath(request, i.FinishProcess);};
		//Begin the thread
		threadStart.Invoke();
	}

	public void FinishProcess(Result result)
	{
		//Lock the result thread
		lock(results) {results.Enqueue(result);}
	}
}

public struct Result
{
	public Vector2[] trace;
	public bool success;
	public Action<Vector2[], bool> callback;

	public Result(Vector2[] trace, bool success, Action<Vector2[], bool> callback)
	{
		this.trace = trace;
		this.success = success;
		this.callback = callback;
	}
}

public struct Request
{
	public Vector2 start, end;
	public Action<Vector2[], bool> callback;

	public Request(Vector2 start, Vector2 end, Action<Vector2[], bool> callback)
	{
		this.start = start;
		this.end = end;
		this.callback = callback;
	}
}
}