using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiJumpTrigger : MonoBehaviour
{
    private GameObject yeti;
    private YetiJump yetiJump;
    private YetiSnowballAttack yetiSnowballAttack;
    private YetiFightStateManager yetiFightStateManager;

    private void Awake()
    {
        yeti = GameObject.FindGameObjectWithTag(Harmony.Tags.Yeti);
        yetiJump = yeti.GetComponent<YetiJump>();
        yetiSnowballAttack = yeti.GetComponent<YetiSnowballAttack>();
        yetiFightStateManager = yeti.GetComponent<YetiFightStateManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Harmony.Tags.Player) && !yetiJump.JumpStarted() && yeti.activeInHierarchy && !yetiFightStateManager.dead)
        {
            yetiSnowballAttack.SetYetiActivated();
            yetiJump.CallJumpOnAscensionPosition(transform.parent.gameObject);
        }
    }
}
