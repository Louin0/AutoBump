using UnityEngine;

	[RequireComponent(typeof(UnityEngine.Camera))]
	public class CameraZoom : MonoBehaviour
	{
		[SerializeField] AnimationCurve zoomCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

		public float minZoomFOV = 15f;
		public float maxZoomFOV = 60f;

		[SerializeField] float sensitivity = 50f;
		[SerializeField] float smoothSpeed = 2f;

		[SerializeField] string zoomInputName = "Mouse ScrollWheel";
		public UnityEngine.Camera usedCamera;

		public float currentZoom;
		float t = 0;
		// Use this for initialization
		void Start ()
		{
			if(usedCamera == null)
			{
				Debug.LogWarning("No camera set !", gameObject);
				usedCamera = UnityEngine.Camera.main;
			}
		
			if(usedCamera != null) currentZoom = usedCamera.fieldOfView;
		}
	
		// Update is called once per frame
		void Update ()
		{
			float zoomAxis = Input.GetAxis(zoomInputName);
			if (zoomAxis != 0f)
			{
				t = Mathf.Clamp(t + Time.deltaTime * zoomAxis * sensitivity, 0f, 1f);
				currentZoom = Helper.CurvedLerp(minZoomFOV, maxZoomFOV, zoomCurve, 1 - t);
			}
			usedCamera.fieldOfView = Mathf.MoveTowards(usedCamera.fieldOfView, currentZoom, smoothSpeed);
		}
	}