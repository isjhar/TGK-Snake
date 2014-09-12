using UnityEngine;
using System.Collections;

public class SnakeController : MonoBehaviour {
	
	// private
	private State state;
	private Direction direction;
	private float tempDelay;
	private Vector3 tileScale, offset;
	private ArrayList snake;
	private ArrayList watcherList;
	private int counter, totalScore,tailPosition, maxWidthTile, minWidthTile, maxHeightTile, minHeightTile, speed;



	// public
	public Transform head;
	public Transform body;
	public Transform tail;
	public Direction directionInitSnake;
	public int initSnakeLenght;
	public int score;
	
	
	// Use this for initialization
	public void Init () {
		// init game


		tileScale = GameObject.Find ("Map").GetComponent<MapManager> ().GetTileScale();
		offset = GameObject.Find ("Map").GetComponent<MapManager> ().GetOffset ();


		//init screen
		maxWidthTile = GameObject.Find ("Map").GetComponent<MapManager> ().GetWidth ()-1;
		minWidthTile = 0;
		maxHeightTile = 0;
		minHeightTile = -GameObject.Find ("Map").GetComponent<MapManager> ().GetHeight()+1;





		// state = State.Start;
		speed = 1;
		snake = new ArrayList ();
		watcherList = new ArrayList ();


		state = State.Play;

	}

	void Update(){
		//CheckState ();
		//Debug.Log (watcherList.Count);
	}
	
	public void CheckState(){
//		Debug.Log (state);
		Tile tileChange;
		BodySnake temp;
		Watcher watcher;
		Direction tempDirection;
		if (state == State.Play){
			temp = (BodySnake) FindHead();
//			Debug.Log (temp.GetTransform().name + " Ular (" + temp.GetTransform().transform.position.x + "," + temp.GetTransform().transform.position.y + ")");
			direction = temp.GetDirection();
			if(direction == Direction.Up || direction == Direction.Down){
				if(Input.GetKeyDown(KeyCode.RightArrow)){
					tempDirection = Direction.Right;
					tileChange = temp.GetTile();
					watcher = new Watcher(tempDirection,tileChange);
					watcherList.Add(watcher);
				} else if(Input.GetKeyDown(KeyCode.LeftArrow)){
					tempDirection = Direction.Left;
					tileChange = temp.GetTile();
					watcher = new Watcher(tempDirection,tileChange);
					watcherList.Add(watcher);
				}
			} else {
				if(Input.GetKeyDown(KeyCode.UpArrow)){
					tempDirection = Direction.Up;
					tileChange = temp.GetTile();
					watcher = new Watcher(tempDirection,tileChange);
					watcherList.Add(watcher);
				} else if(Input.GetKeyDown(KeyCode.DownArrow)){
					tempDirection = Direction.Down;
					tileChange = temp.GetTile();
					watcher = new Watcher(tempDirection,tileChange);
					watcherList.Add(watcher);
				}
			}
		} else if(state == State.GameOver){
			Application.LoadLevel("Game");
			Debug.Log ("load new");
		}
	}
	
	public void SetDirection(){
		BodySnake partOfSnake;
		Watcher watcherTemp;
		Tile currentPosition, watcherTile;
		bool isLast = false;
		BodySnake headSnake = FindHead ();
		headSnake.GetTransform ().GetComponent<BoxCollider> ().enabled = false;
		int indexDeleted = 0;

//		Debug.Log ("Panjang Ular " + snake.Count);
		for(int i = 0;i < snake.Count;i++){
//			Debug.Log (i + ". WatcherList : " + watcherList.Count);
			partOfSnake = (BodySnake) snake[i];
			currentPosition = partOfSnake.GetTile();
			for(int j = 0;j < watcherList.Count;j++){
				watcherTemp = (Watcher) watcherList[j];
				watcherTile = watcherTemp.GetTile();
				if(currentPosition.GetX() == watcherTile.GetX() && currentPosition.GetY() == watcherTile.GetY ()){
					watcherTemp.IncrementNumberOfPass();
//					Debug.Log (partOfSnake.GetTransform().name+ ". bagian tubuh ular ke-" + i + " sama dengan watcher gan");

					RotateSnake(watcherTemp.GetDirection(),partOfSnake.GetDirection(),partOfSnake.GetTransform());
					partOfSnake.SetDirection(watcherTemp.GetDirection());


					if (watcherTemp.GetNumberOfPass() == snake.Count){
						isLast = true;
						indexDeleted = j;
					} 
					break;
				
				}



			}

			//if(isLast && (i == (snake.Count - 1))){
			if(isLast){
				watcherList.RemoveAt(indexDeleted);
				isLast = false;
			}

			Tile newPosition = new Tile (currentPosition.GetX(),currentPosition.GetY ());
			if (partOfSnake.GetDirection() == Direction.Up) {
				if(newPosition.GetY () + speed > maxHeightTile){
					newPosition.SetY(minHeightTile);
				} else {
					newPosition.SetY(newPosition.GetY()+speed);
				}
			} else if(partOfSnake.GetDirection() == Direction.Down){
				if(newPosition.GetY() - speed < minHeightTile){
					newPosition.SetY(maxHeightTile);
				} else {
					newPosition.SetY(newPosition.GetY()-speed);
				}

			} else if(partOfSnake.GetDirection() == Direction.Right){
				if(newPosition.GetX() + speed > maxWidthTile){
					newPosition.SetX(minWidthTile);
				} else {
					newPosition.SetX(newPosition.GetX () + speed);
				}
			} else if(partOfSnake.GetDirection() == Direction.Left){
				if(newPosition.GetX() - speed < minWidthTile){
					newPosition.SetX(maxWidthTile);
				} else {
					newPosition.SetX(newPosition.GetX () - speed);
				}

			}

			partOfSnake.GetTransform().transform.position = GetPosition(newPosition,tileScale,offset);
			partOfSnake.SetTile(newPosition);
		}

		headSnake.GetTransform ().GetComponent<BoxCollider> ().enabled = true;
			
	}

	public void SetState(State state){
		this.state = state;
	}

	public void addLength(){
		BodySnake temp = FindTail ();
		Tile newTailPosition = temp.GetTile ();
		Tile newBodyPosition = new Tile (newTailPosition.GetX (), newTailPosition.GetY ());
		Direction tempDirection = temp.GetDirection ();

		switch (tempDirection) {
		case Direction.Down :
			newTailPosition.SetY(newTailPosition.GetY()+speed);
			break;
		case Direction.Left :
			newTailPosition.SetX(newTailPosition.GetX()+speed);
			break;
		case Direction.Right :
			newTailPosition.SetX(newTailPosition.GetX()-speed);
			break;
		case Direction.Up :
			newTailPosition.SetY(newTailPosition.GetY()-speed);
			break;
		}
		temp.GetTransform ().transform.position = GetPosition(newTailPosition,tileScale,offset);
		Transform transformTemp = (Transform) Instantiate(body,GetPosition(newBodyPosition,tileScale,offset), Quaternion.identity);
		BodySnake newBody = new BodySnake (tempDirection, transformTemp, newBodyPosition); 

		snake.Add (newBody);
		ArrayList list = GameObject.Find ("Food(Clone)").GetComponent<FoodController> ().GetForbidTile ();
		list.Add (newBodyPosition);
//		Debug.Log ("addLength ");
	}

	public void InitHead(Tile tile){
		Transform temp = (Transform) Instantiate (head, GetPosition(tile,tileScale,offset), Quaternion.identity);
		BodySnake bodySnake = new BodySnake (directionInitSnake, temp, tile);
		snake.Add(bodySnake);
	}

	public void InitBody(Tile tile){
		Transform temp = (Transform) Instantiate (body, GetPosition(tile,tileScale,offset), Quaternion.identity);
		BodySnake bodySnake = new BodySnake (directionInitSnake, temp, tile);
		snake.Add(bodySnake);
	}

	public void InitTail(Tile tile){
		Transform temp = (Transform) Instantiate (tail, GetPosition(tile,tileScale,offset), Quaternion.identity);
		BodySnake bodySnake = new BodySnake (directionInitSnake, temp, tile);
		snake.Add(bodySnake);
	}

	public BodySnake FindTail(){
		BodySnake temp = null;
		for (int i = 0; i < snake.Count; i++) {
			temp = (BodySnake) snake[i];
			if(temp.GetTransform().name.Equals("Tail(Clone)")){
				break;
			}
		}
		return temp;
	}

	public BodySnake FindHead(){
		BodySnake temp = null;
		for (int i = 0; i < snake.Count; i++) {
			temp = (BodySnake) snake[i];
			if(temp.GetTransform().name.Equals("Head(Clone)")){
				return temp;
			}
		}
		return temp;
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

	public Vector3 GetPosition(Tile tile, Vector3 scale, Vector3 offset){
		Vector3 tempPosition = new Vector3((tile.GetX()*scale.x)+offset.x,(tile.GetY()*scale.y)-offset.y,0);
		return tempPosition;
	}

	public void RotateSnake(Direction from, Direction to, Transform snake){
		switch(from){
		// Dari Bawah
		case Direction.Down  : 
			switch(to) {
			case Direction.Right :
				snake.transform.Rotate(new Vector3(snake.transform.rotation.x, snake.transform.rotation.y, 90));
				break;
			case Direction.Left :
				snake.transform.Rotate(new Vector3(snake.transform.rotation.x, snake.transform.rotation.y, -90));
				break;
			}
			break;

		// Dari Atas
		case Direction.Up  : 
			switch(to) {
			case Direction.Right :
				snake.transform.Rotate(new Vector3(snake.transform.rotation.x, snake.transform.rotation.y, -90));
				break;
			case Direction.Left :
				snake.transform.Rotate(new Vector3(snake.transform.rotation.x, snake.transform.rotation.y, 90));
				break;
			}
			break;

		// Dari Kiri
		case Direction.Left  : 
			switch(to) {
			case Direction.Up :
				snake.transform.Rotate(new Vector3(snake.transform.rotation.x, snake.transform.rotation.y, -90));
				break;
			case Direction.Down :
				snake.transform.Rotate(new Vector3(snake.transform.rotation.x, snake.transform.rotation.y, 90));
				break;
			}
			break;

		// Dari Kanan
		case Direction.Right  : 
			switch(to) {
			case Direction.Up :
				snake.transform.Rotate(new Vector3(snake.transform.rotation.x, snake.transform.rotation.y, 90));
				break;
			case Direction.Down :
				snake.transform.Rotate(new Vector3(snake.transform.rotation.x, snake.transform.rotation.y, -90));
				break;
			}
			break;
		}
	}
}
