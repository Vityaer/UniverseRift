using Models;
using UnityEngine;

[System.Serializable]
public class BaseObject : BaseModel, VisualAPI
{
    protected Sprite sprite = null;
    public virtual Sprite Image { get => sprite; }
    //API
    public bool IsNull() { return (sprite == null); }
    public virtual string GetTextAmount() { return string.Empty; }
    public virtual string GetName() { return "string.Empty"; }
    //Visial API
    public virtual void ClearUI() { this.UI = null; }
    public virtual VisualAPI GetVisual() { return (this as VisualAPI); }
    public virtual void ClickOnItem() { Debug.Log("не переопредили ClickOnItem(BaseObject)"); }
    protected ThingUI UI;
    public virtual void SetUI(ThingUI UI) { Debug.Log("не переопредили SetUI(BaseObject)"); }
    public virtual void UpdateUI() { Debug.Log("не переопредили UpdateUI(BaseObject)"); }
    public virtual BaseObject Clone() { Debug.Log("не переопредили Clone()"); return new BaseObject(); }
}
