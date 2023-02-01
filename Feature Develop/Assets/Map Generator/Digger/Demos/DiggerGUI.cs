using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace MapGenerator.Digger
{
public class DiggerGUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI plotCounter;
	[SerializeField] TMP_InputField limitField;
	[SerializeField] TMP_InputField globalDigChanceField;
	[SerializeField] Toggle directionalPriorityToggle;
	[SerializeField] TMP_InputField[] directionalPriorityFields;
	[SerializeField] TextMeshProUGUI[] miningConstraintTexts;

	[SerializeField] DiggerBuilder_Room builder;
	DiggerConfig config;

	void Start()
	{
		//Set the config to be builder's config
		config = builder.diggerConfig;
		RefreshEdit();
	}

	void RefreshEdit()
	{
		//@ Update the input field to be display it digger config variable
		limitField.text = config.limit.ToString();
		globalDigChanceField.text = config.digChance.global.ToString();
		directionalPriorityToggle.SetIsOnWithoutNotify(config.digChance.useDirectionalPriority);
		directionalPriorityFields[0].text = config.digChance.up.ToString();
		directionalPriorityFields[1].text = config.digChance.down.ToString();
		directionalPriorityFields[2].text = config.digChance.left.ToString();
		directionalPriorityFields[3].text = config.digChance.right.ToString();
		miningConstraintTexts[0].text = config.miningConstraint.minimum.ToString();
		miningConstraintTexts[1].text = config.miningConstraint.maximum.ToString();
	}

	void Update()
	{
		//Display the amount of plot has dig and the amount need to dig
		plotCounter.text = "Plot: " + config.dugs.Count + "/" + config.limit;
	}

	//* Convert input field's text to int (prevent when there is no input)
	float FieldToFloat(TMP_InputField f, bool hundredCap = false) 
	{
		//Convert the field text to float
		float value; float.TryParse(f.text, out value);
		//Cap the float to 0 -> 100 if needed
		if(hundredCap) value = Mathf.Clamp(value, 0, 100);
		//Set the field text to value to prevent incorrect input (dont set while inputing)
		if(f.text != "") f.text = value.ToString();
		//Return the float
		return value;
	}

	public void EditLimitAmount() {config.limit = (int)FieldToFloat(limitField);}

	public void EditGlobalDigChance() {config.digChance.global = FieldToFloat(globalDigChanceField,true);}

	public void EditDirectionalChance(int dir)
	{
		//If direction are -1 then toggle between using directional priority or not
		if(dir == -1) config.digChance.useDirectionalPriority = !config.digChance.useDirectionalPriority;
		//Get the input field for directional chance
		TMP_InputField[] fields = directionalPriorityFields;
		//@ Set the variable of the given direction according to it edit
		switch(dir)
		{
			case 0: config.digChance.up    = FieldToFloat(fields[0], true); break;
			case 1: config.digChance.down  = FieldToFloat(fields[1], true); break;
			case 2: config.digChance.left  = FieldToFloat(fields[2], true); break;
			case 3: config.digChance.right = FieldToFloat(fields[3], true); break;
		}
	}

	//Modify min/max mining constraint to then refrsh them
	public void EditMinimumMiningConstraint(int modify)
	{
		config.miningConstraint.minimum += modify;
		miningConstraintTexts[0].text = config.miningConstraint.minimum.ToString();

	}
	public void EditMaximumMiningConstraint(int modify)
	{
		config.miningConstraint.maximum += modify;
		miningConstraintTexts[1].text = config.miningConstraint.maximum.ToString();
	}
}
} //? Close namespace