namespace SyzygyStudio
{
    using System;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MapReader
    {
        enum Mark { count,name,size,map,direction,def};

        /// <summary>
        /// 地图数量。
        /// </summary>
        int count;

        /// <summary>
        /// 地图名字。
        /// </summary>
        string[] name;

        /// <summary>
        /// 地图大小。
        /// </summary>
        Vector2Int[] size;

        /// <summary>
        /// 地图数据列表。
        /// </summary>
        List<int[,]> maps = null;

        /// <summary>
        /// 敌人出生点方向。
        /// </summary>
        List<Vector2[]> directions = null;

        int[] birthPointCount;
        int[] groundCount;

        public static int birthPointCountMax { get; private set; }
        public static int groundCountMax { get; private set; }

        Mark mark = Mark.def;


        public MapReader(byte[] content)
        {
            Debug.LogError("加载地图。");
            //Res.Debug_Text.text += "加载地图！";
            if (maps == null) maps = new List<int[,]>();
            if (directions == null) directions = new List<Vector2[]>();
            try
            {
                //Res.Debug_Text.text += "try!";
                MemoryStream memoryStream = new MemoryStream(content);
                StreamReader streamReader = new StreamReader(memoryStream);
                if (streamReader.ReadLine() != "MapData") throw new DirectoryNotFoundException();

                string all = streamReader.ReadToEnd();
                string[] s = all.Split('{', '}');

                int i = 0; //数组标号。

                foreach (var ss in s)
                {
                    string s_not_space = ss.Trim();
                    switch (s_not_space)
                    {
                        case "Count":
                            mark = Mark.count;
                            break;

                        case "Name":
                            mark = Mark.name;
                            break;

                        case "Size":
                            mark = Mark.size;
                            break;

                        case "Map":
                            mark = Mark.map;
                            break;

                        case "Direction":
                            mark = Mark.direction;
                            break;

                        case "": break;

                        default:
                            switch (mark)
                            {
                                case Mark.count:
                                    count = Convert.ToInt32(s_not_space);
                                    name = new string[count];
                                    size = new Vector2Int[count];
                                    birthPointCount = new int[count];
                                    groundCount = new int[count];
                                    break;

                                case Mark.name:
                                    name[i] = s_not_space;
                                    break;

                                case Mark.size:
                                    string[] xy = s_not_space.Split(',');
                                    size[i] = new Vector2Int(Convert.ToInt32(xy[0]), Convert.ToInt32(xy[1]));
                                    break;

                                case Mark.map:
                                    string[] just_num = s_not_space.Replace('(', ' ').Replace(')', ' ').Trim().Split(',');
                                    int[] just_num_int = new int[just_num.Length];
                                    int c = 0;
                                    foreach (var j in just_num)
                                    {
                                        just_num_int[c] = Convert.ToInt32(j);
                                        c++;
                                    }
                                    foreach (var j in just_num_int)
                                    {
                                        switch (j)
                                        {
                                            case 0: break;

                                            case 1:
                                                groundCount[i]++;
                                                break;

                                            case 2:
                                                birthPointCount[i]++;
                                                break;
                                        }
                                    }

                                    int[,] temp_map = new int[size[i].x, size[i].y];

                                    for (int j = 0; j < just_num_int.Length; j++)
                                    {
                                        temp_map[j / size[i].y, j % size[i].y] = just_num_int[j];
                                    }

                                    int[,] temp_map_flip = temp_map;
                                    /*
                                    for (int k = 0; k < temp_map.GetLength(0); k++)
                                    {
                                        for (int l = 0; l < temp_map.GetLength(1); l++)
                                        {
                                            temp_map_flip[k, l] = temp_map[temp_map.GetLength(0) - k - 1, l];
                                        }
                                    }
                                    */
                                    /*
                                    List<int[]> vs = new List<int[]>(temp_map.GetLength(0));

                                    for (int j = 0; j < just_num_int.Length; j++)
                                    {
                                        vs[j / map_y][j % map_y] = just_num_int[j];
                                        //temp_map[j / map_y, j % map_y] = just_num_int[j];
                                    }
                                    */
                                    maps.Add(temp_map_flip);
                                    break;

                                case Mark.direction:
                                    string[] just_num_2 = s_not_space.Replace('(', ' ').Replace(')', ' ').Trim().Split(',');
                                    if (just_num_2.Length % 2 != 0) throw new Exception("Map Direction read errors!");

                                    Vector2 temp_v2 = new Vector2();
                                    Vector2[] temp_v2s = new Vector2[just_num_2.Length / 2];
                                    bool isFull = false;
                                    int index = 0;
                                    foreach (var f in just_num_2)
                                    {
                                        if (!isFull)
                                        {
                                            temp_v2.x = Convert.ToSingle(f);
                                            isFull = true;
                                        }
                                        else
                                        {
                                            temp_v2.y = Convert.ToSingle(f);
                                            temp_v2s[index] = temp_v2;
                                            index++;
                                            isFull = false;
                                        }
                                    }

                                    directions.Add(temp_v2s);

                                    i++;
                                    break;

                                case Mark.def: break;
                            }
                            break;            
                    }
                }
                streamReader.Close();
                streamReader.Dispose();
                memoryStream.Close();
                memoryStream.Dispose();
                //Res.Debug_Text.text += "加载完成！";
            }
            catch (DirectoryNotFoundException dnfe)
            {
                Debug.LogError("MapNotExist");
                //Res.Debug_Text.text += dnfe.Message;
                Application.Quit();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                //Res.Debug_Text.text += ex.Message;
                Application.Quit();
            }
            finally
            {
                foreach (var v in groundCount)
                {
                    if (v > groundCountMax) groundCountMax = v;
                }
                foreach (var v in birthPointCount)
                {
                    if (v > birthPointCountMax) birthPointCountMax = v;
                }
                //Res.Debug_Text.text += "完毕！";
            }
                
           
        }

        /// <summary>
        /// 根据id得到指定地图。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int[,] GetMap(int id)
        {
            return maps[id - 1];
        }

        /// <summary>
        /// 根据id得到指定地图的敌人攻击方向。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Vector2[] GetDirection(int id)
        {
            return directions[id - 1];
        }

        /// <summary>
        /// 得到地图数量。
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return count;
        }
    }
}

