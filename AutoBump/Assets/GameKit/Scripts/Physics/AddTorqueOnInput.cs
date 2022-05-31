using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AddTorqueOnInput : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody rigid;

	[Header("Forces")]
	[SerializeField] ForceMode forceMode = ForceMode.Force;
	[SerializeField] bool isTorqueLocal = true;
    [SerializeField] Vector3 torque = Vector3.zero;

	[Header("Input")]
    [SerializeField] string inputName = "Horizontal";

	private void Awake ()
	{
		if(rigid == null)
		{
			rigid = GetComponent<Rigidbody>();
		}
	}
	// Update is called once per frame
	void FixedUpdate()
    {
        if(Input.GetButton(inputName))
		{
			if(isTorqueLocal)
			{
				rigid.AddRelativeTorque(torque, forceMode);
			}
			else
			{
				rigid.AddTorque(torque, forceMode);
			}
		}
    }
}
