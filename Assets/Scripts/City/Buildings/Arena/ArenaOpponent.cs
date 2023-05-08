using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArenaOpponent
{
	[Header("Info")]
	[SerializeField] private string name;
	[SerializeField] private Sprite avatar;
	[SerializeField] private int level, score, winCount, loseCount;
	[SerializeField]
	[Header("Squad")]
	public Mission mission;

	public string Name => name;
    public Sprite Avatar => avatar;
    public int Level => level;
	public int Score => score;
	public int WinCount => winCount;
	public int LoseCount => loseCount;

}