using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteUI : MonoBehaviour
{
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

    public bool isActive { get; private set; }

    private NoteManager note;
    private InventoryUI inventoryUI;
    private PauseManager pauseManager;

    private int currentIndex = 0;
    private List<GameObject> noteSlots = new List<GameObject>();
    private List<NoteItem> allNotes = new List<NoteItem>();

    private const int visibleSlots = 3;

 
    private void Awake()
    {
        note = FindObjectOfType<NoteManager>();
        inventoryUI = FindObjectOfType<InventoryUI>();
        pauseManager = FindObjectOfType<PauseManager>();
    }

    void Start()
    {
        Initialize();
        UpdateNoteUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!notePanel.activeSelf && !inventoryUI.isActive && !pauseManager.isActive)
            {
                isActive = true;
                SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1.0f);
                StartCoroutine(ToggleNotePanel());
                Time.timeScale = 0.0f;
                currentIndex = 0;
                noteProgess.fillAmount = Mathf.Max((float)currentIndex / (allNotes.Count - 1), 0.1f);
            }
        }

        if (Input.GetKeyDown(KeyCode.X) && notePanel.activeSelf)
        {
            SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1.0f);
            StartCoroutine(ToggleNotePanel());
            Time.timeScale = 1.0f;
            isActive = false;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && notePanel.activeSelf)
        {
            currentIndex = (currentIndex - 1 + allNotes.Count) % allNotes.Count;
            noteProgess.fillAmount = Mathf.Max((float)currentIndex / (allNotes.Count - 1), 0.1f);

            SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
            UpdateNoteUI();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && notePanel.activeSelf)
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

            if (itemName != null)
            {
                itemName.text = item.isCollected ? item.noteName : "???";
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
            noteNameText.text = item.noteName;
            noteDateText.text = item.noteDate;
            noteDetailText.text = item.noteDetail;
        }
        else
        {
            noteNameText.text = "???";
            noteDateText.text = "Date Unknown";
            noteDetailText.text = "This note has not been found yet, but there's likely something hidden inside. Take a look around... Maybe the answer you've been seeking is inside.";
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
        yield return new WaitForSecondsRealtime(0.1f);
        notePanel.SetActive(!notePanel.activeSelf);

        if (notePanel.activeSelf)
        {
            UpdateNoteUI();
        }
    }

}
