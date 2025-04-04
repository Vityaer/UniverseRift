﻿using UnityEngine;

namespace Effects
{
    public class CameraShake : MonoBehaviour
    {

        public enum ShakeMode { OnlyX, OnlyY, OnlyZ, XY, XZ, XYZ };

        private static Transform tr;
        private static float elapsed, i_Duration, i_Power, percentComplete;
        private static ShakeMode i_Mode;
        private static Vector3 originalPos;
        public Vector3 OriginalPos { set => originalPos = value; }

        void Awake()
        {
            tr = gameObject.GetComponent<Transform>();
        }

        void Start()
        {
            percentComplete = 1;
        }

        public static void Shake(float duration, float power)
        {
            if (percentComplete == 1) originalPos = tr.localPosition;
            i_Mode = ShakeMode.XYZ;
            elapsed = 0;
            i_Duration = duration;
            i_Power = power;
        }

        public static void Shake(float duration, float power, ShakeMode mode)
        {
            if (percentComplete == 1) originalPos = tr.localPosition;
            i_Mode = mode;
            elapsed = 0;
            i_Duration = duration;
            i_Power = power;
        }

        void Update()
        {
            if (elapsed < i_Duration)
            {
                elapsed += Time.deltaTime;
                percentComplete = elapsed / i_Duration;
                percentComplete = Mathf.Clamp01(percentComplete);
                Vector3 rnd = Random.insideUnitSphere * i_Power * (1f - percentComplete) / 10;

                switch (i_Mode)
                {
                    case ShakeMode.XYZ:
                        tr.localPosition = originalPos + rnd;
                        break;
                    case ShakeMode.OnlyX:
                        tr.localPosition = originalPos + new Vector3(rnd.x, 0, 0);
                        break;
                    case ShakeMode.OnlyY:
                        tr.localPosition = originalPos + new Vector3(0, rnd.y, 0);
                        break;
                    case ShakeMode.OnlyZ:
                        tr.localPosition = originalPos + new Vector3(0, 0, rnd.z);
                        break;
                    case ShakeMode.XY:
                        tr.localPosition = originalPos + new Vector3(rnd.x, rnd.y, 0);
                        break;
                    case ShakeMode.XZ:
                        tr.localPosition = originalPos + new Vector3(rnd.x, 0, rnd.z);
                        break;
                }
            }
        }
    }
}