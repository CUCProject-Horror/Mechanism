using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceMove : MonoBehaviour
{
	private Rigidbody body;
	// Use this for initialization
	void Start ()
	{	
		body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		Vector3 v = Vector3.zero;
		v.x = Input.GetAxis( "Horizontal" );
		v.z = Input.GetAxis( "Vertical" );
		v *= 15f;
		body.AddForce( v );
	}
}
