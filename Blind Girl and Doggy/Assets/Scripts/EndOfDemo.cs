using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfDemo : MonoBehaviour
{
    [SerializeField] private AudioClip clips;
    private bool isPressed = false;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.IsZPressed() && !isPressed)
        {
            isPressed = true;
            StartCoroutine(GoToTitle());
        }
    }

    IEnumerator GoToTitle()
    {
        audioSource.clip = clips;
        audioSource.Play();

        yield return new WaitForSeconds(clips.length + 1.0f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}
