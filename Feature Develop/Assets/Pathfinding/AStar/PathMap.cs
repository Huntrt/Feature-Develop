using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.AStar
{
public class PathMap : MonoBehaviour
{
	[SerializeField] bool mapGizmos;
	public LayerMask unwalkableLayer;
	public Vector2 mapSize;
	public float nodeSize;
    Node[,] nodes; //? 2 Dimension arry for X and Y axis
	public static PathMap i;

	Vector2Int mapGrid; public float gridSize {get {return mapGrid.x * mapGrid.y;}}

	void Awake()
	{
		//Create singleton
		i = this;
		//Get how many node could fit in the map
		mapGrid.x = Mathf.CeilToInt(mapSize.x / nodeSize);
		mapGrid.y = Mathf.CeilToInt(mapSize.y / nodeSize); 
		//Create grid for map
		CreateGrid();
	}

	void CreateGrid()
	{
		//Create mode for all grid X and Y amount
		nodes = new Node[mapGrid.x, mapGrid.y];
		//Get the bottom left corner of the map
		Vector2 bottomLeft = new Vector2(transform.position.x-mapSize.x/2, transform.position.y-mapSize.y/2);
		//Move bottom left upward half of node size to make sure in centered
		bottomLeft += Vector2.one * (nodeSize/2);

		//Go through all the apth grid x and y
		for (int x = 0; x < mapGrid.x; x++) for (int y = 0; y < mapGrid.y; y++)
		{
			//Go from the left onward in X axis
			float Xpos = bottomLeft.x + (x * nodeSize);
			//Go from the bottom upward in Y axis
			float Ypos = bottomLeft.y + (y * nodeSize);
			//Save X and Y position has get
			Vector2 pos = new Vector2(Xpos, Ypos);
			//Check at this point to see if it there any collider that is unwalkable
			bool walkable = !Physics2D.OverlapCircle(pos, nodeSize/2, unwalkableLayer);
			//Create an new node at this x,y that has determent it position, coordinates, and walkability 
			nodes[x,y] = new Node(new Vector2Int(x,y), pos, walkable);
		}
	}

	public Node GetNodeAtPosition(Vector2 position)
	{
		//Convert coordinates from position given
		Vector2Int coord = new Vector2Int
		(
			Mathf.RoundToInt((position.x + mapSize.x / 2 - (nodeSize/2)) / nodeSize),
			Mathf.RoundToInt((position.y + mapSize.y / 2 - (nodeSize/2)) / nodeSize)
		);
		//Clamp coodrinates inside the map incase of position are out of bounds
		coord.x = Mathf.Clamp(coord.x, 0, mapGrid.x -1);
		coord.y = Mathf.Clamp(coord.y, 0, mapGrid.y -1);

		//Return the node at coordinate finded
		return nodes[coord.x, coord.y];
	}

	public List<Node> GetNeighbors(Node node)
	{
		List<Node> neightbors = new List<Node>();
		//Check all the neighbor in all direction
		for (int x = -1; x <= 1; x++) for (int y = -1; y <= 1; y++)
		{
			//Skip if in the center
			if(x == 0 && y == 0) continue;
			//Check this direction at the given node coordinates
			Vector2Int checks = node.coordinates + new Vector2Int(x,y);
			//If the check does exist and it is inside path's grid
			if(checks.x >= 0 && checks.x < mapGrid.x && checks.y >= 0 && checks.y < mapGrid.y)
			{
				//Add it into neighbor
				neightbors.Add(nodes[checks.x, checks.y]);
			}
		}
		
		return neightbors;
	}

	void OnDrawGizmos()
	{
		//Colors
			Color walkableColor = new Color(Color.white.r, Color.white.g, Color.white.b, 0.3f);
			Color unwalkableColor = new Color(Color.red.r, Color.red.g, Color.red.b, 0.7f);
		//
		
		Gizmos.DrawWireCube(transform.position, mapSize);

		if(nodes != null && mapGizmos)
		{
			foreach (Node node in nodes)
			{
				Gizmos.color = (node.walkable)? walkableColor : unwalkableColor;
				Gizmos.DrawCube(node.position, Vector2.one * (nodeSize - nodeSize/7));
			}
		}
	}
}
}