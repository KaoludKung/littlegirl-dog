using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeartManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI heartAmount;
    [SerializeField] AudioClip failedSound;
    public static HeartManager instance;

    private DogController dogController;
    private GirlController girlController;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        dogController = FindObjectOfType<DogController>();
        girlController = FindObjectOfType<GirlController>();
        InitializetHeart();
    }

    void InitializetHeart()
    {
        if (!PlayerDataManager.Instance.GetIsSpined())
        {
            int random = Random.Range(1, 3);
            PlayerDataManager.Instance.UpdateHearts(random);
            PlayerDataManager.Instance.UpdateIsSpined(true);
            PlayerDataManager.Instance.SavePlayerData();

            StartCoroutine(ShowRandomNumbers(random));
        }
        else
        {
            heartAmount.text = "X " + PlayerDataManager.Instance.GetHearts();
        }
    }

    IEnumerator ShowRandomNumbers(int finalValue)
    {
        float duration = 3.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            int fakeValue = Random.Range(0, 3);
            heartAmount.text = "X " + fakeValue;

            yield return null;
        }

        heartAmount.text = "X " + finalValue;
    }

    public void HeartDecrease()
    {
        CharacterManager.Instance.SetIsActive(false);
        CharacterManager.Instance.SoundPause();
        GameOverManager.instance.SetIsActive(true);

        foreach (AnimatorControllerParameter parameter in dogController.Animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                dogController.Animator.SetBool(parameter.name, false);
            }
        }

        dogController.Animator.SetInteger("BarkType", 0);
        dogController.Animator.SetBool("isDeath", true);

        int heart = PlayerDataManager.Instance.GetHearts() - 1;
        PlayerDataManager.Instance.UpdateHearts(heart);
        PlayerDataManager.Instance.SavePlayerData();

        if (PlayerDataManager.Instance.GetHearts() != 0)
        {
            StartCoroutine(Death());
        }
        else
        {
            JumpScare.instance.jumpScare();
        }

        heartAmount.text = "X " + heart;
    }

    IEnumerator Death()
    {
        SoundFXManager.instance.PlaySoundFXClip(failedSound, transform, false, 1.0f);
        yield return new WaitForSeconds(1.2f);
        GameOverManager.instance.OpenPanel();
    }

    public void ResetGirlAnimation()
    {
        foreach (AnimatorControllerParameter parameter in girlController.Animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                girlController.Animator.SetBool(parameter.name, false);
            }
        }
    }

    public void GirlDeath()
    {
        girlController.Animator.SetBool("isDeath", true);
    }

}
