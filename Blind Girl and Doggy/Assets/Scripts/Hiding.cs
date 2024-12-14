using UnityEngine;

public class Hiding : MonoBehaviour, Interactable
{
    public static Hiding instance;
    private SpriteRenderer spriteRenderer;
    private string originalSortingLayer;
    private int originalSortingOrder;
    private bool isHidden = false;

    public bool IsHidden => isHidden;

    private void Awake()
    {
        instance = this;
    }

    public void Interact()
    {
        ToggleHide();
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSortingLayer = spriteRenderer.sortingLayerName;  // ???????? Sorting Layer ??????????????
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleHide();
        }
    }

    void ToggleHide()
    {
        if (isHidden)
        {
            isHidden = false;
            spriteRenderer.sortingLayerName = originalSortingLayer;  // ??????????????? Sorting Layer ????
            spriteRenderer.sortingOrder = originalSortingOrder;
            Debug.Log("Player exited hiding.");
        }
        else
        {
            isHidden = true;
            spriteRenderer.sortingLayerName = "Hiding Spots";  // ??????? Sorting Layer ?????????????
            spriteRenderer.sortingOrder = -1;
            Debug.Log("Player is hiding.");
        }
    }
}
