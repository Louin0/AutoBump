using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsOnInput : EventBase
{
    [Header("Input")]
    [Tooltip("Input used for triggering the events")]
    [SerializeField] string inputName = "Input Name (Case Sensitive)";
   
    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if(Input.GetButtonDown(inputName))
		{
            TriggerEvents();
        }
    }
}
