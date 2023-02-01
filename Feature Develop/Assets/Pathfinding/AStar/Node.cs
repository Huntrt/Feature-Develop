using UnityEngine;

namespace Pathfinding.AStar
{
	public class Node : IHeapItem<Node>
	{
		public Vector2Int coordinates;
		public Vector2 position;
		public bool walkable;
		public Node parent;
		int heapIndex;

		//For assign value when create node
		public Node(Vector2Int coord, Vector2 pos, bool walk)
		{
			coordinates = coord;
			position = pos;
			walkable = walk;
		}

		public int gCost, hCost;
		public int fCost {get {return gCost + hCost;}}
		public int HeapIndex {get{return heapIndex;} set{heapIndex = value;}}

		public int CompareTo(Node compareNode)
		{
			//Compare the f cost of this node to given node
			int compared = fCost.CompareTo(compareNode.fCost);
			//Compare h cost instead if f cost are equal
			if(compared == 0) compared = hCost.CompareTo(compareNode.hCost);
			//Return reverse since node are in reverse order
			return -compared;
		}
	}
}