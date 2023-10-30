using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] GameObject minimap;
    [SerializeField] AnimControllerCanvas anim;
    [SerializeField] bool isOpen;


    private void Update()
    {
        Control();
    }

    void Control()
    {
        if(Input.GetKeyDown(KeyCode.M) && !isOpen)
        {
            minimap.SetActive(true);
            anim.PlayOpen();
            isOpen = true;
            FindObjectOfType<PCMovement>().canMove = false;
            StartCoroutine(Fade());
        }
    }


    IEnumerator Fade()
    {
        yield return new WaitForSeconds(0.75f);
        anim.PlayAnim();
        isOpen = false;
        FindObjectOfType<PCMovement>().canMove = true;
    }

}
