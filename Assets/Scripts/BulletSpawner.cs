﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns a line of bullets that moves from right to left, with one bullet missing.
/// </summary>
public class BulletSpawner : MonoBehaviour
{
    public Lever Lever;

    [Space]
    public float Cooldown;
    public List<Transform> SpawnPoints;
    [Space]
    public Bullet BulletPrefab;

    private bool State = false;

    /// <summary>
    /// Subscribes the spawner to a lever
    /// </summary>
    private void Start()
    {
        Lever.OnStateChange += Toggle;
    }

    /// <summary>
    /// Loops through a cooldown, and spawns bullets occasionally
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator Loop()
    {
        float timer = 0;
        while (State)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Spawn(BulletPrefab);
                timer = Cooldown;
            }

            yield return null;
        }
    }

    /// <summary>
    /// Spawn a row of bullets, with one missing
    /// </summary>
    /// <param name="bullet">The prefab of the bullet</param>
    private void Spawn(Bullet bullet)
    {
        int rnd = Random.Range(0, SpawnPoints.Count);
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            if (i != rnd)
            {
                Instantiate(bullet, SpawnPoints[i].position, Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// Turns the spawner on or off
    /// </summary>
    /// <param name="state"></param>
    public void Toggle(bool state)
    {
        State = state;
        if (State)
            StartCoroutine(Loop());
    }
}