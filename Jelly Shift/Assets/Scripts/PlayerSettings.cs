using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upperpik/Settings")]
public class PlayerSettings : ScriptableObject
{
    public bool isPlaying;
    public float speed;
    public float borderMin;
    public float borderMax;
    public float borderHeightMin;
    public float borderHeightMax;
    public float minX, maxY;
    public float maxX, minY;
    public float sensitivity;
}
