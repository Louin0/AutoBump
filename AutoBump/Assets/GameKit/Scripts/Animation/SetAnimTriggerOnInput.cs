using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimTriggerOnInput : MonoBehaviour
{
	[System.Serializable]
	public class TriggerInfo
	{
		public string _triggerParameterName = "Trigger";
		public Animator _animator;

		public TriggerInfo(string parameterName, Animator animator)
		{
			_animator = animator;
			_triggerParameterName = parameterName;
		}
	}

	[SerializeField] bool playOnce = false;
	[SerializeField] string inputName = "Fire1";

	[Space]

	public List<TriggerInfo> triggers = new List<TriggerInfo>(1);

	bool hasPressed = false;

	// Use this for initialization
	void Start ()
	{
		if(triggers.Count == 0)
		{
			//animators[0] = GetComponent<Animator>();
			Debug.LogWarning("No Trigger set !", gameObject);
		}
	}

	void InputCheck()
	{
		if (Input.GetButtonDown(inputName))
		{
			for (int i = 0; i < triggers.Count; i++)
			{
				triggers[i]._animator.SetTrigger(triggers[i]._triggerParameterName);
			}
			hasPressed = true;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(playOnce)
		{
			if(!hasPressed)
			{
				InputCheck();
			}
		}
		else
		{
			InputCheck();
		}
	}
}
