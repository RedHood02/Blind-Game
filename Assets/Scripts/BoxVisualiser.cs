using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoxVisualiser : MonoBehaviour
{
	[SerializeField] Transform boxCollider;

    private void Awake()
    {
		boxCollider = GetComponent<Transform>();
    }

    private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Vector3 vect = new(boxCollider.localScale.x, boxCollider.localScale.y, boxCollider.localScale.z);
		Gizmos.DrawWireCube(transform.position, vect);
	}
}
