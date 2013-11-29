using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public struct Player {
	public string name;
	public int number;
	public bool active;
	public bool finished;
	public float time;
	public int rank;

	public Player(string name, int number, bool active) {
		this.name = name;
		this.number = number;
		this.active = active;
		this.finished = false;
		this.rank = 0;
		this.time = 0.0f;
	}
}

public struct GameLevel {
	public string name;
	public float gold_t;
	public float silver_t;
	public float bronze_t;
	public float best_t;

	public GameLevel(string name, float gold_t, float silver_t, float bronze_t) {
		this.name = name;
		this.gold_t = gold_t;
		this.silver_t = silver_t;
		this.bronze_t = bronze_t;
		this.best_t = PlayerPrefs.GetFloat(name, 0.0f);
	}
}

public class GameState : MonoBehaviour {
	public static IList<Player> players;
	public static IList<GameLevel> levels;
	public static GameLevel chosen_level;
	public static int rank; //< What rank has the currently finished player.
	public static bool initialized = false;

	// Use this for initialization
	public static void init (List<bool> active) {
		if (!initialized) {
			players = new List<Player>();
			foreach (int i in Enumerable.Range(1,4)) 
				players.Add(new Player("player" + i.ToString(), i, active[i - 1]));

			levels = new List<GameLevel>();
			levels.Add(new GameLevel("INTRO", 10f, 15f, 20f));
			levels.Add(new GameLevel("BOXES", 10f, 15f, 20f));

			chosen_level = levels.First();

			rank = 1;
			initialized = true;
		}
	}

	public static void nullify() {
		foreach (int i in Enumerable.Range(0,3)) {
			Player temp = players[i];
			temp.finished = false;
			temp.time = 0f;
			temp.rank = 0;
			players[i] = temp;
		}
		rank = 1;
	}
}
