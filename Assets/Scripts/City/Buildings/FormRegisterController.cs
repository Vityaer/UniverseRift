using Cysharp.Threading.Tasks;
using Network.DataServer.Messages;
using Network.DataServer;
using ObjectSave;
using UnityEngine;
using System;

public class FormRegisterController : Building
{
    [SerializeField] private PanelRegistration panelRegistration;
    private PlayerInfo playerInfo;

    protected override void Start()
    {
        GameController.Instance.RegisterOnLoadGame(OnLoadGame);
    }

    protected override void OnLoadGame()
    {
        Debug.Log("name: " + GameController.GetPlayerInfo.Name);
        if (GameController.GetPlayerInfo.Name.Equals(string.Empty))
        {
            Debug.Log("OpenForRegister");
            panelRegistration.Open();
        }
    }

    public async UniTaskVoid CreateAccount(string name)
    {
        var data = new Registration { Name = name };
        var result = await DataServer.PostData(data);
        if (Int32.TryParse(result, out int id))
        {
            GameController.Instance.RegisterPlayer(name, id);
            GetStartPack();
            SaveGame();
        }
    }

    private void GetStartPack()
    {
        GameController.Instance.AddResource(new Resource(TypeResource.SimpleHireCard, 10));
        GameController.Instance.AddResource(new Resource(TypeResource.Diamond, 100));
        GameController.Instance.AddResource(new Resource(TypeResource.CoinFortune, 5));
    }
}