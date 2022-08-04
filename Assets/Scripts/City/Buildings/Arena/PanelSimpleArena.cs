using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSimpleArena : BasePanelScript{
    List<ArenaOpponent> opponentsSimpleArena = new List<ArenaOpponent>();
    [SerializeField] private List<ArenaOpponentUI> arenaOpponentsUI = new List<ArenaOpponentUI>();
	protected override void OnOpen(){
    	opponentsSimpleArena = new List<ArenaOpponent>();
    	Client.Instance.GetListOpponentSimpleArena(opponentsSimpleArena);
    	for(int i = 0; i < 3; i++){
    		arenaOpponentsUI[i].SetData(opponentsSimpleArena[i]);
    	}
	}
}