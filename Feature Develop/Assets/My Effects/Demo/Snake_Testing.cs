using UnityEngine;

public class Snake_Testing : MonoBehaviour
{
    Camera cam;
	[SerializeField] float speed;
	[SerializeField] SnakeLine snakeLine;
	[SerializeField] SnakeBody snakeBody;

	void Start()
	{
		cam = Camera.main;
	}

	void Update()
	{
		//Get mouse position
		Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
		//Look toward mouse only when not right under it
		if(mousePos != (Vector2)transform.position) transform.up = mousePos - (Vector2)transform.position;
		//Move toward the mouse with speed
		transform.position = Vector3.MoveTowards(transform.position, mousePos, speed * Time.deltaTime);
	
		//@ Grow and shrink snake if any of it method are active
		if(Input.GetKey(KeyCode.U))
		{
			if(snakeLine != null) snakeLine.Grow();
			if(snakeBody != null) snakeBody.Grow();
		}
		if(Input.GetKey(KeyCode.I))
		{
			if(snakeLine != null) snakeLine.Shrink();
			if(snakeBody != null) snakeBody.Shrink();
		}
	}
}