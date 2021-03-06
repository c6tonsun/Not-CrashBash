﻿using UnityEngine;

public class UIMenuButton : MonoBehaviour
{
    public UIMenuButton upItem;
    public UIMenuButton downItem;
    public UIMenuButton leftItem;
    public UIMenuButton rightItem;
    [HideInInspector]
    public bool isHighlighted;

    public UIMenu nextMenu;
    public UIMenu backMenu;

    [Space(15)]
    public bool isMusicNoice;
    [Range(0f, 1f)]
    public float musicNoice;

    [Space(15)]
    public bool isSoundNoice;
    [Range(0f, 1f)]
    public float soundNoice;

    [Space(15)]
    public bool isPlayerAmount;
    [Range(0, 3)]
    public float playerAmount;

    public bool isCpuDifficulty;
    [Range(1, 5)]
    public float cpuDifficulty = 1;

    [Space(15)]
    public bool isMustach;
    [Range(-1, 9)]
    public int mustach;

    [Space(15)]
    public bool isSceneID;
    [Range(0, 9)]
    public int sceneID;

    [Space(15)]
    public bool isModeSelection;
    public bool isElimination;

    [Space(15)]
    public bool isQuit;

    [Space(15)]
    public bool isContinue;

    [Space(15)]
    public bool isExitToMenu;
    
    [Space(15)]
    public Transform mute;
    public Transform noice;
    
    public void UpdateMusicSlider()
    {
        transform.position = Vector3.Lerp(mute.position, noice.position, musicNoice);
        transform.rotation = Quaternion.Lerp(mute.rotation, noice.rotation, musicNoice);
    }

    public void UpdateSoundSlider()
    {
        transform.position = Vector3.Lerp(mute.position, noice.position, soundNoice);
        transform.rotation = Quaternion.Lerp(mute.rotation, noice.rotation, soundNoice);
    }

    public void UpdatePlayerSlider()
    {
        transform.position = Vector3.Lerp(mute.position, noice.position, (playerAmount/2));
        transform.rotation = Quaternion.Lerp(mute.rotation, noice.rotation, (playerAmount/2));
    }

    public void UpdateDifSlider()
    {
        transform.position = Vector3.Lerp(mute.position, noice.position, (cpuDifficulty/3));
        transform.rotation = Quaternion.Lerp(mute.rotation, noice.rotation, (cpuDifficulty/3));
    }
}
