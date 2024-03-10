using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RiverOptionHandler : MonoBehaviour
{
    [SerializeField] private Transform riversObject;

    private void OnDisable()
    {
        riversObject.gameObject.SetActive(false);
    }
}
