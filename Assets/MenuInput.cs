﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInput : MonoBehaviour {

    public Animator animator;
    private bool transitioning;

    private int buttonAmount;

    private int mapID = -1;


    [SerializeField]
    private GameObject SelectedButton;
    [SerializeField]
    private GameObject[] StartMenuButtons;
    [SerializeField]
    private GameObject[] MapSelectButtons;
    [SerializeField]
    private GameObject[] OptionsButtons;

    [SerializeField]
    private Transform[] cameraPoints;
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField][Tooltip("How long it takes between menu swaps")]
    private float lerpTime = 1;

    private float highlightScaleMulti = 1;
   


    public enum Select
    {
        preStart = 6,
        first = 0,
        second = 1,
        third = 2,
        fourth = 3,
        fifth = 4,
        sixth = 5
    }   
    public enum Menu
    {
        main = 0,
        mapselect = 1,
        options = 2
    }
    Menu menus = Menu.main;
    Select selected = Select.preStart;


    private IEnumerator LerpCamera(Transform start, Transform end, float lerpTime, Menu toMenu)
    {
        transitioning = true;
        var elapsedTime = 0f;
        while (elapsedTime < lerpTime) {
            mainCamera.transform.position = Vector3.Lerp(start.position, end.position, elapsedTime / lerpTime);
            mainCamera.transform.rotation = Quaternion.Lerp(start.rotation, end.rotation, elapsedTime / lerpTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        menus = toMenu;
        Debug.Log("Menu changed to " + menus);
        transitioning = false;
        Debug.Log("Transition completed, stopping coroutine");
        StopCoroutine("LerpCamera");
    }

    // Use this for initialization
    void Start () {
        //mainCamera = FindObjectOfType<Camera>().gameObject;
        mainCamera.transform.position = cameraPoints[0].position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!transitioning) GetInput();

        if (menus == Menu.main)
        {
            buttonAmount = StartMenuButtons.Length;
            HandleSelectedHighlight(StartMenuButtons);
        }
        if (menus == Menu.mapselect)
        {
            buttonAmount = MapSelectButtons.Length;
            HandleSelectedHighlight(MapSelectButtons);
        }
        if(menus == Menu.options)
        {
            buttonAmount = OptionsButtons.Length;
            HandleSelectedHighlight(OptionsButtons);
        }
        highlightScaleMulti = 1.1f + Mathf.PingPong(Time.time/30f, 0.06f);
    }

    /// <summary>
    /// Runs Gameobject array through and enables or disables all gameobjects in it.
    /// </summary>
    /// <param name="array">The array in question</param>
    /// <param name="setActive">State you want objects to be in it</param>
    private void SetButtonsActive(GameObject[] array, bool setActive)
    {
        foreach (var Button in array)
        {
            Button.SetActive(setActive);
        }
        SelectedButton.SetActive(setActive);       
    }

    private void HandleSelectedHighlight(GameObject[] MenuArray)
    {
        if (selected == Select.preStart)
        {
            SetButtonsActive(MenuArray, false);
        }
        else
        {
            SetButtonsActive(MenuArray, true);
            SelectedButton.transform.position = MenuArray[(int)selected].transform.position;
            SelectedButton.transform.rotation = MenuArray[(int)selected].transform.rotation;
            var buttonScale = MenuArray[(int)selected].transform.localScale;
            SelectedButton.transform.localScale = new Vector3(
                buttonScale.x * highlightScaleMulti, 
                buttonScale.y * highlightScaleMulti, 
                buttonScale.z * 0.85f);
        }
    }

    private void GetInput()
    {
        var input = Input.GetAxis("Vertical");
        if (Mathf.Abs(input)>0.25f)
        {
#region downwards
            if (selected == Select.first && input < 0 && buttonAmount > 1)
            {
                selected = Select.second;
            }
            if (selected == Select.second && input < 0 && buttonAmount > 2)
            {
                selected = Select.third;
            }
            if (selected == Select.third && input < 0 && buttonAmount > 3)
            {
                selected = Select.fourth;
            }
            if (selected == Select.fourth && input < 0 && buttonAmount > 4)
            {
                selected = Select.fifth;
            }
            if (selected == Select.fifth && input < 0 && buttonAmount > 5)
            {
                selected = Select.sixth;
            }
            #endregion
            #region upwards
            if (selected == Select.second && input > 0 && buttonAmount > 1)
            {
                selected = Select.first;
            }
            if (selected == Select.third && input > 0 && buttonAmount > 2)
            {
                selected = Select.second;
            }
            if (selected == Select.fourth && input > 0 && buttonAmount > 3)
            {
                selected = Select.third;
            }
            if (selected == Select.fifth && input > 0 && buttonAmount > 4)
            {
                selected = Select.fourth;
            }
            if (selected == Select.sixth && input > 0 && buttonAmount > 5)
            {
                selected = Select.fifth;
            }
            #endregion
            Debug.Log(selected);
        }
        if (Input.GetButtonDown("Submit"))
        {

            if (menus == Menu.main) //If current menu is start menu
            {
                if (selected == Select.first)
                {
                    Debug.Log("select Play");
                    //Transport to second menu
                    StartCoroutine(LerpCamera(cameraPoints[0],  //Start point 
                        cameraPoints[1],                        //End point
                        lerpTime,                               //Lerp length in seconds
                        Menu.mapselect));                       //Next menu
                }
                else if (selected == Select.second)
                {
                    //Application.Quit();
                    Debug.Log("application quit");
                }
                else if (selected == Select.preStart)
                {
                    animator.SetInteger("StartPresses", 1);
                    selected = Select.first;
                }
            }

            if (menus == Menu.mapselect) // If current menu is map select menu
            {
                if (selected == Select.first)
                {
                    Debug.Log("select map1");
                    mapID = 1;
                    StartCoroutine(LerpCamera(cameraPoints[1],  //Start point 
                        cameraPoints[2],                        //End point
                        lerpTime,                               //Lerp length in seconds
                        Menu.options));                       //Next menu
                }
                if (selected == Select.second)
                {
                    selected = Select.first;
                    Debug.Log("select map2");
                    mapID = 2;
                    StartCoroutine(LerpCamera(cameraPoints[1],  //Start point 
                        cameraPoints[2],                        //End point
                        lerpTime,                               //Lerp length in seconds
                        Menu.options));                       //Next menu
                }
            }

            if (menus == Menu.options) //If current select is game options menu
            {
                if (selected == Select.first)
                {
                    Debug.Log("start map");
                    if(mapID != -1) SceneManager.LoadScene(mapID);
                }
            }
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (menus == Menu.main)
            {
                animator.SetInteger("StartPresses", 0);
                selected = Select.preStart;
            }
            if (menus == Menu.mapselect)
            {
                StartCoroutine(LerpCamera(cameraPoints[1],
                        cameraPoints[0],                    
                        lerpTime,                              
                        Menu.main));
            }
            if (menus == Menu.options)
            {
                StartCoroutine(LerpCamera(cameraPoints[2],
                        cameraPoints[1],
                        lerpTime,
                        Menu.mapselect));
            }
        }
    }
}