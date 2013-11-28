using UnityEngine;
using System.Collections;

public class Finish : MonoBehaviour {
	Control control;

	// Use this for initialization
	void Start () {
		Control control;
	}
	
	// Update is called once per frame
	void Update () {
		control = transform.parent.parent.GetComponent<Control>();
	}

	public void OnTriggerEnter2D(Collider2D collider) {
		Debug.Log(collider.tag);
		if (collider.tag.Equals("Finish")) {
			control.finished();
		}
	}
}
