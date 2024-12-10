using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHideBodyEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("TutorialEnemy"))
        {
            GameManager.Instance.ChangeDetectionState(2);
            StartCoroutine(GameOverCoroutine(2));
        }
    }

    IEnumerator GameOverCoroutine(int time)
    {
        yield return new WaitForSeconds(time);
        GameManager.Instance.LoseMenu();
    }
}
