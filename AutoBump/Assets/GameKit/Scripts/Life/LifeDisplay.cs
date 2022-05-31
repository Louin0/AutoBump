using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDisplay : MonoBehaviour
{
	[Tooltip("The life component we want to display")]
	public Life lifeToDisplay;
	[SerializeField] Slider lifeBar;
	
	void Awake ()
	{
		if (lifeToDisplay == null)
		{
			lifeToDisplay = FindObjectOfType<Life>();
			Debug.Log("LifeToDisplay n'a pas été assigné ! Pensez à drag & drop le component Life du GameObject dont vous voulez afficher la vie !", gameObject);
		}
		InitLifeBarValues();
	}

	public void InitLifeBarValues()
	{
		if(lifeBar == null)
		{
			lifeBar = GetComponentInChildren<Slider>();
		}

		lifeBar.minValue = 0;
		if (lifeToDisplay != null)
		{
			lifeBar.value = lifeToDisplay.CurrentLife;
			lifeBar.maxValue = lifeToDisplay.maxLife;
		}
	}

	public void UpdateValue()
	{
		if (lifeToDisplay != null)
		{
			lifeBar.value = lifeToDisplay.CurrentLife;
		}
	}
	
	void Update ()
	{
		UpdateValue();
	}
}
