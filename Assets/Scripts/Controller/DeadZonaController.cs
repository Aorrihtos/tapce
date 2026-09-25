using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZonaController : MonoBehaviour
{
    [SerializeField]
    private GameObject _lifes;

    [SerializeField]
    private GameObject _playerPref;

    [SerializeField]
    private PointCounterController _pointCounterController;

    private GameObject _camera;
    private RectTransform _lifesTransform;




    private float _lifeImgWidthPerUnit = 49f;

    void Awake()
    {
        _lifesTransform = _lifes.GetComponent<RectTransform>();
        _camera = GameObject.FindGameObjectWithTag("Cinemachine");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && _lifesTransform != null)
        {
            float newScaleX = _lifesTransform.rect.width - _lifeImgWidthPerUnit;
            if (newScaleX <= 0)
            {
                // Save the current score, reset points and load the next scene
                PlayerPrefs.SetInt("lastScore", _pointCounterController.GetPoints());
                _pointCounterController.ResetPoints();
                SceneManager.LoadScene("ScoreScene");
            }
            else
            {
                // Destroy the current player, generates new one and set the following camera
                Destroy(collider.gameObject);
                _lifesTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newScaleX);
                var newInstance = Instantiate(_playerPref, new Vector3(0, 0, 0), Quaternion.identity);
                newInstance.gameObject.GetComponentInChildren<Animator>().SetTrigger("Respawn");
                _camera.GetComponent<CinemachineVirtualCamera>().Follow = newInstance.transform;
            }
        }
    }

}
