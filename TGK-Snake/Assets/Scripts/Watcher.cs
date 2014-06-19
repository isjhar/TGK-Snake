using UnityEngine;
using System.Collections;

public class Watcher {
	private Direction direction;
	private Vector3 point;

	public Watcher (Direction direction, Vector3 point){
		this.direction = direction;
		this.point = point;
	}

	public Direction GetDirection(){
		return direction;
	}

	public Vector3 GetPoint(){
		return point;
	}
}
