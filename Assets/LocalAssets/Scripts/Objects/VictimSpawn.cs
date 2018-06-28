using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictimSpawn : MonoBehaviour {
	private int countDown = 0;

	public int CountDown {
		get { return countDown; }
		set { countDown = value; }
	}

	public void setWaitTime () {
		if (this.CountDown <= 0) {
			this.CountDown = 5 * 60;
		}
	}

	public void updateCountDown () {
		if (countDown > 0) {
			countDown -= 1;
		}
	}

	void Update () {
		updateCountDown ();
	}
}
