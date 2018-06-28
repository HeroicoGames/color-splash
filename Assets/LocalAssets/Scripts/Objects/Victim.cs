using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Victim : MonoBehaviour {

	public enum Status {free, rescued};
	public Status status = Status.free;
	public string color;
	public float speed;

	public string Color {
		get { return color; }
		set { color = value; }
	}

//	public void Inititialize(string new_color) {
//		this.color = new_color;
//	}

//	public abstract void Attack ();

	// Kamikaze for example
//	public abstract void SpecialAttack ();

//	public abstract void Scape ();

	void Start() {
		gameObject.GetComponent<Rigidbody2D> ().velocity = -transform.up * speed;
	}

}
