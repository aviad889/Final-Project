using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject[] playerLives;
    private int numOfLives, i;
    private GameObject player;
    private float startingPlayerPositionX = -8.9f;

    void Start()
    {
        player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        numOfLives = playerLives.Length;
        for (i = 0; i < numOfLives; i++)
        {
            playerLives[i].SetActive(true);
        }
    }
    public void RespawnPlayer()
    {
        numOfLives--;
        if (numOfLives <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else
        {
            playerLives[numOfLives].SetActive(false);
            player.transform.position = transform.position;
        }
    }
    void Update()
    {
        if (player.transform.position.x < startingPlayerPositionX)
        {
            player.transform.position = transform.position;
        }
        if (player.transform.position == transform.position)
        {
            player.GetComponent<Animator>().SetBool("isHit", false);
        }
    }
}
