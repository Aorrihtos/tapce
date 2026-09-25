using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{

    public Animator fadeAnimator;

    public void ChangeScene()
    {
        StartCoroutine(ChangeWithFade());
    }

    IEnumerator ChangeWithFade()
    {
        fadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("StartScreen");
    }
}
