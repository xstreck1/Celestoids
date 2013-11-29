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

	public Player(string name, int number) {
		this.name = name;
		this.number = number;
		this.active = false;
		this.finished = false;
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

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(transform.gameObject);

		players = new List<Player>();
		foreach (int i in Enumerable.Range(1,4)) 
			players.Add(new Player("player" + i.ToString(), i));

		levels = new List<GameLevel>();
		levels.Add(new GameLevel("INTRO", 10f, 15f, 20f));
		levels.Add(new GameLevel("BOXES", 10f, 15f, 20f));

		chosen_level = levels.First();
	}

	void Start() {
	}
	
	// Update is called once per frame
	void Update () {

	}
}
