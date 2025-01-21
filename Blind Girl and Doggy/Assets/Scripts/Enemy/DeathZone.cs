using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private FunctionType type;
    [SerializeField] private GameObject deathZone;
    [SerializeField] private Bridge bridge;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        EnableBoxCollider(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Dog"))
        {
            if(type == FunctionType.DeathZone)
            {
                HeartManager.instance.HeartDecrease();
            }
        }

        if (collision.CompareTag("Player"))
        {
            if (type == FunctionType.DestroyBridge)
            {
                boxCollider.enabled = false;
                StartCoroutine(bridge.DestroyBridge());
                bridge.SetBoxCollider(false);
                EnableBoxCollider(true);
            }
        }
    }

    public void EnableBoxCollider(bool b)
    {
            BoxCollider2D boxCollider = deathZone.GetComponent<BoxCollider2D>();
            boxCollider.enabled = b;    
    }

}

public enum FunctionType
{
    DestroyBridge,
    DeathZone
}
