using UnityEngine;
using System.Collections;

public class Timers : MonoBehaviour {
	// Related to the time
	float top_time = float.MaxValue; // This will store the best time that has been achieved up till now.1
	const int FONST_SIZE = 26;
	GUIText record;
	GUIText runtime;

	// Use this for initialization
	void Start () {
		// Determine what is the current top time
		top_time = GameState.chosen_level.best_t == 0.0f ? float.MaxValue : GameState.chosen_level.best_t;
		if (top_time > GameState.chosen_level.silver_t)
			top_time = GameState.chosen_level.silver_t;
		else if (top_time > GameState.chosen_level.gold_t)
			top_time = GameState.chosen_level.gold_t;

		record = transform.Find ("record").guiText;
		record.fontSize = FONST_SIZE;
		record.pixelOffset = new Vector2(Screen.width  / 2f - 40f, Screen.height / 2f - 20f);
		record.text = top_time == .0f ? "no best time yet" : "Next medal: "  + top_time.ToString("0.00");

		runtime = transform.Find ("runTime").guiText;
		runtime.fontSize = FONST_SIZE;
		runtime.pixelOffset = new Vector2( - Screen.width / 2f + 40f, Screen.height / 2f - 20f);
		runtime.text = "Runtime: " + Time.timeSinceLevelLoad.ToString("0.00");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		runtime.text = "Runtime: " +  Time.timeSinceLevelLoad.ToString("0.00");
	}
}
