using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleMenu
{
    public class MenuItem
    {
        public ConsoleColor SelectColor { get; set; } = ConsoleColor.Blue;
        public ConsoleColor HoverColor { get; set; } = ConsoleColor.Cyan;
        public ConsoleColor InactiveColor { get; set; } = ConsoleColor.Gray;
        public bool IsSelected { get; set; }
        public bool IsHovered { get; protected set; }
        public bool IsInactive { get; protected set; }

        public string Label { get; set; }
        public string Preview { get; set; }

        public int Width { get; private set; }
        public int Padding { get; set; } = 2;

        public bool OffsetChildMenus { get; set; } = false;

        public MenuItem? Parent { get; protected set; }
        public Action? OnSelectedCallback { get; set; }
        public Action? OnDeselectedCallback { get; set; }
        public Action? OnHoverCallback { get; set; }
        public Action? OnHoverEndCallback { get; set; }

        private int _parentIndex = 0;
        private readonly List<MenuItem> _childItems = new List<MenuItem>();

        public MenuItem(string label, string preview, MenuItem? parent = null, Action? onSelectedCallback = null)
        {
            Label = label;
            Preview = preview;
            Parent = parent;
            Width = label.Length;
            OnSelectedCallback = onSelectedCallback;
        }

        public virtual void AddChildItem(MenuItem item)
        {
            item.Parent = this;
            _childItems.Add(item);
            RecalculateWidth();
        }

        public void RecalculateWidth()
        {
            Width = Label.Length;
            _childItems.ForEach(c =>
            {
                c.RecalculateWidth();
                c.Width = _childItems.Max(i => i.Width);
            });
        }

        public void ClearChildItems()
        {
            _childItems.ForEach(c => c.Parent = null);
            _childItems.Clear();
        }

        private void OnSelect()
        {
            IsSelected = true;
            OnSelectedCallback?.Invoke();
        }

        private void OnDeselect()
        {
            IsSelected = false;
            OnDeselectedCallback?.Invoke();
        }

        private void OnHover()
        {
            if (IsHovered) return;
            IsHovered = true;
            OnHoverCallback?.Invoke();
        }

        private void OnHoverEnd()
        {
            if (!IsHovered) return;
            IsHovered = false;
            OnHoverEndCallback?.Invoke();
        }

        public virtual void Render(int vOffset, int hOffset)
        {
            if (hOffset >= 0)
            {
                Console.SetCursorPosition(hOffset, vOffset);

                if (IsSelected)
                {
                    Console.ForegroundColor = SelectColor;
                }
                else if (IsHovered)
                {
                    Console.ForegroundColor = HoverColor;
                }
                else if (IsInactive)
                {
                    Console.ForegroundColor = InactiveColor;
                }

                Console.Write(Label);
                Console.ForegroundColor = ConsoleColor.White;
            }

            if (IsSelected)
            {
                int i = OffsetChildMenus ? vOffset : 0;
                _childItems.ForEach(item => item.Render(i++, hOffset + Width + Padding));
            }
            else if (IsHovered)
            {
                Console.SetCursorPosition(hOffset + Width + Padding, vOffset);
                Console.Write(Preview);
            }
        }

        public MenuItem TrySelectChild(int index, out int newIndex)
        {
            if (0 <= index && index < _childItems.Count)
            {
                newIndex = 0;
                _childItems[index]._parentIndex = index;
                _childItems[index].OnSelect();
                for (int i = 0; i < _childItems.Count; i++)
                {
                    if (i != index) _childItems[i].IsInactive = true;
                }

                return _childItems[index];
            }

            newIndex = index;
            return this;
        }

        public MenuItem TrySelectParent(int index, out int newIndex)
        {
            if (Parent != null)
            {
                newIndex = _parentIndex;
                OnDeselect();
                Parent._childItems.ForEach(child => child.IsInactive = false);
                return Parent;
            }

            newIndex = index;
            return this;
        }

        public void HoverChild(int index)
        {
            if (index < 0 || index >= _childItems.Count || _childItems[index].IsHovered)
            {
                return;
            }

            _childItems.ForEach(child => child.OnHoverEnd());
            _childItems[index].OnHover();
        }

        public bool HasChildren()
        {
            return _childItems.Count > 0;
        }

        public int GetChildCount()
        {
            return _childItems.Count;
        }
    }
}