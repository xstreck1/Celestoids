using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {
	public static List<string> levels;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		SuperInputMapper.UpdateJoysticks();

		if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_O, OuyaSDK.OuyaPlayer.player1)) {
			Player temp = GameState.players[0];
			temp.active = true;
			GameState.players[0] = temp;
			transform.Find("Bodies").Find("player1").gameObject.SetActive(true);
		}

		if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_O, OuyaSDK.OuyaPlayer.player2)) {
			Player temp = GameState.players[1];
			temp.active = true;
			GameState.players[1] = temp;
			transform.Find("Bodies").Find("player2").gameObject.SetActive(true);
		}

		if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_O, OuyaSDK.OuyaPlayer.player3)) {
			Player temp = GameState.players[2];
			temp.active = true;
			GameState.players[2] = temp;
			transform.Find("Bodies").Find("player3").gameObject.SetActive(true);
		}

		if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_O, OuyaSDK.OuyaPlayer.player4)) {
			Player temp = GameState.players[1];
			temp.active = true;
			GameState.players[1] = temp;
			transform.Find("Bodies").Find("player4").gameObject.SetActive(true);
		}

		if (SuperInputMapper.GetButtonDown(OuyaSDK.KeyEnum.BUTTON_U)) {
			Application.LoadLevel("intro");
		}
	}
}
