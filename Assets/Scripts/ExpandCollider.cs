using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ExpandCollider : MonoBehaviour
{
    [SerializeField] SphereCollider zoneCollider;
    [SerializeField] float duration, colliderScaleEnd, scale;


    private void Start()
    {
    }

    private void Update()
    {
        DoShit();
    }

    void DoShit()
    {
        if(zoneCollider != null)
        {
            DOTween.timeScale = scale;
            DOTween.To(() => zoneCollider.radius, x => zoneCollider.radius = x, colliderScaleEnd, duration);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, zoneCollider.radius);
    }
}
