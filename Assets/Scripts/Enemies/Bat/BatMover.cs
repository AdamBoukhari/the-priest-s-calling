using Cinemachine;
using System.Collections;
using UnityEngine;

public class BatMover : MonoBehaviour
{
    /*La forme de la trajectoire peut etre modifie comme on veut en deplacant les points des Paths
	Il est important de garder l'ordre des points (1, 2, 3, 4)
	Si l'on veut creer des trajectoire plus complexe on peut rajouter des chemins et faire suivre par exemple deux chemins a la suite a la chauve-souris avant de se poser.
	On peut ajuster sa vitesse et le temps de pause entre deux trajet si besoin.*/
    [SerializeField] float moveSpeed = 0.5f;
    [SerializeField] private Transform[] paths; //Les deux chemins sont des enfants de la chauve-souris ils determinent sa trajection suivant si le joueur arrive par la droite ou la gauche.

    private Vector2 batPosition;
    private Vector3 initialPosition;
    private Animator animator;
    private CinemachineVirtualCamera vCam;
    private Collider2D vCamZone;
    private Transform playerTransform;
    private PolygonCollider2D zone;

    private float distanceBetweenStartAndEndPath;
    private bool isInGround;
    private int pathToGo;
    private float time;
    private float minZonePointX;
    private float maxZonePointX;
    private float moveTimer;
    private bool coroutineAllowed;
    private bool coroutineEnded;
    private bool isReturnOnCeiling;
    private bool flyToLeft;
    private bool flyToRight;
    private bool isReturnOnInitialPositon;
    private float returnOnInitialStateCounter;
    private float distanceToPlayer;
    private bool playerDetected;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag(Harmony.Tags.Player).GetComponent<Transform>();
        animator = GetComponent<Animator>();
        Initialisation();
        minZonePointX = transform.position.x;
        maxZonePointX = transform.position.x;
        initialPosition = transform.position;
        isReturnOnInitialPositon = false;
        returnOnInitialStateCounter = 0;
    }

    private void Initialisation()
    {
        time = 0f;
        coroutineAllowed = false;
        playerDetected = false;
        coroutineEnded = true;
        moveTimer = 0f;
        isReturnOnCeiling = true;
        flyToLeft = false;
        flyToRight = false;
        isInGround = false;
    }

    // Update is called once per frame
    void Update()
    {
        vCam = (CinemachineVirtualCamera)GameObject.FindGameObjectWithTag(Harmony.Tags.MainCamera).GetComponent<CinemachineBrain>().ActiveVirtualCamera;
        if (vCam)
        {
            vCamZone = vCam.GetComponentInParent<CinemachineConfiner>()?.m_BoundingShape2D;
        }


        if (!GetComponent<LifeManager>().IsDead())
        {
            if (vCamZone != zone && !isReturnOnInitialPositon)
            {
                playerDetected = false;
                StopAllCoroutines();
                ReturnOnInitialState();
            }
            else
            {
                if (isReturnOnCeiling)
                {
                    distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

                    if (--moveTimer <= 0)
                    {
                        if (playerDetected && coroutineAllowed)
                        {
                            animator.SetBool(Harmony.AnimatorParameters.Sleep, false);
                            if (!coroutineEnded)
                            {
                                StartCoroutine(GoByPath(pathToGo));
                            }
                            else
                            {
                                coroutineEnded = false;
                            }

                        }
                        else if (!playerDetected && !coroutineAllowed && vCamZone == zone)
                        {
                            CheckIfPlayerArrived();
                        }
                    }
                    else
                    {
                        ResetAnimatorToSleep();
                    }
                }
                else
                {
                    MoveToCeiling();
                }
            }
        }
        else
        {
            StopAllCoroutines();
            FallInGround();
        }
    }

    private void ResetAnimatorToSleep()
    {
        animator.SetBool(Harmony.AnimatorParameters.WakeUp, false);
        animator.SetBool(Harmony.AnimatorParameters.Fly, false);
        animator.SetBool(Harmony.AnimatorParameters.Sleep, true);
    }

    private void FallInGround()
    {
        isReturnOnCeiling = false;
        if (!isInGround)
        {
            transform.position += 20 * moveSpeed * Time.deltaTime * Vector3.down;
        }
    }

    private void ReturnOnInitialState()
    {
        if (returnOnInitialStateCounter <= 0)
        {
            Initialisation();
            animator.SetBool(Harmony.AnimatorParameters.WakeUp, true);
            ++returnOnInitialStateCounter;
        }

        transform.position = Vector2.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime * 10);
        if (transform.position == initialPosition)
        {
            isReturnOnInitialPositon = true;
            animator.SetBool(Harmony.AnimatorParameters.Fly, false);
            animator.SetBool(Harmony.AnimatorParameters.Sleep, true);
        }

    }

    private void MoveToCeiling()
    {

        if (!isReturnOnCeiling)
        {
            if (transform.position.y >= initialPosition.y)
            {
                animator.SetBool(Harmony.AnimatorParameters.isReturnOnCeiling, true);
                Initialisation();
                moveTimer = 150f;
            }
            else
            {
                transform.position += 10 * moveSpeed * Time.deltaTime * Vector3.up;
            }
        }
    }

    private void IsWakeUp()
    {
        animator.SetBool(Harmony.AnimatorParameters.WakeUp, false);
        animator.SetBool(Harmony.AnimatorParameters.Fly, true);
    }

    private void CheckIfPlayerArrived()
    {
        //Si le joueur est en dessous de la chauve-souris, elle ne se declanchera pas si il passe au dessus d'elle.
        if (playerTransform.position.y < transform.position.y)
        {
            //Si le joueur arrive par la gauche alors on prend sa trajectoire de gauche
            if (playerTransform.position.x < transform.position.x)
            {
                pathToGo = 0;
                distanceBetweenStartAndEndPath = Vector2.Distance(paths[pathToGo].GetChild(0).position, paths[pathToGo].GetChild(3).position);
                if (distanceToPlayer < distanceBetweenStartAndEndPath)
                {
                    flyToRight = false;
                    flyToLeft = true;

                    PlayerIsDetected();
                }
            }
            //Si le joueur arrive par la droite alors on prend sa trajectoire de droite
            else if (playerTransform.position.x > transform.position.x)
            {
                pathToGo = 1;
                distanceBetweenStartAndEndPath = Vector2.Distance(paths[pathToGo].GetChild(0).position, paths[pathToGo].GetChild(3).position);
                if (distanceToPlayer < distanceBetweenStartAndEndPath)
                {
                    flyToLeft = false;
                    flyToRight = true;

                    PlayerIsDetected();
                }
            }
        }
    }

    private void PlayerIsDetected()
    {
        playerDetected = true;
        coroutineAllowed = true;
        animator.SetBool(Harmony.AnimatorParameters.WakeUp, true);
    }

    private IEnumerator GoByPath(int pathToGo)
    {

        coroutineAllowed = false;

        //Recuperration des points de passage pour suivre la trajectoire.
        Vector2 pathPosition0 = paths[pathToGo].GetChild(0).position;
        Vector2 pathPosition1 = paths[pathToGo].GetChild(1).position;
        Vector2 pathPosition2 = paths[pathToGo].GetChild(2).position;
        Vector2 pathPosition3 = paths[pathToGo].GetChild(3).position;



        while (time < 1)
        {
            time += Time.deltaTime * moveSpeed;

            //Formule pour calculer les positions via la courbe de Bezier voir https://fr.wikipedia.org/wiki/Courbe_de_B%C3%A9zier
            batPosition = Mathf.Pow(1 - time, 3) * pathPosition0 +
                3 * Mathf.Pow(1 - time, 2) * time * pathPosition1 +
                3 * (1 - time) * Mathf.Pow(time, 2) * pathPosition2 +
                Mathf.Pow(time, 3) * pathPosition3;

            transform.position = batPosition;
            CheckIfPointsAreInZoneBounds();
            //On attend la fin de la frame pour passer a la position suivante
            yield return new WaitForEndOfFrame();
        }


        time = 0f;
        isReturnOnInitialPositon = false;
        playerDetected = false;
        coroutineEnded = true;
        //Reset du timer pour permettre a la chauve-souris de se poser
        moveTimer = 150f;
    }

    private void CheckIfPointsAreInZoneBounds()
    {
        if (flyToLeft)
        {
            if (transform.position.x < minZonePointX)
            {
                transform.position = new Vector2(minZonePointX, transform.position.y);
            }
        }

        if (flyToRight)
        {
            if (transform.position.x > maxZonePointX)
            {
                transform.position = new Vector2(maxZonePointX, transform.position.y);
            }
        }

        if (transform.position.y > initialPosition.y)
        {
            transform.position = new Vector2(transform.position.x, initialPosition.y);
        }
    }

    //Ajout du trigger pour corriger la position car apres plusieurs trajets elle a tendance a monter dans le plafond
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & Harmony.Layers.Ground) != 0)
        {
            if (!isReturnOnCeiling && !isInGround)
            {
                isInGround = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!zone && collision.gameObject.CompareTag(Harmony.Tags.CameraZone))
        {
            zone = collision.gameObject.GetComponent<PolygonCollider2D>();
            GetZoneLimits();
        }
    }

    private void GetZoneLimits()
    {
        minZonePointX = zone.bounds.min.x;
        maxZonePointX = zone.bounds.max.x;
    }
}
