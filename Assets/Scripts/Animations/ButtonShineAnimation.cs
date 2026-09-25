using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonShineAnimation : MonoBehaviour
{
    private Animator animator;
    
    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        while (true)
        {
            animator.SetBool("play", true);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            animator.SetBool("play", false);
            yield return new WaitForSeconds(5);
        }
    }
}
