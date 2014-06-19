using UnityEngine;
using System.Collections;

public class SnakeController : MonoBehaviour {
	
	// private
	private State state;
	Direction direction;
	float speed, tempDelay, maxWidthScreen, minWidthScreen, maxHeightScreen, minHeightScreen;
	ArrayList snake;
	ArrayList watcherList;
	int counter, totalScore;
	
	
	// public
	public float delay;
	public Transform head;
	public Transform body;
	public int initSnakeLenght;
	public int score;
	
	
	// Use this for initialization
	void Start () {
		// init game
		state = State.Start;
		speed = head.localScale.x;
		tempDelay = delay;
		snake = new ArrayList ();
		watcherList = new ArrayList ();
		
		
		// init ular
		Transform temp = (Transform) Instantiate (head, new Vector3 (0.125f,0.125f,0), Quaternion.identity);
		BodySnake bodySnake = new BodySnake (Direction.Right, temp);
		snake.Add(bodySnake);
		float snakeLength = -0.125f;
		for (int i = 0; i < initSnakeLenght; i++) {
			temp = (Transform) Instantiate(body,new Vector3(snakeLength,0.125f,0),Quaternion.identity);
			bodySnake = new BodySnake (Direction.Right, temp);
			snake.Add(bodySnake);
			snakeLength -= speed;
		}

		//init screen
		maxWidthScreen = Camera.main.orthographicSize * Camera.main.aspect;
		minWidthScreen = -maxWidthScreen;
		maxHeightScreen = Camera.main.orthographicSize;
		minHeightScreen = -maxHeightScreen;
	
		
	}
	
	// Update is called once per frame
	void Update () {
		CheckState ();
		if(tempDelay < 0){
			SetDirection ();
			tempDelay = delay;
		} else {
			tempDelay -= Time.deltaTime;
		}
	}
	
	void CheckState(){
		Vector3 pointChange;
		BodySnake temp;
		Watcher watcher;
		Direction tempDirection;
		if (state == State.Start) {
			if(Input.GetKey (KeyCode.Space)){
				state = State.Play;
				GameObject.Find("Start").GetComponent<GUIText>().enabled = false;
				GameObject.Find("Score").GetComponent<GUIText>().enabled = true;
				GameObject.Find("Value").GetComponent<GUIText>().enabled = true;
				MakananController makananCtrl = GameObject.Find ("Makanan").GetComponent<MakananController> ();
				makananCtrl.CreateMakanan ();
			}
		} else if (state == State.Play){
			temp = (BodySnake) snake[0];
			//Debug.Log ("Head Ular (" + temp.GetTransform().transform.position.x + "," + temp.GetTransform().transform.position.y + ")");
			direction = temp.GetDirection();
			if(direction == Direction.Up || direction == Direction.Down){
				if(Input.GetKeyDown(KeyCode.RightArrow)){
					tempDirection = Direction.Right;
					pointChange = temp.GetTransform().transform.position;
					watcher = new Watcher(tempDirection,pointChange);
					watcherList.Add(watcher);
				} else if(Input.GetKeyDown(KeyCode.LeftArrow)){
					tempDirection = Direction.Left;
					pointChange = temp.GetTransform().transform.position;
					watcher = new Watcher(tempDirection,pointChange);
					watcherList.Add(watcher);
				}
			} else {
				if(Input.GetKeyDown(KeyCode.UpArrow)){
					tempDirection = Direction.Up;
					pointChange = temp.GetTransform().transform.position;
					watcher = new Watcher(tempDirection,pointChange);
					watcherList.Add(watcher);
				} else if(Input.GetKeyDown(KeyCode.DownArrow)){
					tempDirection = Direction.Down;
					pointChange = temp.GetTransform().transform.position;
					watcher = new Watcher(tempDirection,pointChange);
					watcherList.Add(watcher);
				}
			}
		} else if(state == State.GameOver){
			if(Input.GetKeyDown(KeyCode.Space)){
				Application.LoadLevel("Menu");

			}
		}
	}
	
	void SetDirection(){
		BodySnake partOfSnake;
		Watcher watcherTemp;
		Vector3 currentPosition, watcherPoint;
		bool isLast = false;

		if (state == State.Play){
			for(int i = 0;i < snake.Count;i++){
				partOfSnake = (BodySnake) snake[i];
				currentPosition = partOfSnake.GetTransform().transform.position;
				for(int j = 0;j < watcherList.Count;j++){
					watcherTemp = (Watcher) watcherList[j];
					watcherPoint = watcherTemp.GetPoint();
					if(currentPosition == watcherPoint){
						partOfSnake.SetDirection(watcherTemp.GetDirection());

						if(i == 0){
							BoxCollider collider = partOfSnake.GetTransform().GetComponent<BoxCollider>();
							Vector3 newColliderPosition = new Vector3();
							newColliderPosition = collider.center;
							float center = partOfSnake.GetTransform().localScale.y;
							switch(partOfSnake.GetDirection()){
								case Direction.Up :
									newColliderPosition.x = 0;
									newColliderPosition.y = center;
								break;
								case Direction.Down :
									newColliderPosition.x = 0;
									newColliderPosition.y = -center;
									break;
								case Direction.Left :
									newColliderPosition.x = -center;
									newColliderPosition.y = 0;
									break;
								case Direction.Right :
									newColliderPosition.x = center;
									newColliderPosition.y = 0;
									break;
							}
							collider.center = newColliderPosition;
						}

						if (i == (snake.Count - 1)){
							isLast = true;
						} 
					}
				}

				if(isLast){
					watcherList.RemoveAt(0);
					isLast = false;
				}

				Vector3 newPosition = new Vector3 (currentPosition.x,currentPosition.y,currentPosition.z);
				if (partOfSnake.GetDirection() == Direction.Up) {
					if(newPosition.y + speed > maxHeightScreen){
						newPosition.y = -newPosition.y;
					} else {
						newPosition.y += speed;
					}
				} else if(partOfSnake.GetDirection() == Direction.Down){
					if(newPosition.y - speed < minHeightScreen){
						newPosition.y = -newPosition.y;
					} else {
						newPosition.y -= speed;
					}

				} else if(partOfSnake.GetDirection() == Direction.Right){
					if(newPosition.x + speed > maxWidthScreen){
						newPosition.x = -newPosition.x;
					} else {
						newPosition.x += speed;
					}
				} else if(partOfSnake.GetDirection() == Direction.Left){
					if(newPosition.x - speed < minWidthScreen){
						newPosition.x = -newPosition.x;
					} else {
						newPosition.x -= speed;
					}

				}

				partOfSnake.GetTransform().transform.position = newPosition;

			}
			
		}
	}

	public void SetState(State state){
		this.state = state;
	}

	public void addLength(){
		BodySnake temp = (BodySnake)snake [snake.Count - 1];


		Vector3 newBodyPosition = temp.GetTransform ().transform.position;
		Direction tempDirection = temp.GetDirection ();

		switch (tempDirection) {
		case Direction.Down :
			newBodyPosition.y += speed; 
			break;
		case Direction.Left :
			newBodyPosition.x += speed;
			break;
		case Direction.Right :
			newBodyPosition.x -= speed;
			break;
		case Direction.Up :
			newBodyPosition.y -= speed;
			break;
		}
		Transform transformTemp = (Transform) Instantiate(body,newBodyPosition,Quaternion.identity);
		BodySnake newBody = new BodySnake (tempDirection, transformTemp); 
		snake.Add (newBody);
	}

	public ArrayList GetWatcherList(){
		return watcherList;
	}

	public ArrayList GetSnake(){
		return snake;
	}

	public void addScore(){
		counter++;
		totalScore = counter * score;
	}

	public int GetTotalScore(){
		return totalScore;
	}
}
