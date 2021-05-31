using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 m_Movement = Vector3.zero;


    void Update()
    {
        GetInputs();
    }

    protected void GetInputs()
    {
        m_Movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
    }
}
