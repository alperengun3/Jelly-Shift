using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private Camera uiCamera;

    private static ObjectManager instance;

   
    public Camera UiCamera { get => uiCamera; set => uiCamera = value; }
    public static ObjectManager Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
