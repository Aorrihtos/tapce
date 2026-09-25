using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private Animator textAnimator;
    public GameObject startText;

    public float transitionTime = 1f;

    private AudioManager _audioManager;



    void Awake()
    {
        textAnimator = startText.GetComponent<Animator>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Start()
    {
        _audioManager.PlayBackground(_audioManager.bg_startScreen);
    }

    public void LoadNextScene(InputAction.CallbackContext callbackContext)
    {
        if (!callbackContext.performed || !SplashScreen.isFinished)
            return;
        StartCoroutine(LoadLevel("SetIdentifierScene"));
    }

    IEnumerator LoadLevel(string scene)
    {

        // Start Animation
        textAnimator.SetTrigger("Started");

        // Play SFX
        _audioManager.PlaySFX(_audioManager.sfx_pressStart);

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Load Scene
        SceneManager.LoadScene(scene);
    }
}
