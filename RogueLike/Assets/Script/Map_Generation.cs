using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Generation : MonoBehaviour
{

    public int colums = 10, rows = 10;
    public GameObject[] Floar;
    public GameObject[] Wall;
    public GameObject[] Enemy;
    public GameObject[] Exit;
    public GameObject[] Player;

    void BoardSetup()
    {
        for (int x = -1;x < colums + 1; x++)
        {
            for (int y = -1;y < rows +1; y++)
            {
                GameObject toInstantiate;
                if (x == -1 || x == colums || y == -1 || y == rows)
                {
                    toInstantiate = Wall[0];
                }
                else
                {
                    toInstantiate = Floar[0];
                }
                Instantiate(toInstantiate,new Vector3(x,y,0),Quaternion.identity);
            }
        }
    }

    public void SetupScene()
    {
        BoardSetup();
    }

    void Start()
    {
        SetupScene();
        Instantiate(Player[0],new Vector3(1,1,0),Quaternion.identity);
    }

    //

}
