using UnityEngine;

public class Snake_Testing : MonoBehaviour
{
    Camera cam;
	[SerializeField] float speed;

	void Start()
	{
		cam = Camera.main;
	}

	void Update()
	{
		//Get mouse position
		Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
		//Move toward the mouse with speed
		transform.position = Vector3.MoveTowards(transform.position, mousePos, speed * Time.deltaTime);
		//Look toward mouse only when not right under it
		if(mousePos != (Vector2)transform.position) transform.up = mousePos - (Vector2)transform.position;
	}
}
