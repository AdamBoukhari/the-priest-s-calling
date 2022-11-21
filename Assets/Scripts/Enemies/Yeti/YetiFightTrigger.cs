using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiFightTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private GameObject cameraFollowPoint;
    [SerializeField] private YetiFightStateManager yetiFightStateManager;
    [SerializeField] private GameObject pillars;
    private Animator pillarsAnimator;
    private AudioSource audioSource;

    private GameObject player;
    private PlayerInteraction playerInteraction;
    private CinemachineBasicMultiChannelPerlin vcamNoise;

    //Represents the pillar animation duration.
    private const float SHAKE_TIMER_TOTAL = 5f;
    private float startingIntensity = 5f;
    private float shakeTimer;
    private bool activated = false;

    private void Awake()
    {
        player = GameObject.Find(Harmony.Tags.Player);
        playerInteraction = player.GetComponent<PlayerInteraction>();
        vcamNoise = vcam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

        pillarsAnimator = pillars.GetComponent<Animator>();
        audioSource = pillars.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            vcamNoise.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / SHAKE_TIMER_TOTAL));
        }
    }

    private void StartCombatDebutAnimation()
    {
        if(activated)
        {
            return;
        }

        activated = true;
        vcam.Follow = cameraFollowPoint.transform;
        yetiFightStateManager.ActivateHealthBar();
        yetiFightStateManager.ActivateTempest();
        StartCoroutine(PlayPillarsEntryAnimation());
    }

    public void StartCombatEndingAnimation()
    {
        StartCoroutine(PlayPillarsExitAnimation());
    }

    public void ResetCombatState()
    {
        activated = false;
        PillarsExit();
    }

    private IEnumerator PlayPillarsEntryAnimation()
    {
        pillars.SetActive(true);
        audioSource.Play();
        ShakeCamera(startingIntensity, SHAKE_TIMER_TOTAL);

        playerInteraction.DeactivatePlayerControls();
        yield return new WaitForSeconds(SHAKE_TIMER_TOTAL);
        playerInteraction.ActivatePlayerControls();
        yetiFightStateManager.SetStartFight();
    }

    private IEnumerator PlayPillarsExitAnimation()
    {
        ShakeCamera(startingIntensity, SHAKE_TIMER_TOTAL);
        audioSource.Play();
        pillarsAnimator.SetTrigger(Harmony.AnimatorParameters.FightEnded);

        yield return new WaitForSeconds(SHAKE_TIMER_TOTAL);
        vcam.Follow = player.transform;
        pillars.SetActive(false);
    }

    private void PillarsExit()
    {
        yetiFightStateManager.DeactivateTempest();
        vcam.Follow = player.transform;
        pillars.SetActive(false);
    }

    //Could be put in singleton script for entire project usage
    private void ShakeCamera(float intensity, float time)
    {
        vcamNoise.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    public void SetActivated()
    {
        activated = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Harmony.Tags.Player) && yetiFightStateManager.IsOutOfZone() && !pillars.activeInHierarchy)
        {
            StartCombatDebutAnimation();
        }
    }
}
