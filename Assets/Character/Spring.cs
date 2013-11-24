using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void ShootSpring() {
		animation.CrossFade("spring_shot");
		Debug.Log("SpringShot");
	}
}
