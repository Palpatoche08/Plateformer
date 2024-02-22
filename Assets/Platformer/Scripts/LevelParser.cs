using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelParser : MonoBehaviour
{
    public string filename;
    public GameObject rockPrefab;
    public GameObject brickPrefab;
    public GameObject questionBoxPrefab;
    public GameObject stonePrefab;
    public Transform environmentRoot;

    // --------------------------------------------------------------------------
    void Start()
    {
        LoadLevel();
    }

    // --------------------------------------------------------------------------
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadLevel();
        }
    }

    // --------------------------------------------------------------------------
    private void LoadLevel()
    {
        string fileToParse = $"{Application.dataPath}/Resources/{filename}.txt"; // Corrected path format
        Debug.Log($"Loading level file: {fileToParse}");

        Stack<string> levelRows = new Stack<string>();

        // Get each line of text representing blocks in our level
        using (StreamReader sr = new StreamReader(fileToParse))
        {
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                levelRows.Push(line);
            }
        }
        
        int row = 0; 
        // Go through the rows from bottom to top
        while (levelRows.Count > 0)
        {
            string currentLine = levelRows.Pop();
            char[] letters = currentLine.ToCharArray();

            for (int column = 0; column < letters.Length; column++)
            {
                var letter = letters[column];
                GameObject prefabToInstantiate = GetPrefab(letter);

                if (prefabToInstantiate != null)
                {
                    Vector3 newPos = new Vector3(column, row, 0f);
                    GameObject newObj = Instantiate(prefabToInstantiate, newPos, Quaternion.identity, environmentRoot);
                }
                // Todo - Instantiate a new GameObject that matches the type specified by letter
                // Todo - Position the new GameObject at the appropriate location by using row and column
                // Todo - Parent the new GameObject under levelRoot

            }

            row++;
        }
    }

    // --------------------------------------------------------------------------
    private void ReloadLevel()
    {
        foreach (Transform child in environmentRoot)
        {
            Destroy(child.gameObject);
        }
        LoadLevel();
    }

    private GameObject GetPrefab(char letter)
    {
        switch (letter)
        {
            case 'b':
                return brickPrefab;
            case 'x':
                return rockPrefab;
            case '?':
                return questionBoxPrefab;
            case 's':
                return stonePrefab;
            default:
                return null;
        }
    }
}
