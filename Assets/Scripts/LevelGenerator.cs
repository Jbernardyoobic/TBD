﻿using System.Linq.Expressions;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public GameObject[] levels;

    private int currentLevel = -1;

    private void Update() {
        if (currentLevel < 0) {
            GenerateLevel(0);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            GenerateLevel(0);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            GenerateLevel(1);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3)) {
            GenerateLevel(2);
        }
    }

    void ClearLevel() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    void GenerateLevel(int mapIndex) {
        ClearLevel();
        currentLevel = mapIndex;
        Instantiate(levels[mapIndex], gameObject.transform.position, Quaternion.identity, transform);
    }
}
