using System.IO.IsolatedStorage;

namespace HelloWorld
{
    public class Config
    {
        //自定义背景
        public static bool IsBackground
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("IsBackground") ?
                       (bool)IsolatedStorageSettings.ApplicationSettings["IsBackground"] : false;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["IsBackground"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        //第三方App
        public static bool IsThirdApp
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("IsThirdApp") ?
                       (bool)IsolatedStorageSettings.ApplicationSettings["IsThirdApp"] : true;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["IsThirdApp"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        //Tile通知
        public static bool IsTileNotie
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("IsTileNotie") ?
                       (bool)IsolatedStorageSettings.ApplicationSettings["IsTileNotie"] : true;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["IsTileNotie"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        //背景图
        public static string BackImg
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("BackImg") ?
                       (string)IsolatedStorageSettings.ApplicationSettings["BackImg"] : null;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["BackImg"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        //打开状态颜色索引
        public static int openColor
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("openColor") ?
                       (int)IsolatedStorageSettings.ApplicationSettings["openColor"] : 13;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["openColor"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        //关闭状态颜色索引
        public static int closeColor
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("closeColor") ?
                       (int)IsolatedStorageSettings.ApplicationSettings["closeColor"] : 1;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["closeColor"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        //默认状态颜色索引
        public static int defaultColor
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("defaultColor") ?
                       (int)IsolatedStorageSettings.ApplicationSettings["defaultColor"] : 0;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["defaultColor"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        //打开状态颜色RGB
        public static string openColorPath
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("openColorPath") ?
                       (string)IsolatedStorageSettings.ApplicationSettings["openColorPath"] : "#FFE51400";
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["openColorPath"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        //打开状态颜色RGB
        public static string closeColorPath
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("closeColorPath") ?
                       (string)IsolatedStorageSettings.ApplicationSettings["closeColorPath"] : "#FF60A917";
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["closeColorPath"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        //默认状态颜色RGB
        public static string defaultColorPath
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("defaultColorPath") ?
                       (string)IsolatedStorageSettings.ApplicationSettings["defaultColorPath"] : "#FF76608A";
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["defaultColorPath"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        //不透明度
        public static int OpacityValue
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("OpacityValue") ?
                       (int)IsolatedStorageSettings.ApplicationSettings["OpacityValue"] : 8;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["OpacityValue"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }
    }
}
