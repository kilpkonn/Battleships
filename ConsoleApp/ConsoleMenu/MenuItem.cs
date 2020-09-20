using System;
using System.Collections.Generic;

namespace ConsoleMenu
{
    public class MenuItem
    {
        public bool IsSelected { get; private set; }
        public string Label { get; set; }
        public string Preview { get; set; }
        public Action OnSelectedCallback { get; set; }
        public Action? OnDeselectedCallback { get; set; }
        
        public readonly List<MenuItem> ChildItems = new List<MenuItem>();

        public MenuItem(string label, string preview, Action onSelectedCallback)
        {
            Label = label;
            Preview = preview;
            OnSelectedCallback = onSelectedCallback;
        }

        public virtual void AddChildItem(MenuItem item)
        {
            ChildItems.Add(item);
        }

        public virtual void OnSelect()
        {
            IsSelected = true;
            OnSelectedCallback();
        }

        public virtual void OnDeselect()
        {
            IsSelected = false;
            OnDeselectedCallback?.Invoke();
        }

        public virtual void Render(int offset, int level)
        {
            Console.SetCursorPosition(level * 8, offset);
            Console.Write(Label);

            if (IsSelected)
            {
                ChildItems.ForEach(item => item.Render(offset++, level + 1));
            }
        }

        public bool HasChildren()
        {
            return ChildItems.Count > 0;
        }
    }
}