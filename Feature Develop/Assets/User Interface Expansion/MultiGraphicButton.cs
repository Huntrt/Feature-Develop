using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
#pragma warning disable //Disable OnValidate warning

public class MultiGraphicButton : StatedButton
{
	[Tooltip("All the target need tint")] public List<TargetTint> tints;
	//The target graphic and it color block for tint
	[System.Serializable] public class TargetTint {public Graphic targetGraphic; public ColorBlock color;}

	///When an value got change in the inspector
    void OnValidate()
	{
		//Reset all the target graphic color back to normal color
		Tinting(state.Normal);
	}

	protected override void Start()
	{
		//Run start of StatedButton/Selectable/UIBehaviour
		base.Start();
		//Tinting upon start
		Tinting(currentState);
		//Tint the target when the button change it state
		onStateChange.AddListener(Tinting);
	}

	public void Tinting(state state)
	{
		//Don't update the color if the button state are either holded or release
		if(state == state.Holded || state == state.Released) {return;}
		//Go through all the target graphic need to tint if there is target
		if(tints.Count != 0) for (int t = 0; t < tints.Count; t++)
		{
			//Get the current tint
			TargetTint tint = tints[t];
			//Don't tint the color if there is no target graphic
			if(tint.targetGraphic == null) {continue;}
			//The color graphic target will tint to
			Color nextColor = Color.black;
			//Set the next color base on the current button state
			switch(state)
			{
				case state.Normal: nextColor = tint.color.normalColor; break;
				case state.Highlighted: nextColor = tint.color.highlightedColor; break;
				case state.Pressed: nextColor = tint.color.pressedColor; break;
				case state.Selected: nextColor = tint.color.selectedColor; break;
				case state.Disabled: nextColor = tint.color.disabledColor; break;
			}
			//Multiplying the next color with color multiplier
			nextColor *= tint.color.colorMultiplier;
			//Fade the current target graphic color into the next color with fade duration 
			tint.targetGraphic.CrossFadeColor(nextColor, tint.color.fadeDuration, true, true);
		}
	}
}