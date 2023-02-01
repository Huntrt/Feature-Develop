using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator.Boxes
{
public class BoxesBuilder_Floor : MonoBehaviour
{
	public BoxesConfig config;

	public Builder builder; [System.Serializable] public class Builder
	{
		public GameObject floor;
		[HideInInspector] public GameObject grouper;
	}

	public void BeginBoxes()
	{
		//Create an empty boxes
		List<PlotData> boxes = new List<PlotData>();
		//@ Set empty boxes as generate boxes or bodd as given
		if(!config.boddInstead) 
		boxes = BoxesAlgorithm.i.GenerateBoxes(config.origin, config.size, config.spacing);
		else
		boxes = BoxesAlgorithm.i.GenerateBodd(config.origin, config.size, config.spacing);
		//Create an new grouper
		builder.grouper = new GameObject(); builder.grouper.name = "Floor Group";
		//Go through all the point of config
		for (int b = 0; b < boxes.Count; b++)
		{
			//Create an floor at this point position
			GameObject floor = Instantiate(builder.floor, boxes[b].position, Quaternion.identity);
			//Gruop the floor up
			floor.transform.SetParent(builder.grouper.transform);
		}
		//Save the boxes into config
		config.boxes = boxes;
	}

	//Destroy the grouper
	public void ClearFloor() {Destroy(builder.grouper);}
}
}
