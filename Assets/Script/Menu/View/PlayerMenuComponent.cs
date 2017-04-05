﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuComponent : MenuBase {
    

    public override void LoadSelections() {
        PossibiliScelteAttuali.Clear();
        switch (ScelteFatte.Count) {
            case 0:
                if (firstLevelSelections != null)
                {
                    foreach (ISelectable selectable in firstLevelSelections)
                    {
                        PossibiliScelteAttuali.Add(selectable);
                    }
                }
                break;
            case 1:
                CellDoomstock cell = GameManager.I.gridController.Cells[CurrentPlayer.XpositionOnGrid, CurrentPlayer.YpositionOnGrid];
                switch (ScelteFatte[0].UniqueID)
                {   
                    case " + Building":

                        foreach (BuildingData building in CurrentPlayer.BuildingsDataPrefabs)
                        {
                            BuildingData newBuildingInstance = Instantiate<BuildingData>(building);
                            PossibiliScelteAttuali.Add(newBuildingInstance);
                        }
                        break;
                    case " - Building":
                            PossibiliScelteAttuali.Add(cell.building);
                        break;
                    case " -  People":
                        
                        foreach (PopulationData item in cell.building.Population) { 
                            PossibiliScelteAttuali.Add(item);
                        }
                        break;
                
                    default:
                        break;
                }
                break; 
         

            default:
                DoAction();
                return;
        }
        RefreshItemList();
        IndiceDellaSelezioneEvidenziata = 0;
    }

    public override void DoAction() {
        switch (ScelteFatte[0].UniqueID)
        {
            case " + Building":
                CurrentPlayer.DeployBuilding(ScelteFatte[1] as BuildingData);
                break;
            case " - Building":
                CurrentPlayer.DestroyBuilding(ScelteFatte[1].UniqueID);
                break;
            case " -  People":
                CellDoomstock cell = GameManager.I.gridController.Cells[CurrentPlayer.XpositionOnGrid, CurrentPlayer.YpositionOnGrid];
                CurrentPlayer.RemovePopulationFromBuilding(ScelteFatte[1].UniqueID, cell.building);
                //Chiamare funzione population
                break;
            default:
                break;
        }
        Close();
        
    }

    protected override void CreateMenuItem(ISelectable _item)
    {
        GameObject newGO = Instantiate(ButtonPrefab, MenuItemsContainer);
        SelectableMenuItem newItem = newGO.GetComponent<SelectableMenuItem>();
        newItem.SetData(_item);
    }

}
