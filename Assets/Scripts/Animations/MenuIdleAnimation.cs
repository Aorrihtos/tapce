using System.Collections;
using UnityEngine;

public class MenuIdleAnimation : MonoBehaviour
{

    [SerializeField]
    private GameObject armRight;

    private Animator animator;


    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(ManageAnimationLoop());
    }

    void DeactivateArm()
    {
        var animate = armRight.gameObject.GetComponent<Animator>();
        if (animate.enabled)
        {
            animate.enabled = false;
        }
        armRight.SetActive(false);
    }

    void ActivateArm()
    {
        armRight.SetActive(true);
    }

    void SetBlinkAnimationFalse()
    {
        animator.SetBool("MenuTapceBlinks", false);
    }

    void SetIdleAnimationFalse()
    {
        animator.SetBool("MenuTapceIdle", false);
    }

    IEnumerator ManageAnimationLoop()
    {
        string animationName = "MenuTapceIdle";

        while (true)
        {
            var rnd = new System.Random();

            switch (rnd.Next(2))
            {
                case 0:
                    animationName = "MenuTapceBlinks";
                    break;
                case 1:
                    animationName = "MenuTapceIdle";
                    break;
            }

            yield return new WaitForSeconds(3f);
            animator.SetBool(animationName, true);
        }

    }

}
