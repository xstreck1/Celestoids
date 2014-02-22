using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class SetScores : MonoBehaviour {
	Transform ranks;
	Transform scores;

	void Awake() {
		GameState.init(new List<bool> {true, false, false, false}, "TEST");
	}

	// Set numbers that are displayed to players
	void SetRanks() {
		ranks = transform.Find("ranks");
		ranks.Find("highscore_r").GetComponent<TextMesh>().text = GameState.chosen_level.best_t == 0.0f? "newly finished" : ("Best: " + GameState.chosen_level.best_t.ToString("0.000s"));
		ranks.Find("gold_r").GetComponent<TextMesh>().text = "Gold: " +  GameState.chosen_level.gold_t.ToString("0.000s");
		ranks.Find("silver_r").GetComponent<TextMesh>().text = "Silver: " + GameState.chosen_level.silver_t.ToString("0.000s");
		ranks.Find("bronze_r").GetComponent<TextMesh>().text = "Bronze: " + GameState.chosen_level.bronze_t.ToString("0.000s");
	}

	// Set medal for a respective player based on time
	void setMedal(string player_name, float player_time) {
		Transform old_medal = scores.Find (player_name).Find ("medal");
		if (player_time < GameState.chosen_level.best_t && GameState.chosen_level.best_t < GameState.chosen_level.gold_t)
			Instantiate(GameObject.Find("highscore"), old_medal.position, old_medal.rotation);
		else if (player_time < GameState.chosen_level.gold_t)
			Instantiate(GameObject.Find("gold"), old_medal.position, old_medal.rotation);
		else if (player_time < GameState.chosen_level.silver_t)
			Instantiate(GameObject.Find("silver"), old_medal.position, old_medal.rotation);
		else if (player_time < GameState.chosen_level.bronze_t)
			Instantiate(GameObject.Find("bronze"), old_medal.position, old_medal.rotation);
		scores.Find(player_name).Find("medal").gameObject.SetActive(false);
	}

	// Set content for individual players
	float SetPlayerScores() {
		scores = transform.Find("scores");
		float best_score = 0.0f;

		foreach (Player player in GameState.players) {
			string score = "";

			// Determine the rank of the player
			if (player.rank != 0) {
				score = player.rank.ToString();
				if (player.rank == 1) {
					best_score = player.time;
				}
			}

			// If player finished, set medal and score
			if (player.time == 0.0f) {
				if (player.active) {
					score += "yielded";
				}
				else {
					score = "-";
				}
				scores.Find(player.name).Find("medal").gameObject.SetActive(false);
			} else {
				score += ". : " + player.time.ToString("0.000") + "s";
				setMedal(player.name, player.time);
			}
			scores.Find(player.name).GetComponent<TextMesh>().text = score;
		}

		return best_score;
	}

	// Save a highscore if it was achieved
	void CaptureHighScore(float best_score) {
		// If player yields, the score is 0.0f
		if (best_score != 0.0f && (best_score < GameState.chosen_level.best_t || GameState.chosen_level.best_t == 0.0f)) {
			PlayerPrefs.SetFloat(GameState.chosen_level.name, best_score);
			PlayerPrefs.Save();
			int current_index = GameState.levels.IndexOf(GameState.chosen_level);
			GameState.chosen_level.best_t = best_score;
			GameState.levels[current_index] = GameState.chosen_level;
		} else {
			transform.Find("new_highscore").gameObject.SetActive(false);
		}
	}

	// Use this for initialization
	void Start () {
		SetRanks ();
		float best = SetPlayerScores ();
		CaptureHighScore (best);
	}
	
	// Update is called once per frame
	void Update () {
		if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_O)) {
			GameState.nullify();
			Application.LoadLevel("main_menu");

		}
	}
}
