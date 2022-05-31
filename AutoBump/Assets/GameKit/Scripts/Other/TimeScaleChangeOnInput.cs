using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleChangeOnInput : MonoBehaviour
{
    public enum RevertMode { Toggle, Release, Timer };
    [Header("Reverting")]
    [SerializeField] RevertMode revertMode = RevertMode.Toggle;

    [SerializeField] string inputName = "Fire1";

    [Header("Time Scale")]
    [SerializeField] float targetTimeScale = 0.2f;
    [SerializeField] float timeToReach = 0.2f;

    [SerializeField] float revertTimerDuration = 2f;

    bool hasBeenPressed = false;

    float timer = 0f;

    void Update()
    {
        if (Input.GetButtonDown(inputName))
		{
			switch (revertMode)
			{
				case RevertMode.Toggle:
					{
                        if (hasBeenPressed)
                        {
                            RevertTimeSlow();
                            hasBeenPressed = false;
                        }
                        else
						{
                            TriggerTimeSlow();
                            hasBeenPressed = true;
						}
                    }
                
				break;
				case RevertMode.Release:
					{
                        TriggerTimeSlow();
					}
				break;
				case RevertMode.Timer:
					{
                        TriggerTimeSlow();
                        StartCoroutine(RevertTimeScaleAfterSeconds());
					}
				break;

			}
		}

        if(Input.GetButtonUp(inputName))
		{
            if(revertMode == RevertMode.Release)
			{
                RevertTimeSlow();
			}
		}
    }

    public IEnumerator RevertTimeScaleAfterSeconds()
	{
        yield return new WaitForSecondsRealtime(revertTimerDuration);

        RevertTimeSlow();
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

	private void OnDestroy ()
	{
        Time.timeScale = 1f;
        Time.fixedDeltaTime = .02f;
    }
}
