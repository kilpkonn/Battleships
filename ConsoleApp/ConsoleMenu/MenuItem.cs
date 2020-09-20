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

        public virtual void Render(uint offset, uint level)
        {
            Console.WriteLine(Label);
        }
    }
}