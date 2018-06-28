using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator : MonoBehaviour {

	Dictionary <string, int[]> hexadecimalColors = new Dictionary <string, int[]> ();

	private int[] currentColorInRgb;

	public string CurrentColor {
		get { return currentColorInRgb.ToString (); }
		set { currentColorInRgb = hexadecimalColors [value]; }
	}

	// Use this for initialization
	void Start () {
		hexadecimalColors.Add ("Red", new int[] {255, 81, 81});
		hexadecimalColors.Add ("Green", new int[] {135, 222, 170});
		hexadecimalColors.Add ("Blue", new int[] {42, 127, 255});

		setColor ("Red");
	}

	public void setColor (string color) {
		CurrentColor = color;

		this.GetComponent <SpriteRenderer> ().color = new Color32 (
			(byte)currentColorInRgb[0], (byte)currentColorInRgb[1], (byte)currentColorInRgb[2], 255
		);
	}
		
}
