using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayNameManager : MonoBehaviour
{
    [SerializeField] private string mapName;
    [SerializeField] private TMPro.TextMeshProUGUI text;
    [SerializeField] private Image background;



    [SerializeField] private float clockDisplay;
    private float timerDisplay;
    // Start is called before the first frame update
    void Start()
    {
        text.text = mapName;
        timerDisplay = clockDisplay;
    }

    // Update is called once per frame
    void Update()
    {
        timerDisplay -= Time.deltaTime;
        if (timerDisplay <= 0)
        {
            background.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
        }
    }
}