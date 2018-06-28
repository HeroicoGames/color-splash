using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSplash : MonoBehaviour {

	private SplashColor currentColor = SplashColor.Red;

	public enum SplashColor {Red, Green, Blue};
	public float speed;

	public string CurrentColor
	{
		get { return currentColor.ToString(); }
		set { 
			if (value == "Red") {
				currentColor = SplashColor.Red;
			} else
				if (value == "Green") {
					currentColor = SplashColor.Green;
				} else
					if (value == "Blue") {
						currentColor = SplashColor.Blue;	
					}
		}
	}

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D> ().velocity = transform.up * speed;
	}

	void OnTriggerEnter2D(Collider2D other) {

		Victim victimComponent = other.gameObject.GetComponent<Victim> ();

		if (victimComponent != null) {

			if (victimComponent.Color == CurrentColor) {
				Destroy (other.gameObject);
				Destroy (gameObject);

				// TODO: Best place for this?
				Planet planet = GameObject.Find("Planet").GetComponent<Planet>();
				planet.VictimsRescued += 1;
				planet.splashShip.GetComponent<SplashShip>().RecoveryColor ();
			}
//			print(
//				"Victim Color: " + victimComponent.Color + " " +
//				"Splash Color: " + CurrentColor
//			);
		}
	}
}
