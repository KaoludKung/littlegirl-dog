using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potions : MonoBehaviour, Interactable
{
    [Header("Player")]
    [SerializeField] private GameObject dogPlayer;
    [SerializeField] private GameObject girlPlayer;

    [Header("Potion Setting")]
    [SerializeField] private GameObject potionObject;
    [SerializeField] private Sprite alertSprite;
    [SerializeField] private AudioClip drinkClip;

    private bool isAdd = false;
    private int effectDurationInFrames = 25;
    private BoxCollider2D potionCollider;
    private GirlController girlController;
    private DogController dogController;

    void Start()
    {
        potionCollider = GetComponent<BoxCollider2D>();
        girlController = FindObjectOfType<GirlController>();
        dogController = FindObjectOfType<DogController>();
    }

    public void Interact()
    {
        if (!girlController.IsMoving && !girlController.IsBarking)
        {
            potionCollider.enabled = false;
            potionObject.SetActive(false);
            StartCoroutine(DrinkPotion());
            //AchievementUnlock.Instance.UnlockAchievement(9);
        }
    }

    IEnumerator DrinkPotion()
    {
        girlController.SetIsInteract(true);
        girlController.InteractionIcon.SetActive(false);
        CharacterManager.Instance.SetIsActive(false);
        SetInteractionAnimation(true);
        SoundFXManager.instance.PlaySoundFXClip(drinkClip, transform, false, 0.85f);

        yield return new WaitForSeconds(0.25f);
        ApplyRandomEffect();

        yield return new WaitForSeconds(0.75f);

        SetInteractionAnimation(false);
        CharacterManager.Instance.SetIsActive(true);
        girlController.SetIsInteract(false);
    }

    void ApplyRandomEffect()
    {
        int effectId = Random.Range(1, 7);

        switch (effectId)
        {
            case 1:
                dogController.AdjustStamina(0.5f);
                break;
            case 2:
                dogController.AdjustStamina(-0.5f);
                break;
            case 3:
                StartCoroutine(AdjustSpeedOverTime(true, -2.5f, new Color32(246, 106, 155, 255)));
                break;
            case 4:
                StartCoroutine(AdjustSpeedOverTime(false, -2.5f, new Color32(246, 106, 155, 255)));
                break;
            case 5:
                StartCoroutine(AdjustSpeedOverTime(true, 2.5f, new Color32(255, 192, 75, 255)));
                break;
            case 6:
                StartCoroutine(AdjustSpeedOverTime(false, 2.5f, new Color32(255, 192, 75, 255)));
                break;
        }
    }

    IEnumerator AdjustSpeedOverTime(bool isDog, float speedAdjustment, Color32 flashColor)
    {
        var sprite = isDog ? dogPlayer.GetComponent<SpriteRenderer>() : girlPlayer.GetComponent<SpriteRenderer>();

        if (isDog)
        {
            dogController.AdjustSpeed(speedAdjustment);
        }
        else
        {
            girlController.AdjustSpeed(speedAdjustment);
        }


        for (int i = 0; i < effectDurationInFrames; i++)
        {
            sprite.color = i % 2 != 0 ? Color.white : flashColor;
            yield return new WaitForSeconds(1.5f); // Reduce wait time for smoother visuals
        }

        sprite.color = Color.white; // Reset to default color
        
        if (isDog)
        {
            dogController.AdjustSpeed(-speedAdjustment);
        }
        else
        {
            girlController.AdjustSpeed(-speedAdjustment);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isAdd)
        {
            isAdd = true;
            girlController.AddInteractSprite(alertSprite);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isAdd)
        {
            isAdd = false;
        }
    }

    private void SetInteractionAnimation(bool state)
    {
        girlController.Animator?.SetBool("isInteract", state);
    }
}
