using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MainMenu : MonoBehaviour {
	public static List<string> levels;
	
	// Update is called once per frame
	void Update () {
		SuperInputMapper.UpdateJoysticks();

		foreach (int i in Enumerable.Range(1,4)) {
			if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_O, (OuyaSDK.OuyaPlayer) i)) {
				Player temp = GameState.players[i - 1];
				temp.active = true;
				GameState.players[i - 1] = temp;
				transform.Find("Bodies").Find("player" + i.ToString()).gameObject.SetActive(true);
			}
		}

		foreach (int i in Enumerable.Range(1,4)) {
			if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_A, (OuyaSDK.OuyaPlayer) i)) {
				Player temp = GameState.players[i - 1];
				temp.active = false;
				GameState.players[i - 1] = temp;
				transform.Find("Bodies").Find("player" + i.ToString()).gameObject.SetActive(false);
			}
		}

		if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_U)) {
			bool enabled = false;
			foreach (Player p in GameState.players) {
				enabled |= p.active;
			}
			if (enabled)
				Application.LoadLevel("intro");
		}
	}
}
