using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeOnInput : MonoBehaviour
{
	[Range(0f, 3f)]
	[SerializeField] float shakeDuration = 1f;
	[Range(0f, 3f)]
	[SerializeField] float intensity = 0.5f;

	[SerializeField] string inputName = "Fire1";

	public bool useCooldown = true;
	[SerializeField] float cooldown = 1.0f;

	[SerializeField] bool onlyOnce = false;

	[SerializeField] CameraShaker targetToShake = null;

	bool hasShaken = false;

	float timer;
	// Use this for initialization
	void Start ()
	{
		if (targetToShake == null)
		{
			targetToShake = FindObjectOfType<CameraShaker>();
		}
	}

	bool CanShake()
	{
		if(timer > 0f)
		{
			timer -= Time.deltaTime;
			return false;
		}
		else
		{
			return true;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{

		if(useCooldown)
		{
			if(!CanShake())
			{
				return;
			}

			if (onlyOnce)
			{
				if (hasShaken)
				{
					return;
				}

				if (Input.GetButton(inputName))
				{
					targetToShake.Shake(shakeDuration, intensity);
					timer = cooldown;
					hasShaken = true;
				}
			}
			else
			{
				if (Input.GetButton(inputName))
				{
					targetToShake.Shake(shakeDuration, intensity);
					timer = cooldown;
				}
			}

		}
		else
		{
			if (onlyOnce)
			{
				if (hasShaken)
				{
					return;
				}

				if (Input.GetButtonDown(inputName))
				{
					targetToShake.Shake(shakeDuration, intensity);
					hasShaken = true;
				}
			}
			else
			{
				if (Input.GetButtonDown(inputName))
				{
					targetToShake.Shake(shakeDuration, intensity);
				}
			}
		}
	}
}
