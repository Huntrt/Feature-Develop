using System.Collections.Generic;
using UnityEngine;

public class WeightRandom_Test : MonoBehaviour
{
    [System.Serializable] public class ColorWeight {public Color color; public float weight, apply;}
	[SerializeField] ColorWeight[] colorTests;
	[SerializeField] SpriteRenderer[] coloringRenders;

	void Start()
	{
		WeightingRenderColor();
	}

	void Update()
	{
		if(Input.GetKey(KeyCode.C)) WeightingRenderColor();
	}

	void WeightingRenderColor()
	{
		//List to save all color weight
		float[] weights = new float[colorTests.Length];
		//The sum of all weight
		float sum = 0;
		//Go through all the color test
		for (int w = 0; w < colorTests.Length; w++)
		{
			//Reset this color applying
			colorTests[w].apply = 0;
			//Save this color weight
			weights[w] = colorTests[w].weight;
			//Increase weight sum
			sum += weights[w];
		}
		//Go through all the color render need to color
		for (int r = 0; r < coloringRenders.Length; r++)
		{
			//This weighted color will be apply 
			colorTests[WeightRandom.WeightingIndex(weights, sum)].apply++;
		}
		//How many times has apply this color
		float applyCount = 0;
		//Which current color being use
		int currentColor = 0;
		//Go through all the render need to color
		for (int r = 0; r < coloringRenders.Length; r++)
		{
			//Set this render color to be current color
			coloringRenders[r].color = colorTests[currentColor].color;
			//Current color has been apply
			applyCount++;
			//If current color are out of apply
			if(applyCount >= colorTests[currentColor].apply)
			{
				//Go to next color
				currentColor++; applyCount = 0;
			}
		}
	}

	[SerializeField] Transform renderGrid;
	[SerializeField] bool GetRender;

	void OnValidate() 
	{
		//Get all sprite render of render grid's children
		if(GetRender) coloringRenders = renderGrid.GetComponentsInChildren<SpriteRenderer>();
	}
}