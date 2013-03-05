using System.Drawing;

namespace meijing.ui.module
{
    public partial class GetResource
    {
        public static Icon GetIcon(IconType theIcon)
        {
            Icon micon = null;

            switch (theIcon)
            {
                case IconType.Yes:
                    micon = meijing.ui.Properties.Resources.ok;
                    break;
                case IconType.No:
                    micon = meijing.ui.Properties.Resources.DELETE;
                    break;
                case IconType.UserGuide:
                    micon = meijing.ui.Properties.Resources.books;
                    break;
            }
            return micon;
        }
        public static Image GetImage(ImageType theImage)
        {
            Image micon = null;
            switch (theImage)
            {
                case ImageType.Blank:
                    micon = meijing.ui.Properties.Resources.Blank;
                    break;
                case ImageType.AccessDB:
                    micon = meijing.ui.Properties.Resources.AccessDB;
                    break;
                case ImageType.ShutDown:
                    micon = meijing.ui.Properties.Resources.ShutDown;
                    break;
                case ImageType.Option:
                    micon = meijing.ui.Properties.Resources.Option;
                    break;
                case ImageType.Refresh:
                    micon = meijing.ui.Properties.Resources.Refresh;
                    break;
                case ImageType.NextPage:
                    micon = meijing.ui.Properties.Resources.NextPage;
                    break;
                case ImageType.PrePage:
                    //水平翻转
                    micon = meijing.ui.Properties.Resources.NextPage;
                    micon.RotateFlip(RotateFlipType.Rotate180FlipY);
                    break;
                case ImageType.LastPage:
                    micon = meijing.ui.Properties.Resources.LastPage;
                    break;
                case ImageType.FirstPage:
                    //水平翻转
                    micon = meijing.ui.Properties.Resources.LastPage;
                    micon.RotateFlip(RotateFlipType.Rotate180FlipY);
                    break;
                case ImageType.Query:
                    micon = meijing.ui.Properties.Resources.Query;
                    break;
                case ImageType.Filter:
                    micon = meijing.ui.Properties.Resources.Filter;
                    break;
                case ImageType.WebServer:
                    micon = meijing.ui.Properties.Resources.WebServer;
                    break;
                case ImageType.Database:
                    micon = meijing.ui.Properties.Resources.Database;
                    break;
                case ImageType.Collection:
                    micon = meijing.ui.Properties.Resources.Collection;
                    break;
                case ImageType.Keys:
                    micon = meijing.ui.Properties.Resources.Keys;
                    break;
                case ImageType.KeyInfo:
                    micon = meijing.ui.Properties.Resources.KeyInfo;
                    break;
                case ImageType.DBKey:
                    micon = meijing.ui.Properties.Resources.DBkey;
                    break;
                case ImageType.Document:
                    micon = meijing.ui.Properties.Resources.Document;
                    break;
                case ImageType.Smile:
                    micon = meijing.ui.Properties.Resources.Smile;
                    break;
                case ImageType.User:
                    micon = meijing.ui.Properties.Resources.User;
                    break;
                default:
                    break;
            }
            return micon;
        }
    }
    public enum IconType
    {
        Yes,
        No,
        UserGuide
    }
    public enum ImageType
    {
        Blank,
        /// <summary>
        /// Access数据库
        /// </summary>
        AccessDB,
        ShutDown,
        NextPage,
        PrePage,
        FirstPage,
        LastPage,
        Query,
        Filter,
        /// <summary>
        /// 选项
        /// </summary>
        Option,
        /// <summary>
        /// 刷新
        /// </summary>
        Refresh,

        WebServer,
        Database,
        Collection,

        Keys,
        KeyInfo,
        DBKey,
        Document,
        User,
        Smile

    }
}
