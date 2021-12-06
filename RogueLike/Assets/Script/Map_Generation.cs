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
    public GameObject[] root_tile;


    public void SetupScene()
    {
        /*BoardSetup();*/
        Area map_area = new Area(null, null, -1, -1, colums, rows);
        GameObject maptile;
        List<Room> rooms = new List<Room>();
        Level level = new Level(colums, rows);

        maptile = map_tile[0];

        map_area.splitRondom();
        map_area.map_Gene(map_tile);
        rooms = Roomlist(map_area, rooms);
        List<Room> rooms1 = rooms;
        makePassage(map_area,rooms1,level);
        /*PassageGene(level, root_tile);*/
        RoomToLevel(level,Floar,rooms);
        BoardGene(level,Wall,Floar) ;
        /* Room_Gene(rooms, Room_tile);*/
        
    }

    void Start()
    {
        BoardSetup();

        Instantiate(Player[0], new Vector3(2, 2, 0), Quaternion.identity);
    }


    void BoardSetup()
    {
        Area map_area = new Area(null, null, -1, -1, colums, rows);
        GameObject maptile;
        List<Room> rooms = new List<Room>();
        Level level = new Level(colums, rows);

        maptile = map_tile[0];

        map_area.splitRondom();
        map_area.map_Gene(map_tile);
        rooms = Roomlist(map_area, rooms);
        List<Room> rooms1 = rooms;
        makePassage(map_area, rooms1, level);
        /*PassageGene(level, root_tile);*/
        RoomToLevel(level, Floar, rooms);
        BoardGene(level, Wall, Floar);
        /* Room_Gene(rooms, Room_tile);*/
    }

    static int[] MapInitialize(int colums, int rows)
    {
        int[] Map = new int[colums * rows];
        for (int i = 0; i < colums * rows; i++)
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

        public Area(Area p, Area[] c, float x, float y, float w, float h)
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
            float minAreaWidth = 6;
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
            float minAreaWidth = 6;
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
            int minwidth = 6;
            bool ok;
            float d1 = (int)Math.Floor((double)UnityEngine.Random.Range(minwidth, t.width - minwidth));
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

            if (this.child != null)
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

        public bool has_Ancestor(Area a)
        {
            Area t = this;
            if (t == a)
            {
                return true;
            }else if(t.parent == null){
                return false;
            }else if (t.parent == a)
            {
                return true;
            }
            else
            {
                return t.parent.has_Ancestor(a);
            }
        }

    }

    public class Room
    {
        public float x;
        public float y;
        public float width;
        public float height;
        public Area area;

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

        width = (int)Math.Floor((double)UnityEngine.Random.Range(3, area.width - 3));
        height = (int)Math.Floor((double)UnityEngine.Random.Range(3, area.height - 3));
        x = area.x + (int)Math.Floor((double)UnityEngine.Random.Range(2, area.width - width));
        y = area.y + (int)Math.Floor((double)UnityEngine.Random.Range(2, area.height - height));

        Room room = new Room(x, y, width, height, area);

        return room;
    }

    public List<Room> Roomlist(Area area, List<Room> rooms)
    {
        if (area.child != null)
        {
            foreach (Area a in area.child)
            {
                Roomlist(a, rooms);
            }

        }
        else
        {
            rooms.Add(area.room);
        }
        return rooms;
    }

    public void Room_Gene(List<Room> rooms, GameObject[] room_tile)
    {
        GameObject roomtile = room_tile[0];
        foreach (Room room in rooms)
        {
            roomtile.transform.localScale = new Vector3(room.width, room.height, 0);
            Instantiate(roomtile, new Vector3(room.x + room.width / 2, room.y + room.height / 2, 0), Quaternion.identity);

        }
    }

    public void PassageGene(Level level,GameObject[] root_tile)
    {
        GameObject roottile = root_tile[0];
        for (int y = 0;y < colums;y ++)
        {
            for (int x = 0; x<rows;x++)
            {
                int i = level.XYToIndex(x, y);
                if (level.tile[i] == 0)
                {
                    /*roottile.transform.localScale = new Vector3(x, y, 0);*/
                    Instantiate(roottile, new Vector3((float)(x+0.5), (float)(y+0.5), 0), Quaternion.identity);
                }
            }
        }
    }

    public void RoomToLevel(Level level,GameObject[] Floar, List<Room> rooms )
    {
        GameObject floar = Floar[0];
        foreach(Room room in rooms)
        {
            for (float y = room.y; y < room.y + room.height; y ++)
            {
                for (float x = room.x; x < room.x + room.width; x++ )
                {
                    level.puttile((int)x,(int)y,0);
                    /* Instantiate(floar, new Vector3((float)(x + 0.5), (float)(y + 0.5), 0), Quaternion.identity);
                */}
            }
        }
    }

    public void BoardGene(Level level, GameObject[] Wall,GameObject[] Floar)
    {
        GameObject wall = Wall[0];
        GameObject floar = Floar[0];

        for (int x = -1; x < colums + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate;
                int i = level.XYToIndex(x, y);
                if (x == -1 || x == colums || y == -1 || y == rows || level.tile[i] == 1)
                {
                    toInstantiate = Wall[0];
                }
                else if (level.tile[i] == 0)
                {
                    toInstantiate = Floar[0];
                }
                else
                {
                    toInstantiate = Wall[0];
                }
               
                Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity);
            }
        }

    }


    public class Level
    {
        public int[] tile;
        int colums;
        int rows;
        public Level(int colums, int rows )
        {
            this.tile = MapInitialize(colums,rows);
            this.colums = colums;
            this.rows = rows;
        }

        public int XYToIndex(int x,int y)
        {
            if(x<-1 || x >= this.colums || y < -1 || y >= this.rows)
            {
                return -1;

            }
            else
            {
                return y * this.rows + x;
            }
        }
        public int tileAt(int x,int y)
        {
            int i = this.XYToIndex(x,y);
            if (i == -1)
            {
                return 1;
            }
            else
            {
                return this.tile[i];
            }
        }

        public bool puttile(int x, int y, int n)
        {
            int i = this.XYToIndex(x, y);
            if (i == -1) { return false; }
            this.tile[i] = n;
            return true;
        }

    }

    static Level makePassage(Area area,List<Room> rooms, Level level)
    {
        if (area.child != null)
        {

            Area c0 = area.child[0];
            Area c1 = area.child[1];
            List<Room> rooms0 = new List<Room>();
            List<Room> rooms1 = new List<Room>();
            Room n0;
            Room n1;




            foreach (Room r in rooms)
            {
                /*Debug.Log(r.x);*/
                if (r.area.has_Ancestor(c0))
                {
                    rooms0.Add(r);
                }
                else if (r.area.has_Ancestor(c1))
                {
                    rooms1.Add(r);

                }

            }

            if (c0.y == c1.y)
            {
                float d = c0.x + c0.width;
                n0 = minabsX1(rooms0, d);
                n1 = minabsX2(rooms1, d);
                int o0 = (int)Math.Floor((double)UnityEngine.Random.Range(n0.y, n0.y + n0.height));
                int o1 = (int)Math.Floor((double)UnityEngine.Random.Range(n1.y, n1.y + n1.height));

                for (int x = (int)(n0.x + n0.width); x < d; x++)
                {
                    level.puttile(x, o0, 0);
                }
                for (int x = (int)n1.x -1; x > d ; x--)
                {
                    level.puttile(x, o1, 0);
                }
                for (int y = Math.Min(o0, o1); y <= Math.Max(o0, o1); y++)
                {
                    level.puttile((int)d, y, 0);
                }


            }
            else if (c0.x == c1.x)
            {
                float d = c0.y + c0.height;
                n0 = minabsY1(rooms0, d);
                n1 = minabsY2(rooms1, d);
                int o0 = (int)Math.Floor((double)UnityEngine.Random.Range(n0.x, n0.x + n0.width));
                int o1 = (int)Math.Floor((double)UnityEngine.Random.Range(n1.x, n1.x + n1.width));

                for (int y = (int)(n0.y + n0.height); y < d; y++)
                {
                    level.puttile(o0, y, 0);
                }
                for (int y = (int)n1.y -1 ; y > d; y--)
                {
                    level.puttile(o1, y, 0);
                }
                for (int x = Math.Min(o0, o1); x <= Math.Max(o0, o1); x++)
                {
                    level.puttile(x, (int)d, 0);
                }

            }

            /*if (c0.child != null)
            {*/
            makePassage(c0, rooms, level);
            /*}
            else if (c1.child != null)
            {*/
            makePassage(c1, rooms, level);
            /*}*/
        }
        return level;
                    
        
    }
    
    static Room minabsX1(List<Room> rooms,float d)
    {
        float min = 100;
        Room min_room = rooms[0];
        for (int i = 0;i < rooms.Count();i++ )
        {
            if (min > Math.Abs(rooms[i].x + rooms[i].width - d))
            {
                min = Math.Abs(rooms[i].x + rooms[i].width - d);
                min_room = rooms[i];
            }
            
        }
        return min_room;
    }
    
    static Room minabsX2(List<Room> rooms, float d)
    {
        float min = 100;
        Room min_room = rooms[0];
        for (int i = 0; i < rooms.Count(); i++)
        {
            if (min > Math.Abs(rooms[i].x - d))
            {
                min = Math.Abs(rooms[i].x - d);
                min_room = rooms[i];
            }

        }
        return min_room;
    }

    static Room minabsY1(List<Room> rooms, float d)
    {
        float min = 100;
        Room min_room = rooms[0];
        for (int i = 0; i < rooms.Count(); i++)
        {
            if (min > Math.Abs(rooms[i].y + rooms[i].height - d))
            {
                min = Math.Abs(rooms[i].y + rooms[i].height - d);
                min_room = rooms[i];
            }

        }
        return min_room;
    }

    static Room minabsY2(List<Room> rooms, float d)
    {
        float min = 100;
        Room min_room = rooms[0];
        for (int i = 0; i < rooms.Count(); i++)
        {
            if (min > Math.Abs(rooms[i].y  - d))
            {
                min = Math.Abs(rooms[i].y  - d);
                min_room = rooms[i];
            }

        }
        return min_room;
    }

}
