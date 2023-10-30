using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteText : MonoBehaviour
{
    [SerializeField] Transform player;

    private void Update()
    {
        if(player.position.x > transform.position.x)
        {
            Destroy(gameObject);
        }
    }
}
