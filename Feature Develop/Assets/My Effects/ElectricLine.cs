using UnityEngine;

public class ElectricLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
	[Tooltip("The effect will refresh with every second")]
	public float interval; float intervalCounter;
	[Tooltip("The minimum and maximum range for how big electric will be")]
	public Amplitude amplitude;
	[Tooltip("percent: Next point will space an percentage of total line\n\nShrink Distance: Next point will use total distance if it distance are too big\n\nRaw Distance: Next point will just use distance")]
	public Spacing spacing = new Spacing();
	[HideInInspector] public Vector2 target;
	public event System.Action onDraw;

	[System.Serializable] 
	public class Spacing 
	{
		public enum Mode {percent, shrinkDistance, rawDistance}
		public Mode mode;
		public float min; public float max;
	}
	[System.Serializable] 
	public class Amplitude 
	{
		[Tooltip("Will the point be amplify up down in order")]
		public bool inOrder; 
		public float min; public float max;
	}

	public void Draw()
	{
		//Counting interval counter
		intervalCounter += Time.deltaTime;
		//If interval counter has reach needed amount
		if(intervalCounter >= interval) 
		{
			//Create the line
			CreateLine();
			//Call on draw event after finish draw electric
			onDraw?.Invoke();
			//Reset the interval counter
			intervalCounter -= intervalCounter;
		}
	}

	void CreateLine()
	{
		//Save the line renderer then set reset is position count
		LineRenderer line = lineRenderer; line.positionCount = 1;
		//Print an wanring if line renderer using world space
		if(!line.useWorldSpace) {Debug.LogWarning("It recommend to disable line renderer 'useWorldSpace' ElectricLine.cs");}
		//Set the line first position at this object position
		line.SetPosition(0, transform.position);
		//Begin setting up in between point
		SetupPoints(line);
		//Add the line final position to be target position
		line.SetPosition(line.positionCount-1, target);
	}

	void SetupPoints(LineRenderer line)
	{
		//Set start position as this object position
		Vector2 start = transform.position;
		//Print an warning if start has the same position as target
		if(start == target) {Debug.LogWarning("'start' and 'target' shouldn't be at the same position for electric");}
		//Get the total distance from start to target
		float totalDist = Vector2.Distance(start, target);
		//Get euler angle from of start to taget
		float angle = Mathf.Atan2(target.y - start.y, target.x - start.x) * (180/Mathf.PI);
		//Get direction from start to target 
		Vector2 direction = (target - start).normalized;
		//The amount of distance has occupied
		float occupiedDist = 0;
		//Which point index currently use
		int curPoint = 0;
		//First point is space from start position
		Vector2 spaced = start;
		//While haven't occupied the total distance
		while (occupiedDist <= totalDist)
		{
			//Added an new line position that is current point
			line.positionCount++; curPoint++;
			//Getting distance to space
			float distance = SpacingPoints(totalDist);
			//Spacing away toward direction with distance just get
			spaced += direction * distance;
			//Has occupied the distance
			occupiedDist += distance;
			//Get point position by amplify toward angle with randomize amplitude amount
			Vector2 pointPos = spaced + (AmplifyAngle(angle) * Random.Range(amplitude.min, amplitude.max));
			//Set current line point with it pos
			line.SetPosition(curPoint, pointPos);
		}
	}

	float SpacingPoints(float total)
	{
		//Randomize the distance to be space
		float distance = Random.Range(spacing.min, spacing.max);
		//If spacing by percent
		if(spacing.mode == Spacing.Mode.percent)
		{
			//Return distance as percented of total length
			return (distance / 100) * total;
		}
		//If spacing by shrinking distance
		else if(spacing.mode == Spacing.Mode.shrinkDistance)
		{
			//Get atleast an point of total to be distance if the distance has get are bigger than total
			if(distance >= total) distance = Random.Range(0, total);
			//Return distance has shrink
			return distance;
		}
		//Return raw distance if spacing by raw distance
		else if(spacing.mode == Spacing.Mode.rawDistance) return distance;
		
		return -1;
	}

	int side; Vector2 AmplifyAngle(float angle)
	{
		//The rotation that will apply to angle
		float rot = 0;
		//If amplitude not in order then radomly choosed if rotation will be 90 or -90
		if(!amplitude.inOrder) {rot = 90; if(Random.Range(0,2) == 0) {rot = -90;} ;}
		//If amplitude are in order
		else 
		{
			//Randomly decide first side will be 1 or 2
			if(side == 0) {side = Random.Range(1,3);}
			//Rotation be 90 if side 1 and -90 if side 2 while cycle between side
			if(side == 1) {side = 2; rot = 90;} else {side = 1; rot = -90;}
		}
		//Convert angle that get increase with rotation to radians
		float radians = (angle + rot) * Mathf.Deg2Rad;
		//Return the direction to amplify by apply cos, sin to radians
		return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
	}
}