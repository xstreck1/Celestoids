using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {

	private const float ROTATION_FORCE = 25000f;
	public bool left_fixed_top = false;
	public bool left_fixed_bot = false;
	public bool right_fixed_bot = false;
	public bool right_fixed_top = false;

	public Transform body;

	// Use this for initialization
	void Start () {
		body = transform.FindChild("Body");
	}

	// Update is called once per frame
	void Update () {
		/*if (Input.GetButtonDown ("SpringL" )) {
			if (body.FindChild("springL").GetComponent<Spring>().spring_possible)
				body.rigidbody2D.AddForce(body.transform.rotation * Vector3.right * ROTATION_FORCE);
			body.FindChild("springL").animation.CrossFade("spring_shot");

		}
		if (Input.GetButtonDown ("SpringR" )) {
			if (body.FindChild("springR").GetComponent<Spring>().spring_possible)
				body.rigidbody2D.AddForce(transform.Find("Body").transform.rotation * Vector3.left * ROTATION_FORCE);
			body.FindChild("springR").animation.CrossFade("spring_shot");
		}*/

		float left_angle = Quaternion.Angle(transform.Find("LegL").rotation, body.rotation);
		left_fixed_bot = left_angle < 5f; 
		left_fixed_top = left_angle > 115f;

		float right_angle = Quaternion.Angle(transform.Find("LegR").rotation, body.rotation);
		right_fixed_bot = right_angle < 5f; 
		right_fixed_top = right_angle > 115f;

		// Debug.Log("Left angle: " + left_angle.ToString() + " Right angle: " + right_angle.ToString());
	}
}
