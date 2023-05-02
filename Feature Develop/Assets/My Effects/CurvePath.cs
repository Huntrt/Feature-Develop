using UnityEngine;

public class CurvePath
{
    public static Vector2 Curve(Vector2 start, Vector2 curve, Vector2 end, float progress, float offset)
	{
		//If progress are from start -> curve
		if(progress >= 0 && progress <= 0.5f)
		{
			//Return the vector slerp between start to curve at double of the given progress
			return SlerpBetween(start, curve, progress * 2, offset);
		}
		//If progress are from curve -> end
		if(progress > 0.5f && progress <= 1)
		{
			//Return the vector slerp between curve to end at double of the given progress (-1)
			return SlerpBetween(curve, end, (progress * 2) - 1, offset);
		}
		//Send an error when progress are out of range
		Debug.LogError("Curve path only accept progress between 0 to 1"); return Vector2.zero;
	}

	static Vector2 SlerpBetween(Vector3 start, Vector3 end, float progress, float offset)
	{
		//Get the center between given start and end
		Vector3 center = (start + end) * 0.5f;
		//Make the center Y axist out base on given offset
		center -= new Vector3(0, offset);
		//Get the relative of start to center
		Vector3 startRel = start - center;
		//Get the relative of end to center
		Vector3 endRel = end - center;
		//Return the slerp between start to end at given progress that take into account of center
		return (Vector2)(Vector3.Slerp(startRel, endRel, progress) + center);
	}
}