using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {
	string level_name;
	float top_time = float.MaxValue;

	void Awake() {
		GameState.init(new List<bool> {true, false, false, false}, Application.loadedLevelName);
	}

	// Use this for initialization
	void Start () {
		top_time = GameState.chosen_level.best_t == 0.0f ? float.MaxValue : GameState.chosen_level.best_t;
		if (top_time > GameState.chosen_level.bronze_t) 
			top_time = GameState.chosen_level.bronze_t;
		else if (top_time > GameState.chosen_level.silver_t)
			top_time = GameState.chosen_level.silver_t;
		else if (top_time > GameState.chosen_level.gold_t)
			top_time = GameState.chosen_level.gold_t;

		transform.Find("record").guiText.pixelOffset = new Vector2(Screen.width  / 2f - 40f, Screen.height / 2f - 20f);
		transform.Find("record").guiText.text = top_time == .0f ? "no best time yet" : "Next Rank: "  + top_time.ToString("0.00");

		transform.Find("runTime").guiText.pixelOffset = new Vector2( - Screen.width / 2f + 40f, Screen.height / 2f - 20f);
		transform.Find("runTime").guiText.text = "Runtime: " + Time.timeSinceLevelLoad.ToString("0.00");
	}
	
	// Update is called once per frame
	void Update () {
		SuperInputMapper.UpdateJoysticks();
		bool finish = true;
		foreach (Player player in GameState.players) {
			finish &= (player.finished || !player.active);
		}
		if (finish)
			Application.LoadLevel("scoreboard");
	}

	void OnGUI() {
		transform.Find("runTime").guiText.text = "Runtime: " +  Time.timeSinceLevelLoad.ToString("0.00");
	}
}
