using UnityEngine;

public class ArtifactSystem : MonoBehaviour
{
    public ItemsList artifactList;

    public Item GetArtifact(int ID) { return artifactList.GetArtifact(ID); }
    void Awake()
    {
        instance = this;
    }
    private static ArtifactSystem instance;
    public static ArtifactSystem Instance { get => instance; }

}
