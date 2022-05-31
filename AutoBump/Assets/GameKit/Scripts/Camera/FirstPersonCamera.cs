using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
	[SerializeField] private Transform transformToRotateWithCamera;

	//[Header("Mouse Parameters")]
	[SerializeField] private string mouseXInputName = "Mouse X";
	[SerializeField] private string mouseYInputName = "Mouse Y";
	
	[Range(50f, 500f)]
	[SerializeField] private float mouseSensitivityX = 250f;
	
	[Range(50f, 500f)]
	[SerializeField] private float mouseSensitivityY = 250f;

	//[Header("Clamp Parameters")]
	[SerializeField] public float minXAxisClamp = -85f;
	[SerializeField] public float maxXAxisClamp = 85f;

	public bool displayDebugInfo = true;
	private float xAxisClamp;
	
	private void Awake ()
	{
		LockCursor();
		xAxisClamp = 0;

		if(transformToRotateWithCamera == null)
		{
			Debug.LogWarning("No Player Body Set !", gameObject);
			transformToRotateWithCamera = GetComponentInParent<Mover>().transform;
		}
	}

	private static void LockCursor ()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void CameraRotation ()
	{
		var mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivityX * Time.deltaTime;
		var mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivityY * Time.deltaTime;

		xAxisClamp += mouseY;

		if(xAxisClamp > maxXAxisClamp)
		{
			xAxisClamp = maxXAxisClamp;
			mouseY = 0.0f;
			ClampXAxisRotationToValue(360f - maxXAxisClamp);
		}
		if (xAxisClamp < minXAxisClamp)
		{
			xAxisClamp = minXAxisClamp;
			mouseY = 0.0f;
			ClampXAxisRotationToValue(minXAxisClamp * -1f);
		}

		transform.Rotate(Vector3.left * mouseY);
		
		if(transformToRotateWithCamera != null)
		{
			transformToRotateWithCamera.Rotate(Vector3.up * mouseX);	
		}
	}

	private void ClampXAxisRotationToValue(float value)
	{
		var transform1 = transform;
		
		var eulerRotation = transform1.eulerAngles;
		eulerRotation.x = value;
		transform1.eulerAngles = eulerRotation;
	}

	private void FixedUpdate ()
	{
		CameraRotation();
	}
}
