﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTwo : PlayerBase {

    public override void UsePopulation()
    {
        base.UsePopulation();
        //Con U aggiungo a me 1 di popolazione e lo tolgo al GameManager
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (GameManager.I.Population > 0)
            {
                GameManager.I.Population -= 1;
                population += 1;
                UpdateGraphic("people: " + population + " press U to add, O to remove"); 
            }
        }
        //Con O tolgo 1 dalla mia popolazione
        if (Input.GetKeyDown(KeyCode.O) && population > 0)
        {
            population -= 1;
            GameManager.I.Population += 1;
            if (population <= 0)
                population = 0;
            UpdateGraphic("people: " + population + " press U to add, O to remove");
        }
    }
    void Update()
    {
        UsePopulation();
    }
}
