using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {
	string level_name;
	float top_time;
	float start_time;

	// Use this for initialization
	void Start () {
		level_name = PlayerPrefs.GetString("LevelName", "WrongLevelName");
		top_time = PlayerPrefs.GetFloat(level_name, 0.0f);

		transform.Find("record").guiText.pixelOffset = new Vector2(Screen.width  / 2f - 10f, Screen.height / 2f - 10f);
		transform.Find("record").guiText.text = top_time == .0f ? "no best time yet" : top_time.ToString();

		transform.Find("runTime").guiText.pixelOffset = new Vector2( - Screen.width / 2f + 10f, Screen.height / 2f - 10f);
		transform.Find("runTime").guiText.text = Time.timeSinceLevelLoad.ToString("0.00");
	}
	
	// Update is called once per frame
	void Update () {
		SuperInputMapper.UpdateJoysticks();
	}

	void OnGUI() {
		transform.Find("runTime").guiText.text = Time.timeSinceLevelLoad.ToString("0.00");
	}
}
