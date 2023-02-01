using UnityEngine;

public class TopDownControl : MonoBehaviour
{ 
	public float speed;
	public Vector2 input;
	public Rigidbody2D rb;
	[SerializeField] Camera cameraFollow;
	
    void Update()
    {
		//Running function
		MoveInput();
	}

	Vector2 direction; void MoveInput()
	{
		//Using the horizontal and vertical for movement input
		input = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
		#region Optional keycode
		//@ Using keycode for movement input
		// if(Input.GetKey(KeyManager.i.Up)) {inputDirection.y = 1;}
		// if(Input.GetKey(KeyManager.i.Down)) {inputDirection.y = -1;}
		// if(Input.GetKey(KeyManager.i.Left)) {inputDirection.x = -1;}
		// if(Input.GetKey(KeyManager.i.Right)) {inputDirection.x = 1;}
		#endregion
        //Get direction by moving in input at speed
        direction = input.normalized * speed;
	}


	void FixedUpdate()
	{
		//Moving the player toward the velocity has get
		rb.MovePosition(rb.position + direction * Time.fixedDeltaTime);
	}

	void LateUpdate()
	{
		//Make the camera follow player if there is camera assign
		if(cameraFollow != null) {cameraFollow.transform.position = new Vector3(transform.position.x, transform.position.y,-10);}
	}
}
