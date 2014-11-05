/**
 * 日期：2012.12.5
 * */
using System.Windows;
using System.Collections.Generic;

namespace HelloWorld
{
    /// <summary>
    /// 封装的公共方法、变量
    /// </summary>
    public class Lython
    {

        //颜色列表
        public static ColorData[] ColorList = new ColorData[]
        {
            new ColorData(){ColorPath = Application.Current.Resources["PhoneAccentColor"].ToString(), Name = "主题颜色"},

            new ColorData(){ColorPath = "#FFA4C400", Name = "酸橙色"},
            new ColorData(){ColorPath = "#FF60A917", Name = "绿色"},
            new ColorData(){ColorPath = "#FF008A00", Name = "翠绿色"},
            new ColorData(){ColorPath = "#FF00ABA9", Name = "青色"},

            new ColorData(){ColorPath = "#FF1BA1E2", Name = "蓝绿色"},
            new ColorData(){ColorPath = "#FF0050EF", Name = "钴蓝色"},
            new ColorData(){ColorPath = "#FF6A00FF", Name = "靛蓝色"},
            new ColorData(){ColorPath = "#FFAA00FF", Name = "紫罗兰色"},

            new ColorData(){ColorPath = "#FFF472D0", Name = "粉红色"},
            new ColorData(){ColorPath = "#FFD80073", Name = "洋红色"},
            new ColorData(){ColorPath = "#FFA20025", Name = "暗红色"},
            new ColorData(){ColorPath = "#FFE51400", Name = "红色"},

            new ColorData(){ColorPath = "#FFFA6800", Name = "橙色"},
            new ColorData(){ColorPath = "#FFF0A30A", Name = "琥珀色"},
            new ColorData(){ColorPath = "#FFD8C100", Name = "黄色"},
            new ColorData(){ColorPath = "#FF825A2C", Name = "褐色"},

            new ColorData(){ColorPath = "#FF6D8764", Name = "橄榄色"},
            new ColorData(){ColorPath = "#FF647687", Name = "青灰色"},
            new ColorData(){ColorPath = "#FF76608A", Name = "淡紫色"},
            new ColorData(){ColorPath = "#FF7A3B3F", Name = "灰褐色"},
            new ColorData(){ColorPath = "#FF000000", Name = "黑色"},
        };
    }
}
