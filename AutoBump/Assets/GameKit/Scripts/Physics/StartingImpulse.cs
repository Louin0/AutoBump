using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StartingImpulse : MonoBehaviour
{
	[SerializeField] Vector3 impulsePower = new Vector3(0f, 0f, 40f);
	[SerializeField] bool isLocal = true;
	[SerializeField] Rigidbody rigid;

	void Awake()
	{
		if (!rigid && !TryGetComponent<Rigidbody>(out rigid))
		{
			return;
		}

		if (isLocal)
		{
			rigid.AddRelativeForce(impulsePower, ForceMode.Impulse);
			return;
		}

		rigid.AddForce(impulsePower, ForceMode.Impulse);
	}
}
