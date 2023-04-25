using UnityEngine;

public class PoolerCast_Testing : MonoBehaviour
{
	public Blocks[] blocks; [System.Serializable] public class Blocks
	{
		public GameObject obj; 
		public float weight;
	}
	public Transform[] casts;

	void Update()
	{
		//Pooling an block at this object with an random rotation
		if(Input.GetKey(KeyCode.Space))
		{
			//Cast object for each of the cast
			for (int c = 0; c < casts.Length; c++) CastObject(casts[c].position);
		}
	}

	void CastObject(Vector2 pos)
	{
		//Get total chance could use
		float chance = 0; for (int b = 0; b < blocks.Length; b++) chance += blocks[b].weight;
		//Randomly choose value inside of chance
		chance = Random.Range(0, chance);
		//Go through all the block
		for (int b = 0; b < blocks.Length; b++)
		{
			//Choose object if it weight take all the chance
			if(chance - blocks[b].weight <= 0)
			{
				//Pool the choosed object at given position and random rotation
				Pooler.i.Create(blocks[b].obj, pos, Quaternion.Euler(0,0,Random.Range(0,360)));
			}
			//Decrease chance with it weight if it failed to choose
			else chance -= blocks[b].weight;
		}
	}
}