using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[ExecuteInEditMode]
public class BoxVisualiser : MonoBehaviour
{
	[SerializeField] BoxCollider boxCollider;

    private void Awake()
    {
		boxCollider = GetComponent<BoxCollider>();
		boxCollider.isTrigger = true;
    }


    private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Vector3 vect = new(boxCollider.size.x, boxCollider.size.y, boxCollider.size.z);
		Gizmos.DrawWireCube(transform.position, vect);
	}
}
