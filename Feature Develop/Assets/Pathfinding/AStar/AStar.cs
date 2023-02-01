using System.Collections.Generic;
using UnityEngine;
using System;

namespace Pathfinding.AStar
{
public class AStar : MonoBehaviour
{
	[SerializeField] PathMap map;
	
    public void FindPath(Request request, Action<Result> result)
	{
		Vector2[] waypoints = new Vector2[0];
		bool findSuccess = false;

		//@ Get the start and target node at the requested position
		Node startNode = map.GetNodeAtPosition(request.start);
		Node targetNode = map.GetNodeAtPosition(request.end);

		///Only need to find path if both start and target are walkable
		if(startNode.walkable && targetNode.walkable)
		{
			//Create an heap for open set
			Heap<Node> OPEN = new Heap<Node>(Mathf.RoundToInt(map.gridSize));
			//Create an hash set to find in closed node
			HashSet<Node> CLOSED = new HashSet<Node>();

			OPEN.Add(startNode);

			//When there still open node
			while (OPEN.Count > 0)
			{
				//Get the current node from open heap then closed it
				Node curNode = OPEN.RemoveFirst(); CLOSED.Add(curNode);

				///Has find success if curret node has reached the target node
				if(curNode == targetNode) {findSuccess = true; break;}

				//Go through all the neighbor of current node
				foreach (Node neighbor in map.GetNeighbors(curNode))
				{
					//Skip if the neighbor node is not walkable or it is still closed
					if(!neighbor.walkable || CLOSED.Contains(neighbor)) continue;
					//Get the cost from the current node to neighbor
					int costToNeighbor = curNode.gCost + GetDistance(curNode, neighbor);
					//If the cost to neighhbor g cost are lower than this neighbor or neighbor not open
					if(costToNeighbor < neighbor.gCost || !OPEN.Contains(neighbor))
					{
						//Cost to neighbor are now this neighbor g cost
						neighbor.gCost = costToNeighbor;
						//This neighbor h cost are now the distance from itself with target
						neighbor.hCost = GetDistance(neighbor, targetNode);
						//Set current node as this neighbor parent
						neighbor.parent = curNode;
						//Open this neighbor if haven't or else update it
						if(!OPEN.Contains(neighbor)) OPEN.Add(neighbor); else OPEN.UpdateItems(neighbor);
					}
				}
			}
		}
		//If successfully find an path
		if(findSuccess) 
		{
			//get waypoints by rertrace path from start to target
			waypoints = RetracePath(startNode, targetNode);
			//Only successfully find if there is waypoint
			findSuccess = waypoints.Length > 0;
		}
		//Call the result with waypoints has found
		result(new Result(waypoints, findSuccess, request.callback));
	}

	Vector2[] RetracePath(Node startNode, Node endNode)
	{
		//List of node will be trace back
		List<Node> trace = new List<Node>();
		//Start from the end node
		Node curNode = endNode;
		//When need to match current to start node
		while (curNode != startNode)
		{
			//Traced current node
			trace.Add(curNode);
			//Current node are now it parent
			curNode = curNode.parent;
		}
		//Get waypoint from simplify path
		Vector2[] waypoints = SimplifyPath(trace);
		//Reverse the waypoint then return them
		Array.Reverse(waypoints); return waypoints;
	}

	Vector2[] SimplifyPath(List<Node> path)
	{
		List<Vector2> waypoints = new List<Vector2>();
		Vector2 oldDir = Vector2.zero;

		//Go through all the path
		for (int i = 1; i < path.Count; i++)
		{
			//New direction from coordinates of the preivous path to this path
			Vector2 newDir = path[i-1].coordinates - path[i].coordinates;
			//If new direction are change from the old one then add it into waypoint
			if(newDir != oldDir) waypoints.Add(path[i].position);
			oldDir = newDir;
		}
		return waypoints.ToArray();
	}

	int GetDistance(Node origin, Node target)
	{
		//Get the distance from origin to target
		Vector2Int dist = new Vector2Int
		(
			//Make sure x and y are alway positive
			Mathf.Abs(origin.coordinates.x - target.coordinates.x),
			Mathf.Abs(origin.coordinates.y - target.coordinates.y)
		);

		//@ Return 14 cost for each diagonal with 10 cost of left over each vertical/horizontal as distance
		if(dist.x > dist.y) 
			return 14 * dist.y + 10 * (dist.x - dist.y);
		else
			return 14 * dist.x + 10 * (dist.y - dist.x);
	}
}
}