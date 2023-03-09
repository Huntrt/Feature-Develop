using UnityEngine;

public class MyEffect_Settings : MonoBehaviour
{
    [SerializeField] GameObject[] effectTestingObjects;

	public void OneshotEffect(GameObject effectEnable)
	{
		//Go through the effect testing object
		for (int e = 0; e < effectTestingObjects.Length; e++)
		{
			//Disable each of them
			effectTestingObjects[e].SetActive(false);
		}
		//Active the effect given to
		effectEnable.SetActive(true);
	}
}