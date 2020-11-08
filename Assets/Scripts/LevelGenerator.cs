using System.Linq.Expressions;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public GameObject[] levels;
    public int currentLevel;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            GenerateLevel(0);
            currentLevel = 0;
        }

        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            GenerateLevel(1);
            currentLevel = 1;
        }

        if (Input.GetKeyDown(KeyCode.Keypad3)) {
            GenerateLevel(2);
            currentLevel = 2;
        }
    }

    void ClearLevel() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    public void GenerateLevel(int mapIndex) {
        ClearLevel();
        Instantiate(levels[mapIndex], gameObject.transform.position, Quaternion.identity, transform);
    }
}
