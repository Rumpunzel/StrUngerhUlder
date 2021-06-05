using System;
using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("Strungerhulder/UI/MultiInputSelectableElement")]
public class MultiInputSelectableElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
	private MenuSelectionHandler m_MenuSelectionHandler;


	private void Awake()
	{
		m_MenuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();
	}


	public void OnPointerEnter(PointerEventData eventData)
	{
		m_MenuSelectionHandler.HandleMouseEnter(gameObject);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		m_MenuSelectionHandler.HandleMouseExit(gameObject);
	}

	public void OnSelect(BaseEventData eventData)
	{
		m_MenuSelectionHandler.UpdateSelection(gameObject);
	}
}
