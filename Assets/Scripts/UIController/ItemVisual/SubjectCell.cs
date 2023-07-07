using Common;
using Common.Resourses;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace UIController.ItemVisual
{
    public class SubjectCell : UiView
    {
        public Image Image;
        public Image Background;
        public Backlight Backlight;
        public TMP_Text Amount;

        public void SetData<T>(T baseObject) where T : BaseObject
        {
            Image.enabled = true;
            Image.sprite = baseObject.Image;
            //Background.sprite = baseObject.Rating;
            Amount.text = baseObject.ToString();

            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }

        public void Clear()
        {
            Image.enabled = false;
            Amount.text = string.Empty;
        }

        public void Disable()
        {
            Clear();
            gameObject.SetActive(false);
        }

    }
}
