using UnityEngine;
using System.Collections;

public class Watcher {
	private Direction direction;
	private Tile tile;
	private int numberOfPass;

	public Watcher (Direction direction, Tile tile){
		this.direction = direction;
		this.tile = tile;
		numberOfPass = 0;
	}

	public Direction GetDirection(){
		return direction;
	}

	public Tile GetTile(){
		return tile;
	}

	public int GetNumberOfPass(){
		return numberOfPass;
	}

	public void IncrementNumberOfPass(){
		numberOfPass++;
	}
}
