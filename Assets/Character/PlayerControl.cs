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

		int player_count = 0;

		// Attach player
		foreach (Player player in GameState.players) {
			player_count += player.active ? 1 : 0;

			if (player.name.Equals(name)) {
				player_state = player;
			}
		}

		// Start camera and player, if required
		gameObject.SetActive(player_state.active);
		if (player_count == 1) {
			transform.Find("camera").gameObject.SetActive(true);
			transform.Find("camera_s").gameObject.SetActive(false);
		} else {
			transform.Find("camera").gameObject.SetActive(false);
			transform.Find("camera_s").gameObject.SetActive(true);
		}

	}

	// Update is called once per frame
	void Update () {
		// Move the camera
		Vector3 camera = body.transform.position;
		camera.z = CAM_DIST;
		transform.FindChild("camera").transform.position = camera;
		transform.FindChild("camera_s").transform.position = camera;

		if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_Y, (OuyaSDK.OuyaPlayer) player_state.number)) {
			disable();
		}
	}

	public float getBodyAngle(string leg_name) {
		return Quaternion.Angle(transform.Find(leg_name).rotation, transform.Find("Body").rotation);
	}

	public bool isFixed(string leg, bool bot) {
		if (bot) 
			return (getBodyAngle(leg) < 60f) && (getAxisAngle(leg) < 30f);
		else 
			return (getBodyAngle(leg) > 60f) && (getAxisAngle(leg) < 30f);
	}

	public float getAxisAngle(string leg_name) {
		Transform axis = leg_name.Equals("LegL") ? body.Find("axisL") : body.Find("axisR");
		return Quaternion.Angle(transform.Find(leg_name).rotation, axis.rotation);
	}

	public void finished() {
		player_state.time = Time.timeSinceLevelLoad;
		player_state.rank = GameState.rank++;
		disable();
	}

	private void disable() {
		transform.Find("LegL").gameObject.SetActive(false);
		transform.Find("LegR").gameObject.SetActive(false);
		player_state.finished = true;
		foreach (int i in Enumerable.Range(0,3)) {
			if (GameState.players[i].name.Equals(name)) {
				GameState.players[i] = player_state;
			}
		}
	}
}
