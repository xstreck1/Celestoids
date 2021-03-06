﻿using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {
	string level_name;
	public float translate_width = 1000f; // The width of the level - needs to be set manually 

	void Awake() {
		// Initialize the game if it was not already, otherwise this conducts instantiation of the level
		GameState.init(new List<bool> {true, false, false, false}, Application.loadedLevelName);
		GameState.nullify();
		// Disable counter
		if (countPlayers() > 1) {
			GetComponent<Timers> ().enabled = false;
			transform.Find("record").gameObject.SetActive(false);
			transform.Find("runTime").gameObject.SetActive(false);
		}
	}

	// Use this for initialization
	void Start () {
		int player_no = 0;
		// Instantiate 
		foreach (Player player in GameState.players) {
			if (!player.active)
				continue;
			
			GameObject level_obj = (GameObject) Instantiate(GameObject.Find("Level"));
			level_obj.transform.Translate(Vector3.right * translate_width * player_no);
			
			GameObject player_obj = GameObject.Find(player.name);
			GameObject ref_player = GameObject.Find("player1");
			player_obj.transform.position = ref_player.transform.position;
			player_obj.transform.rotation = player_obj.transform.rotation;
			player_obj.transform.Translate(Vector3.right * translate_width * player_no);
			
			player_obj.transform.Find("camera").GetComponent<Camera>().rect = computePlayerCamera(countPlayers(), player_no);
			player_obj.transform.Find("camera").GetComponent<Camera>().orthographicSize = computeCameraSize(countPlayers());
			player_no++;
		}
		
		foreach (Player player in GameState.players) 
			if (!player.active)
				GameObject.Find(player.name).SetActive(false);
		
		GameObject.Find("Level").SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		bool finish = true;
		foreach (Player player in GameState.players) {
			finish &= (player.finished || !player.active);
		}

		if (finish && !GameState.chosen_level.name.Contains("TUTORIAL"))
			Application.LoadLevel("scoreboard");
		else if (finish && GameState.chosen_level.name.Contains("TUTORIAL"))
			finishTutorial();
	}

	int countPlayers() {
		int player_count = 0;
		foreach (Player player in GameState.players) 
			player_count += player.active ? 1 : 0;
		return player_count;
	}

	// Position the camera based on the id of the player and the total number of players
	Rect computePlayerCamera(int player_count, int player_no) {
		if (player_count == 1) {
			return new Rect (0f, 0f, 1f, 1f);	
		} else if (player_count == 2) {
			return new Rect(0f, 0.505f * (player_no % 2),1f, 0.495f);
		} else {
			return new Rect(0.505f * (player_no % 2), 0.505f * ((player_no / 2) % 2), 0.495f,0.495f);
		}
	}

	// Position the camera based on the id of the player and the total number of players
	float computeCameraSize(int player_count) {
		if (player_count == 1) {
			return 18f;	
		} else if (player_count == 2) {
			return 14f;
		} else {
			return 16f;
		}
	}

	void finishTutorial() {
		bool finished = 
			(GameState.players [0].rank + GameState.players [1].rank + GameState.players [2].rank + GameState.players [3].rank) > 0 ? true : false;
		switch (GameState.chosen_level.name) {
		case "TUTORIAL":
			GameState.nullify();
			foreach (GameLevel level in GameState.levels)  {
				if (level.name.Equals("TUTORIAL_1")) {
					GameState.chosen_level = level;
					break;
				}
			}
			Application.LoadLevel("TUTORIAL_1");
			return;
		case "TUTORIAL_1":
			GameState.nullify();
			if (finished) {
				GameState.chosen_level = GameState.levels[GameState.levels.IndexOf(GameState.chosen_level) + 1];
				Application.LoadLevel("TUTORIAL_2");
			}
			else 
				Application.LoadLevel("TUTORIAL_1");
			return;
		case "TUTORIAL_2":
			GameState.nullify();
			if (finished){
				GameState.chosen_level = GameState.levels[GameState.levels.IndexOf(GameState.chosen_level) + 1];
				Application.LoadLevel("TUTORIAL_3");
			}
			else 
				Application.LoadLevel("TUTORIAL_2");
			return;
		case "TUTORIAL_3":
			GameState.nullify();
			if (finished){
				GameState.chosen_level = GameState.levels[GameState.levels.IndexOf(GameState.chosen_level) + 1];
				Application.LoadLevel("TUTORIAL_4");
			}
			else 
				Application.LoadLevel("TUTORIAL_3");
			return;
		case "TUTORIAL_4":
			GameState.nullify();
			if (finished) {
				GameState.chosen_level = GameState.levels[0];
				Application.LoadLevel("MAIN_MENU");
			}
			else 
				Application.LoadLevel("TUTORIAL_4");
			return;
		}
	}
}
