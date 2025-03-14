using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDisplay : MonoBehaviour
{
    [SerializeField] GameObject shadow;
    [SerializeField] Transform playerTarget;
    [SerializeField] Transform dogTarget;
    private float detectDistance = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTarget != null)
        {
            float distanceX = Mathf.Abs(dogTarget.position.x - playerTarget.position.x);

            if(distanceX < detectDistance)
            {
                shadow.SetActive(true);
            }
            else
            {
                shadow.SetActive(false);
            }
        }
    }
}
