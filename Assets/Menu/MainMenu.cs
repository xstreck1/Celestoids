using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MainMenu : MonoBehaviour {
	readonly int TUTORIAL_COUNT = 5;

	Color getColor(GameLevel level) {
		if (level.best_t == 0.0f) 
			return new Color(0.35f,0.35f,0.35f);
		else if (level.best_t < level.gold_t)
			return new Color(0.9f,0.85f,0f);
		else if (level.best_t < level.silver_t)
			return new Color(0.8f,0.85f,0.96f);
		else if (level.best_t < level.bronze_t)
			return new Color(0.6f,0.45f,0.33f);
		else 
			return new Color(0.35f,0.35f,0.35f);
	}

	void setMedal(GameLevel level) {
		Transform medals = transform.Find ("Medals");
		string[] medal_names = {"gold", "silver", "bronze"};
		foreach (string medal in medal_names) {
			medals.Find(medal).gameObject.SetActive(false);
		}
		medals.Find("single_only").gameObject.SetActive(false);

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
		transform.Find("level_name").GetComponent<TextMesh>().text = GameState.chosen_level.ID.ToString() + ". " + GameState.chosen_level.name;
		transform.Find("level_name").GetComponent<TextMesh>().color = getColor(GameState.chosen_level);
		setMedal (GameState.chosen_level);
	}

	void Awake() {
		GameState.init(new List<bool> {false, false, false, false}, "TUTORIAL");
		// PlayerPrefs.DeleteAll ();
	}

	void Start() {
		setName();
		foreach (int i in Enumerable.Range(1,4)) {
			transform.Find("Bodies").Find("player" + i.ToString()).gameObject.SetActive(GameState.players[i - 1].active);
		}
		foreach (string joy in Input.GetJoystickNames ())
			Debug.Log(joy);
		TextMesh configure = GameObject.Find("configure").GetComponent<TextMesh>();
#if UNITY_WEBPLAYER
		configure.text = "Web version supports XBOX controllers only.\nTo set up for other controllers, click to download the standalone version.";
#else
        Object.Destroy(GameObject.Find("GameDownload").GetComponent<OpenPage>());
		configure.text = "Supports XBOX-style controllers.\nFor other controllers please use the Input tab in the launcher to configure.";
#endif

	}

	// Update is called once per frame
	void Update () {
		SuperInputMapper.UpdateJoysticks();

		foreach (int i in Enumerable.Range(1,4)) {
			Player player = GameState.players[i - 1];

			// Log in 
			if (Input.GetButtonDown("P" + i + " connect")) {
				player.active = true;
				GameState.players[i - 1] = player;
				transform.Find("Bodies").Find("player" + i.ToString()).gameObject.SetActive(true);
			} // Log ouf 
			else if (Input.GetButtonDown("P" + i + " disconnect")) {
				player.active = false;
				GameState.players[i - 1] = player;
				transform.Find("Bodies").Find("player" + i.ToString()).gameObject.SetActive(false);
			}

			// Previous level
			int current_index = GameState.levels.IndexOf(GameState.chosen_level);
			if (Input.GetButtonDown("P" + i + " left button")) {
				current_index = (current_index == 0) ? (GameState.levels.Count() - TUTORIAL_COUNT - 1) : current_index - 1;
				GameState.chosen_level = GameState.levels[current_index];
				setName();
			} // Next level
			else if (Input.GetButtonDown("P" + 1 + " right button"))  {
				current_index = (current_index == GameState.levels.Count() - TUTORIAL_COUNT - 1) ? 0 : current_index + 1;
				GameState.chosen_level = GameState.levels[current_index];
				setName();
			}

			// Start the game
			if (Input.GetButtonDown("P" + i + " start")) {
				int enabled = 0;
				foreach (Player p in GameState.players) {
					enabled += p.active ? 1 : 0;
				}
				if (GameState.chosen_level.name.Equals("TUTORIAL")) {
					if (enabled == 1) 
						Application.LoadLevel(GameState.chosen_level.name);
					else
						transform.Find ("Medals").Find("single_only").gameObject.SetActive(true);
				}
				else if (enabled >= 1)
					Application.LoadLevel(GameState.chosen_level.name);
			}
		}
	}
}
