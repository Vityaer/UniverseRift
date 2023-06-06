using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThingUI : MonoBehaviour
{
    [Header("Info")]
    public Image border;
    public Image background;
    public Backlight backlight;
    public Image imageThing;
    public RatingHero ratingThing;
    public ItemSliderController sliderAmount;
    public TextMeshProUGUI textAmount;
    public GameObject doneForUse;
    public Image selectBorder;
    public Button SubjectButton;

    [Header("controller")]
    public bool OnlyMainData = false;

    public void UpdateUI(Sprite image, string text)
    {
        if (imageThing.sprite != null) Clear();
        imageThing.sprite = image;
        //ratingThing?.ShowRating(rating);
        textAmount.text = text;
        imageThing.enabled = true;
    }

    public void UpdateUI(Sprite image, int amount = 0)
    {
        Clear();
        imageThing.sprite = image;
        textAmount.text = (amount > 0) ? amount.ToString() : string.Empty;
        //ratingThing?.ShowRating(rating);
        imageThing.enabled = true;
    }

    public void UpdateUI(Resource res)
    {
        Clear();
        Debug.Log(res.ToString());
        imageThing.sprite = res.Image;
        textAmount.text = res.ToString();
        imageThing.enabled = true;
    }

    public void UpdateUI(SplinterController splinterController)
    {
        Clear();
        imageThing.sprite = splinterController.splinter.Image;
        if (sliderAmount != null)
        {
            if (OnlyMainData == false)
            {
                sliderAmount.SetAmount(splinterController.splinter.Amount, splinterController.splinter.RequireAmount);
            }
            else
            {
                sliderAmount.Hide();
            }
        }
        imageThing.enabled = true;
    }

    public void Select()
    {
        selectBorder.enabled = true;
    }

    public void Diselect()
    {
        selectBorder.enabled = false;
    }

    public void SwitchDoneForUse(bool flag)
    {
        if (doneForUse != null)
            doneForUse.SetActive(flag);
    }

    public void Clear()
    {
        imageThing.sprite = null;
        ratingThing?.ShowRating(0);
        sliderAmount?.Hide();
        imageThing.enabled = false;
        textAmount.text = string.Empty;
        Diselect();
        SwitchDoneForUse(false);
    }
}
