using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Helper
{
	public enum FXFeedbackType {ParticleSystem, VFXGraph, Instantiate, None};
	public enum VerticalAxis { Y, Z };
	
	/// <summary>
	/// Remaps a value from a min and a max to another min and max value ranges
	/// </summary>
	/// <param name="value">Value we remap from</param>
	/// <param name="from 1">Min value to remap from</param>
	/// <param name="to 1">Min value to remap to</param>
	/// <param name="from 2">Max value to remap from</param>
	/// <param name="to 2">Max value to remap to</param>
	/// <returns>Remapped value</returns>
	public static float Remap (this float value, float from1, float to1, float from2, float to2)
	{
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

	/// <summary>
	/// Adds a float to a Vector3's x, y and z values.
	/// </summary>
	/// <param name="v">Vector3 we want to increment</param>
	/// <param name="f">float value to add to x, y and z values</param>
	/// <returns></returns>
	public static Vector3 PlusFloat (this Vector3 v, float f)
	{
		return new Vector3(v.x + f, v.y + f, v.z + f);
	}

	#region Curved Lerp
	/////// Float ///////
	/// 
	/// <summary>
	/// Returns a float lerped between two values according to an AnimationCurve.
	/// </summary>
	public static float CurvedLerp (float minValue, float maxValue, AnimationCurve curve, float t)
	{
		float curveEvaluate = curve.Evaluate(t);

		float lerpedValue = Mathf.Lerp(minValue, maxValue, curveEvaluate);

		return lerpedValue;
	}
	
	/// <summary>
	/// Returns a float lerped between two values according to an AnimationCurve.
	/// </summary>
	public static float CurvedLerp (float minValue, float maxValue, float t)
	{
		AnimationCurve animCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
		float curveEvaluate = animCurve.Evaluate(t);

		float lerpedValue = Mathf.Lerp(minValue, maxValue, curveEvaluate);

		return lerpedValue;
	}

	//// Vector3 ////
	///
	/// <summary>
	/// Returns a float lerped between two values according to an AnimationCurve.
	/// </summary>
	public static float CurvedLerp(this Vector2 minMaxValue, AnimationCurve curve, float t)
	{
		float curveEvaluate = curve.Evaluate(t);

		float lerpedValue = Mathf.Lerp(minMaxValue.x, minMaxValue.y, curveEvaluate);

		return lerpedValue;
	}
	
	/// <summary>
	/// Returns a float lerped between two values according to an AnimationCurve.
	/// </summary>
	public static float CurvedLerp (this Vector2 minMaxValue, float t)
	{
		AnimationCurve animCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
		float curveEvaluate = animCurve.Evaluate(t);

		float lerpedValue = Mathf.Lerp(minMaxValue.x, minMaxValue.y, curveEvaluate);

		return lerpedValue;
	}


	#endregion
	
	/// <summary>
	/// Returns a Vector3 corresponding to Input Direction on two axis
	/// </summary>
	/// <param name="hInputName">Horizontal Input Name used in the Input Manager</param>
	/// <param name="vInputName">Vertical Input Name used in the Input Manager</param>
	/// <param name="verticalAxis">Which axis is considered up ?</param>
	/// <returns>Normalized movement direction</returns>
	public static Vector3 GetMovementDirection (string hInputName, string vInputName, VerticalAxis verticalAxis)
	{
		Vector3 moveDir = Vector3.zero;

		moveDir.x = Input.GetAxis(hInputName);
		if (verticalAxis == VerticalAxis.Y)
		{
			moveDir.y = Input.GetAxis(vInputName);
		}
		else
		{
			moveDir.z = Input.GetAxis(vInputName);
		}
		
		moveDir.Normalize();
		
		return moveDir;
	}

	/// <summary>
	/// Returns a if target GameObject is on the ground (Round check shape)
	/// </summary>
	/// <param name="from">Pivot position</param>
	/// <param name="radius">Range of the ground check</param>
	/// <param name="wallAvoidanceLayers">Which Layers are considered as Ground ?</param>
	/// <returns>Is the object on the ground ?</returns>
	public static bool GroundCheck (Vector3 from, float radius, LayerMask wallAvoidanceLayers)
	{
		return Physics.CheckSphere(from, radius, wallAvoidanceLayers);
	}

	/// <summary>
	/// Returns a if target GameObject is on the ground (Square check shape)
	/// </summary>
	/// <param name="from">Pivot position</param>
	/// <param name="halfExtents">Half extents of the ground check</param>
	/// <param name="rotation">Rotation used for the square check</param>
	/// <param name="wallAvoidanceLayers">Which Layers are considered as Ground ?</param>
	/// <returns>Is the object on the ground ?</returns>
	public static bool BoxGroundCheck (Vector3 from, Vector3 halfExtents, Quaternion rotation, LayerMask wallAvoidanceLayers)
	{
		return Physics.CheckBox(from, halfExtents, rotation, wallAvoidanceLayers);
	}

	/// <summary>
	/// Check if time elapsed is superior to cooldown. Best suited for non periodic cooldowns.
	/// </summary>
	/// <param name="timer">Timer reference</param>
	/// <param name="cooldown">Cooldown used for reference</param>
	/// <returns>Is the cooldown over ?</returns>
	public static bool PunctualCooldownCheck(this ref float timer, float cooldown)
	{
		if(Time.time < timer) return false;
		
		timer = Time.time + cooldown;
		return true;
	}

	/// <summary>
	/// Check if time elapsed is superior to cooldown. Best suited for periodic cooldowns.
	/// </summary>
	/// <param name="timer">Timer reference</param>
	/// <param name="cooldown">Cooldown used for reference</param>
	/// <returns>Is the cooldown over ?</returns>
	public static bool PeriodicCooldownCheck (this ref float timer, float cooldown)
	{
		if (Time.time < timer) return false;

		timer += cooldown;
		return true;
	}

	/// <summary>
	/// Converts World space coordinates to Local Space
	/// </summary>
	/// <param name="t">Transform used for reference</param>
	/// <param name="world">World Space Coordinates</param>
	/// <returns>Local Space coordinates</returns>
	public static Vector3 WorldToLocalSpace(this Vector3 world, Transform t)
	{
		Vector3 localPos = t.right * world.x + t.up * world.y + t.forward * world.z;

		return localPos;
	}

	/// <summary>
	/// Checks for wall collision. Used to prevent movement against walls
	/// </summary>
	/// <param name="t">Transform used for reference</param>
	/// <param name="offset">Offset from GameObject's pivot point</param>
	/// <param name="halfExtents">Size of the check box</param>
	/// <param name="wallAvoidanceLayers">Which layers are considered as Walls ?</param>
	/// <returns>Is the object facing a wall ?</returns>
	public static bool WallCheck (Transform t, Vector3 offset, Vector3 halfExtents, LayerMask wallAvoidanceLayers)
	{
		Vector3 localOffset = offset.WorldToLocalSpace(t);

		Vector3 checkPos = t.position + localOffset;

		return Physics.CheckBox(checkPos, halfExtents, t.rotation, wallAvoidanceLayers);
	}

	/// <summary>
	/// Converts an angle in degrees into a direction, depending on specified transform
	/// </summary>
	/// <param name="t">Transform used for reference</param>
	/// <param name="angleInDegrees">Offset from GameObject's pivot point</param>
	/// <returns>Is the object facing a wall ?</returns>
	public static Vector3 DirFromAngle (float angleInDegrees, Transform t)
	{
		angleInDegrees += t.eulerAngles.y;

		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}
	
	/// <summary>
	/// Converts an angle in degrees into a direction
	/// </summary>
	/// <param name="angleInDegrees">Angle in degrees</param>
	/// <returns>Direction converted from target angle</returns>
	public static Vector3 DirFromAngle (float angleInDegrees)
	{
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}


	private static readonly Dictionary<float, WaitForSeconds> waitDictionary = new Dictionary<float, WaitForSeconds>();
	/// <summary>
	/// A more efficient take on the WaitForSeconds used in Coroutines.
	/// </summary>
	/// <param name="time">Wait time</param>
	/// <returns></returns>
	public static WaitForSeconds GetWait(float time)
	{
		if (waitDictionary.TryGetValue(time, out var wait))
		{
			return wait;
		}

		waitDictionary[time] = new WaitForSeconds(time);
		return waitDictionary[time];
	}

	private static PointerEventData _eventDataCurrentPosition;
	private static List<RaycastResult> _results;

	/// <summary>
	/// Checks if cursor is over UI
	/// </summary>
	/// <returns>Is cursor over UI ?</returns>
	public static bool IsOverUi ()
	{
		_eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
		_results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
		return _results.Count > 0;
	}

	/// <summary>
	/// Converts rect transform position into world space coordinates
	/// </summary>
	/// <param name="element">Element we need to get world position of</param>
	/// <returns>World Space Coordinates of rect transform</returns>
	public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
	{
		RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera.main, out var result);
		return result;
	}

	/// <summary>
	/// Deletes all children objects from target transform
	/// </summary>
	/// <param name="t">Transform reference</param>
	/// <returns>World Space Coordinates of rect transform</returns>
	public static void DeleteChildren(this Transform t)
	{
		foreach(Transform child in t)
		{
			Object.Destroy(child.gameObject);
		}
	}
}
