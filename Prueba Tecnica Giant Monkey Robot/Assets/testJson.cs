using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;


public class testJson : MonoBehaviour {

    public Text title; //UI gameobject for chart title
    public GameObject gridSpace, cellPrefab, columnPrefab; //ScrollView UI element that will contain the DATA and the prefabs that are going to be instantiated inside of it
    public Button refresher; //Button used for JSON refreshing
    // Use this for initialization
    void Start() {

        string json = File.ReadAllText(Application.dataPath + "/StreamingAssets/JsonChallenge.json"); //Loads JSON File from specified path


        /*SPECIFICATIONS FOR JSON CREATION
         * 
         * JSON FILE Requires the following to work with the table:
         * 
         * Title String that holds chart title
         * 
         * ColumnHeaders string array that holds column headers title and amount (It's not limited by any means, it can hold as many columns as needed)
         * 
         * Data: Array of "person" class objects 
         * 
         * Person class is a class that contain a string array that holds person (or item/entity) data (It's not limited by any means, it can hold as many data as needed)
         * 
         * Data and column count mismatching is not a problem. Chart script will handle by showing as much data as there are columns
         * 
         * 
         * Example of a JSON FILE that works:
         * 
         * 
         * {"Title":"Chart title","ColumnHeaders":["columnA","columnB","columnC"],"Data":[{"dataFields":["data1","data2","data3"]},{"dataFields":["data4","data5","data6"]},{"dataFields":["data7","data8","data9"]}]}
         * 
         */


        tabla tablaCargada = JsonUtility.FromJson<tabla>(json); //Deserialize JSON file into "tabla" class

       
        title.text = tablaCargada.Title; 

        gridSpace.GetComponent<GridLayoutGroup>().constraintCount = tablaCargada.ColumnHeaders.Length; //Assign number of columns based on number of columns given in JSON file

        for (int i = 0; i < tablaCargada.ColumnHeaders.Length; i++) //First for cycle instantiate columnHeaders UI elements with their assigned text from JSON file
        {
            GameObject prefab;
            prefab = Instantiate(columnPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            prefab.transform.parent = gridSpace.transform;
            prefab.transform.localScale = new Vector3(1, 1, 1);
            prefab.name = "Columna " + i + " : " + tablaCargada.ColumnHeaders[i];
            prefab.GetComponent<Text>().text = tablaCargada.ColumnHeaders[i];
        }

        for (int i = 0; i < tablaCargada.Data.Length; i++) //Second for cycle instantiate cells of data with their assigned data from daraFields array in JSON
        {
            for (int j = 0; j < tablaCargada.ColumnHeaders.Length; j++) //Data instantiation is limited to columnHeaders count, in order to avoid spawning more data than there are columns and thus unbalance the table
            {
                GameObject prefabCell;
                prefabCell = Instantiate(cellPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                prefabCell.transform.parent = gridSpace.transform;
                prefabCell.transform.localScale = new Vector3(1, 1, 1);
                prefabCell.name = "Fila " + i + "  Data " + j + " : " + tablaCargada.Data[i].dataFields[j];
                prefabCell.GetComponent<Text>().text = tablaCargada.Data[i].dataFields[j];
            }

        }


        refresher.onClick.AddListener(refreshJSON); 
    }

    // Update is called once per frame
    void Update() {

    }
    [System.Serializable]
    public class tabla{ 
        public string Title;

        public string[] ColumnHeaders;

        public person[] Data;
    }
    [System.Serializable]
    public class person{
        public string[] dataFields;

        public person(int size)
        {
            dataFields = new string[size];
        }
    }

    public void refreshJSON()
    {
        Resources.Load(Application.dataPath + "/StreamingAssets/JsonChallenge.json"); //Reload JSON file from specified path

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name); //Reload scene for chart rebuilding
    }

}
