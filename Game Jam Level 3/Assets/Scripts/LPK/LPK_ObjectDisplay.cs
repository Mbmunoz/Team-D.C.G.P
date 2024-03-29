﻿/***************************************************
File:           LPK_ObjectDisplay.cs
Authors:        Christopher Onorati
Last Updated:   3/1/2019
Last Version:   2018.3.4

Description:
  This component serves as a display of values that is
  indicated by spawning and destroying objects.

This script is a basic and generic implementation of its 
functionality. It is designed for educational purposes and 
aimed at helping beginners.

Copyright 2018-2019, DigiPen Institute of Technology
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* CLASS NAME  : LPK_ObjectDisplay
* DESCRIPTION : Create a display using objects (like hearts, arrows, etc.)
**/
[RequireComponent(typeof(Transform))]
public class LPK_ObjectDisplay : LPK_LogicBase
{
    /************************************************************************************/

    [Header("Component Properties")]

    [Tooltip("What object to serve as a display.")]
    [Rename("Display Prefab")]
    public GameObject m_pObjectPrefab;

    [Tooltip("Where to spawn the next object relative to the previous.")]
    [Rename("Display Offset")]
    public Vector3 m_vecSpawnOffset;

    [Tooltip("Maximum amount of objects to show in the display.")]
    [Rename("Max Display Objects")]
    public int m_iMaxDisplayObjects = 5;

    /************************************************************************************/

    int m_iPreviousValue = 0;
    List<GameObject> m_pDisplayObjects = new List<GameObject>();

    /**
    * FUNCTION NAME: OnStart
    * DESCRIPTION  : Initializes events.
    * INPUTS       : None
    * OUTPUTS      : None
    **/
    override protected void OnStart()
    {
        LPK_EventManager.OnLPK_DisplayUpdate += OnEvent;
    }

    /**
    * FUNCTION NAME: OnEvent
    * DESCRIPTION  : Changes the count of objects to use in the display.
    * INPUTS       : data - Event data to parse for validation.
    * OUTPUTS      : None
    **/
    override protected void OnEvent(LPK_EventManager.LPK_EventData data)
    {
        //Incorrect object.
        if (!ShouldRespondToEvent(data))
            return;

        if(data.m_flData.Count < 1)
            return;

        if (m_bPrintDebug)
            LPK_PrintDebug(this, "Display Update");

        //If the new value is greater than the older, create new display objects
        CreateObjects((int)data.m_flData[0]);

        //if the new value is smaller, then destroy the difference
        RemoveObjects((int)data.m_flData[0]);
    
        //Update old value to match the new one
        m_iPreviousValue = (int)data.m_flData[0];
    }

    /**
    * FUNCTION NAME: CreateObjects
    * DESCRIPTION  : Creates objects used in the display.
    * INPUTS       : createCount - How many objects to create in this pass.
    * OUTPUTS      : None
    **/
    void CreateObjects(int createCount)
    {
        //If the new value is greater than the older, create new display objects
        if (createCount > m_iPreviousValue)
        {
            for (int i = 0; i < createCount - m_iPreviousValue && m_pDisplayObjects.Count < m_iMaxDisplayObjects; i++)
            {
                Vector3 ownerPos = transform.position;
                GameObject obj = Instantiate(m_pObjectPrefab, transform.position + (m_vecSpawnOffset * m_iPreviousValue), Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                m_pDisplayObjects.Add(obj);
            }
        }
    }

    /**
    * FUNCTION NAME: RemoveObjects
    * DESCRIPTION  : Removes objects used in the display.
    * INPUTS       : removeCount - How many objects to remove in this pass.
    * OUTPUTS      : None
    **/
    void RemoveObjects(int removeCount)
    {
        if (removeCount < m_iPreviousValue && m_pDisplayObjects.Count > 0)
        {
            int totalLosses = Mathf.Abs(removeCount - m_iPreviousValue);

            for (int i = 0; i < totalLosses; i++)
            {
                GameObject obj = m_pDisplayObjects[m_pDisplayObjects.Count - 1 - i];
                Object.Destroy(obj);
            }
        }
    }
}
