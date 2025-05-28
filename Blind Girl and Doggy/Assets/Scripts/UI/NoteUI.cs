using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteUI : MonoBehaviour
{
    [Header("Translation Options")]
    [SerializeField] private NoteTranslate noteTransations;
    //0:en 1:th
    [SerializeField] private TMP_FontAsset[] fontName;

    [Header("Note Settings")]
    [SerializeField] private GameObject notePanel;
    [SerializeField] private GameObject noteSlotPrefab;
    [SerializeField] private Transform noteSlotContainer;

    [SerializeField] private TextMeshProUGUI noteNameText;
    [SerializeField] private TextMeshProUGUI noteDateText;
    [SerializeField] private TextMeshProUGUI noteDetailText;
    [SerializeField] private Image noteProgess;
    //[SerializeField] private TextMeshProUGUI noteCount;

    [SerializeField] private Sprite[] slotSelect;
    [SerializeField] private AudioClip[] clips; // 0: selected, 1: pressed
    [SerializeField] private bool isUnlock = true;

    public bool isActive { get; private set; }

    private NoteManager note;
    private int currentIndex = 0;
    private List<GameObject> noteSlots = new List<GameObject>();
    private List<NoteItem> allNotes = new List<NoteItem>();

    private const int visibleSlots = 3;
    private float localLastMoveTime = 0f;

    private void Awake()
    {
        note = FindObjectOfType<NoteManager>();
    }

    void Start()
    {
        Initialize();
        UpdateNoteUI();
    }

    void Update()
    {
        if (notePanel.activeSelf)
        {
            CharacterManager.Instance.SetIsActive(false);
        }

        if (InputManager.Instance.IsEPressed())
        {
            if (!notePanel.activeSelf && isUnlock && !UIManager.Instance.IsAnyUIActive)
            {
                isActive = true;
                CharacterManager.Instance.SoundPause();
                CharacterManager.Instance.SetIsActive(false);
                SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1.0f);
                StartCoroutine(ToggleNotePanel());
                UIManager.Instance.ToggleTimeScale(true);
                currentIndex = 0;
                noteProgess.fillAmount = Mathf.Max((float)currentIndex / (allNotes.Count - 1), 0.1f);
            }
        }

        if (InputManager.Instance.IsXPressed() && notePanel.activeSelf)
        {
            SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1.0f);
            StartCoroutine(ToggleNotePanel());
            StartCoroutine(Delay());
        }

        if (InputManager.Instance.IsUpPressed(ref localLastMoveTime) && notePanel.activeSelf)
        {
            currentIndex = (currentIndex - 1 + allNotes.Count) % allNotes.Count;
            noteProgess.fillAmount = Mathf.Max((float)currentIndex / (allNotes.Count - 1), 0.1f);

            SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
            UpdateNoteUI();
        }
        else if (InputManager.Instance.IsDownPressed(ref localLastMoveTime) && notePanel.activeSelf)
        {
            currentIndex = (currentIndex + 1) % allNotes.Count;
            noteProgess.fillAmount = Mathf.Max((float)currentIndex / (allNotes.Count - 1), 0.1f);

            SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
            UpdateNoteUI();
        }
    }

    public void UpdateNoteUI()
    {
        foreach (Transform child in noteSlotContainer)
        {
            Destroy(child.gameObject);
        }
        noteSlots.Clear();

        allNotes = new List<NoteItem>(note.Items);
        //Debug.Log("Loaded notes: " + note.Items.Count);

        for (int i = 0; i < visibleSlots; i++)
        {
            int index = (currentIndex + i) % allNotes.Count; 
            GameObject itemSlot = Instantiate(noteSlotPrefab, noteSlotContainer);
            noteSlots.Add(itemSlot);

            NoteItem item = allNotes[index];
            var itemName = itemSlot.transform.Find("note_text").GetComponent<TextMeshProUGUI>();


            itemName.font = PlayerDataManager.Instance.GetLanguage() == 1 ? fontName[1] : fontName[0];

            if (itemName != null)
            {
                itemName.fontSizeMax = PlayerDataManager.Instance.GetLanguage() == 1 ? 42 : 60;

                if (PlayerDataManager.Instance.GetLanguage() == 0)
                {
                    itemName.text = item.isCollected ? item.noteName : "???";
                }
                else
                {
                    itemName.text = item.isCollected ? noteTransations.noteList[item.id - 1].nameTranslation[PlayerDataManager.Instance.GetLanguage() - 1] : "???";
                }
            }

            itemSlot.GetComponent<Image>().color = new Color(itemSlot.GetComponent<Image>().color.r,
                                                               itemSlot.GetComponent<Image>().color.g,
                                                               itemSlot.GetComponent<Image>().color.b,
                                                               item.isCollected ? 1.0f : 0.6f);
        }

        UpdateNoteMenu();
    }

    void UpdateNoteMenu()
    {
        for (int i = 0; i < visibleSlots; i++)
        {
            var slot = noteSlots[i];
            var slotImage = slot.GetComponent<Image>();

            if (i == 0) // current slot (first slot in the visible area)
            {
                slotImage.sprite = slotSelect[1];

                // show the note's description of the first slot
                ShowItemDescription(allNotes[(currentIndex + i) % allNotes.Count]);
            }
            else
            {
                slotImage.sprite = slotSelect[0];
            }
        }
    }

    private void ShowItemDescription(NoteItem item)
    {
        if (item.isCollected)
        {
            if (PlayerDataManager.Instance.GetLanguage() == 0)
            {

                noteNameText.text = item.noteName;
                noteDateText.text = item.noteDate;
                noteDetailText.text = item.noteDetail.Replace("\\n", "\n");
            }
            else
            {
                noteNameText.text = noteTransations.noteList[item.id - 1].nameTranslation[PlayerDataManager.Instance.GetLanguage() - 1];
                noteDateText.text = noteTransations.noteList[item.id - 1].dateTranslation[PlayerDataManager.Instance.GetLanguage() - 1];
                noteDetailText.text = noteTransations.noteList[item.id - 1].descriptionTranslation[PlayerDataManager.Instance.GetLanguage() - 1].Replace("\\n", "\n");
            }
        }
        else
        {
            noteNameText.text = "???";
            noteDateText.text = LocalizationManager.Instance.GetText(41, PlayerDataManager.Instance.GetLanguage());
            noteDetailText.text = LocalizationManager.Instance.GetText(42, PlayerDataManager.Instance.GetLanguage()).Replace("\\n", "\n");
        }
    }

    void Initialize()
    {
        noteNameText.text = "";
        noteDateText.text = "";
        noteDetailText.text = "";
    }

    IEnumerator ToggleNotePanel()
    {
        noteNameText.fontSizeMin = PlayerDataManager.Instance.GetLanguage() == 1 ? 48 : 60;
        noteDetailText.fontSizeMax = PlayerDataManager.Instance.GetLanguage() == 1 ? 30 : 42;

        yield return new WaitForSecondsRealtime(0.1f);
        notePanel.SetActive(!notePanel.activeSelf);

        if (notePanel.activeSelf)
        {
            UpdateNoteUI();
        }
    }

    IEnumerator Delay()
    {
        if (isActive)
        {
            yield return new WaitForSecondsRealtime(0.3f);
            isActive = false;
            UIManager.Instance.ToggleTimeScale(false);
            CharacterManager.Instance.SoundUnPause();

            if (!CharacterManager.Instance.isHiding())
                CharacterManager.Instance.SetIsActive(true);
        }
    }

    public void Unlock(bool n)
    {
        isUnlock = n;
    }
}

[System.Serializable]
public class NoteTransation
{
    public string[] noteName;
    public string[] noteDate;
    public string[] noteDetail;
}
