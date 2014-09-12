using UnityEngine;
using System.Collections;

public class BodySnake {
	private Direction direction;
	private Transform transform;
	private Tile tile;

	public BodySnake(Direction direction, Transform transform, Tile tile){
		this.direction = direction;
		this.transform = transform;
		this.tile = tile;
	}

	public Direction GetDirection(){
		return direction;
	}

	public Transform GetTransform(){
		return transform;
	}

	public Tile GetTile(){
		return tile;
	}

	public void SetDirection(Direction direction){
		this.direction = direction;
	}

	public void SetTransform(Transform transform){
		this.transform = transform;
	}

	public void SetTile(Tile tile){
		this.tile = tile;
	}


}
