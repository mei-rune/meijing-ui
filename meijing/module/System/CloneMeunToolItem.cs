﻿using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace meijing.ui.module
{
    static class CloneMeunToolItem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string str)
        {
            if (str == null || str.Length == 0)
                return false;
            foreach (char c in str)
            {
                if (!Char.IsNumber(c))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 复制菜单项目
        /// </summary>
        /// <param name="orgMenuItem">原始的菜单</param>
        /// <returns>克隆的菜单</returns>
        public static ToolStripMenuItem Clone(this ToolStripMenuItem orgMenuItem)
        {
            ToolStripMenuItem cloneMenuItem = new ToolStripMenuItem();
            ///!!!typeof的参数必须是ToolStripMenuItem的基类!!!如果使用Control则不能取到值!!!
            ///感谢CSDN网友beargo在帖子【如何获取事件已定制方法名?】里面的提示，网上的例子没有说明这个问题
            ///坑爹啊。。。。。。。。
            Delegate[] _List = GetObjectEventList(orgMenuItem, "EventClick", typeof(ToolStripItem));
            if (!SystemManager.MONO_MODE)
            {
                //悲催MONO不支持
                if (_List != null && _List[0] != null)
                {
                    try
                    {
                        cloneMenuItem.Click += new EventHandler(
                                (x, y) => { _List[0].DynamicInvoke(x, y); }
                        );
                    }
                    catch (Exception ex)
                    {
                        SystemManager.ExceptionDeal(ex, cloneMenuItem.Text);
                    }
                }
            }
            cloneMenuItem.Text = orgMenuItem.Text;
            cloneMenuItem.Enabled = orgMenuItem.Enabled;
            cloneMenuItem.BackgroundImage = orgMenuItem.BackgroundImage;
            cloneMenuItem.Image = orgMenuItem.Image;
            //子菜单的复制
            foreach (ToolStripMenuItem item in orgMenuItem.DropDownItems)
            {
                cloneMenuItem.DropDownItems.Add(item.Clone());
            }
            return cloneMenuItem;
        }
        /// <summary>
        /// 复制菜单项目StripButton
        /// </summary>
        /// <param name="orgMenuItem">原始的菜单</param>
        /// <returns>克隆的菜单StripButton</returns>
        public static ToolStripButton CloneFromMenuItem(this ToolStripMenuItem orgMenuItem)
        {
            ToolStripButton cloneButton = new ToolStripButton();
            //!!!typeof的参数必须是ToolStripMenuItem的基类!!!如果使用Control则不能取到值!!!
            ///感谢CSDN网友beargo在帖子【如何获取事件已定制方法名?】里面的提示，网上的例子没有说明这个问题
            ///坑爹啊。。。。。。。。
            Delegate[] _List = GetObjectEventList(orgMenuItem, "EventClick", typeof(ToolStripItem));
            if (!SystemManager.MONO_MODE)
            {
                //悲催MONO不支持
                if (_List != null && _List[0] != null)
                {
                    try
                    {
                        cloneButton.Click += new EventHandler(
                                (x, y) => { _List[0].DynamicInvoke(x, y); }
                        );
                    }
                    catch (Exception ex)
                    {
                        SystemManager.ExceptionDeal(ex);
                    }
                }
            }
            cloneButton.Image = orgMenuItem.Image;
            cloneButton.Enabled = orgMenuItem.Enabled;
            cloneButton.Text = orgMenuItem.Text;
            cloneButton.Checked = orgMenuItem.Checked;
            cloneButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            return cloneButton;
        }
        /// <summary>   
        /// 获取控件事件  zgke@sina.com qq:116149   
        /// </summary>   
        /// <param name="p_Control">对象</param>   
        /// <param name="eventName">事件名 EventClick EventDoubleClick  这个需要看control.事件 是哪个类的名字是什么</param> 
        /// <param name="eventType">如果是WINFROM控件  使用typeof(Control)</param> 
        /// <returns>委托列</returns>   
        public static Delegate[] GetObjectEventList(object obj, String eventName, Type eventType)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic);
            if (propertyInfo != null)
            {
                object eventList = propertyInfo.GetValue(obj, null);
                if (eventList != null && eventList is EventHandlerList)
                {
                    EventHandlerList list = (EventHandlerList)eventList;
                    FieldInfo fieldInfo = eventType.GetField(eventName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                    if (fieldInfo == null)
                    {
                        return null;
                    }
                    Delegate objDelegate = list[fieldInfo.GetValue(obj)];
                    if (objDelegate == null)
                    {
                        return null;
                    }
                    return objDelegate.GetInvocationList();
                }
            }
            return null;
        }
    }
}
