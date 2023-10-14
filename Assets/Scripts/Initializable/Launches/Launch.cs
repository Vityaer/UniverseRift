using UnityEngine;
using Sirenix.OdinInspector;

namespace Initializable.Launches
{
    public class Launch : MonoBehaviour
    {
        [EnumToggleButtons, HideLabel]
        [SerializeField] private LaunchType _launchType;
    }
}
