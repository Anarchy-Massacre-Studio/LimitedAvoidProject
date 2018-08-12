namespace SyzygyStudio
{
    using System;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MapReader
    {
        enum Mark { count,name,size,map,def};

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

        Mark mark = Mark.def;


        public MapReader(string path)
        {
            if (maps == null) maps = new List<int[,]>();
            try
            {
                StreamReader streamReader = new StreamReader(path);
                if (streamReader.ReadLine() != "MapData") throw new DirectoryNotFoundException();

                string all = streamReader.ReadToEnd();
                string[] s = all.Split('{', '}');

                int i = 0;
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

                        case "": break;

                        default:
                            switch (mark)
                            {
                                case Mark.count:
                                    count = Convert.ToInt32(s_not_space);
                                    name = new string[count];
                                    size = new Vector2Int[count];
                                    break;

                                case Mark.name:
                                    name[i] = s_not_space;
                                    break;

                                case Mark.size:
                                    string[] xy = s_not_space.Split(',');
                                    size[i] = new Vector2Int(Convert.ToInt32(xy[0]), Convert.ToInt32(xy[1]));
                                    break;

                                case Mark.map:
                                    string num = s_not_space.Replace('(', ' ').Replace(')', ' ').Trim();
                                    string[] just_num = num.Split(',');
                                    int[] just_num_int = new int[just_num.Length];
                                    int c = 0;
                                    foreach (var j in just_num)
                                    {
                                        just_num_int[c] = Convert.ToInt32(j);
                                        c++;
                                    }
                                    int map_x = size[i].x;
                                    int map_y = size[i].y;

                                    int[,] temp_map = new int[map_x, map_y];
                                    
                                    for(int j = 0; j < just_num_int.Length; j++)
                                    {
                                        temp_map[j / map_y, j % map_y] = just_num_int[j];
                                    }
                                    


                                    maps.Add(temp_map);
                                    i++;
                                    break;

                                case Mark.def: break;
                            }
                            break;
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                Debug.LogError("MapNotExist");
                Application.Quit();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Application.Quit();
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
        /// 得到地图数量。
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return count;
        }
    }
}

