using UnityEngine;

public class FeatureData : MonoBehaviour
{
	[TextArea(10,100)] public string description;
	public string sceneName;
	public GameObject indicator;

	//Send data of this feature to display when click it button
	public void SendData() {FeatureDataDisplay.i.DisplayData(this);}
}