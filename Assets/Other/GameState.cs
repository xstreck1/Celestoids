using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public struct Player {
	public string name;
	public bool active;
	public bool finished;
	public float time;

	public Player(string name) {
		this.name = name;
		this.active = false;
		this.finished = false;
		this.time = 0.0f;
	}
}

public class GameState : MonoBehaviour {
	public static IList<Player> players;

	// Use this for initialization
	void Start () {
		players = new List<Player>();
		foreach (int i in Enumerable.Range(1,4)) 
			players.Add(new Player("player" + i.ToString()));

	}
	
	// Update is called once per frame
	void Update () {

	}
}
