using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerControl : MonoBehaviour {
	private Transform body;
	public Player player_state;

	private readonly float CAM_DIST = -10f;

	// Use this for initialization
	void Start () {
		body = transform.Find("Body");

		// Attach player
		foreach (Player player in GameState.players) {
			if (player.name.Equals(name)) {
				player_state = player;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		// Move the camera
		Vector3 camera = body.transform.position;
		camera.z = CAM_DIST;
		transform.FindChild("camera").transform.position = camera;
		// transform.FindChild("camera_s").transform.position = camera;

		if (Input.GetButtonDown("P" + "1" + " yield")) {
			disable();
		}
	}

	// Obtain the angle between the body and the leg
	public float getBodyAngle(string leg_name) {
		return Quaternion.Angle(transform.Find(leg_name).rotation, transform.Find("Body").rotation);
	}

	// Obtain the angle between respective axis and the leg
	public float getAxisAngle(string leg_name) {
		Transform axis = leg_name.Equals("LegL") ? body.Find("axisL") : body.Find("axisR");
		return Quaternion.Angle(transform.Find(leg_name).rotation, axis.rotation);
	}

	// For blocking of the rotation.
	public bool isFixed(string leg, bool bot) {
		if (bot) 
			return (getBodyAngle(leg) < 60f) && (getAxisAngle(leg) < 30f);
		else 
			return (getBodyAngle(leg) > 60f) && (getAxisAngle(leg) < 30f);
	}

	// Called when the player touches the flag
	public void finish() {
		if (! player_state.finished) {
			player_state.time = Time.timeSinceLevelLoad;
			player_state.rank = GameState.rank;
			GameState.rank += 1;
			disable();
		}
	}

	// Disables the player
	private void disable() {
		transform.Find("LegL").gameObject.SetActive(false);
		transform.Find("LegR").gameObject.SetActive(false);
		player_state.finished = true;
		foreach (int i in Enumerable.Range(0,4)) {
			if (GameState.players[i].name.Equals(name)) {
				GameState.players[i] = player_state;
			}
		}
	}
}
