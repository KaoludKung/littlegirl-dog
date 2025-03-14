using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpScare : MonoBehaviour
{
    [SerializeField] int maxJumpscare;
    [SerializeField] Image deathVisual;
    //0: many faces 1: corpse 2: hunter
    [SerializeField] Sprite[] visuals;
    //0: many faces 1: corpse 2: hunter
    [SerializeField] AudioClip[] clips;
    public static JumpScare instance;

    private void Awake()
    {
        instance = this;
    }

    public void Jumpscare()
    {
        int i = Random.Range(1, maxJumpscare + 1);
        PlayerDataManager.Instance.UpdateIsSpined(false);
        PlayerDataManager.Instance.SavePlayerData();

        switch (i)
        {
            case 1: 
                StartCoroutine(JumpScareAnimation1());
                break;
            case 2:
                StartCoroutine(JumpScareAnimation2());
                break;
            case 3:
                StartCoroutine(JumpScareAnimation3());
                break;
        }
    }

    IEnumerator JumpScareAnimation1()
    {
        yield return new WaitForSeconds(0.5f);
        deathVisual.sprite = visuals[0];
        deathVisual.gameObject.SetActive(true);
        SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 0.6f);
        yield return new WaitForSeconds(clips[0].length);
        deathVisual.sprite = visuals[1];
        yield return new WaitForSeconds(2.5f);
        GameOverManager.instance.OpenPanel();
        
    }

    IEnumerator JumpScareAnimation2()
    {
        yield return new WaitForSeconds(0.5f);
        deathVisual.sprite = visuals[2];
        deathVisual.gameObject.SetActive(true);
        SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 0.6f);
        yield return new WaitForSeconds(clips[1].length);
        deathVisual.sprite = visuals[3];
        yield return new WaitForSeconds(2.5f);
        GameOverManager.instance.OpenPanel();

    }

    IEnumerator JumpScareAnimation3()
    {
        yield return new WaitForSeconds(0.5f);
        deathVisual.sprite = visuals[4];
        deathVisual.gameObject.SetActive(true);
        SoundFXManager.instance.PlaySoundFXClip(clips[2], transform, false, 0.6f);
        yield return new WaitForSeconds(clips[2].length + 1.0f);
        GameOverManager.instance.OpenPanel();

    }
}
