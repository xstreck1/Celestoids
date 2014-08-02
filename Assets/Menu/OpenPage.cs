using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpenPage : MonoBehaviour {
	Dictionary<string, string> links;

	// Use this for initialization
	void Start () {
		links = new Dictionary<string, string> () {
			{"Twitter", "https://twitter.com/PunyOne"},
			{"WebPage", "http://www.justaconcept.org/"},
            {"GameDownload", "http://www.justaconcept.org/Gallery/Celestoids/"}
		};
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown() {
        string name = this.name;
        Application.OpenURL(links[name]);
	}
}
