using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Publisher : MonoBehaviour
{
    public delegate void PlayerLandingEvent();
    public static event PlayerLandingEvent PlayerLanding;

    public delegate void EnemyDiedEvent();
    public static event EnemyDiedEvent EnemyDied;

    public delegate void FetchDataToSave(bool switchScene);
	public static event FetchDataToSave FetchData;

	public delegate void PushLoadedData(bool loadPosition, bool playerDead);
	public static event PushLoadedData PushData;

	public delegate void EnemyDyingEvent(GameObject enemy, PolygonCollider2D resurrectionZone);
	public static event EnemyDyingEvent EnemyDying;

	public delegate void NecromancerSpawnEvent(PolygonCollider2D resurrectionZone, GameObject necromancer);
	public static event NecromancerSpawnEvent NecromancerSpawn;

	public delegate void NecromancerDeadEvent(PolygonCollider2D resurrectionZone, GameObject necromancer);
	public static event NecromancerDeadEvent NecromancerDead;

	public delegate void EnemyReviveEvent(GameObject enemy);
	public static event EnemyReviveEvent EnemyRevive;

    public void CallPlayerLandingEvent()
	{
        PlayerLanding?.Invoke();
    }

    public void CallEnemyDiedEvent()
    {
        EnemyDied?.Invoke();
    }

	public void CallFetchDataToSave(bool switchScene)
	{
        FetchData?.Invoke(switchScene);
    }

	public void CallPushLoadedData(bool loadPosition, bool playerDead)
	{
        PushData?.Invoke(loadPosition, playerDead);
    }

    public void CallNecromancerDeadInZone(PolygonCollider2D resurrectionZone, GameObject necromancer)
    {
        NecromancerDead?.Invoke(resurrectionZone, necromancer);
    }

    public void CallNecromancerInZone(PolygonCollider2D resurrectionZone, GameObject necromancer)
    {
        NecromancerSpawn?.Invoke(resurrectionZone, necromancer);
    }

    public void CallEnemyDyingEvent(GameObject enemy, PolygonCollider2D resurrectionZone)
	{
        EnemyDying?.Invoke(enemy, resurrectionZone);
    }

	public void CallEnemyRessurected(GameObject enemy)
    {
        EnemyRevive?.Invoke(enemy);
    }
}
