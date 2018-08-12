namespace SyzygyStudio
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    /// <summary>
    /// 组，组中内容只有被拿取时会被激活，使用完毕后会被隐藏，T必须是继承Transfom的类型。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Group<T> : IEnumerable<T> where T : Transform
    {
        /// <summary>
        /// 维护项的私有列表。
        /// </summary>
        private List<T> group;
        /// <summary>
        /// 项的位置和旋转。
        /// </summary>
        private Pose pose;

        private BackAction<Transform> backActionT = null;
        private BackAction<Transform, float> backActionTF = null;
        /// <summary>
        /// 实例化空组。
        /// </summary>
        public Group() : this(new T[0]) { }

        /// <summary>
        /// 以一些项来实例化组。
        /// </summary>
        /// <param name="item"></param>
        public Group(params T[] item)
        {
            group = new List<T>(item);
            BackAll();
        }

        /// <summary>
        /// 以一个List来实例化组。 
        /// </summary>
        /// <param name="list"></param>
        public Group(List<T> list)
        {
            group = list;
            BackAll();
        }

        /// <summary>
        /// 向组中增加项。
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            group.Add(item);
            item.gameObject.SetActive(false);
            item.gameObject.AddComponent<BackInfo>();
            if (backActionT != null) item.GetComponent<BackInfo>().SetAction(backActionT);
            if (backActionTF != null) item.GetComponent<BackInfo>().SetAction(backActionTF);
        }

        /// <summary>
        /// 从组中移除项。
        /// </summary>
        /// <param name="item"></param>
        public void Remove(T item, bool isDestroy = true)
        {
            group.Remove(item);
            if (isDestroy) UnityEngine.Object.DestroyImmediate(item.gameObject);
        }

        /// <summary>
        /// 清空组。
        /// </summary>
        public void Clear(bool isDestroy = true)
        {
            if (isDestroy)
            {
                foreach (var g in group)
                {
                    UnityEngine.Object.DestroyImmediate(g.gameObject);
                }
            }
            group.Clear();
        }

        /// <summary>
        /// 从组中拿取一个可用项，无可用项则返回null。
        /// </summary>
        /// <returns></returns>
        public T Take()
        {
            foreach (var g in group)
            {
                if (!g.gameObject.activeSelf)
                {
                    g.gameObject.SetActive(true);
                    return g;
                }
            }
            return null;
        }

        /// <summary>
        /// 通过项的Transform信息设置项的隐藏方式。
        /// </summary>
        /// <param name="action"></param>
        public void Back(BackAction<Transform> action)
        {
            foreach(var g in group)
            {
                if (g.GetComponent<BackInfo>() == null) g.gameObject.AddComponent<BackInfo>();
                backActionTF = null;
                backActionT = action;
                g.GetComponent<BackInfo>().SetAction(action);
            }
        }

        /// <summary>
        /// 通过项的Transform信息和一个float计时器来设置项的隐藏方式。
        /// </summary>
        /// <param name="action"></param>
        public void Back(BackAction<Transform, float> action)
        {
            foreach(var g in group)
            {
                if (g.GetComponent<BackInfo>() == null) g.gameObject.AddComponent<BackInfo>();
                backActionT = null;
                backActionTF = action;
                g.GetComponent<BackInfo>().SetAction(action);
            }
        }

        /// <summary>
        /// 将项隐藏。
        /// </summary>
        public void BackAll()
        {
            foreach (var g in group)
            {
                g.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 设置项的位置和旋转。
        /// </summary>
        /// <param name="pose"></param>
        public void SetPose(Pose pose)
        {
            foreach (var g in group)
            {
                g.SetPositionAndRotation(pose.position, pose.rotation);
            }
        }

        /// <summary>
        /// 设置项的位置。
        /// </summary>
        /// <param name="pos"></param>
        public void SetPosition(Vector3 pos)
        {
            foreach (var g in group)
            {
                g.position = pos;
            }

        }

        /// <summary>
        /// 设置项的旋转。
        /// </summary>
        /// <param name="rot"></param>
        public void SetRotation(Quaternion rot)
        {
            foreach (var g in group)
            {
                g.rotation = rot;   
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)group).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)group).GetEnumerator();
        }

        /// <summary>
        /// 通过下标访问某个项，但不能修改。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                return group[index];
            }
            private set
            {
                group[index] = value;
            }
        }
    }
}
