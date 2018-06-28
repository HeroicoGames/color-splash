using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

	[System.Serializable]
	public struct VictimConstraint {
		public GameObject victim;
		public int number;
	}
	public int victimsToRescue;
	public int victimsToGenerate;
	public List<VictimConstraint> victimConstraints = new List<VictimConstraint> ();
	public GameObject splashShip;
	public GameObject[] spawnPoints;

	private int victimsGenerated;
	private int victimsRescued;

	public int VictimsRescued {
		get { return victimsRescued; }
		set { victimsRescued = value; }
	}

	public int CountVictims() {
		return GameObject.FindGameObjectsWithTag ("Victim").Length;
	}

	public int CountVictimsByColor(string color) {
		/// color: Red, Blue or Green.
		/// Sensible case
		string name = color + "Victims";
		return GameObject.Find(name).transform.childCount;
	}

	public void SetVictimParent(GameObject victim, string color) {
		/// color: Red, Blue or Green.
		/// Sensible case
		victim.transform.SetParent(GameObject.Find(color + "Victims").transform);
	}

	void GenerateVictims() {

		if (victimsGenerated < victimsToGenerate) {
			// TODO: Estas variables se ven desordenadas aquí
			int randomSpawn = Random.Range (0, spawnPoints.Length);
			int randomVictim = Random.Range (0, victimConstraints.Count);
			GameObject victimPrefab = victimConstraints [randomVictim].victim;
			GameObject spawnPoint = spawnPoints [randomSpawn];
			VictimSpawn victimSpawn = spawnPoint.GetComponent <VictimSpawn> ();
			string colorName = victimPrefab.GetComponent<Victim> ().name;

			if (victimConstraints[randomVictim].number > CountVictimsByColor(colorName) && victimSpawn.CountDown <= 0) {
				GameObject instantiateVictim = Instantiate (
					victimPrefab, spawnPoint.transform.position, Quaternion.identity
				) as GameObject;

				SetVictimParent (instantiateVictim, colorName);

				victimsGenerated ++;
			}

			victimSpawn.setWaitTime ();
		}

	}

	void IsFinishMission() {

		bool isCompletedMission = IsCompletedMission ();
		bool isFailedMission = IsFailedMission ();
		bool isDefeatSplashShip = IsDefeatSplashShip ();

		if (isCompletedMission == true) {
			print ("Winner!!!");
			Time.timeScale = 0;
		} else if (isFailedMission == true) {
			print ("Failed mission");
			Time.timeScale = 0;
		} else if (isDefeatSplashShip == true) {
			print ("Victims won");
			Time.timeScale = 0;
		}

	}

	private bool IsCompletedMission() {

		if (CountVictims() == 0 && victimsGenerated >= victimsToGenerate) {
			if (victimsRescued >= victimsToRescue) {
				return true;
			}
		}
			
		return false;
	}

	private bool IsFailedMission() {

		if (CountVictims() == 0 && victimsGenerated >= victimsToGenerate) {
			if (victimsRescued < victimsToRescue) {
				return true;
			}
		}
	
		return false;
	}

	// TODO: Unified logic with IsDeadSplasShip() method of SplashShip.cs
	private bool IsDefeatSplashShip() {
		return false;
	}

	// Update is called once per frame
	void Update () {
		GenerateVictims ();
		IsFinishMission ();
	}
}
