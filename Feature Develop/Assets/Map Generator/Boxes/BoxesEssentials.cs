using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator.Boxes
{
	[System.Serializable] public class BoxesConfig
	{
		public bool boddInstead;
		public Vector2 origin, size;
		public float spacing;
		public List<PlotData> boxes;
	}
}