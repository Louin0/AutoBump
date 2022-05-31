using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeOnDestroy : MonoBehaviour
{
	[Range(0f, 3f)]
	[SerializeField] float shakeDuration = 1f;
	[Range(0f, 3f)]
	[SerializeField] float intensity = 1f;

	[SerializeField] CameraShaker targetToShake;

	[SerializeField] SceneMgr sceneManager;

	void Awake ()
	{
		if(targetToShake == null)
		{
			targetToShake = FindObjectOfType<CameraShaker>();
		}

		if(sceneManager == null)
		{
			sceneManager = FindObjectOfType<SceneMgr>();
		}
	}

	private void OnDestroy ()
	{
		if (sceneManager.isLoadingScene)
		{
			return;
		}

		targetToShake.Shake(shakeDuration, intensity);
	}
}
