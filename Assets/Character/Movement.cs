using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
		private int stick_count = 7;
		private float roll_speed = 0.02f;
		private float roll_bound = 8.0f;
		private float roll_out = 0f; // Compared with the bound

		private float rotation_speed = 100f;
		private float correction_speed; // This determines how fast will the leg try to move againts pressure
		private float correction_factor = 10f; // Factor of movement againts pressure
		private float last_angle = 0f; // The last angle of the leg w.r.t. the body (used for correction)

		PlayerControl control;
		private HingeJoint2D hingeJoint2D;
		private JointMotor2D hingeMotor2D;
		private PiercePiece piercing_stick;
		private ConnectPiece connecting_stick;
		private Transform wheel_brake;
		private int player_input; // ID of the player used in queries for controls
		

		readonly float TIME_DELAY = 0.1f; // How long to block
		float collision_timer = 0f; // If below 0, allow the movement
		private bool in_collision_body;
	
		// Use this for initialization
		void Start ()
		{
				// Attach player
				foreach (Player player in GameState.players) {		
						if (player.name.Equals (transform.parent.name)) {
								player_input = player.number;
						}
				}
				hingeJoint2D = GetComponent<HingeJoint2D> ();
				hingeMotor2D = hingeJoint2D.motor;
				control = transform.parent.GetComponent<PlayerControl> ();
				correction_speed = correction_factor * (name.Equals ("LegR") ? -1f : 1f);
				last_angle = control.getBodyAngle (name);
				piercing_stick = transform.FindChild ("sticks_" + (stick_count - 1).ToString ()).GetComponent<PiercePiece> ();
				connecting_stick = transform.FindChild ("sticks_" + (stick_count).ToString ()).GetComponent<ConnectPiece> ();
				wheel_brake = transform.FindChild ("wheel").FindChild ("brake");
				in_collision_body = false;
		}

		// Roll up or down with the sticks.
		void rollLeg (float vertical)
		{
				// Control if extending is possible (the stick does not pierce the wheel or the leg is not screwing the body.
				if (vertical > 0f && (piercing_stick.in_collision || control.getAxisAngle (name) < 20f || in_collision_body))
						return;
				// Contol if retracting is possible (the last stick is connected to the wheel optically)
				if (vertical < 0f && !connecting_stick.in_collision)
						return;

				float roll_lenght = vertical * (stick_count - 1) * roll_speed;

				// Move sticks
				for (int i = 1; i < stick_count; i++) 
						transform.Find ("sticks_" + i.ToString ()).Translate (vertical * Vector3.down * i * roll_speed);
				transform.Find ("sticks_" + stick_count.ToString ()).Translate (roll_lenght * Vector3.down);

				// Move wheel
				Vector3 connected_anchror = transform.Find ("wheel").GetComponent<HingeJoint2D> ().connectedAnchor;
				connected_anchror.y -= roll_lenght;
				transform.Find ("wheel").GetComponent<HingeJoint2D> ().connectedAnchor = connected_anchror;

				// Update current status
				roll_out += roll_lenght;
		}

		// Return true only if the leg is in an angle that's allowed for rotation (in either direction).
		bool valid_angle (float horizontal)
		{
				bool valid_left = name.Equals ("LegL") && 
						((horizontal < 0f && !control.isFixed ("LegL", true)) || 
						(horizontal > 0f && !control.isFixed ("LegL", false)));
				bool valid_right = name.Equals ("LegR") && 
						((horizontal > 0f && !control.isFixed ("LegR", true)) || 
						(horizontal < 0f && !control.isFixed ("LegR", false)));
				return valid_left || valid_right;
		}

		// Rotate the leg
		void turnLeg (float horizontal)
		{
				hingeMotor2D.motorSpeed = 0f;
				if (horizontal != 0f && valid_angle (horizontal)) {
						hingeMotor2D.motorSpeed = rotation_speed * horizontal;
						last_angle = control.getBodyAngle (name);
				} 
				// hingeJoint2D.limits = blocking_limits;
				hingeJoint2D.motor = hingeMotor2D;
		}

		// set a drag on the wheel if there is a weak braking, else fix the angle of the wheel
		void brakeWheel (float brake)
		{
				transform.Find ("wheel").rigidbody2D.fixedAngle = false;

				if (brake > 0f && brake < 0.9f)
						transform.Find ("wheel").rigidbody2D.angularDrag = Mathf.Pow (1000000f, brake);
				else if (brake >= 0.9f)
						transform.Find ("wheel").rigidbody2D.fixedAngle = true;
				else 
						transform.Find ("wheel").rigidbody2D.angularDrag = 0.01f;

				float scale = Mathf.Max (brake, 0.2f);
				wheel_brake.localScale = new Vector3 (scale, scale, 1f);
		}

		void FixedUpdate ()
		{
				string stick = (name.Equals ("LegR")) ? "right" : "left";

				if (GameState.chosen_level.extension_allowed) {
						float vertical = Input.GetAxis ("P" + player_input + " " + stick + " vertical");
						if ((vertical > 0f && roll_out < roll_bound) || (vertical < 0f && roll_out > 0f))
								rollLeg (vertical);
				}
		
				if (GameState.chosen_level.rotation_allowed) {
						float horizontal = Input.GetAxis ("P" + player_input + " " + stick + " horizontal");
						turnLeg (-1 * horizontal);
				}
		
				if (GameState.chosen_level.break_allowed) {
						float wheel_brake = Input.GetAxis ("P" + player_input + " " + stick + " trigger");
			            if ((name.Equals ("LegR") && wheel_brake > 0f) || (name.Equals ("LegL") && wheel_brake < 0f))
							wheel_brake = 0f;

						wheel_brake = Mathf.Abs (wheel_brake);
						brakeWheel (wheel_brake);

						if (Input.GetButton("P" + player_input + " " + stick + " low button")) 
							brakeWheel (1f);
						else if (wheel_brake == 0f)
							brakeWheel (wheel_brake);
				}

		}

		void Update() {
		
			collision_timer -= Time.deltaTime;
			if (collision_timer < 0f)
				in_collision_body = false;
		}

		// Control that body is not in collision with the leg.
		void OnCollisionEnter2D (Collision2D collision2d)
		{
				if (collision2d.gameObject.name.Equals ("body_front")) {
						in_collision_body = true;
						collision_timer = TIME_DELAY;
				}
		}
	
		void OnCollisionExit2D (Collision2D collision2d)
		{
				if (collision2d.gameObject.name.Equals ("body_front"))
						in_collision_body = false;
		}
}