namespace SyzygyStudio
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// 有关地图的信息。
    /// </summary>
    public static class MapInfo
    {
        /// <summary>
        /// 地图难度等级枚举。
        /// </summary>
        public enum MapLevel
        {
            Easy, Medium, Difficult, Nightmare, Hell, Endless
        };

        /// <summary>
        /// 难度升级所需时间。
        /// </summary>
        public static class LevelTime
        {
            /// <summary>
            /// 简单难度到中等难度。
            /// </summary>
            public const float E2M = 60f;
            /// <summary>
            /// 中等难度到困难难度。
            /// </summary>
            public const float M2D = 60f;
            /// <summary>
            /// 困难难度到噩梦难度。
            /// </summary>
            public const float D2N = 60f;
            /// <summary>
            /// 噩梦难度到地狱难度。
            /// </summary>
            public const float N2H = 80f;
            /// <summary>
            /// 地狱难度到无尽模式。
            /// </summary>
            public const float H2E = 120f;
        }

        public static MapReader OpenMap(string path)
        {
            return new MapReader(path);
        }

    }
}
