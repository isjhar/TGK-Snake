using UnityEngine;
using System.Collections;

public class BodySnake {
	private Direction direction;
	private Transform transform;

	public BodySnake(Direction direction, Transform transform){
		this.direction = direction;
		this.transform = transform;
	}

	public Direction GetDirection(){
		return direction;
	}

	public Transform GetTransform(){
		return transform;
	}

	public void SetDirection(Direction direction){
		this.direction = direction;
	}

	public void SetTransform(Transform transform){
		this.transform = transform;
	}


}
