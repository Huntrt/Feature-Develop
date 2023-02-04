using UnityEngine;

public class ElectricLine_Testing : MonoBehaviour
{
    [SerializeField] ElectricLine electricLine;
	[SerializeField] float moveSpeed;
	[SerializeField] Transform[] chainObjs;
	Vector2[] chainTargets;
	Vector3 inputDirection;
	Camera cam;

	void Start() 
	{
		cam = Camera.main;
	}
	
    void Update()
    {
        if(Input.GetMouseButton(0))
		{
			//Chain target amount is object chain with extra 1
			chainTargets = new Vector2[chainObjs.Length+1];
			//The first chain target are mouse position
			chainTargets[0] = cam.ScreenToWorldPoint(Input.mousePosition);
			//Go through all the electric line target to set each of it to be chain object position
			for (int t = 1; t < chainTargets.Length; t++) chainTargets[t] = (Vector2)chainObjs[t-1].position;
			//Draw line along chain target
			electricLine.Draw(transform.position, chainTargets);
		}
		MoveInput();
    }

	Vector2 velocity; void MoveInput()
	{
		//Set the input horizontal and vertical direction
		inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"),0);
        //Make diagonal movement no longer faster than vertical, horizontal
        velocity = inputDirection.normalized;
        //Add the speed to velocity
        velocity *= moveSpeed;
	}

	void FixedUpdate()
	{
		//Moving the player using velocity has get
		transform.position += (Vector3)(velocity * Time.fixedDeltaTime);
	}
}
