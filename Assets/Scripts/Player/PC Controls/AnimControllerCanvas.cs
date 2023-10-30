using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimControllerCanvas : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject image;
    public void PlayAnim()
    {
        anim.Play("Close");
    }

    public void TurnOffConvas()
    {
        image.SetActive(false);
    }
}
