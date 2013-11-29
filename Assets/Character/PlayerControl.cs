using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerControl : MonoBehaviour {
	private Transform body;
	private Player player_state;

	private readonly float CAM_DIST = -10f;

	// Use this for initialization
	void Start () {
		body = transform.Find("Body");

		// Attach player
		foreach (Player player in GameState.players) {
			if (player.name.Equals(name)) {
				player_state = player;
				break;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		// Move the camera
		Vector3 camera = body.transform.position;
		camera.z = CAM_DIST;
		transform.FindChild("camera").transform.position = camera;

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
		disable();
	}

	private void disable() {
		transform.Find("LegL").gameObject.SetActive(false);
		transform.Find("LegR").gameObject.SetActive(false);
		player_state.finished = true;
	}
}
