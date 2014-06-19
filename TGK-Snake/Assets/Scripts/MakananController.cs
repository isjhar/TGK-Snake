using UnityEngine;
using System.Collections;

public class MakananController : MonoBehaviour {
	float maxWidthScreen, maxHeightScreen, scale, multiple, startPoint;
	int maxWidthCell, minWidthCell, maxHeightCell, minHeightCell;
	public float offset;

	public Transform makanan;
	void Start(){
		maxWidthScreen = Camera.main.aspect * Camera.main.orthographicSize;
		maxHeightScreen = Camera.main.orthographicSize;
		scale = makanan.transform.localScale.x;
		multiple = 0.25f;
		startPoint = 0.125f;

		maxHeightCell = (int) Mathf.Ceil(maxHeightScreen / (scale * 2));
		maxHeightCell++;
		minHeightCell = -maxHeightCell;

		maxWidthCell = (int) Mathf.Ceil(maxWidthScreen / (scale * 2));
		maxWidthCell += 1;
		minWidthCell = -maxWidthCell + 1;
	}

	public void CreateMakanan(){
		SnakeController snakeCtrl = GameObject.Find ("Snake").GetComponent<SnakeController> ();

		float positionX, positionY; 
		int GeneratePositionX, GeneratePositionY;

		GeneratePositionX = Random.Range (minWidthCell, maxWidthCell);
		GeneratePositionY = Random.Range (minHeightCell, maxHeightCell);
		positionX = (GeneratePositionX * multiple) + startPoint;
		positionY = (GeneratePositionY * multiple) + startPoint;

		BodySnake temp;
		for (int i = 0; i < snakeCtrl.GetSnake().Count; i++) {
			temp = (BodySnake) snakeCtrl.GetSnake()[i];
			if(temp.GetTransform().transform.position.x == positionX && temp.GetTransform().transform.position.y == positionY){
				positionX += (scale * 2);
			}
		}
		Instantiate (makanan, new Vector3 (positionX, positionY, 0), Quaternion.identity);
	}
}
