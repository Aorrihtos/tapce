using System.Collections;
using UnityEngine;

public class LifeBallBlinkAnimation : MonoBehaviour
{
    private Animator animator;
    private bool IsAnimationPlaying = false;
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if(!IsAnimationPlaying)
            StartCoroutine(Blink());
    }

    private IEnumerator Blink(){
        IsAnimationPlaying = true;
        animator.SetTrigger("Blink");
        yield return new WaitForSeconds(Random.Range(5f, 20f));
        IsAnimationPlaying = false;
    }
}
