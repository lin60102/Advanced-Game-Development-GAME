

using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {

	Transform _tranform;

	float damping = 0.1f;

	
	void Start () {
	
		_tranform = this.transform;
	}
	

	void Update () {

		_tranform.Translate(damping*(new Vector3(-Input.GetAxis("Horizontal"),0f,-Input.GetAxis("Vertical"))));
	
	}
}
