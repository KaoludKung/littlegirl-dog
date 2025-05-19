using System.Collections;
using UnityEngine;

public class TriggerPositionSaver : MonoBehaviour
{
    [Header("Balloon Settings")]
    [SerializeField] private GameObject Ballon;
    [SerializeField] private Sprite BallonHappySprite;
    [SerializeField] private AudioClip ballonClips;
    [SerializeField] private FloatingBalloon BallonPrevious;

    [Header("Girl Position when respawn")]
    [SerializeField] private Vector3 positionGirl;

    private Vector3 positionAtTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dog"))
        {
            if (BallonPrevious != null)
                BallonPrevious.SetSafePoint();

            SoundFXManager.instance.PlaySoundFXClip(ballonClips, transform, false, 0.8f);
            Ballon.GetComponent<SpriteRenderer>().sprite = BallonHappySprite;
            positionAtTrigger = collision.transform.position;
            StartCoroutine(SaveSpawn());
        }
    }

    IEnumerator SaveSpawn()
    {
        PlayerDataManager.Instance.UpdateDogPosition(positionAtTrigger);
        PlayerDataManager.Instance.UpdateGirlPosition(positionGirl);
        PlayerDataManager.Instance.SavePlayerData();
        
        yield return new WaitForSeconds(ballonClips.length + 0.2f);
        Destroy(gameObject);
    }

    public Vector3 GetPositionAtTrigger()
    {
        return positionAtTrigger;
    }
}
