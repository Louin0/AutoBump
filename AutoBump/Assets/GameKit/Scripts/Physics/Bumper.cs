using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
	//[Header("Forces")]
	[Tooltip("Force and direction of the propulsion")]
	public Vector3 bumpForce = new Vector3(0f, 300f, 0f);
	[Tooltip("Do we add an additional force towards the colliding object ?")]
	public bool bumpTowardsOther = false;
	[Tooltip("Additional force added towards the colliding object ?")]
	[SerializeField] float additionalForceTowardsOther = 500f;
	[SerializeField] bool preventInputHolding = false;

	//[Header("Tag")]
	[Tooltip("Do we bump only objects with a specific tag ?")]
	public bool useTag = false;
	[Tooltip("Name of the tag used on collision")]
	public string tagName = "Case sensitive";

	public bool displayDebugInfo = true;

	private void Awake ()
	{
		if (tagName == "Case Sensitive" && useTag)
		{
			useTag = false;
			Debug.Log("No tag set ! Setting useTag to false", gameObject);
		}
	}
	private void OnCollisionEnter (Collision collision)
	{
		Vector3 toOther = collision.transform.position - transform.position;
		Rigidbody col = collision.rigidbody;

		if(col == null)
		{
			return;
		}

		if (useTag)
		{
			if (collision.gameObject.CompareTag(tagName))
			{
				if (preventInputHolding)
				{
					Jumper j = collision.gameObject.GetComponent<Jumper>();

					if (j)
					{
						j.isbeingBumped = true;
					}
				}

				ApplyBump(col, toOther);
			}
		}
		else
		{
			ApplyBump(col, toOther);
		}
	}

	void ApplyBump (Rigidbody col, Vector3 dir)
	{
		col.velocity = Vector3.zero;

		if (bumpTowardsOther)
		{
			col.AddForce(dir.normalized * additionalForceTowardsOther);
		}

		col.AddForce(bumpForce);
	}

	void TriggerHandler(Collider other, Vector3 dir)
	{
		Rigidbody otherRigid = other.GetComponent<Rigidbody>();
		if (otherRigid == null)
		{
			otherRigid = other.GetComponentInParent<Rigidbody>();
		}

		if (otherRigid != null)
		{
			if (preventInputHolding)
			{
				Jumper j = otherRigid.gameObject.GetComponent<Jumper>();

				if (j)
				{
					j.isbeingBumped = true;
				}
			}

			otherRigid.velocity = Vector3.zero;

			if (bumpTowardsOther)
			{
				otherRigid.AddForce(dir.normalized * additionalForceTowardsOther + bumpForce);
			}

			otherRigid.AddForce(bumpForce);
		}
		else
		{
			Debug.LogWarning("No rigidbody found on colliding object ! Could not bump", gameObject);
		}
	}


	private void OnTriggerEnter (Collider other)
	{
		Vector3 toOther = other.transform.position - transform.position;
		if (useTag)
		{
			if (other.gameObject.CompareTag(tagName))
			{
				TriggerHandler(other, toOther);
			}
		}
		else
		{
			TriggerHandler(other, toOther);
		}
	}
}
