using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleCollision : MonoBehaviour
{
    [SerializeField] private int id;
    private bool collected = false;

    private void OnEnable()
    {
        Publisher.PushData += LoadCollectible;
        Publisher.FetchData += SaveCollectible;
    }

    private void OnDisable()
    {
        Publisher.PushData -= LoadCollectible;
        Publisher.FetchData -= SaveCollectible;
    }

    private void SaveCollectible(bool switchScene)
    {
        foreach (Collectible collectible in XMLManager.Instance.savedData.collectibleDB.listOfCollectibles)
        {
            if (collectible.id == id && collectible.levelId == SceneSwitcher.Instance.GetActualLevel())
            {
                collectible.collected = collected;
            }
        }
    }

    private void LoadCollectible(bool loadPosition, bool playerDead)
    {
        foreach (Collectible collectible in XMLManager.Instance.savedData.collectibleDB.listOfCollectibles)
        {
            if (collectible.levelId == SceneSwitcher.Instance.GetActualLevel() && collectible.id == id && collectible.collected)
            {
                collected = collectible.collected;
                collected = true;
                gameObject.SetActive(false);
            }
        }
    }

    private void Collect()
    {
        collected = true;
        XMLManager.Instance.Save(false);
        AchievementManager.Instance.UnlockAllCollectiblesAchievement();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Harmony.Tags.Player)
        {
            collision.GetComponent<AudioSource>().PlayOneShot(Sounds.collectSnd);
            Collect();
        }
    }

}
