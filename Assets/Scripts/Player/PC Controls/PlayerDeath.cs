using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] GameObject canvasVisor;
    [SerializeField] StudioEventEmitter deathSFX;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            GetComponent<PCMovement>().canMove = false;
            canvasVisor.SetActive(true);
            deathSFX.Play();
            Destroy(collision.gameObject);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
