using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MainMenu : MonoBehaviour {

	void setName() {
		transform.Find("level_name").GetComponent<TextMesh>().text = GameState.chosen_level.name;		
	}

	// Update is called once per frame
	void Update () {
		SuperInputMapper.UpdateJoysticks();

		foreach (int i in Enumerable.Range(1,4)) {
			Player temp = GameState.players[i - 1];

			if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_O, (OuyaSDK.OuyaPlayer) i)) {
				temp.active = true;
				GameState.players[i - 1] = temp;
				transform.Find("Bodies").Find("player" + i.ToString()).gameObject.SetActive(true);
			} else if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_A, (OuyaSDK.OuyaPlayer) i)) {
				temp.active = false;
				GameState.players[i - 1] = temp;
				transform.Find("Bodies").Find("player" + i.ToString()).gameObject.SetActive(false);
			}

			int current_index = GameState.levels.IndexOf(GameState.chosen_level);
			if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_LB, (OuyaSDK.OuyaPlayer) i)) {
				current_index = (current_index == 0) ? (GameState.levels.Count() - 1) : current_index - 1;
				GameState.chosen_level = GameState.levels[current_index];
				setName();
			} else 	if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_RB, (OuyaSDK.OuyaPlayer) i)) {
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

	void Start() {
		setName();
	}
}
