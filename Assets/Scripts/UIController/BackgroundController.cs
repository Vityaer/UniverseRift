using UnityEngine;

public class BackgroundController : MonoBehaviour
{

    private static BackgroundController instance;
    public static BackgroundController Instance { get => instance; }
    public GameObject currentBackground;

    [Header("List background")]
    public GameObject cityBackground;

    void Awake()
    {
        instance = this;
    }

    public void OpenBackground(GameObject newBackground)
    {
        if (currentBackground != null) currentBackground.SetActive(false);
        currentBackground = newBackground;
        currentBackground.SetActive(true);
    }

    public void OpenCityBackground()
    {
        OpenBackground(cityBackground);
    }
}
