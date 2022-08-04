using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArenaOpponent{
	[Header("Info")]
	[SerializeField] private string name;
	public string Name{get => name;}
	[SerializeField] private Sprite avatar;
	public Sprite Avatar{get => avatar;}
	[SerializeField] private int level, score, winCount, loseCount;
	public int Level{get => level;}
	public int Score{get => score;}
	public int WinCount{get => winCount;}
	public int LoseCount{get => loseCount;}
	[Header("Squad")]
	[SerializeField]
	public Mission mission;
}