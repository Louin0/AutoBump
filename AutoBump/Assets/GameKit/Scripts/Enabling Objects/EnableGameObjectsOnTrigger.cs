using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGameObjectsOnTrigger : MonoBehaviour
{
	public enum RevertOn { InputUp, SecondPress, TriggerExit, Timer, Never }
	public List<GameObject> gameObjectToEnable = new List<GameObject>();

	public bool useTag = false;
	public string tagName = "Case Sensitive";

	public bool onlyOnce = false;
	public bool disableInstead = false;
	public bool displayDebugInfo = false;
	public RevertOn revertOn = RevertOn.TriggerExit;

	public float revertAfterCooldown = 1f;

	public bool requireResources = false;
	public ResourceManager resourceManager;
	public int resourceIndex = 0;
	public int resourceCostOnUse = 1;

	public bool requireInput = false;

	public string inputName = "Case Sensitive";

	private bool wasActivated = false;

	private bool isInside = false;

	private void Update ()
	{
		if (revertOn == RevertOn.InputUp)
		{
			if (Input.GetButtonUp(inputName))
			{
				if (wasActivated)
				{
					EnableComponents(false);
				}
			}
		}

		if (!isInside) return;
		
		if (requireInput)
		{
			InputCheck();
		}
	}

	void EnableComponents (bool enable)
	{
		if (wasActivated == !enable)
		{
			if(disableInstead)
			{
				for (int i = 0; i < gameObjectToEnable.Count; i++)
				{
					gameObjectToEnable[i].SetActive(!enable);
				}
			}
			else
			{
				for (int i = 0; i < gameObjectToEnable.Count; i++)
				{
					gameObjectToEnable[i].SetActive(enable);
				}
			}
		}
		wasActivated = enable;
	}
	void ResourceCheck ()
	{
		if (requireResources)
		{
			if (resourceManager.ChangeResourceAmount(resourceIndex, resourceCostOnUse * -1))
			{
				if (revertOn == RevertOn.Timer && !wasActivated)
				{
					StopAllCoroutines();
					StartCoroutine(RevertAfterTime());
				}
				EnableComponents(true);
			}
		}
		else
		{
			if (revertOn == RevertOn.Timer && !wasActivated)
			{
				StopAllCoroutines();
				StartCoroutine(RevertAfterTime());
			}
			EnableComponents(true);
		}
	}

	public IEnumerator RevertAfterTime ()
	{
		yield return new WaitForSeconds(revertAfterCooldown);

		EnableComponents(false);
	}

	private void InputCheck ()
	{
		if (!Input.GetButtonDown(inputName)) return;
		
		if (revertOn == RevertOn.SecondPress && wasActivated)
		{
			EnableComponents(false);
			return;
		}

		ResourceCheck();
	}

	private void OnTriggerEnter (Collider other)
	{
		if (useTag)
		{
			Rigidbody r = other.GetComponent<Rigidbody>();
			if (r == null)
			{
				r = other.GetComponentInParent<Rigidbody>();
			}

			if (r == null) return;
			if (!r.CompareTag(tagName)) return;
			
			isInside = true;
			
			if (!requireInput)
			{
				ResourceCheck();
			}
		}
		else
		{
			isInside = true;
			if (!requireInput)
			{
				ResourceCheck();
			}
		}
	}

	private void OnTriggerExit (Collider other)
	{
		if (revertOn == RevertOn.TriggerExit)
		{
			if (useTag)
			{
				Rigidbody r = other.GetComponent<Rigidbody>();
				if (r == null)
				{
					r = other.GetComponentInParent<Rigidbody>();
				}
				if (r.CompareTag(tagName))
				{
					EnableComponents(false);
				}
			}
			else
			{
				EnableComponents(false);
			}
		}
		else
		{
			isInside = false;
		}
	}
}
