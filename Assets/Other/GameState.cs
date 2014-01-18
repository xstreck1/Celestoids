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
		this.best_t = PlayerPrefs.GetFloat(name, 0.0f);
		this.gold_t = gold_t;
		this.silver_t = silver_t;
		this.bronze_t = bronze_t;
	}

	public Color getColor() {
		if (best_t == 0.0f) 
			return new Color(255,255,255);
		else if (best_t < gold_t)
			return new Color(225,215,0);
		else if (best_t < silver_t)
			return new Color(230,232,250);
		else if (best_t < bronze_t)
			return new Color(140,120,83);
		else 
			return new Color(255,255,255);
	}
}

public class GameState : MonoBehaviour {
	public static IList<Player> players;
	public static IList<GameLevel> levels;
	public static GameLevel chosen_level;
	public static int rank; //< What rank has the currently finished player.
	public static bool initialized = false;

	// Use this for initialization
	public static void init (List<bool> active, string level_to_choose) {
		if (!initialized) {
			players = new List<Player>();
			foreach (int i in Enumerable.Range(1,4)) 
				players.Add(new Player("player" + i.ToString(), i, active[i - 1]));

			levels = new List<GameLevel>();
			// levels.Add(new GameLevel("TEST", 5f, 10f, 30f));
			levels.Add(new GameLevel("BEGINNING", 30f, 60f, 90f));
			levels.Add(new GameLevel("BOX TROUBLE", 20f, 45f, 100f));
			levels.Add(new GameLevel("DOWNWARD!", 45f, 60f, 90f));
			levels.Add(new GameLevel("TEST", 1f, 2f, 3f));

			foreach (GameLevel level in levels)  {
				if (level_to_choose.Equals(level.name)) {
					chosen_level = level;
					break;
				}
			}

			rank = 1;
			initialized = true;
		}
	}

	public static void nullify() {
		foreach (int i in Enumerable.Range(0,4)) {
			Player temp = players[i];
			temp.finished = false;
			temp.time = 0f;
			temp.rank = 0;
			players[i] = temp;
		}
		rank = 1;
	}
}
