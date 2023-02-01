using UnityEngine;
using TMPro;

namespace MapGenerator.Boxes
{
public class BoxesGUI : MonoBehaviour
{
	public TextMeshProUGUI plotCounter;
	public TMP_InputField xSizeField, ySizeField;
	public TMP_InputField spacingField;

	[SerializeField] BoxesBuilder_Floor builder;
	BoxesConfig config;

	void Start()
	{
		//Get the both config of builder
		config = builder.config;
		RefreshEdit();
	}

	void RefreshEdit()
	{
		//@ Update the input field to be display it boxes config variable
		xSizeField.text = config.size.x.ToString();
		ySizeField.text = config.size.y.ToString();
		spacingField.text = config.spacing.ToString();
	}

	void Update()
	{
		//Display the point amount from config
		plotCounter.text = "Plot: " + config.boxes.Count;
	}	//* Convert input field's text to int (prevent when there is no input)
	
	float FieldToFloat(TMP_InputField f, bool correct = true) 
	{
		//Convert the field text to decimal
		float value; float.TryParse(f.text, out value);
		//Set the field text to value to prevent incorrect input (dont set while inputing or not needed)
		if(correct) if(f.text != "") f.text = value.ToString();
		//Return the decimal
		return value;
	}

	public void EditXSize() {config.size.x = (int)FieldToFloat(xSizeField);}
	public void EditYSize() {config.size.y = (int)FieldToFloat(ySizeField);}
	public void EditSpacing() {config.spacing = (float)FieldToFloat(spacingField, false);}
}
}