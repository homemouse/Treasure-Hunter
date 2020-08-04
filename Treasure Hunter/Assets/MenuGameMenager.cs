using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MenuGameMenager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioListener audioListener;
    // Start is called before the first frame update
    void Start()
    {
        //audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("CharacterScene");
    }
}
