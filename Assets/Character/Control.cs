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
		body = transform.Find("Body");
	}

	// Update is called once per frame
	void Update () {
		Vector3 camera = body.transform.position;
		camera.z = CAM_DIST;
		transform.FindChild("camera").transform.position = camera;
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
		PlayerPrefs.SetFloat(name + "_time", Time.timeSinceLevelLoad);
		// PlayerPrefs.SetFloat("WrongLevelName", Time.timeSinceLevelLoad);
		PlayerPrefs.Save();
		transform.Find("LegL").gameObject.SetActive(false);
		transform.Find("LegR").gameObject.SetActive(false);
	}
}
