using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private GameObject floor;
    [SerializeField] private Transform rope;
    [SerializeField] private Transform[] bridge;
    [SerializeField] private AudioClip breakClip;
    [SerializeField] private AudioClip[] bridgeClips;

    private int charactersOnBridge = 0;
    private Vector3[] bridgePosition;
    private Vector3 ropePosition;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private GirlController controller;
    private DogController dcontroller;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Character"), LayerMask.NameToLayer("Bridge"), true);
        rb = rope.GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        controller = FindObjectOfType<GirlController>();
        dcontroller = FindObjectOfType<DogController>();
        bridgePosition = new Vector3[bridge.Length];
        ropePosition = rope.position;

        for (int i = 0; i < bridge.Length; i++)
        {
            bridgePosition[i] = bridge[i].position;
            //Debug.Log("Bridge" + i + ": " + bridgePosition[i]);
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Dog"))
        {
              charactersOnBridge++;
              if(charactersOnBridge > 1)
              {
                  StartCoroutine(Wait());
                  StartCoroutine(DestroyBridge());
                  StartCoroutine(Death());
              }
         
        }

        if (collision.CompareTag("Dog"))
            dcontroller.SetNewClip(bridgeClips[0]);

        if (collision.CompareTag("Player"))
            dcontroller.SetNewClip(bridgeClips[1]);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Dog"))
        {
            charactersOnBridge--;
        }

        if (collision.CompareTag("Dog"))
            dcontroller.SetNewClip(null, true);

        if (collision.CompareTag("Player"))
            dcontroller.SetNewClip(null, true);
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);
        controller.SetIsMoving(false);
        controller.Animator.SetBool("isWalk", false);
        controller.Animator.SetBool("isDeath", true);
    }

    public IEnumerator DestroyBridge()
    {
        yield return new WaitForSeconds(1.5f);

        //controller.SetIsMoving(false);
        //controller.Animator.SetBool("isWalk", false);

        floor.SetActive(false);
        SoundFXManager.instance.PlaySoundFXClip(breakClip, transform, false, 1.0f);

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public IEnumerator Death()
    {
        yield return new WaitForSeconds(2.0f);
        HeartManager.instance.HeartDecrease();
        yield return new WaitForSeconds(1.5f);

        StartCoroutine(ResetBridge());
    }

    public IEnumerator ResetBridge()
    {
        floor.SetActive(true);

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        yield return new WaitForSeconds(0.2f);

        rope.position = ropePosition;
        var ropeRb = rope.GetComponent<Rigidbody2D>();
        if (ropeRb != null)
        {
            ropeRb.velocity = Vector2.zero;
            ropeRb.angularVelocity = 0f;
        }

        for (int i = 0; i < bridge.Length; i++)
        {
            var rb = bridge[i].GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }

        for (int i = 0; i < bridge.Length; i++)
        {
            bridge[i].position = bridgePosition[i];
        }

        yield return null;

        for (int i = 0; i < bridge.Length; i++)
        {
            var rb = bridge[i].GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }

        //yield return new WaitForSeconds(1.0f);
        //StartCoroutine(DestroyBridge());
    }

    public void SetBoxCollider(bool b)
    {
        boxCollider.enabled = b;
    }

}

