using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {

	private const float ROTATION_FORCE = 2500f;
	public bool left_fixed_top = false;
	public bool left_fixed_bot = false;
	public bool right_fixed_bot = false;
	public bool right_fixed_top = false;

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

		float left_angle = Quaternion.Angle(transform.Find("LegL").rotation, transform.Find("Body").rotation);
		left_fixed_bot = left_angle < 5f; 
		left_fixed_top = left_angle > 115f;

		float right_angle = Quaternion.Angle(transform.Find("LegR").rotation, transform.Find("Body").rotation);
		right_fixed_bot = right_angle < 5f; 
		right_fixed_top = right_angle > 115f;

		Debug.Log("Left angle: " + left_angle.ToString() + " Right angle: " + right_angle.ToString());
	}
}
