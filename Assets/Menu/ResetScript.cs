using UnityEngine;
using System.Collections;

public class ResetScript : MonoBehaviour 
{
	public void Reset()
	{
		PlayerPrefs.DeleteAll();
	}
}