using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildScript : Building{
    public Guild guildInfo;
    void Start(){
		   
    }
}

public class Guild{
	private int id;
	public int ID{get => id;}

	private string name;
	public string Name{get => name;}

	private int level;
	public int Level{get => level;}

	public Guild(int id, string name, int level){
		this.id = id;
		this.name = name;
		this.level = level;
	}
}
