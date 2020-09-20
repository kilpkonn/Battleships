using System;
using System.Collections.Generic;

namespace ConsoleMenu
{
    public class MenuItem
    {
        public bool IsSelected { get; set; }
        public bool IsHovered { get; set; }
        
        public string Label { get; set; }
        public string Preview { get; set; }
        
        public MenuItem? Parent { get; protected set; }
        public Action OnSelectedCallback { get; set; }
        public Action? OnDeselectedCallback { get; set; }
        
        private readonly List<MenuItem> _childItems = new List<MenuItem>();

        public MenuItem(string label, string preview, Action onSelectedCallback, MenuItem? parent = null)
        {
            Label = label;
            Preview = preview;
            OnSelectedCallback = onSelectedCallback;
            Parent = parent;
        }

        public virtual void AddChildItem(MenuItem item)
        {
            item.Parent = this;
            _childItems.Add(item);
        }

        private void OnSelect()
        {
            IsSelected = true;
            OnSelectedCallback();
        }

        private void OnDeselect()
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
                _childItems.ForEach(item => item.Render(offset++, level + 1));
            }
        }

        public MenuItem TrySelectChild(int index, out int newIndex)
        {
            if (0 <= index && index < _childItems.Count)
            {
                newIndex = 0;
                _childItems[index].OnSelect();
                return _childItems[index];
            }

            newIndex = index;
            return this;
        }

        public MenuItem TrySelectParent(int index, out int newIndex)
        {
            if (Parent != null)
            {
                newIndex = 0;
                OnDeselect();
                return Parent;
            }

            newIndex = index;
            return this;
        }

        public bool HasChildren()
        {
            return _childItems.Count > 0;
        }
    }
}