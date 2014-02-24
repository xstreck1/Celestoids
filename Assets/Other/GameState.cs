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
	public int ID;
	public string name;
	public float gold_t;
	public float silver_t;
	public float bronze_t;
	public float best_t;
	public bool rotation_allowed;
	public bool extension_allowed;
	public bool break_allowed;

	public GameLevel(int ID, string name, float gold_t, float silver_t, float bronze_t, bool rotation_allowed, bool extension_allowed, bool break_allowed) {
		this.ID = ID;
		this.name = name;
		this.best_t = PlayerPrefs.GetFloat(name, 0.0f);
		this.gold_t = gold_t;
		this.silver_t = silver_t;
		this.bronze_t = bronze_t;
		this.rotation_allowed = rotation_allowed;
		this.extension_allowed = extension_allowed;
		this.break_allowed = break_allowed;
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
			levels.Add(new GameLevel(levels.Count(), "TUTORIAL", 0f, 0f, 0f, false, false, false));
			levels.Add(new GameLevel(levels.Count(), "PEBBLE", 20f, 40f, 60f, true, true, true));
			levels.Add(new GameLevel(levels.Count(), "SUNDAY WALK", 30f, 60f, 90f, true, true, true));
			levels.Add(new GameLevel(levels.Count(), "AVALANCHE", 20f, 50f, 70f, true, true, true));
			levels.Add(new GameLevel(levels.Count(), "SPELUNKING", 30f, 50f, 80f, true, true, true));
			levels.Add(new GameLevel(levels.Count(), "JUMP", 20f, 40f, 60f, true, true, true));
			levels.Add(new GameLevel(levels.Count(), "DOWNWARD!", 45f, 60f, 90f, true, true, true));
			levels.Add(new GameLevel(levels.Count(), "HOLE TO FILL", 10f, 15f, 20f, true, true, true));
			levels.Add(new GameLevel(levels.Count(), "MATTER OF CHOICE", 30f, 50f, 80f, true, true, true));
			levels.Add(new GameLevel(levels.Count(), "BARREL ROLL", 20f, 50f, 100f, true, true, true));
			levels.Add(new GameLevel(levels.Count(), "RAINY DAY", 30f, 70f, 150f, true, true, true));
			levels.Add(new GameLevel(levels.Count(), "TOWER OF BOX", 30f, 60f, 100f, true, true, true));
			levels.Add(new GameLevel(levels.Count(), "WAREHOUSE", 60f, 130f, 250f, true, true, true));
			levels.Add(new GameLevel(levels.Count(), "60M BOXES", 60f, 130f, 200f, true, true, true));
			levels.Add(new GameLevel(levels.Count(), "THE STAIRS", 60f, 160f, 300f, true, true, true));
			levels.Add(new GameLevel(levels.Count(), "TEST", 1f, 2f, 3f, true, true, true));
			levels.Add(new GameLevel(levels.Count(), "TUTORIAL_1", 1f, 2f, 3f, true, true, false));
			levels.Add(new GameLevel(levels.Count(), "TUTORIAL_2", 1f, 2f, 3f, true, true, false));
			levels.Add(new GameLevel(levels.Count(), "TUTORIAL_3", 1f, 2f, 3f, true, true, true));
			levels.Add(new GameLevel(levels.Count(), "TUTORIAL_4", 1f, 2f, 3f, true, true, true));

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
