using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeartManager : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI heartAmount;
    [SerializeField] Image[] heartImage;
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
            //int random = Random.Range(1, 5);
            PlayerDataManager.Instance.UpdateHearts(3);
            PlayerDataManager.Instance.UpdateIsSpined(true);
            PlayerDataManager.Instance.SavePlayerData();
            HeartUpdate();

            //StartCoroutine(ShowRandomNumbers(random));
        }
        else
        {
            HeartUpdate();
            //heartAmount.text = "X " + PlayerDataManager.Instance.GetHearts();
        }
    }

    void HeartUpdate()
    {
        if(PlayerDataManager.Instance.GetHearts() == 3)
        {
            heartImage[0].color = new Color32(255, 255, 255, 255);
            heartImage[1].color = new Color32(255, 255, 255, 255);
            heartImage[2].color = new Color32(255, 255, 255, 255);
        }else if(PlayerDataManager.Instance.GetHearts() == 2)
        {
            heartImage[0].color = new Color32(255, 255, 255, 255);
            heartImage[1].color = new Color32(255, 255, 255, 255);
            heartImage[2].color = new Color32(66, 66, 66, 255);
        }
        else if (PlayerDataManager.Instance.GetHearts() == 1)
        {
            heartImage[0].color = new Color32(255, 255, 255, 255);
            heartImage[1].color = new Color32(66, 66, 66, 255);
            heartImage[2].color = new Color32(66, 66, 66, 255);
        }
        else
        {
            heartImage[0].color = new Color32(66, 66, 66, 255);
            heartImage[1].color = new Color32(66, 66, 66, 255);
            heartImage[2].color = new Color32(66, 66, 66, 255);
        }

    }

    /*
    IEnumerator ShowRandomNumbers(int finalValue)
    {
        float duration = 3.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            int fakeValue = Random.Range(0, 3);
            //heartAmount.text = "X " + fakeValue;

            yield return null;
        }

        //heartAmount.text = "X " + finalValue;
    }*/

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
        HeartUpdate();

        if (PlayerDataManager.Instance.GetHearts() != 0)
        {
            StartCoroutine(Death());
            //JumpScare.instance.jumpScare();
        }
        else
        {
            JumpScare.instance.Jumpscare();
        }

        //heartAmount.text = "X " + heart;
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
