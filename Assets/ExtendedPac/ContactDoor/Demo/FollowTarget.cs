using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Quick & dirty script to allow follow camera for demo
public class FollowTarget : MonoBehaviour
{
	public Transform target;
	[Range(0.1f,10f)]
	public float speed = 1.0f;
	private Vector3 offset;

	void Start ()
	{
		if (target)
		{
			offset = target.position - gameObject.transform.position;
		}
	}

	void Update ()
	{
		if (target)
		{
			Vector3 tgt = target.position - offset;
			gameObject.transform.position = Vector3.Lerp( gameObject.transform.position, tgt, Time.deltaTime * speed);
		}
	}
}
