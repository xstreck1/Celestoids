using UnityEngine;
using System.Collections;

public class EyeRotation : MonoBehaviour
{
	private const float ROTATION_SPEED = 125f;
	private HingeJoint2D hingeJoint2D;
	private int player_input;

	// Use this for initialization
	void Start ()
	{
		// Attach player
		foreach (Player player in GameState.players) {		
			if (player.name.Equals(transform.parent.parent.name)) {
				player_input = player.number;
			}
		}
		hingeJoint2D = GetComponent<HingeJoint2D>();
	}

	// Rotate the leg
	void rotateEye(float horizontal) {
		// See if the legs are either existent and if so, if they are active
		if (horizontal != 0f && (transform.parent.parent.Find("LegL") == null || !transform.parent.parent.Find("LegL").gameObject.activeSelf)) {
			JointMotor2D hingeMotor2D = hingeJoint2D.motor;
			hingeJoint2D.useMotor = true;
			hingeMotor2D.motorSpeed = ROTATION_SPEED * horizontal;
			hingeJoint2D.motor = hingeMotor2D;
		} else {
			hingeJoint2D.useMotor = false;
		}
	}
	
	// Input is obtained in the fixed update
	void FixedUpdate () {
		float horizontal = name.Equals("eyeballR") ? SuperInputMapper.GetAxis("RX", (OuyaSDK.OuyaPlayer)  player_input) : SuperInputMapper.GetAxis("LX", (OuyaSDK.OuyaPlayer)  player_input);
		rotateEye(-1 * horizontal);
	}
}