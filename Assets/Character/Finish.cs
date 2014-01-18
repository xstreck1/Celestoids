using UnityEngine;
using System.Collections;

public class Finish : MonoBehaviour {
	PlayerControl control;

	// Use this for initialization
	void Start () {
		control = transform.parent.parent.GetComponent<PlayerControl>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	// End if the finish gets hit
	public void OnTriggerEnter2D(Collider2D collider) {
		Debug.Log(collider.tag);
		if (collider.tag.Equals("Finish")) {
			control.finish();
		}
	}
}
