using System.Collections;
using UnityEngine;

public class ScoreSceneTapceFallingAnimation : MonoBehaviour
{
    private Animator animator;
    private bool IsAnimationPlaying = false;
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if(!IsAnimationPlaying)
            StartCoroutine(Fall());
    }

    private IEnumerator Fall(){
        IsAnimationPlaying = true;
        animator.SetTrigger("Fall");
        yield return new WaitForSeconds(Random.Range(3f, 7f));
        IsAnimationPlaying = false;
    }
}
