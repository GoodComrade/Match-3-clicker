using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPManagerCharacter : MonoBehaviour 
{

	// ...Your custom data here
	public int				Level = 1;
	public int				CurrentXP = 0;
	public int				Life = 100;
	public float			Power = 10;
	public float			Defense = 10;

	// ...Get xp curves component
	public XpManager		XPComponent;


	private void Start()
	{
		RefreshXP ();
	}

	// Collect XP ingame
	void OnTriggerEnter (Collider _hit)
	{		
		// Add +10 experience
		CurrentXP += 10;

		// Call method to refresh current values.
		RefreshXP();

		Destroy(_hit.gameObject);	
	}

	void RefreshXP ()
	{
		CheckLevel();

		// Refresh values.
		Life =		(int) XPComponent.GetXPValue("Life", Level);
		Power =		(int) XPComponent.GetXPValue("Power", Level);
		Defense =	(int) XPComponent.GetXPValue("Defense", Level);
		
	}
	
	void CheckLevel()
	{
		if( CurrentXP >= XPComponent.GetXPValue("XP", Level) )
		{
			// Add new level.
			Level++;
		}
	}
}
