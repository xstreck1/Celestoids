using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {

	private const float ROTATION_FORCE = 25000f;
	public bool left_fixed_top = false;
	public bool left_fixed_bot = false;
	public bool right_fixed_bot = false;
	public bool right_fixed_top = false;

	public Transform body;

	private const float CAM_DIST = -10f;

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

		// Control blocation of the motors

		left_fixed_bot = getBodyAngle("LegL") < 60f && getAxisAngle("LegL") < 30f; 
		left_fixed_top = getBodyAngle("LegL") > 60f && getAxisAngle("LegL") < 30f;
		
		right_fixed_bot = getBodyAngle("LegR") < 60f && getAxisAngle("LegR") < 30f;
		right_fixed_top = getBodyAngle("LegR") > 60f  && getAxisAngle("LegR") < 30f;

		// Debug.Log("Left angle: " + left_angle.ToString() + " Right angle: " + right_angle.ToString());

		Vector3 camera = transform.Find("Body").transform.position;
		camera.z = CAM_DIST;
		transform.FindChild("camera").transform.position = camera;
	}

	public float getBodyAngle(string leg_name) {
		return Quaternion.Angle(transform.Find(leg_name).rotation, body.rotation);
	}

	public float getAxisAngle(string leg_name) {
		Transform axis = leg_name.Equals("LegL") ? body.Find("axisL") : body.Find("axisR");
		return Quaternion.Angle(transform.Find(leg_name).rotation, axis.rotation);
	}

}
