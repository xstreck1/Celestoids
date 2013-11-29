using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class SetScores : MonoBehaviour {
	Transform ranks;
	Transform scores;

	void Awake() {
		GameState.init(new List<bool> {true, false, false, false});
	}

	// Use this for initialization
	void Start () {

		ranks = transform.Find("ranks");
		scores = transform.Find("scores");

		ranks.Find("highscore").GetComponent<TextMesh>().text = GameState.chosen_level.best_t == 0.0f? "newly finished" : GameState.chosen_level.best_t.ToString("0.000s");
		ranks.Find("gold").GetComponent<TextMesh>().text = GameState.chosen_level.gold_t.ToString("0.000s");
		ranks.Find("silver").GetComponent<TextMesh>().text = GameState.chosen_level.silver_t.ToString("0.000s");
		ranks.Find("bronze").GetComponent<TextMesh>().text = GameState.chosen_level.bronze_t.ToString("0.000s");
		
		float best_score = 0.0f;

		foreach (Player player in GameState.players) {
			string score = "";

			if (player.rank != 0) {
				score = player.rank.ToString();
				if (player.rank == 1)
					best_score = player.time;
			}

			if (player.time == 0.0f) {
				if (player.active) 
					score += "yielded";
				else 
					score = "-";
			} else {
				score += " : " + player.time.ToString("0.000") + "s";
			}
			scores.Find(player.name).GetComponent<TextMesh>().text = score;
		}

		if (best_score != 0.0f && (best_score < GameState.chosen_level.best_t || GameState.chosen_level.best_t == 0.0f)) {
			PlayerPrefs.SetFloat(GameState.chosen_level.name, best_score);
			PlayerPrefs.Save();
			GameState.chosen_level.best_t = best_score;
			transform.Find("new_highscore").gameObject.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_O)) {
			GameState.nullify();
			Application.LoadLevel("main_menu");

		}
	}
}
