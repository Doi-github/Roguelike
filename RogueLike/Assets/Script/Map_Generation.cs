using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class Map_Generation : MonoBehaviour
{

    public int colums = 20, rows = 20;
    
    
    public GameObject[] Floar;
    public GameObject[] Wall;
    public GameObject[] Enemy;
    public GameObject[] Exit;
    public GameObject[] Player;
    public GameObject[] map_tile;
    public GameObject[] Room_tile;

    public void SetupScene()
    {
        /*BoardSetup();*/
        Area map_area = new Area(null, null, -1, -1, colums, rows);
        GameObject maptile;
        List<Room> rooms = new List<Room>();

        maptile = map_tile[0];

        map_area.splitRondom();
        map_area.map_Gene(map_tile);
        rooms = Roomlist(map_area,rooms);
        Room_Gene(rooms,Room_tile);


    }

    void Start()
    {
        SetupScene();
        
        Instantiate(Player[0], new Vector3(2, 2, 0), Quaternion.identity);
    }


    void BoardSetup()
    {
        int[] Map = MapInitialize(colums,rows);

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

    int[] MapInitialize(int colums,int rows)
    {
        int[] Map = new int[colums*rows];
        for (int i = 0; i < colums*rows; i++)
        {
            Map[i] = 1;
        }

        return Map;
    }

    public class Area
    {
        Area parent;
        public Area[] child;
        public float x;
        public float y;
        public float width;
        public float height;
        public Room room;

        public Area(Area p,Area[] c, float x, float y, float w, float h)
        {
            this.parent = p;
            this.child = c;
            this.x = x;
            this.y = y;
            this.width = w;
            this.height = h;
        }

        public bool splitX(float d)
        {
            float minAreaWidth = 5;
            float w = this.width - d;
            if (d < minAreaWidth || w < minAreaWidth)
            {
                return false;
            }

            Area[] c = { new Area(this, null, this.x, this.y, d, this.height),
                         new Area(this, null, this.x + d, this.y, this.width - d, this.height)};


            this.child = c;
            
            return true;
        }


        public bool splitY(float d)
        {
            float minAreaWidth = 5;
            float h = this.height - d;
            if (d < minAreaWidth || h < minAreaWidth)
            {
                return false;
            }

            Area[] c = { new Area(this, null, this.x, this.y, this.width, d),
                         new Area(this, null, this.x, this.y + d, this.width, h)};


            this.child = c;

            return true;
        }

        public void splitRondom()
        {
            Area t = this;
            int minwidth = 5;
            bool ok; 
            float d1 = (int)Math.Floor((double)UnityEngine.Random.Range(minwidth, t.width -minwidth));
            float d2 = (int)Math.Floor((double)UnityEngine.Random.Range(minwidth, t.height - minwidth));
            
            
            if (UnityEngine.Random.value > 0.5)
            {
               ok = t.splitX(d1);
                /*Debug.Log("splitX");
                Debug.Log(d1);*/
            }
            else
            {
               ok = t.splitY(d2);
                /*Debug.Log("splitY");
                Debug.Log(d2);*/
            }

            if (ok)
            {
                 foreach (Area a in t.child)
                 {
                    a.splitRondom();
                 }
            }
            else
            {
                this.room = make_Room(this);
            }
           
        }

        public void map_Gene(GameObject[] map_tile)
        {
            GameObject maptile;
            
            maptile = map_tile[0];
            
            if(this.child != null)
            {
                foreach (Area a in this.child)
                {
                    a.map_Gene(map_tile);
                }
            }
            else
            {
                maptile.transform.localScale = new Vector3(this.width, this.height, 0);
                Instantiate(maptile, new Vector3(this.x + this.width / 2, this.y + this.height / 2, 0), Quaternion.identity);

                /*roomtile.transform.localScale = new Vector3(this.room.width, this.room.height, 0);
                Instantiate(roomtile, new Vector3(this.room.x + this.room.width / 2, this.room.y + this.room.height / 2, 0), Quaternion.identity);
            */
            }
            
        }
        
    }
    
    public class Room
    {
        public float x;
        public float y;
        public float width;
        public float height;
        Area area;

        public Room(float x, float y, float width, float height, Area area)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.area = area;
        }

    }

    static Room make_Room(Area area)
    {
        float x;
        float y;
        float width;
        float height;

        width = (int)Math.Floor((double)UnityEngine.Random.Range(2, area.width - 2));
        height = (int)Math.Floor((double)UnityEngine.Random.Range(2, area.height - 2));
        x = area.x + (int)Math.Floor((double)UnityEngine.Random.Range(2,area.width -width));
        y = area.y + (int)Math.Floor((double)UnityEngine.Random.Range(2, area.height - height));

        Room room = new Room(x,y,width,height,area);

        return room;
    }

    public List<Room> Roomlist(Area area,List<Room> rooms)
    {
              
        
        if (area.child != null)
        {
            foreach (Area a in area.child)
            {
                Roomlist(a,rooms);
            }

        }
        else
        {
            rooms.Add(area.room);
        }
        return  rooms;
    }

    public void Room_Gene(List<Room> rooms,GameObject[] room_tile)
    {
        GameObject roomtile = room_tile[0];
        foreach(Room room in rooms)
        {
            roomtile.transform.localScale = new Vector3(room.width, room.height, 0);
            Instantiate(roomtile, new Vector3(room.x + room.width / 2, room.y + room.height / 2, 0), Quaternion.identity);
            
        }
    }
}
