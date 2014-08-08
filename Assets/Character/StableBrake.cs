using UnityEngine;
using System.Collections;

public class StableBrake : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 parent_rotation = transform.parent.localEulerAngles;
		parent_rotation.z = -parent_rotation.z;
		transform.localRotation = Quaternion.Euler(parent_rotation);
	}
}
