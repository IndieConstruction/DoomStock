﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour {

    /// <summary>
    /// testo visibile per ogni risorsa.
    /// </summary>
    public Text FoodText, StoneText, WoodText, FaithText, SpiritText, HealthcareText, HappinessText;

    #region Logger

    public Logger logger;

    #endregion

    #region LifeCylce

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        UpdateGraphic();
    }

    private void UpdateGraphic()
    {
        FoodText.text = " = " + GameManager.I.GetResourceDataByID("Food").Value.ToString();
        StoneText.text = " = " + GameManager.I.GetResourceDataByID("Stone").Value.ToString();
        WoodText.text = " = " + GameManager.I.GetResourceDataByID("Wood").Value.ToString();
        FaithText.text = " = " + GameManager.I.GetResourceDataByID("Faith").Value.ToString();
        SpiritText.text = " = " + GameManager.I.GetResourceDataByID("Spirit").Value.ToString();
        HealthcareText.text = " = " + GameManager.I.GetResourceDataByID("Healthcare").Value.ToString();
        HappinessText.text = " = " + GameManager.I.GetResourceDataByID("Happiness").Value.ToString();
    }

    #endregion

    #region Functionalities

    #region Menu

    [Header("All Menu")]
    public MenuBase _menuBase;
    public PlayerMenuComponent P1_Menu;
    public PlayerMenuComponent P2_Menu;
    public PlayerMenuComponent P3_Menu;
    public PlayerMenuComponent P4_Menu;
    #endregion

    #endregion

    #region API
    public void SetFoodTextColor() {
        if (GameManager.I.populationManager.IsFoodEnough() == false)
        {
            FoodText.color = Color.yellow;
        }
        else
        {
            FoodText.color = Color.white;
        }
    }

    public List<ISelectable> FirstLevelSelectables = new List<ISelectable>();

    public IMenu ShowMenu(MenuTypes _type, Player _player) {
        FirstLevelSelectables.Clear();
        switch (_type) {
            case MenuTypes.PopulationMenu:
            _menuBase.Init(_player);
            return _menuBase;
            case MenuTypes.Player:

            CellDoomstock cell = GameManager.I.gridController.Cells[_player.XpositionOnGrid, _player.YpositionOnGrid];
            switch (cell.Status) {
                case CellDoomstock.CellStatus.Empty:
                FirstLevelSelectables.Add(
              new Selector() { UniqueID = " + Building", NameLable = "Add Building" } as ISelectable);
                break;
                case CellDoomstock.CellStatus.Filled:
                if (cell.building.PlayerOwner == _player) {
                    FirstLevelSelectables.Add(new Selector() { UniqueID = " - Building", NameLable = "Rem Building" } as ISelectable);
                    if (cell.building.Population.Count > 0) {
                        FirstLevelSelectables.Add(new Selector() { UniqueID = " -  People", NameLable = "Rem People" } as ISelectable);
                    }
                    if (GameManager.I.populationManager.GetAllFreePeople().Count > 0 && cell.building.Population.Count < cell.building.PopulationLimit) {
                        FirstLevelSelectables.Add(new Selector() { UniqueID = " + People", NameLable = "Add People" } as ISelectable);
                    }
                } else {
                    FirstLevelSelectables.Add(new Selector() { UniqueID = " Info ", NameLable = "Info" } as ISelectable);
                }

                break;
                case CellDoomstock.CellStatus.Hole:
                FirstLevelSelectables.Add(new Selector() { UniqueID = " + People", NameLable = "Add People" } as ISelectable);
                break;
                case CellDoomstock.CellStatus.Debris:
                FirstLevelSelectables.Add(new Selector() { UniqueID = " Info ", NameLable = "Info" } as ISelectable);
                if (_player.CanRemoveDebris()) {
                    FirstLevelSelectables.Add(new Selector() { UniqueID = " - Debris", NameLable = "Remove debris" } as ISelectable);
                }
                
                    
                break;
                
                default:
                break;
            }
            switch (_player.ID) {
                case "PlayerOne":
                P1_Menu.Init(_player, FirstLevelSelectables);
                return P1_Menu;
                case "PlayerTwo":
                P2_Menu.Init(_player, FirstLevelSelectables);
                return P2_Menu;
                case "PlayerThree":
                FirstLevelSelectables.Add(
                    new Selector() { UniqueID = "Miracle" } as ISelectable
                );
                P3_Menu.Init(_player, FirstLevelSelectables);
                return P3_Menu;
                case "PlayerFour":
                P4_Menu.Init(_player, FirstLevelSelectables);
                return P4_Menu;
                default:
                break;
            }
            break;
            default:
            break;
        }
        return null; // Menù not found
    }

    /// <summary>
    /// Funzione per scrivere all'interno del logger
    /// </summary>
    /// <param name="_stringToWrite">Cosa scrivere all'interno del logger</param>
    public void WriteInLogger(string _stringToWrite, logType _typeOfLog) {
        //controlla se c'è il collegamento al logger 
        if (logger != null)
            logger.WriteInLogger(_stringToWrite, _typeOfLog);
    }

    #endregion
}


public class Selector : ISelectable {
    public string UniqueID { get; set; }
    public string NameLable { get; set; }
}