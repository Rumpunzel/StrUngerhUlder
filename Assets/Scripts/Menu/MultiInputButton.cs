using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// An extension of Unity's base Button class, to support input from both mouse and keyboard/joypad
/// </summary>
[AddComponentMenu("Strungerhulder/UI/MultiInputButton")]
public class MultiInputButton : Button
{
    public bool isSelected;

	private MenuSelectionHandler m_MenuSelectionHandler;
	

	private new void Awake()
	{
		m_MenuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();
	}


	public override void OnPointerEnter(PointerEventData eventData)
	{
		m_MenuSelectionHandler.HandleMouseEnter(gameObject);
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		m_MenuSelectionHandler.HandleMouseExit(gameObject);
	}

	public override void OnSelect(BaseEventData eventData)
	{
		Debug.Log("Onselect ");
		isSelected = true;
		m_MenuSelectionHandler.UpdateSelection(gameObject);
		base.OnSelect(eventData);
	}

	public void UpdateSelected()
	{
		if (m_MenuSelectionHandler == null)
			m_MenuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();
		Debug.Log("UpdateSelected");
		m_MenuSelectionHandler.UpdateSelection(gameObject);
	}

	public override void OnSubmit(BaseEventData eventData)
	{
		if (m_MenuSelectionHandler.AllowsSubmit())
			base.OnSubmit(eventData);
	}
}
