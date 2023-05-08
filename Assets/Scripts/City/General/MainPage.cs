using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MainPage : MonoBehaviour
{
	[SerializeField] private FooterButton btnOpenClose;

    protected virtual void Awake()
	{
		btnOpenClose.RegisterOnChange(Change);
	}

    private void Change(bool isOpen)
	{
		if(isOpen)
		{ 
			Open(); 
		}
		else
		{ 
			Close();
		}
	}

    public virtual void Open()
    {
        MenuController.Instance.CurrentPage = this;
    }

    public abstract void Close();
}