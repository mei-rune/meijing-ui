﻿using System;
using System.Threading;
using meijing.ui.module;


namespace meijing.ui
{
    public class ActionDoneEventArgs : EventArgs
    {
        private readonly String _Message;
        public ActionDoneEventArgs(String Message)
        {
            _Message = Message;
        }
        public string Message { get { return _Message; } }
    }

    /// <summary>
    /// TextChangeEventArgs
    /// </summary>
    public class TextChangeEventArgs : EventArgs
    {
        private readonly String _OldString, _NewString;
        public TextChangeEventArgs(String OldString, String NewString)
        {
            _OldString = OldString;
            _NewString = NewString;
        }
        public String OldString { get { return _OldString; } }
        public String NewString { get { return _NewString; } }
    }
    /// <summary>
    /// SelectedIndexChangeEventArgs
    /// </summary>
    public class SelectedIndexChangeEventArgs : EventArgs
    {
        private readonly int _OldIndex, _NewIndex;
        public SelectedIndexChangeEventArgs(int OldIndex, int NewIndex)
        {
            _OldIndex = OldIndex;
            _NewIndex = NewIndex;
        }
        public int OldIndex { get { return _OldIndex; } }
        public int NewIndex { get { return _NewIndex; } }
    }

    /// <summary>
    /// This Method Copy From CLR via C#
    /// </summary>
    public static class EventArgExtensions
    {
        public static void Raise<TEventArgs>(this TEventArgs e,
        Object sender, ref EventHandler<TEventArgs> eventDelegate)
        where TEventArgs : EventArgs
        {
            // Copy a reference to the delegate field now into a temporary field for thread safety
            EventHandler<TEventArgs> temp =
            Interlocked.CompareExchange(ref eventDelegate, null, null);
            // If any methods registered interest with our event, notify them
            if (temp != null) temp(sender, e);
        }
    }
}
