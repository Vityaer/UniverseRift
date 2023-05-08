using Cysharp.Threading.Tasks;
using Network.DataServer;
using Network.DataServer.Messages;
using UnityEngine;

public class Test : MonoBehaviour
{
    private const string routePlayerRegistration = "Players/Registration";
    private const string routeResource = "Resources/CheckCount";
    public string Name;
    public int Age;

    [ContextMenu("TestRegistration")]
    async UniTaskVoid TestRegistration()
    {
        var data = new Registration { Name = Name };
        var result = await DataServer.PostData(data);
        Debug.Log(result);
    }


    [ContextMenu("GetAllUsers")]
    async UniTaskVoid GetAllUsers()
    {
        var result = await DataServer.DownloadData(routePlayerRegistration);
        Debug.Log(result);
    }

    [ContextMenu("TestCheckResource")]
    async UniTaskVoid TestCheckResource()
    {
        var data = new ResourceEnough { PlayerId = 7, ResourceType = 1, Count = Age, E10 = 0 };
        var result = await DataServer.PostData(data);
        Debug.Log(result);
    }
}
