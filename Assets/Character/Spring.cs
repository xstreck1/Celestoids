using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour {
	public bool spring_possible;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}


	void OnTriggerEnter(Collider collision) {
		spring_possible = true;
	}

	void OnTriggerExit() {
		spring_possible = false;
	}
}
