using UnityEngine;
using System.Collections;

public class Play : MonoBehaviour {
	public void OnClickPlay(){
		Application.LoadLevel ("Game");
	}
	public void OnClickTitle(){
		Application.LoadLevel ("Title");
	}
}
