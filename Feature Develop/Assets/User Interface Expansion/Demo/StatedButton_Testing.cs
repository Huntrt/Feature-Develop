using UnityEngine;
using TMPro;

public class StatedButton_Testing : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stateDisplay;

	public void StateDisplay(StatedButton.state stated)
	{
		stateDisplay.text = stated.ToString();
	}
}