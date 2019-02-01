﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static SaveFile Save;

    public static Camera MainCamera;
    public static SceneSwitcher SceneSwitcher;

    public static GameObject Player;
    public static Movement PlayerMovement;
    public static HealthManager PlayerHealth;
    public static PlayerAnimator PlayerAnimator;
    public static SpriteRenderer PlayerRenderer;

    public GameObject PlayerPrefab;

	// Use this for initialization
	private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
        LoadSave();
        MainCamera = Camera.main;
        SceneSwitcher = MainCamera.GetComponent<SceneSwitcher>();

        Player = Instantiate(PlayerPrefab);
        PlayerMovement = Player.GetComponent<Movement>();
        PlayerHealth = Player.GetComponentInChildren<HealthManager>();
        PlayerAnimator = Player.GetComponent<PlayerAnimator>();
        PlayerRenderer = Player.GetComponent<SpriteRenderer>();

        SceneSwitcher.SwitchIn(Save.ActiveScene, Save.PlayerLocation);
	}

    //Loads the game when the application is opened. Creates a new save if it can't find an existing one
    private void LoadSave()
    {
        Save = ClassToXmlFileIO.Load<SaveFile>("Project_SOUL", "Save");
        if (Save == null)
        {
            Save = new SaveFile();
            ClassToXmlFileIO.Save("Project_SOUL", "Save", Save);
        }
    }

    //Saves the game when the application is closed
    private void OnApplicationQuit()
    {
        if (Save != null)
        {
            ClassToXmlFileIO.Save("Project_SOUL", "Save", Save);
        }
    }
}