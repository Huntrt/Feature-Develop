using UnityEngine;

public class WeightRandom : MonoBehaviour
{
	public static int WeightingIndex(float[] weights, float sum = -1)
	{
		//Get the total sum of given weight list if there no sum given
		if(sum < 0) {sum = 0; foreach (float w in weights) sum += w;}
		//Randomize the sum of weight
		sum = Random.Range(0, sum);
		//Go through all the weight of given list
		for (int i = 0; i < weights.Length; i++)
		{
			//If this weight take all the sum left
			if(sum - weights[i] <= 0)
			{
				//Return this weight index
				return i;
			}
			//Sum lose this weight if there still sum left
			else sum -= weights[i];
		}
		//Send an error if somehow weight null
		Debug.LogError("Weight randomize should not output null"); return -1;
	}
}