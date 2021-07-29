using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockScript : MonoBehaviour
{
    private Material blockMaterial;

    private void Start()
    {
        blockMaterial = GetComponent<MeshRenderer>().material;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            blockMaterial.DOFade(0, 1f).OnComplete(() => Destroy(gameObject, 0.2f));
        }
    }
}
