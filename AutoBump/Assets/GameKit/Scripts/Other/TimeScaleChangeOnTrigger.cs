using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleChangeOnTrigger : MonoBehaviour
{
    [Header("Tag")]
    [SerializeField] bool useTag = true;
    [SerializeField] string tagName = "Player";

    [Header("Time Scale")]
    [SerializeField] float targetTimeScale = 0.2f;
    [SerializeField] float timeToReach = 0.2f;

    [Header("Reset Time Scale")]
    [SerializeField] bool resetTimeScaleOnLeave = true;
    [SerializeField] bool resetTimeScaleOnDestroy = true;

    float timer = 0f;

	private void OnTriggerEnter (Collider other)
	{
        if(useTag)
		{
            if(other.CompareTag(tagName))
			{
                TriggerTimeSlow();
                return;
			}
            return;
		}
        TriggerTimeSlow();
    }

	private void OnTriggerExit (Collider other)
	{
        if(resetTimeScaleOnLeave)
		{
            if (useTag)
            {
                if (other.CompareTag(tagName))
                {
                    RevertTimeSlow();
                    return;
                }
                return;
            }
            RevertTimeSlow();
		}
    }

	private void OnDestroy ()
	{
        if(resetTimeScaleOnDestroy)
		{
            Time.timeScale = 1f;
            Time.fixedDeltaTime = .02f;
		}
	}


    public IEnumerator ChangeTimeSpeed (float targetSpeed)
    {
        timer = 0f;
        float baseTimeScale = Time.timeScale;
        float baseFixedTimeScale = Time.fixedDeltaTime;
        while (timer < timeToReach)
        {
            timer += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(baseTimeScale, targetSpeed, timer / timeToReach);
            Time.fixedDeltaTime = Mathf.Lerp(baseFixedTimeScale, targetSpeed * 0.02f, timer / timeToReach);
            yield return null;
        }
    }

    void TriggerTimeSlow ()
    {
        if (timeToReach > 0f)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeTimeSpeed(targetTimeScale));
        }
        else
        {
            Time.timeScale = targetTimeScale;
            Time.fixedDeltaTime = targetTimeScale * 0.02f;
        }
    }

    void RevertTimeSlow ()
    {
        if (timeToReach > 0f)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeTimeSpeed(1f));
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = .02f;
        }
    }

}
