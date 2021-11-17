﻿

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    [SerializeField] private HeatMapVisual heatMapVisual;
    private Grid grid;
    public Camera mainCamera;
    public GameObject player;

    private void Start() {
        grid = new Grid(250, 113, .08f, Vector3.zero);

        heatMapVisual.SetGrid(grid);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 MPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            MPosition.z = 0f;
            Vector3 position = MPosition;
            grid.AddValue(position, 100, 2, 25);
        }
    }


    private void FixedUpdate(){
    	Vector3 PlayerPosition = player.transform.position;
    	PlayerPosition.z = 0f;
    	PlayerPosition.x += 10f;
    	PlayerPosition.y += 5f;
    	Vector3 p = PlayerPosition;
    	grid.AddValue(p, 10, 1, 10);
    }
}

