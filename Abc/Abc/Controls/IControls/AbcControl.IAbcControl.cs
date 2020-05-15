using Abc.Visuals;
using System;

namespace Abc.Controls
{
    internal abstract partial class AbcControl
    {
        event EventHandler<InvalidationRequestEventArgs> IAbcControl.InvalidationRequest
        {
            add { this.invalidationRequest += value ; }
            remove { this.invalidationRequest -= value ; }
        }

        AbcVisualTree IAbcControl.VisualTree
        {
            get
            {
                return this.VisualTree;
            }
            set
            {
                this.VisualTree = value;
            }
        }

        IAbcVisual IAbcControl.Root
        {
            get
            {
                return this.Root;
            }
        }

        void IAbcControl.RaiseInvalidationRequest(InvalidationRequestFlag flag)
        {
            switch (flag)
            {
                case InvalidationRequestFlag.None:
                    break;
                case InvalidationRequestFlag.Measure:
                    this.InvalidateMeasure();
                    break;
                case InvalidationRequestFlag.Arrange:
                    this.InvalidateArrange();
                    break;
                case InvalidationRequestFlag.Paint:
                    this.InvalidatePaint();
                    break;
                default:
                    break;
            }
        }
    }
}
