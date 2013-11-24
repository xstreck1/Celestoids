using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {

	private const float ROTATION_FORCE = 2500f;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("SpringL" )) {
			transform.Find("Body").rigidbody2D.AddForce(transform.Find("Body").transform.rotation * Vector3.right * ROTATION_FORCE);
		}
		if (Input.GetButtonDown ("SpringR" )) {
			transform.Find("Body").rigidbody2D.AddForce(transform.Find("Body").transform.rotation * Vector3.left * ROTATION_FORCE);
		}
	}
}
