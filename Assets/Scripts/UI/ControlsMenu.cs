using UnityEngine;

public class ControlsMenu : MonoBehaviour
{
    [SerializeField] private GameObject Tutorial;
    void Start()
    {
        Time.timeScale = 0f;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1f;
            Tutorial.SetActive(false);
        }
    }
}
