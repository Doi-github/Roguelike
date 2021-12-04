using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Map_Generation : MonoBehaviour
{

    public int colums = 20, rows = 20;
    
    public GameObject[] Floar;
    public GameObject[] Wall;
    public GameObject[] Enemy;
    public GameObject[] Exit;
    public GameObject[] Player;


    public void SetupScene()
    {
        BoardSetup();
    }

    void Start()
    {
        SetupScene();
        Instantiate(Player[0], new Vector3(2, 2, 0), Quaternion.identity);
    }


    void BoardSetup()
    {
        int[] Map = Random_Map();

        for (int x = -1; x < colums + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate;

                if (x == -1 || x == colums || y == -1 || y == rows)
                {
                    toInstantiate = Wall[0];
                }
                else if (Map[y + 20 * x] == 0)
                {
                    toInstantiate = Floar[0];
                }
                else
                {
                    toInstantiate = Wall[0];
                }
                /*if (x == -1 || x == colums || y == -1 || y == rows)
                {
                    toInstantiate = Wall[0];
                }
                else
                {
                    toInstantiate = Floar[0];
                }*/
                Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }

    int[] Random_Map()
    {
        int[] Map = new int[400];
        for (int i = 0; i < 400; i++)
        {
            Map[i] = 0;
        }

        return Map;
    }

    class Area
    {
        Area parent;
        Area[] child;
        int x;
        int y;
        int width;
        int height;

        public Area(Area p,Area[] c, int x,int y, int w, int h)
        {
            this.parent = p;
            this.child = c;
            this.x = x;
            this.y = y;
            this.width = w;
            this.height = h;
        }

        public bool splitX(int d,Area area)
        {
            int minAreaWidth = 5;
            if (d < minAreaWidth || area.width - d < minAreaWidth)
            {
                return false;
            }

            Area[] c = { new Area(area,new Area[2],area.x,area.y, d,area.height),
                         new Area(area,new Area[2],area.x + d, area.y, area.width - d, area.height)};


            area.child = c;
            
            return true;
        }


        public bool splitY(int d,Area area)
        {
            int minAreaWidth = 5;
            if (d < minAreaWidth || area.width - d < minAreaWidth)
            {
                return false;
            }

            Area[] c = { new Area(area,new Area[2],area.x,area.y, d,area.height),
                         new Area(area,new Area[2],area.x + d, area.y, area.width - d, area.height)};


            area.child = c;

            return true;
        }

        public void splitRondom()
        {
            Area map_area;
            int minwidth = 5;


        }

    }
}
