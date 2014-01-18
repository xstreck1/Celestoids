using UnityEngine;
using System.Collections;

public class MoveStars : MonoBehaviour {
	public float roll_factor = 0.1f; // How many units the player has to move for the stars to move.
	public Vector3 last_player_pos;
	Transform body;

	// Use this for initialization
	void Start () {
		body = transform.parent.Find ("Body");
		last_player_pos = body.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 new_stars_pos = transform.position;
		new_stars_pos.x += (body.position.x - last_player_pos.x) * roll_factor;
		new_stars_pos.y += (body.position.y - last_player_pos.y) * roll_factor;
		transform.position = new_stars_pos;

		last_player_pos = body.position;
	}
}
