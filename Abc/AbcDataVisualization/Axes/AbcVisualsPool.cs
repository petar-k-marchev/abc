using Abc.Visuals;
using System;
using System.Collections.Generic;

namespace AbcDataVisualization
{
    internal class AbcVisualsPool<T>
        where T : IAbcVisual
    {
        private readonly IAbcVisualsContainer container;
        private readonly Func<T> createVisual;
        private readonly List<T> visuals;

        internal AbcVisualsPool(IAbcVisualsContainer container, Func<T> createVisual)
        {
            this.container = container;
            this.createVisual = createVisual;
            this.visuals = new List<T>();
        }

        internal T GetOrCreate(int index)
        {
            if (index < this.visuals.Count)
            {
                T visual = this.visuals[index];
                visual.IsVisible = true;
                return visual;
            }
            else
            {
                T visual = this.createVisual();
                this.visuals.Add(visual);
                this.container.Children.Add(visual);
                return visual;
            }
        }

        internal void Clear()
        {
            for (int i = this.visuals.Count - 1; i >= 0; i--)
            {
                T visual = this.visuals[i];
                this.container.Children.Remove(visual);
            }

            this.visuals.Clear();
        }

        internal void HideAfter(int index)
        {
            int count = this.visuals.Count;
            for (int i = index; i < count; i++)
            {
                T visual = this.visuals[i];
                visual.IsVisible = false;
            }
        }

        internal IEnumerable<T> GetAll()
        {
            return this.visuals;
        }
    }
}
