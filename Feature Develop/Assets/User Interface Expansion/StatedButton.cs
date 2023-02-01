using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using System;


public class StatedButton : Selectable
{
	//An replicate of the default button on click event
	public Button.ButtonClickedEvent onClick;
	[Tooltip("The event when button state change")] public StateChanging onStateChange = new StateChanging();
	[Tooltip("The event when button toggle change")] public Toggling onToggle = new Toggling();
	[Tooltip("The button's current state")] public state currentState; public bool areToggle;
	[Serializable] public class StateChanging : UnityEvent<state> {}
	[Serializable] public class Toggling : UnityEvent<bool> {}
	state defaultState;
	//All the state of button
	public enum state {Normal, Highlighted, Pressed, Selected, Disabled, Holded, Released}
	
	///When the button state are change
	protected override void DoStateTransition(SelectionState transition, bool instant)
	{
		//Run the Selectable's DoStateTransition function and get the state it transition to
		base.DoStateTransition(transition, instant);
		/// Stop if button are not interactable then update state to disable
		if(!interactable) {UpdateState("Disabled"); return;}
		//Saving the default state when transition
		switch(transition)
		{
			//The default state are now normal when transition to normal
			case SelectionState.Normal: defaultState = state.Normal; break;
			//The default state are now highlighted when transition to highlighted
			case SelectionState.Highlighted: defaultState = state.Highlighted; break;
			//The default state are now selected when transition to selected
			case SelectionState.Selected: defaultState = state.Selected; break;
		}
		//Don't change state if the currently holding
		if(currentState == state.Holded) {return;}
		//If transition to pressed
		if(transition == SelectionState.Pressed)
		{
			///Send the onCLick event
			onClick.Invoke();
			///Update state to pressed*
			UpdateState(transition.ToString());
			//Cycle between toggle
			areToggle = !areToggle;
			//Send the toggle event
			onToggle.Invoke(areToggle);
			//Begin holding it if the button gameobject are still active
			if(gameObject.activeInHierarchy) {Holding(true);}
		}
		//Update the state to be the transition state (except for pressed transition*)
		if(transition != SelectionState.Pressed) {UpdateState(transition.ToString());}
	}
	
	///Begin holding the button base on the bool given
	void Holding(bool isHold)
	{
		//Change state to holded if currently hold and back to default state if stop holding
		if(isHold) {UpdateState("Holded");} else {UpdateState(defaultState.ToString());}
	}

	///When no longer press the button
	public override void OnPointerUp(PointerEventData eventData)
	{
		//Run the Selectable's OnPointerUp function
		base.OnPointerUp(eventData);
		/// Stop if button are not interactable then update state to disable
		if(!interactable) {UpdateState("Disabled"); return;}
		//No longer holding button
		Holding(false);
		//Change the state to release
		UpdateState("Released");
	}

	///Stop holding the button when it got disable
	protected override void OnDisable() {base.OnDisable(); Holding(false);}

	///Updating the button excess state
	void UpdateState(string StateSet)
	{
		//Update the current state to the state has given
		currentState = (state)Enum.Parse(typeof(state), StateSet.ToString());
		//Send an event with the current state
		onStateChange.Invoke(currentState);
	}
}