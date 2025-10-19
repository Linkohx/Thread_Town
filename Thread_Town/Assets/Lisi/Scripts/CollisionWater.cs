using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWater : MonoBehaviour
{
    public GameObject returnPoint;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            gameObject.SetActive(false);
            TransitionPanel.Instance.transitionEvent.AddListener(Return);
            TransitionPanel.Instance.Show();
        }
    }

    protected void Return()
    {
        transform.position = returnPoint.transform.position;
        gameObject.SetActive(true);
    }
}
