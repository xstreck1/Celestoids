using UnityEngine;
using System.Collections;

public class ConnectPiece : MonoBehaviour {
	
	public bool in_collision;
	
	// Use this for initialization
	void Start () {
		in_collision = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter2D(Collision2D collision2d) {
		if (collision2d.gameObject.name.Equals("wheel"))
			in_collision = true;
	}
	
	void OnCollisionExit2D(Collision2D collision2d) {
		if (collision2d.gameObject.name.Equals("wheel"))
			in_collision = false;
	}
}
