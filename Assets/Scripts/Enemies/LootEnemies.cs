using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootEnemies : MonoBehaviour
{
    private const int MIN_PERCENTAGE = 1;
    private const int MAX_PERCENTAGE = 100;
    private const int MAX_HEARTS_IN_SCENE = 3;
    [SerializeField] private const int PERCENTAGE_NEEDED_TO_DROP_HEARTS = 20;
    [SerializeField] private GameObject heart;
    private GameObject[] hearts;

    void Awake()
    {
        hearts = new GameObject[MAX_HEARTS_IN_SCENE];
        for(int i = 0; i < MAX_HEARTS_IN_SCENE;i++)
        {
            hearts[i] = Instantiate(heart);
            hearts[i].gameObject.SetActive(false);
        }
    }

    public void RandomDropHearts(Vector2 EPos)
    {
        int chance = Random.Range(MIN_PERCENTAGE,MAX_PERCENTAGE);
        if(chance <= PERCENTAGE_NEEDED_TO_DROP_HEARTS)
        {
            foreach(GameObject heart in hearts)
            {
                if(!heart.activeSelf)
                {
                    heart.SetActive(true);
                    heart.transform.position = EPos;
                    break;
                }
            }
        }
    }

    public void DropHeart(Vector2 EPos)
    {
        Vector2 offset = Vector2.zero;
        foreach (GameObject heart in hearts)
        {
            if (!heart.activeSelf)
            {
                heart.SetActive(true);
                heart.transform.position = EPos + offset;
                offset += Vector2.right / 2;
            }
        }
    }
}
