using UnityEngine;
using System.Collections;

public class CollideDetector : MonoBehaviour {
	SnakeController snakeCtrl;
	MakananController makananCtrl;
	GUIText textScore;
	// Use this for initialization
	void Start () {
		snakeCtrl = GameObject.Find ("Snake").GetComponent<SnakeController> ();
		makananCtrl = GameObject.Find ("Makanan").GetComponent<MakananController> ();
		textScore = GameObject.Find ("Value").GetComponent<GUIText> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter( Collider other ){
		if (other.gameObject.name.Equals ("Body(Clone)")) {
			snakeCtrl.SetState (State.GameOver);	
			GameObject.Find("GameOver").GetComponent<GUIText>().enabled = true;
			GameObject.Find("SpaceBar").GetComponent<GUIText>().enabled = true;

		} else {
			Destroy(other.gameObject);
			makananCtrl.CreateMakanan();
			snakeCtrl.addLength();
			snakeCtrl.addScore();
			textScore.text = snakeCtrl.GetTotalScore().ToString();
		}

	}
}
