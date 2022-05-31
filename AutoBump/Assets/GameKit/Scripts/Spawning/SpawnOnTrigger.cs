using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnTrigger : MonoBehaviour
{
	public bool displayDebugInfo = true;
	//[Header("Collision options")]
	public bool onlyOnce = false;
	public bool useTagOnTrigger = false;
	public string tagName = "Player";

	//[Header("Spawning Options")]
	public bool shareOrientation = true;
	public GameObject[] prefabToSpawn = new GameObject[1];
	public Vector3 randomMinOffset = Vector3.zero;
	public Vector3 randomMaxOffset = Vector3.zero;

	//[Header("Nested spawning")]
	public bool spawnInsideParent = false;
	public bool spawnInsideCollidingObject = false;
	public Transform parent;

	//[Header("Resources")]
	public bool requireResources = false;
	public ResourceManager resourceManager;
	public int resourceIndex = 0;
	public int resourceCostOnUse = 1;

	//[Header("Input")]
	public bool requireInput = false;
	public string inputName = "";

	bool hasSpawned = false;

	Vector3 GetRandomOffset ()
	{
		Vector3 offset = new Vector3(Random.Range(randomMinOffset.x, randomMaxOffset.x), Random.Range(randomMinOffset.y, randomMaxOffset.y), Random.Range(randomMinOffset.z, randomMaxOffset.z));
		return offset;
	}

	private void Start ()
	{
		if(requireResources)
		{
			if(resourceManager == null)
			{
				if((resourceManager = FindObjectOfType<ResourceManager>()) == null)
				{
					Debug.LogError("No Resource Manager in Scene !");
				}
			}
		}
	}

	void SpawnObject()
	{
		int randomIndex = Random.Range(0, prefabToSpawn.Length);

		if (parent == null && spawnInsideCollidingObject == false)
		{
			parent = transform;
		}

		GameObject gameObjectSpawned;

		if (spawnInsideParent)
		{
			if(shareOrientation)
			{	
				gameObjectSpawned = Instantiate(prefabToSpawn[randomIndex], parent.transform.position, parent.transform.rotation);
			}
			else
			{
				gameObjectSpawned = Instantiate(prefabToSpawn[randomIndex], parent.transform.position, prefabToSpawn[randomIndex].transform.rotation);
			}
			gameObjectSpawned.transform.parent = parent;
			gameObjectSpawned.transform.localPosition = GetRandomOffset();
		}
		else
		{
			if (shareOrientation)
			{
				gameObjectSpawned = Instantiate(prefabToSpawn[randomIndex], transform.position + GetRandomOffset(), transform.rotation);
			}
			else
			{
				gameObjectSpawned = Instantiate(prefabToSpawn[randomIndex], transform.position + GetRandomOffset(), prefabToSpawn[randomIndex].transform.rotation);
			}
		}
		
	}

	void ResourceCheck ()
	{
		if (requireResources)
		{
			if (resourceManager.ChangeResourceAmount(resourceIndex, resourceCostOnUse * -1))
			{
				SpawnObject();
				hasSpawned = true;
			}
		}
		else
		{
			SpawnObject();
			hasSpawned = true;
		}
	}

	void TagCheck(Collider other)
	{
		if (spawnInsideCollidingObject)
		{
			parent = other.transform;
		}

		if (useTagOnTrigger)
		{
			Rigidbody r = other.GetComponent<Rigidbody>();
			if (r == null)
			{
				r = other.GetComponentInParent<Rigidbody>();
			}
			if (r != null)
			{
				if (spawnInsideCollidingObject)
				{
					parent = r.transform;
				}

				if (r.gameObject.CompareTag(tagName))
				{
					ResourceCheck();
				}
			}
		}
		else
		{
			ResourceCheck();
		}
	}

	void SpawnCheck(Collider other)
	{
		if (onlyOnce)
		{
			if (hasSpawned)
			{
				return;
			}

			TagCheck(other);
		}
		else
		{
			TagCheck(other);
		}
	}

	private void OnTriggerEnter (Collider other)
	{
		if(requireInput)
		{
			return;
		}

		SpawnCheck(other);
	}

	private void OnTriggerStay (Collider other)
	{
		if(!requireInput)
		{
			return;
		}

		if (Input.GetButtonDown(inputName))
		{
			SpawnCheck(other);
		}
	}
}
