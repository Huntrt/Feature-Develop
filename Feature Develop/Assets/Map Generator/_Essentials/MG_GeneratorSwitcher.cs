using UnityEngine;
using MapGenerator.Boxes;
using MapGenerator.Digger;

namespace MapGenerator
{
public class MG_GeneratorSwitcher : MonoBehaviour
{
	[SerializeField] int cur;
	[SerializeField] GeneratorObjects[] generators;
    [System.Serializable] class GeneratorObjects {public GameObject gui; public GameObject[] objects;}
	[SerializeField] Builder builders; [System.Serializable] class Builder
	{
		public DiggerBuilder_Room digger;
		public BoxesBuilder_Floor bodd;
		public BoxesBuilder_Floor boxes;
	}

	public void Switch(int to)
	{
		//Deactive all the current generator before switch
		ActiveGen(cur, false);
		//How many generator there is to switch
		int amount = generators.Length - 1;
		//Cycle between switching to the last and first gen using int given
		cur += to; if(cur < 0) cur = amount; if(cur > amount) cur = 0;
		//Active all the current generator after switch
		ActiveGen(cur, true);
	}

	void ActiveGen(int gen, bool active)
	{
		//Active or deactive current generator gui
		generators[gen].gui.SetActive(active);
		//Get the object of generator has given
		GameObject[] objs = generators[gen].objects;
		//Active or deactive all the object of given generator
		for (int o = 0; o < objs.Length; o++) objs[o].SetActive(active);
	}

	public void Generate()
	{
		//@ Clear all structure of previous builder
		builders.digger.ClearStructure(false);
		builders.bodd.ClearFloor();
		builders.boxes.ClearFloor();
		//@ Generate using current generator
		switch(cur)
		{
			case 0: builders.digger.BeginDig(); break;
			case 1: builders.bodd.BeginBoxes(); break;
			case 2: builders.boxes.BeginBoxes(); break;
		}
	}

	public void Toggle(bool enable)
	{
		//Get the gui of curren generator
		GameObject gui = generators[cur].gui;
		//Enable gui base on given state
		gui.SetActive(enable);
	}
}
}