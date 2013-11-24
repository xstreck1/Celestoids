﻿using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
	private int stick_count = 7;
	private float roll_speed = 0.015f;
	private float roll_bound = 8.0f;

	public float move_force = 1f;
	public float roll_out = 0f;

	public float rotation_speed = 75f;

	private HingeJoint2D hingeJoint2D;
	private JointMotor2D hingeMotor2D;
	private Vector3 connected_anchror;
	
	// Use this for initialization
	void Start ()
	{
		hingeJoint2D = GetComponent<HingeJoint2D>();
		hingeMotor2D = hingeJoint2D.motor;

	}

	void rollLeg(float vertical) {
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
		Control control = transform.parent.GetComponent<Control>();
		bool valid_left = name.Equals("LegL") && ((horizontal < 0f && !control.left_fixed_bot) || (horizontal > 0f && !control.left_fixed_top));
		bool valid_right = name.Equals("LegR") && ((horizontal > 0f && !control.right_fixed_bot) || (horizontal < 0f && !control.right_fixed_top));
		return valid_left || valid_right;
	}

	void turnLeg(float horizontal) {
		hingeMotor2D.motorSpeed = 0f;
		if (valid_angle(horizontal))
			hingeMotor2D.motorSpeed = rotation_speed * horizontal;
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
		float vertical = name.Equals("LegR") ? Input.GetAxis ("VerticalR" ) : Input.GetAxis ("VerticalL");
		if ((vertical > 0f && roll_out < roll_bound) || (vertical < 0f && roll_out > 0f)) 
			rollLeg(vertical);

		float horizontal = name.Equals("LegR") ? Input.GetAxis ("HorizontalR" ) : Input.GetAxis ("HorizontalL");
		turnLeg(horizontal);
		
		float wheel_brake = -1 * (name.Equals("LegR") ? Input.GetAxis ("BreakR" ) : Input.GetAxis ("BreakL"));
		brakeWheel(wheel_brake);
	}
}