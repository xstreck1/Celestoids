using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MainMenu : MonoBehaviour {

	Color getColor(GameLevel level) {
		if (level.best_t == 0.0f) 
			return new Color(255,255,255);
		else if (level.best_t < level.gold_t)
			return new Color(225,215,0);
		else if (level.best_t < level.silver_t)
			return new Color(230,232,250);
		else if (level.best_t < level.bronze_t)
			return new Color(140,120,83);
		else 
			return new Color(255,255,255);
	}

	void setMedal(GameLevel level) {
		Transform medals = transform.Find ("Medals");
		string[] medal_names = {"gold", "silver", "bronze"};
		foreach (string medal in medal_names) {
			medals.Find(medal).gameObject.SetActive(false);
		}
		if (level.best_t == 0.0f) 
			return;
		else if (level.best_t < level.gold_t)
			medals.Find("gold").gameObject.SetActive(true);
		else if (level.best_t < level.silver_t)
			medals.Find("silver").gameObject.SetActive(true);
		else if (level.best_t < level.bronze_t)
			medals.Find("bronze").gameObject.SetActive(true);
	}

	void setName() {
		transform.Find("level_name").GetComponent<TextMesh>().text = GameState.chosen_level.name;
		transform.Find("level_name").GetComponent<TextMesh>().color = getColor(GameState.chosen_level);
		setMedal (GameState.chosen_level);
	}

	void Awake() {
		GameState.init(new List<bool> {false, false, false, false}, "BEGINNING");
	}

	void Start() {
		setName();
		foreach (int i in Enumerable.Range(1,4)) {
			transform.Find("Bodies").Find("player" + i.ToString()).gameObject.SetActive(GameState.players[i - 1].active);
		}
	}

	// Update is called once per frame
	void Update () {
		SuperInputMapper.UpdateJoysticks();

		foreach (int i in Enumerable.Range(1,4)) {
			Player player = GameState.players[i - 1];

			// Log in 
			if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_O, (OuyaSDK.OuyaPlayer) i)) {
				player.active = true;
				GameState.players[i - 1] = player;
				transform.Find("Bodies").Find("player" + i.ToString()).gameObject.SetActive(true);
			} // Log ouf 
			else if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_A, (OuyaSDK.OuyaPlayer) i)) {
				player.active = false;
				GameState.players[i - 1] = player;
				transform.Find("Bodies").Find("player" + i.ToString()).gameObject.SetActive(false);
			}

			// Previous level
			int current_index = GameState.levels.IndexOf(GameState.chosen_level);
			if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_LB, (OuyaSDK.OuyaPlayer) i)) {
				current_index = (current_index == 0) ? (GameState.levels.Count() - 1) : current_index - 1;
				GameState.chosen_level = GameState.levels[current_index];
				setName();
			} // Next level
			else 	if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_RB, (OuyaSDK.OuyaPlayer) i)) {
				current_index = (current_index == GameState.levels.Count() - 1) ? 0 : current_index + 1;
				GameState.chosen_level = GameState.levels[current_index];
				setName();
			}
		}

		if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_U)) {
			bool enabled = false;
			foreach (Player p in GameState.players) {
				enabled |= p.active;
			}
			if (enabled)
				Application.LoadLevel(GameState.chosen_level.name);
		}
	}
}
