using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


/**
 * https://unity3d.com/learn/tutorials/s/procedural-cave-generation-tutorial
 */

public class GenerateCave : MonoBehaviour
{
    public int width;
    public int height;

    public int cullSize;

    [Range(0,100)]
    public int fill;
    public int[,] map;
    private bool[,] been;

    public bool scuffed;

    public GameObject box;
    public GameObject tl;
    public GameObject tr;
    public GameObject bl;
    public GameObject br;
    public GameObject TSquare;
    public GameObject TLSquare;
    public GameObject TRSquare;

    public string seed;
    public bool randomMap;

    void Start()
    {
        map = new int[width, height];
        been = new bool[width, height];
        // setting map to be completely unvisited
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                been[x,y] = false;
            }
        }
        makeBase();
        drawBase();
        spawnPlayer();
    }

    void spawnPlayer()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector2(7, 7);
    }

    void assignCollider()
    {
        BoxCollider2D temp = GetComponentInChildren<BoxCollider2D>();
        temp.transform.position = new Vector3(transform.position.x + width / 2 - 0.5f, transform.position.y + height / 2 - 0.5f);
        temp.size = new Vector2(width, height);
    }

    // defining set
    void makeBase(){
        if (randomMap) {
            seed = System.DateTime.Now.ToString();
        }
        System.Random random = new System.Random(seed.GetHashCode()); 
        
        for(int x = 0; x < width; x++){
            for(int y = height - 1; y > -1; y--){
                if ((x == 0) || (y == 0) || (x == (width - 1)) || (y == (height - 1))){
                        map[x,y] = 1;
                }
                else {
                    // if less than fill check and fill
                    map[x,y] = (random.Next(0,100) < fill)? 1:0;
                }
            }
        }

        for(int i = 0; i < 5; i++){
            // smoothing edges
            for(int x = 0; x < width - 1; x++){
                for( int y = height - 1; y > -1; y --){
                    int tiles = tilesAround(x,y);
                    if(tiles > 4){
                        map[x,y] = 1;
                    }
                    else if (tiles <= 4) {
                        map[x,y] = 0;
                    }
                }
            }
        }
        for(int x = 5; x < 15; x++){
            for(int y = 5; y < 15; y++){
                map[x,y] = 0;
            }
        }
        if(scuffed){
            cheapRoomFix();
        }
        else {
            roomFix();
        }
    }

    private void resetMap(){
        // setting map to be completely unvisited
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                been[x,y] = false;
            }
        }
    }

    /**
     * @param   size    delete all standalone objects of size or smaller and connects caverns
     */
    public void roomFix(){
        // 0 is empty space
        List<List<Vector2>> Rooms = getRooms(0);
        // 1 is walls
        List<List<Vector2>> Walls = getRooms(1);
        Debug.Log("Rooms " + Rooms.Count);
        // making small rooms fill
        foreach (List<Vector2> room in Rooms){
            if(room.Count < cullSize){
                foreach(Vector2 xy in room){
                    map[(int) xy.x, (int) xy.y] = 1;
                }
            }
        }
        // making small walls fill
        foreach (List<Vector2> wall in Walls){
            if(wall.Count < cullSize){
                foreach(Vector2 xy in wall){
                    map[(int) xy.x, (int) xy.y] = 0;
                }
            }
        }
        Rooms = getRooms(0);
        // rooms and walls are done, now for connections
        int initialRoomCount = Rooms.Count;
        for(int z = 0; z < initialRoomCount; z++){
            if(Rooms.Count < 2){
                return;
            }
            // List<Vector2> mainRoom = Rooms[0];
            int mainX = width;
            int mainY = height;
            int theirX = width;
            int theirY = height;
            Vector2 distance = new Vector2(width, height);
            // checking all other rooms for minimum distance
            // start at 1 since 0 is main room
            int secondRoomIndex = 0;
            for(int i = 1; i < Rooms.Count; i++){
                // comparing to this room
                List<Vector2> connectingRoom = Rooms[i];
                // each coordinate in mainRoom
                for(int a = 0; a < Rooms[0].Count; a++){
                    // other room
                    for(int b = 0; b < connectingRoom.Count; b++){
                        // main room and connecting room respectively
                        Vector2 first = Rooms[0][a];
                        Vector2 second = connectingRoom[b];
                        if(distance.sqrMagnitude > (first - second).sqrMagnitude){
                            secondRoomIndex = i;
                            distance = first - second;
                            mainX = (int) first.x;
                            mainY = (int) first.y;
                            theirX = (int) second.x;
                            theirY = (int) second.y;
                        }
                    }
                }
            }
            // at this point have coordinates of closest points from mainroom to next room
            List<Vector2> changeMe = connectRooms(mainX, mainY, theirX, theirY);
            // room connected now, update Rooms
            Rooms[0].AddRange(changeMe);
            Rooms[0].AddRange(Rooms[secondRoomIndex]);
            Rooms[secondRoomIndex].Clear();
            Rooms.Remove(Rooms[secondRoomIndex]);
        }
        return;
    }

        /**
     * @param   size    delete all standalone objects of size or smaller and connects caverns
     */
    public void cheapRoomFix(){
        // 0 is empty space
        List<List<Vector2>> Rooms = getRooms(0);
        // 1 is walls
        List<List<Vector2>> Walls = getRooms(1);
        // making small rooms fill
        foreach (List<Vector2> room in Rooms){
            if(room.Count < cullSize){
                foreach(Vector2 xy in room){
                    map[(int) xy.x, (int) xy.y] = 1;
                }
            }
        }
        // making small walls fill
        foreach (List<Vector2> wall in Walls){
            if(wall.Count < cullSize){
                foreach(Vector2 xy in wall){
                    map[(int) xy.x, (int) xy.y] = 0;
                }
            }
        }
        Rooms = getRooms(0);
        // rooms and walls are done, now for connections
        int initialRoomCount = Rooms.Count;
        for(int z = 0; z < initialRoomCount; z++){
            if(Rooms.Count < 2){
                return;
            }
            // List<Vector2> mainRoom = Rooms[0];
            int mainX = width;
            int mainY = height;
            int theirX = width;
            int theirY = height;
            Vector2 distance = new Vector2(width, height);
            // checking all other rooms for minimum distance
            // start at 1 since 0 is main room
            int secondRoomIndex = 0;
            // for(int i = 1; i < Rooms.Count; i++){
            //     // comparing to this room
            //     List<Vector2> connectingRoom = Rooms[i];
            //     // each coordinate in mainRoom
            //     for(int a = 0; a < Rooms[0].Count; a++){
            //         // other room
            //         for(int b = 0; b < connectingRoom.Count; b++){
            //             // main room and connecting room respectively
            //             Vector2 first = Rooms[0][a];
            //             Vector2 second = connectingRoom[b];
            //             if(distance.sqrMagnitude > (first - second).sqrMagnitude){
            //                 secondRoomIndex = i;
            //                 distance = first - second;
            //                 mainX = (int) first.x;
            //                 mainY = (int) first.y;
            //                 theirX = (int) second.x;
            //                 theirY = (int) second.y;
            //             }
            //         }
            //     }
            // }
            for(int i = (int) (Rooms[0].Count - 10); i < Rooms[0].Count; i++){
                for(int j = 0; j < (int) (Rooms[1].Count - 10); j++){
                    Vector2 first = Rooms[0][i];
                    Vector2 second = Rooms[1][j];
                    if(distance.sqrMagnitude > (first - second).sqrMagnitude){
                        secondRoomIndex = 1;
                        distance = first - second;
                        mainX = (int) first.x;
                        mainY = (int) first.y;
                        theirX = (int) second.x;
                        theirY = (int) second.y;
                    }
                }
            }
            // at this point have coordinates of closest points from mainroom to next room
            List<Vector2> changeMe = connectRooms(mainX, mainY, theirX, theirY);
            // room connected now, update Rooms
            Rooms[0].AddRange(changeMe);
            Rooms[0].AddRange(Rooms[secondRoomIndex]);
            Rooms[secondRoomIndex].Clear();
            Rooms.Remove(Rooms[secondRoomIndex]);
        }
        return;
    }

    private List<Vector2> connectRooms(int mainX, int mainY, int theirX, int theirY){
        // list of coordinates to change to non walls
        List<Vector2> changeMe = getCoordinates(mainX, mainY, theirX, theirY);
        int x = 0;
        int y = 0;
        List<Vector2> rT = new List<Vector2>();
        // to add surrounding tiles
        for(int i = 0; i < changeMe.Count; i++){
            Vector2 current = changeMe[i];
            x = (int) current.x;
            y = (int) current.y;
            map[x, y] = 0;
            map[x + 1, y]= 0;
            map[x - 1, y]= 0;
            map[x, y + 1] = 0;
            map[x, y - 1] = 0;
            // add to returning vector
            rT.Add(new Vector2(x,y));
            rT.Add(new Vector2(x + 1,y));
            rT.Add(new Vector2(x - 1,y));
            rT.Add(new Vector2(x,y + 1));
            rT.Add(new Vector2(x,y - 1));
        }
        return rT;
    }

    private List<Vector2> getCoordinates(int mainX, int mainY, int theirX, int theirY){
        int x = mainX;
        int y = mainY;
        // list of coordinates from point a to point b
        List<Vector2> atb = new List<Vector2>();
        int dx = theirX - mainX;
        int dy = theirY - mainY;

        bool inverted = false;

        int step = Math.Sign (dx);
        int gradientStep = Math.Sign (dy);
        
        int longest  = Math.Abs (dx);
        int shortest  = Math.Abs (dy);

        if(longest < shortest){
            inverted = true;
            longest = Math.Abs(dy);
            shortest = Math.Abs(dx);
            step = Math.Sign (dy);
            gradientStep = Math.Sign (dx);
        }
        
        int gradientAccumulation = longest / 2;
        for(int i = 0; i < longest; i++){
            atb.Add(new Vector2(x, y));
            if(inverted){
                y += step;
            }
            else {
                x += step;
            }
            gradientAccumulation += shortest;
            if(gradientAccumulation >= longest){
                if(inverted) {
                    x += gradientStep;
                }
                else {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }
        return atb;
    }

    private List<List<Vector2>> getRooms(int type){
        resetMap();
        // returning this list
        List<List<Vector2>> rooms = new List<List<Vector2>>();
        // throwing all blocks into checker
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                // if not visited and matching type, throw to method detecting room
                if((!been[x,y]) && (map[x, y] == type)){
                    rooms.Add(singleRoom(x, y));
                }
            }
        }
        return rooms;
    }

    /**
     * Gets a room based on coordinate and the type of the original coordiante
     */
    private List<Vector2> singleRoom(int x, int y){
        // returning List representing a single room
        List<Vector2> room = new List<Vector2>();
        Queue<Vector2> queue = new Queue<Vector2>();
        // initialize
        queue.Enqueue(new Vector2(x, y));
        // while exploring room
        int counter = 0;
        while(queue.Count > 0){
            counter++;
            Vector2 current = queue.Dequeue();
            if(been[(int) current.x, (int) current.y]){
                continue;
            }
            if(counter < 0){
                return room;
            }
            if((current.x < 0) || (current.x >= width) || (current.y < 0) || (current.y >= height)){
                continue;
            }
            if(map[(int) current.x, (int) current.y] == map[x,y]){
                been[(int) current.x, (int) current.y] = true;
                room.Add(current);
                // add adjacent tiles based on the idea that what is being added to the list is new
                if(current.x < width - 1){
                    if(!been[(int) current.x + 1, (int) current.y]){
                        queue.Enqueue(new Vector2(current.x + 1, current.y));
                    }
                }
                if(current.x > 0){
                    if(!been[(int) current.x - 1, (int) current.y]){
                        queue.Enqueue(new Vector2(current.x - 1, current.y));
                    }
                }
                if(current.y < height - 1){
                    if(!been[(int) current.x, (int) current.y + 1]){
                        queue.Enqueue(new Vector2(current.x, current.y + 1));
                    }
                }
                if(current.y > 0){
                    if(!been[(int) current.x, (int) current.y - 1]){
                        queue.Enqueue(new Vector2(current.x, current.y - 1));
                    }
                }
            }
        }
        return room;
    }

    public int tilesAround(int x, int y){
        if((x < 0) || (x > (width - 1)) || (y < 0) || (y > (height - 1))){
            return -1;
        }
        int walls = 0;
        for(int i = -1; i < 2; i++){
            for(int j = -1; j < 2; j++){
                // if edges
                if(((x + i) <= 0) || ((x + i) >= width - 1) || ((y + j) <= 0) || ((y + j) >= height - 1)){
                    walls++;
                    continue;
                }
                if(map[x + i, y + j] == 1){
                    walls++;
                }
            }
        }
        return walls;
    }

    void drawBase(){
        if(map != null){
            for(int x = 0; x < width ; x++){
                for(int y = height - 1; y > -1; y--){
                    // if edge of map
                    if((x == 0) || (x == width - 1) || (y == 0) || (y == height - 1)){
                        GameObject o = Instantiate(box, new Vector3 ((float) x,(float) y, 0f), Quaternion.identity);
                        o.transform.SetParent(transform, false);
                    }
                    else {
                        drawThing(x, y);
                    }
                }
            }
        }
    }

    private void drawThing(int x, int y){
        // filler boxes
        if ((map[x,y] == 1) && (map[x,y + 1] == 1) && (map[x,y - 1] == 1)){
            // top right empty
            if ((map[x + 1, y + 1] == 0) && (map[x + 1, y] == 1)){
                GameObject o = Instantiate(TRSquare, new Vector3 ((float) x,(float) y, 0f), Quaternion.identity);
                o.transform.SetParent(transform, false);
                map[x,y] = 1;
            }
            // top left empty
            else if ((map[x - 1, y + 1] == 0) && (map[x - 1, y] == 1)){
                GameObject o = Instantiate(TLSquare, new Vector3 ((float) x,(float) y, 0f), Quaternion.identity);
                o.transform.SetParent(transform, false);
                map[x,y] = 1;
            }
            else {
                GameObject o = Instantiate(box, new Vector3 ((float) x,(float) y, 0f), Quaternion.identity);
                o.transform.SetParent(transform, false);
                map[x,y] = 1;
            }
        }
        // checks for if in row by determining if opposites are solids
        else if((((map[x,y + 1] == 1) && (map[x,y - 1] == 1)) ||  ((map[x - 1, y] == 1) && (map[x + 1, y] == 1))) && (map[x,y] == 1)){
            // box with grass check
            if ( map[x, y + 1] == 0) {
                GameObject o = Instantiate(TSquare, new Vector3 ((float) x,(float) y, 0f), Quaternion.identity);
                o.transform.SetParent(transform, false);
                map[x,y] = 1;
            }
            else {
                GameObject o = Instantiate(box, new Vector3 ((float) x,(float) y, 0f), Quaternion.identity);
                o.transform.SetParent(transform, false);
                map[x,y] = 1;
            }
        }
        // checking top left
        else if ((map[x,y] == 1) && (map[x,y + 1] == 1) && (map[x - 1, y] == 1)){
            GameObject o = Instantiate(tl, new Vector3 ((float) x,(float) y, 0f), Quaternion.identity);
            o.transform.SetParent(transform, false);
        }
        // bottom left
        else if ((map[x,y] == 1) && (map[x,y - 1] == 1) && (map[x - 1, y] == 1)){
            GameObject o = Instantiate(bl, new Vector3 ((float) x,(float) y, 0f), Quaternion.identity);
            o.transform.SetParent(transform, false);
        }
        // top right
        else if ((map[x,y] == 1) && (map[x,y + 1] == 1) && (map[x + 1, y] == 1)){
            GameObject o = Instantiate(tr, new Vector3 ((float) x,(float) y, 0f), Quaternion.identity);
            o.transform.SetParent(transform, false);
        }
        // bottom right
        else if ((map[x,y] == 1) && (map[x,y - 1] == 1) && (map[x + 1, y] == 1)){
            GameObject o = Instantiate(br, new Vector3 ((float) x,(float) y, 0f), Quaternion.identity);
            o.transform.SetParent(transform, false);
        }
        // empty
        else {
            map[x,y] = 0;
        }
    }
}
