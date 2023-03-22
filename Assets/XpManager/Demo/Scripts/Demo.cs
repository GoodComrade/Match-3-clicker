using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour 
{
	// Demo fields
	// ==============================================
	public InputField		FieldInputGraphName;
	public Slider			FieldLevelSlider;
	public Text				FieldLevel;
	public Text				FieldXpValueResult;
	// ==============================================

	// Set HERE the script to get data info.
	public XpManager		XpScript;
	
	
	public void OnButton() 
	{

		// ---------------------------------------------------------

		// Set the Graph Name (e.g: "Life", "Strenght", "Defense", "Armor", "Mana", etc)
		string graphName = FieldInputGraphName.text;

		// Set the current level.
		int currentLevel = (int) FieldLevelSlider.value;

		// Get XP Value.
		float xpValue = XpScript.GetXPValue( graphName, currentLevel );

		// ---------------------------------------------------------
		

		// --------------------------------------------------------- Demo only
		Feedback( xpValue );
		// --------------------------------------------------------- Demo only

	}

	// Debug feedback (visual only)
	void Feedback(float _value)
	{
		if(_value == -1)
			FieldXpValueResult.text = "GraphName is invalid!";
		else if(_value == -2)
			FieldXpValueResult.text = "This Level is not available on this Graph";
		else
			FieldXpValueResult.text = _value.ToString();
	}

	// Change Slider values.
	public void OnSliderChange (float _value) 
	{
		FieldLevel.text = "Level " + _value;
	}
}
