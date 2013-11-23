using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
	private int stick_count = 7;
	public float roll_speed = 0.01f;
	public float roll_bound = 9.0f;

	public float move_force = 1f;
	public float roll_out = 0f;

	public float rotation_speed = 50f;

	private HingeJoint2D hingeJoint2D;
	private JointMotor2D hingeMotor2D;
	private Vector3 connected_anchror;
	
	// Use this for initialization
	void Start ()
	{
		hingeJoint2D = GetComponent<HingeJoint2D>();
		hingeMotor2D = hingeJoint2D.motor;

	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		// Cache the horizontal input.
		float vertical = this.gameObject.name.Equals("LegR") ? Input.GetAxis ("Vertical2" ) : Input.GetAxis ("Vertical");
		
		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if (vertical > 0f && roll_out < roll_bound) {
			for (int i = 0; i < stick_count; i++) 
				transform.Find ("sticks_" + i.ToString ()).transform.Translate (1 * Vector3.down * i * roll_speed);
			transform.Find ("sticks_" + stick_count.ToString ()).transform.Translate (1 * Vector3.down * (stick_count -1) * roll_speed);
			connected_anchror = transform.Find("wheel").GetComponent<HingeJoint2D>().connectedAnchor;
			connected_anchror.y -= ((stick_count-1) * roll_speed);
			transform.Find("wheel").GetComponent<HingeJoint2D>().connectedAnchor = connected_anchror;
			roll_out += roll_speed * stick_count;
		}
		else if (vertical < 0f && roll_out > 0f) {
			for (int i = 0; i < stick_count; i++) 
				transform.Find ("sticks_" + i.ToString ()).transform.Translate (-1 * Vector3.down * i * roll_speed);
			transform.Find ("sticks_" + stick_count.ToString ()).transform.Translate (-1 * Vector3.down * (stick_count -1) * roll_speed);
			connected_anchror = transform.Find("wheel").GetComponent<HingeJoint2D>().connectedAnchor;
			connected_anchror.y += ((stick_count-1)  * roll_speed);
			transform.Find("wheel").GetComponent<HingeJoint2D>().connectedAnchor = connected_anchror;
			roll_out -= roll_speed * stick_count;
		}
		
		// Cache the horizontal input.
		float horizontal = this.gameObject.name.Equals("LegR") ? Input.GetAxis ("Horizontal2" ) : Input.GetAxis ("Horizontal");
		Vector3 rotation_point = transform.Find ("sticks_5").position;
		rotation_point.y += 0.4f;
		
		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if (horizontal > 0.1f) {
			hingeMotor2D.motorSpeed = rotation_speed;
		}
		else if (horizontal < -0.1f) {
			hingeMotor2D.motorSpeed = -rotation_speed;
		} else {
			hingeMotor2D.motorSpeed = 0;
		}
		hingeJoint2D.motor = hingeMotor2D;

		float wheel_brake = -1 * (this.gameObject.name.Equals("LegR") ? Input.GetAxis ("Break2" ) : Input.GetAxis ("Break"));
		transform.Find("wheel").rigidbody2D.fixedAngle = false;
		if (wheel_brake > 0f && wheel_brake < 0.9f)
			transform.Find("wheel").rigidbody2D.angularDrag = Mathf.Pow(1000000f, wheel_brake);
		else if (wheel_brake >= 0.9f)
			transform.Find("wheel").rigidbody2D.fixedAngle = true;
		else 
			transform.Find("wheel").rigidbody2D.angularDrag = 0.01f;
		Debug.Log(wheel_brake);
	}
}