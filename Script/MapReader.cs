namespace SyzygyStudio
{
    using System;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MapReader
    {
        enum Mark { count,name,size,map,direction,group,def};

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
        List<List<List<Vector2>>> directions = null;

        /// <summary>
        /// 敌人出生点组。
        /// </summary>
        List<List<List<Vector2>>> groups = null;

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
            if (directions == null) directions = new List<List<List<Vector2>>>();
            if (groups == null) groups = new List<List<List<Vector2>>>();
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

                        case "Group":
                            mark = Mark.group;
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

                                    int[,] temp_map_flip = new int[size[i].x, size[i].y];

                                    int n = size[i].x; //行
                                    int m = size[i].y; //列

                                    for (int k = 0; k < n; k++)
                                    {
                                        for(int l = 0; l < m; l++)
                                        {
                                            temp_map_flip[k, l] = temp_map[n - 1 - k, l];
                                        }
                                    }
                                    maps.Add(temp_map_flip);
                                    break;

                                case Mark.direction:
                                    analysis(s_not_space, directions);

                                    break;

                                case Mark.group:
                                    analysis(s_not_space, groups);

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
            catch (DirectoryNotFoundException)
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
        public List<List<Vector2>> GetDirection(int id)
        {
            return directions[id - 1];
        }

        /// <summary>
        /// 根据id得到指定地图的敌人出生位置。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<List<Vector2>> GetGroup(int id)
        {
            return groups[id - 1];
        }

        /// <summary>
        /// 得到地图数量。
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return count;
        }

        private void analysis(string s_not_space, List<List<List<Vector2>>> output)
        {
            string[] split_middle_brackets = s_not_space.Trim().Replace(" ", "").Split(']');

            var split_middle_brackets_arr = new List<string>();
            foreach (string smb in split_middle_brackets)
            {
                split_middle_brackets_arr.Add(smb);
            }
            split_middle_brackets_arr.Remove("");

            var sub_group = new List<List<Vector2>>();

            foreach (string smba in split_middle_brackets_arr)
            {
                var smb_1 = smba.Replace('(', ' ').Replace(')', ' ').Replace('[', ' ').Trim().Replace(" ", "");
                var smb_2 = smb_1.Split(',');

                var list_v2 = new List<Vector2>();
                var isFull_2 = false;
                Vector2 smb_v2 = new Vector2();
                foreach (var smb_2_2 in smb_2)
                {
                    var smb_2_2_2 = smb_2_2.Trim();

                    if (!isFull_2)
                    {
                        smb_v2.x = Convert.ToSingle(smb_2_2_2);
                        isFull_2 = true;
                    }
                    else
                    {
                        smb_v2.y = Convert.ToSingle(smb_2_2_2);
                        list_v2.Add(smb_v2);
                        isFull_2 = false;
                    }
                }
                sub_group.Add(list_v2);
            }
            output.Add(sub_group);
        }
    }
}

