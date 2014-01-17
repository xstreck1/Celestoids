using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {
	string level_name;
	float top_time = float.MaxValue;

	Rect computePlayerCamera(int player_count, int player_no) {
		if (player_count == 1) {
			return new Rect (0f, 0f, 1f, 1f);	
		} else if (player_count == 2) {
			return new Rect(0f, 0.505f * (player_no % 2),1f, 0.495f);
		} else {
			return new Rect(0.505f * (player_no % 2), 0.505f * ((player_no / 2) % 2), 0.495f,0.495f);
		}
	}

	void InstantiateGameObjects() {
		int player_count = 0;
		foreach (Player player in GameState.players) 
			player_count += player.active ? 1 : 0;

		float translate_width = 200f;
		int player_no = 0;
		// Instantiate 
		foreach (Player player in GameState.players) {

			if (!player.active)
				continue;

			GameObject player_obj = GameObject.Find(player.name);

			GameObject level_obj = (GameObject) Instantiate(GameObject.Find("Level"));
			level_obj.transform.Translate(Vector3.right * translate_width * player_no);
			GameObject ref_player = GameObject.Find("player1");

			player_obj.transform.position = ref_player.transform.position;
			player_obj.transform.rotation = player_obj.transform.rotation;
			player_obj.transform.Translate(Vector3.right * translate_width * player_no);

			player_obj.transform.Find("camera").GetComponent<Camera>().rect = computePlayerCamera(player_count, player_no);
			player_no++;

		}

		foreach (Player player in GameState.players) 
			if (!player.active)
				GameObject.Find(player.name).SetActive(false);

		((GameObject) GameObject.Find("Level")).SetActive(false);
	}

	void Awake() {
		// Initialize the game if it was not already, otherwise this conducts instantiation of the level
		GameState.init(new List<bool> {false, false, false, true}, Application.loadedLevelName);
	}

	// Use this for initialization
	void Start () {
		InstantiateGameObjects();

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
