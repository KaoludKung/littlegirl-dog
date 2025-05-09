using UnityEngine;

public class TriggerPositionSaver : MonoBehaviour
{
    [SerializeField] GameObject Ballon;
    [SerializeField] Sprite BallonHappySprite;
    [SerializeField] Vector3 positionGirl;
    private Vector3 positionAtTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dog"))
        {
            Ballon.GetComponent<SpriteRenderer>().sprite = BallonHappySprite;
            positionAtTrigger = collision.transform.position;
            PlayerDataManager.Instance.UpdateDogPosition(positionAtTrigger);
            PlayerDataManager.Instance.UpdateGirlPosition(positionGirl);
            PlayerDataManager.Instance.SavePlayerData();
            Destroy(gameObject);
        }
    }

    public Vector3 GetPositionAtTrigger()
    {
        return positionAtTrigger;
    }
}
