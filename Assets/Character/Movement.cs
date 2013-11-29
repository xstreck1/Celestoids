using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
	private int stick_count = 7;
	private float roll_speed = 0.015f;
	private float roll_bound = 8.0f;
	
	private float roll_out = 0f;

	private float rotation_speed = 75f;
	private float correction_speed;

	private float last_angle = 0f;

	PlayerControl control;
	private HingeJoint2D hingeJoint2D;
	private JointMotor2D hingeMotor2D;
	private Vector3 connected_anchror;
	
	private PiercePiece piercing_stick;
	private ConnectPiece connecting_stick;

	private OuyaSDK.OuyaPlayer player_input;
	
	// Use this for initialization
	void Start ()
	{
		player_input = (OuyaSDK.OuyaPlayer) (transform.parent.GetComponent<PlayerControl>().player_state.number + 1);
		hingeJoint2D = GetComponent<HingeJoint2D>();
		hingeMotor2D = hingeJoint2D.motor;
		control = transform.parent.GetComponent<PlayerControl>();
		correction_speed = 5f * (name.Equals("LegR") ? -1f : 1f);
		last_angle = control.getBodyAngle(name);
		piercing_stick = transform.FindChild("sticks_" + (stick_count -1).ToString()).GetComponent<PiercePiece>();
		connecting_stick = transform.FindChild("sticks_" + (stick_count).ToString()).GetComponent<ConnectPiece>();
	}

	void rollLeg(float vertical) {
		// Control if extending is possible (the stick does not pierce the wheel or the leg is not screwing the body.
		if (vertical > 0f && (piercing_stick.in_collision || control.getAxisAngle(name) < 20f))
			return;
		if (vertical < 0f && !connecting_stick.in_collision)
			return;

		float roll_lenght = vertical * (stick_count -1) * roll_speed;

		// Move sticks
		for (int i = 1; i < stick_count; i++) 
			transform.Find ("sticks_" + i.ToString ()).Translate (vertical * Vector3.down * i * roll_speed);
		transform.Find ("sticks_" + stick_count.ToString ()).Translate (roll_lenght * Vector3.down);

		// Move wheel
		connected_anchror = transform.Find("wheel").GetComponent<HingeJoint2D>().connectedAnchor;
		connected_anchror.y -= roll_lenght;
		transform.Find("wheel").GetComponent<HingeJoint2D>().connectedAnchor = connected_anchror;

		// Make boundry
		roll_out += roll_lenght;
	}

	bool valid_angle(float horizontal) {
		bool valid_left = name.Equals("LegL") && ((horizontal < 0f && !control.isFixed("LegL", true)) || (horizontal > 0f && !control.isFixed("LegL", false)));
		bool valid_right = name.Equals("LegR") && ((horizontal > 0f && !control.isFixed("LegR", true)) || (horizontal < 0f && !control.isFixed("LegR", false)));
		return valid_left || valid_right;
	}

	void turnLeg(float horizontal) {
		hingeMotor2D.motorSpeed = 0f;
		if (horizontal != 0f && valid_angle(horizontal)) {
			hingeMotor2D.motorSpeed = rotation_speed * horizontal;
			last_angle = control.getBodyAngle(name);
		} else {
			hingeMotor2D.motorSpeed = (last_angle - control.getBodyAngle(name)) * correction_speed;
		}
		// hingeJoint2D.limits = blocking_limits;
		hingeJoint2D.motor = hingeMotor2D;
	}

	void brakeWheel(float brake) {
		transform.Find("wheel").rigidbody2D.fixedAngle = false;

		if (brake > 0f && brake < 0.9f)
			transform.Find("wheel").rigidbody2D.angularDrag = Mathf.Pow(1000000f, brake);
		else if (brake >= 0.9f)
			transform.Find("wheel").rigidbody2D.fixedAngle = true;
		else 
			transform.Find("wheel").rigidbody2D.angularDrag = 0.01f;
	}


	// Update is called once per frame
	void FixedUpdate ()
	{
		float vertical = -1 * (name.Equals("LegR") ?  SuperInputMapper.GetAxis("RY", player_input) : SuperInputMapper.GetAxis("LY", player_input));
		if ((vertical > 0f && roll_out < roll_bound) || (vertical < 0f && roll_out > 0f)) 
			rollLeg(vertical);

		float horizontal = name.Equals("LegR") ? SuperInputMapper.GetAxis("RX", player_input) : SuperInputMapper.GetAxis("LX", player_input);
		turnLeg(-1 * horizontal);

		float wheel_brake = (name.Equals("LegR") ? SuperInputMapper.GetAxis("RT", player_input) : SuperInputMapper.GetAxis("LT", player_input));
		brakeWheel(wheel_brake);
	}
}