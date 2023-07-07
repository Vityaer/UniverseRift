using Models.City.Arena;
using Network.GameServer;
using System.Collections.Generic;
using Ui.Misc.Widgets;
using UnityEngine;

namespace City.Buildings.Arena
{
    public class PanelSimpleArena : BasePanel
    {
        List<ArenaOpponentModel> opponentsSimpleArena = new List<ArenaOpponentModel>();
        [SerializeField] private List<ArenaOpponentView> arenaOpponentsUI = new List<ArenaOpponentView>();

        //protected override void OnOpen()
        //{
        //    opponentsSimpleArena = new List<ArenaOpponentModel>();
        //    Client.Instance.GetListOpponentSimpleArena(opponentsSimpleArena);
        //    for (int i = 0; i < 3; i++)
        //    {
        //        arenaOpponentsUI[i].SetData(opponentsSimpleArena[i]);
        //    }
        //}
    }
}