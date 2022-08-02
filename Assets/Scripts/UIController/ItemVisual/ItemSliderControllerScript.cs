using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemSliderControllerScript : MonoBehaviour{

	public Slider slider;
	public TextMeshProUGUI textSlider;

    void Awake(){
    	if(slider == null) GetComponents();
    }

    public void SetAmount(int currentAmount, int maxAmount){
    	if(slider == null) GetComponents();
        slider.maxValue = maxAmount; 
        slider.value = currentAmount;
		textSlider.text = FunctionHelp.AmountFromRequireCount(currentAmount, maxAmount);
    	Show();
    }
    public void SetAmount(BigDigit currentAmount, BigDigit maxAmount){
    	if(slider == null) GetComponents();
        slider.maxValue = 1f;
        slider.value = (currentAmount/maxAmount).ToFloat();
        textSlider.text = FunctionHelp.AmountFromRequireCount(currentAmount, maxAmount);
        Show();
    }
    public void SetAmount(Resource currentResource, Resource maxResource){
        SetAmount(currentResource.Amount, maxResource.Amount);
    }
    void GetComponents(){
        slider = GetComponent<Slider>();
    }
    public void Hide(){
    	gameObject.SetActive(false);
    }
    void Show(){
    	gameObject.SetActive(true);
    } 
}
