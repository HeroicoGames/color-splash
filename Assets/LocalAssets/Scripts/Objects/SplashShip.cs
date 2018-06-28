using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, yMin, yMax;
}

public class SplashShip : MonoBehaviour
{
	public enum SupplyColor {Red, Green, Blue};
	public SupplyColor supplyColor;
	private bool activeShield = true;
	private float nextSplash;
	private Dictionary <string, bool> activeSupplies = new Dictionary <string, bool> ()
	{
		{"Red", true},
		{"Green", true},
		{"Blue", true}
	};
	private GameObject colorSplashPrefab;
	private GameObject colorGenerator;

	public float speed;
	public float tilt;
	public float splashRate;

	public GameObject colorSplashRedPrefab;
	public GameObject colorSplashGreenPrefab;
	public GameObject colorSplashBluePrefab;
	// Limits for fly 
	public Boundary boundary;

	public string ColorSupply
	{
		get { return supplyColor.ToString(); }
		set { 
			if (value == "Red") {
				supplyColor = SupplyColor.Red;
			} else
				if (value == "Green") {
					supplyColor = SupplyColor.Green;
				} else
					if (value == "Blue") {
						supplyColor = SupplyColor.Blue;	
					}
		}
	}

	// Input (Keyboard) methods

	void Movements() {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector2 (moveHorizontal, moveVertical);
		Rigidbody2D rigidbody2d = gameObject.GetComponent<Rigidbody2D> ();
		rigidbody2d.velocity = movement * speed;

		rigidbody2d.position = new Vector2 
			(
				Mathf.Clamp (rigidbody2d.position.x, boundary.xMin, boundary.xMax), 
				Mathf.Clamp (rigidbody2d.position.y, boundary.yMin, boundary.yMax)
			);

		rigidbody2d.rotation = rigidbody2d.velocity.x * -tilt;
	}

	void ShootSplashColor() {

		GameObject spawnPoint = GameObject.Find ("SpawnSplash");

		if (Input.GetButton("Jump") && Time.time > nextSplash) {

			if (ColorSupply == "Red") {
				colorSplashPrefab = colorSplashRedPrefab;
			} else if (ColorSupply == "Green") {
				colorSplashPrefab = colorSplashGreenPrefab;
			} else if (ColorSupply == "Blue") {
				colorSplashPrefab = colorSplashBluePrefab;
			}

			SetNextSplash ();
			GameObject colorSplash = Instantiate (
				colorSplashPrefab,
				new Vector2(spawnPoint.transform.position.x, spawnPoint.transform.position.y),
				Quaternion.identity
			) as GameObject;

			// TODO: En base a esta línea, creo que lo mejor es tener un solo prefab ColorSplash y cambiarle el color.
			// Pero hasta donde intente no pude cambiarle correctamente el color al prefab animado que tenia
			colorSplash.GetComponent<ColorSplash> ().CurrentColor = ColorSupply;
		}
	}

	// SplaShip methods

	void SetNextSplash() {
		nextSplash = Time.time + splashRate;
	}

	void ChangeColorSupply() {

		if (activeSupplies["Red"] == true && Input.GetButtonDown ("Red")) {
			ColorSupply = "Red";
			colorGenerator.GetComponent <ColorGenerator> ().setColor (ColorSupply);

		} else
			if (activeSupplies["Green"] == true && Input.GetButtonDown("Green")) {
				ColorSupply = "Green";
				colorGenerator.GetComponent <ColorGenerator> ().setColor (ColorSupply);
			} else
				if (activeSupplies["Blue"] == true && Input.GetButtonDown("Blue")) {
					ColorSupply = "Blue";
					colorGenerator.GetComponent <ColorGenerator> ().setColor (ColorSupply);
				}
	}

	void ActiveOrInactiveColor (string color) {
		// TODO: Best approach for this?, does not find for GameObject.name
		ParticleSystem particleSystem = GameObject.Find (
			"Propulsion Color " + color
		).GetComponent <ParticleSystem> ();

		if (particleSystem.isPlaying == false) {
			particleSystem.Play();
		} else
			if (particleSystem.isPlaying == true) {
				particleSystem.Stop();
			}
		colorGenerator.GetComponent <ColorGenerator> ().setColor(NextColorAvailable ());
	}

	void RemoveShieldOrColor() {
		if (activeShield == true) {
			activeShield = false;
			Destroy (GameObject.Find ("Shield"));
		} else if (activeSupplies[ColorSupply] == true) {

			activeSupplies [ColorSupply] = false;

			ActiveOrInactiveColor (ColorSupply);

			ColorSupply = NextColorAvailable ();
		}
	}

	private string NextColorAvailable () {
		foreach (KeyValuePair<string, bool> activeSupply in activeSupplies) {
			if (activeSupply.Value == true) {
				return activeSupply.Key;
			}
		}
		return "";
	}

	private string NextColorNotAvailable () {
		foreach (KeyValuePair<string, bool> activeSupply in activeSupplies) {
			if (activeSupply.Value == false) {
				return activeSupply.Key;
			}
		}
		return "";
	}

	public void RecoveryColor () {
		string nextoColorNotAvailable = NextColorNotAvailable ();
		if (nextoColorNotAvailable != "") {
			print ("Recupere el color: " + nextoColorNotAvailable);
			activeSupplies [nextoColorNotAvailable] = true;
			ActiveOrInactiveColor (nextoColorNotAvailable);
		}
	}

	// TODO: Move Time.timeScale = 0 logic to Planet.cs and unified with IsDefeatSplashShip
	void IsDeadSplasShip() {
		if (NextColorAvailable() == "") {
			print ("Tas muerto papi");
			Time.timeScale = 0;
		}
	}

	// MonoBehaviour methods

	void Start () {
		colorGenerator = GameObject.Find ("ColorGenerator");
	}

	void Update() {
		ShootSplashColor ();
		ChangeColorSupply ();
		// TODO: Best place for this, after the refactor on line 137? 
		IsDeadSplasShip ();
	}

	void FixedUpdate ()
	{
		Movements ();
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.transform.tag == "Victim") {
			Destroy (other.gameObject);
			RemoveShieldOrColor ();
		}
	}
}