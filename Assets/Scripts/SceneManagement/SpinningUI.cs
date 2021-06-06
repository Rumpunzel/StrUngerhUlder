using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningUI : MonoBehaviour
{
    public float rotateSpeed = 200f;

    private RectTransform m_RectComponent;


	private void Start()
	{
		m_RectComponent = GetComponent<RectTransform>();
	}
	private void Update()
	{
		m_RectComponent.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
	}
}
