using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models.Grid;

namespace Fight.Grid
{
    public class GridController : MonoBehaviour
    {
	    private Coroutine _coroutineCheckClick;
        public Transform ParentTemplateObjects;
	    private Stack<HexagonCellScript> way = new Stack<HexagonCellScript>();
        private HexagonCellScript PreviousCell = null;
	    public static bool PlayerCanController = false;
	    protected Action observerFoundWay;
        public GridFactory GridSpawner;
        private BaseGrid _grid;
        public List<HexagonCellScript> Cells => _grid.Cells;
        public List<HexagonCellScript> GetLeftTeamPos => _grid.StartCellsLeftTeam;
        public List<HexagonCellScript> GetRightTeamPos => _grid.StartCellsRightTeam;
         
        public virtual Stack<HexagonCellScript> FindWay(HexagonCellScript startCell, HexagonCellScript finishCell, TypeMovement typeMovement = TypeMovement.Ground){
            way.Clear();
            PreviousCell = null;
            startCell.FindWay(previousCell: null, target: finishCell, typeMovement : typeMovement);
            way.Push(finishCell);
            do{
                if(finishCell != null){
                    PreviousCell = finishCell.PreviousCell;
                    if(PreviousCell != null) way.Push(PreviousCell);
                }
                finishCell = PreviousCell;
            } while((finishCell != startCell) && (finishCell != null));
            OnFoundWay();
            Debug.Log($"founded way(count: {way.Count}) :");
            for(int i = 0; i < way.Count; i++)
            {
                Debug.Log(way.ToArray()[i]);
            }

            return way;
	    }

        public void RegisterOnFoundWay(Action d)
        {
            observerFoundWay += d;
        }

        public void UnregisterOnFoundWay(Action d)
        {
            observerFoundWay -= d;
        }

        private void OnFoundWay()
        {
            if(observerFoundWay != null)
                observerFoundWay();
        }

        private void Start()
        {
            FightControllerScript.Instance.RegisterOnStartFight(StartFight);
            FightControllerScript.Instance.RegisterOnFinishFight(FinishFight);
            _grid = GridSpawner.CreateGrid();
        }

        private void OnDestroy()
        {
            FightControllerScript.Instance.UnregisterOnStartFight(StartFight);
            FightControllerScript.Instance.UnregisterOnFinishFight(FinishFight);
        } 

        private void StartFight()
        {
		    _coroutineCheckClick = StartCoroutine(ICheckClick());
        }
    
        private IEnumerator ICheckClick()
        {
            RaycastHit2D hit;
            while(true)
            {
                if (Input.GetMouseButtonDown(0)){
                    hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down);
                    if(hit != null){
                        if(hit.transform != null){
                            if (hit.transform.CompareTag("HexagonCell")){
                                HexagonCellScript HexagonCell = hit.collider.transform.GetComponent<HexagonCellScript>();
                                HexagonCell.ClickOnMe();
                            }
                            if(hit.transform.CompareTag("Hero")){
                                hit.collider.transform.GetComponent<HeroControllerScript>().ClickOnMe();
                            }
                        }
                    }
                }
                yield return null;
            }

        }

        private void FinishFight()
        {
            StopCoroutine(_coroutineCheckClick);
            _coroutineCheckClick = null;
            CloseGrid();
        }

        public void OpenGrid()
        {
            _grid.OpenGrid();
        }

        public void CloseGrid()
        {
            _grid.CloseGrid();
        }

        private static GridController instance;
        public static GridController Instance => instance;
        void Awake()
        {
            instance = this;
        }
    }
}