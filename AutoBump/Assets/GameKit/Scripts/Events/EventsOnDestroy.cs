using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsOnDestroy : EventBase
{
	private void OnDestroy ()
	{
		TriggerEvents();
	}
}
