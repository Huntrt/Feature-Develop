using System.Collections.Generic;
using UnityEngine;
using System;

namespace MapGenerator
{
	[Serializable] public class PlotData
	{
		public int index = -1;
		public Vector2 coordinate;
		public Vector2 position;
		public bool empty = true;
	}

	[Serializable] public class GenerateStatus
	{
		[HideInInspector] public bool isGenerating; 
		public Action generateComplete;
	}
	
	public static class MapFind
	{
		public static PlotData PlotAtCoordinate(List<PlotData> digList, Vector2 coordinate)
		{
			//Go through all the dig in given list
			for (int d = 0; d < digList.Count; d++)
			{
				//Return the dug that has the same coordinate as given coordinate
				if(digList[d].coordinate == coordinate) return digList[d];
			}
			return null;
		}
		public static PlotData PlotAtPosition(List<PlotData> digList, Vector2 position)
		{
			//Go through all the dig in given list
			for (int d = 0; d < digList.Count; d++)
			{
				//Return the dug that has the same position as given position
				if(digList[d].position == position) return digList[d];
			}
			return null;
		}
	}

	public static class MapCreate
	{
		public static PlotData Plot(int index, Vector2 coord, Vector2 pos)
		{
			//@ Create an empty plot and set given data to it
			PlotData plot = new PlotData();
			plot.index = index;
			plot.coordinate = coord;
			plot.position = pos;
			//Return the new plot created
			return plot;
		}
	}
}
