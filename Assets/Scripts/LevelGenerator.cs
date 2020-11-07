using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public Texture2D[] map;
    public ColorToPrefab[] colorMappings;

    private void Update() {
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
        for (int x = 0; x < map[mapIndex].width; x++) {
            for (int y = 0; y < map[mapIndex].height; y++) {
                GenerateTile(x, y, mapIndex);
            }
        }

    }

    void GenerateTile(int x, int y, int mapIndex) {
        Color pixelColor = map[mapIndex].GetPixel(x, y);

        if (pixelColor.a == 0) {
            return;
        }

        foreach (ColorToPrefab colorMapping in colorMappings) {
            if (colorMapping.color.Equals(pixelColor)) {
                Vector2 position = new Vector2(x, y);
                Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
            }
        }

    }
}
