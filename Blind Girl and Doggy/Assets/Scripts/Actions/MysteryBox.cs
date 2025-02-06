using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBox : MonoBehaviour, Interactable
{
    [SerializeField] GameObject[] flowerCoins;
    [SerializeField] GameObject dialogueL;
    [SerializeField] Sprite alertSprite;
    [SerializeField] AudioClip clip;
    private InventoryItem[] inventoryItems;
    private ActionText actionText;
    private GirlController girlController;

    // Start is called before the first frame update
    void Start()
    {
        actionText = FindObjectOfType<ActionText>();
        girlController = FindAnyObjectByType<GirlController>();
        inventoryItems = new InventoryItem[flowerCoins.Length];
        CheckCoins();
    }

    public void Interact()
    {
        CheckCoins();

        bool allCoinsCollected = true;
        bool soundPlayed = false;

        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i].isCollected && !flowerCoins[i].activeSelf)
            {
                if (!soundPlayed)
                {
                    SoundFXManager.instance.PlaySoundFXClip(clip, transform, false, 1.0f);
                    soundPlayed = true;
                }

                flowerCoins[i].SetActive(true);
                InventoryManager.Instance.RemoveItem(12 + i);

                actionText.ActionDisplay("Margarete has put the flower coin.");
            }
            else if (!inventoryItems[i].isCollected)
            {
                allCoinsCollected = false;
            }
        }

        if (allCoinsCollected)
        {
            EventManager.Instance.UpdateEventDataTrigger(86, true);
            InventoryManager.Instance.AddItem(15);
            StartCoroutine(GetTheKey());
        }
    }

    IEnumerator GetTheKey()
    {
        SoundFXManager.instance.PlaySoundFXClip(clip, transform, false, 1.0f);
        actionText.ActionDisplay("Margarete has put all the flower coins in and then gotten the exit key.");

        yield return new WaitForSeconds(clip.length);
        Destroy(dialogueL);
        Destroy(gameObject);
    }

    void CheckCoins()
    {
        inventoryItems[0] = InventoryManager.Instance.GetItemByID(12);
        inventoryItems[1] = InventoryManager.Instance.GetItemByID(13);
        inventoryItems[2] = InventoryManager.Instance.GetItemByID(14);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            girlController.AddInteractSprite(alertSprite);
            //girlController.InteractionIcon.GetComponent<SpriteRenderer>().sprite = alertSprite;
        }
    }
}
