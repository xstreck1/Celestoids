﻿using UnityEngine;
using System.Collections;

// Controls if the last piece is connected to the wheel
public class ConnectPiece : MonoBehaviour {
	readonly float TIME_DELAY = 0.1f; // How long to block
	float collision_timer = 0f; // If below 0, allow the movement
	public bool in_collision;
	
	// Use this for initialization
	void Start () {
		in_collision = true;
	}
	
	// Update is called once per frame
	void Update () {
		collision_timer -= Time.deltaTime;
		if (collision_timer < 0f)
			in_collision = true;
	}
	
	void OnCollisionEnter2D(Collision2D collision2d) {
		if (collision2d.gameObject.name.Equals("wheel")) {
			in_collision = true;
			collision_timer = TIME_DELAY;
		}
	}
	
	void OnCollisionExit2D(Collision2D collision2d) {
		if (collision2d.gameObject.name.Equals("wheel"))
			in_collision = false;
	}
}
