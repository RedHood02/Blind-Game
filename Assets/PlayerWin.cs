using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerWin : MonoBehaviour
{
    [SerializeField] GameObject canvasVisor;
    [SerializeField] StudioEventEmitter winSFX;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvasVisor.SetActive(true);
            winSFX.Play();
            StartCoroutine(WaitTime());
            FindObjectOfType<PCMovement>().canMove = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1.4f);
        winSFX.SetParameter("Loop Exit", 1);
    }
}
