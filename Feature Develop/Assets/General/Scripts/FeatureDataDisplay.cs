using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class FeatureDataDisplay : MonoBehaviour
{
	[SerializeField] GameObject displayPanel;
	[SerializeField] TextMeshProUGUI featureNameDisplay, featureDescriptionDisplay;
	FeatureData curDisplay; bool hasDisplay;
	//Turn this class into singleton
    public static FeatureDataDisplay i; void Awake() {i = this;}

	public void DisplayData(FeatureData data)
	{
		//Deactive the old indicator if has display
		if(hasDisplay) {curDisplay.indicator.SetActive(false);}
		//Has display then save the given data as current data
		hasDisplay = true; curDisplay = data;
		//Display the name of current data as data's gameobject name
		featureNameDisplay.text = curDisplay.gameObject.name;
		//Display the description of current data
		featureDescriptionDisplay.text = curDisplay.description;
		//Active the current indicator
		curDisplay.indicator.SetActive(true);
		//Active the display panel
		displayPanel.SetActive(true);
	}

	public void CloseDisplay()
	{
		//Dective the current indicator
		curDisplay.indicator.SetActive(false);
		//Dective the display panel
		displayPanel.SetActive(false);
	}

	//Load the current scene ID
	public void LoadFeatureScene() {SceneManager.LoadScene(curDisplay.featureScene.name, LoadSceneMode.Single);}
}
