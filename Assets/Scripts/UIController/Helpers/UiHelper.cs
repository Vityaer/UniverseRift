using Sirenix.OdinInspector;
using UnityEngine;
using Utils.Development;

namespace UI.Helpers
{
    public abstract class UiHelper : MonoBehaviour, ICreatable
    {
        private static GameObject _sfxObject;

        [SerializeField] protected AudioClip Sound;

        protected virtual string DefaultAudioClipName => "ButtonClick.mp3";

        private void Awake()
        {
            if (_sfxObject == null)
                _sfxObject = Resources.Load<GameObject>(Constants.ResourcesPath.SFX_PREFAB);
        }

        protected void PlaySound()
        {
            //var sfxPlayer = Instantiate(_sfxObject).GetComponent<SFXPlayer>();
            //sfxPlayer.Play(Sound);
        }

        [Button("GetAllComponents")]
        public void OnCreateComponent()
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this, "Get components");
            GetComponents();
            Sound = (AudioClip)UnityEditor.AssetDatabase.LoadAssetAtPath($"Assets/Resources/Sounds/SFX/{DefaultAudioClipName}", typeof(AudioClip));
            UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
#endif
        }

        protected abstract void GetComponents();
        protected abstract bool CanWork();
    }
}