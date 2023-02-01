using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator.Boxes
{
public class BoxesAlgorithm : MonoBehaviour
{
	//Set this class to singleton
	public static BoxesAlgorithm i {get{if(_i==null){_i = GameObject.FindObjectOfType<BoxesAlgorithm>();}return _i;}} static BoxesAlgorithm _i;

    public List<PlotData> GenerateBoxes(Vector2 origin, Vector2 size, float spacing)
	{
		//List of plots gonne be return
		List<PlotData> plots = new List<PlotData>();
		//Go through all the x and negative y
		for (int x = 0; x < size.x; x++) for (int y = 0; y < size.y; y++)
		{
			//Use this x,y as coordinate
			Vector2 coord = new Vector2(x,-y);
			//Multiply coordinate at origin with spacing for position
			Vector2 pos = (origin + coord) * spacing;
			//Create plot at coordinate and position then add into list
			plots.Add(MapCreate.Plot(plots.Count, coord, pos));
		}
		//Return all the plot has create
		return plots;
	}

    public List<PlotData> GenerateBodd(Vector2 origin, Vector2 halfSize, float spacing)
	{
		//List of plots gonne be return
		List<PlotData> plots = new List<PlotData>();
		//Go from the negative to the final x axis
		for (int x = (int)-halfSize.x; x < halfSize.x+1; x++) 
		//Go from the negative to the final y axis
		for (int y = (int)-halfSize.y; y < halfSize.y+1; y++)
		{
			//Use this x,y as coordinate
			Vector2 coord = new Vector2(x,y);
			//Multiply coordinate at origin with spacing for position
			Vector2 pos = (origin + coord) * spacing;
			//Create plot at coordinate and position then add into list
			plots.Add(MapCreate.Plot(plots.Count, coord, pos));
		}
		//Return all the plot has create
		return plots;
	}
}
}