using System;
using System.Windows.Forms;

namespace RstegApp.Controls
{
    public partial class Field : UserControl
    {
        public delegate void ValueChangedEventHandler(object myObject, ValueChangedEventArgs myArgs);

        public event ValueChangedEventHandler FieldValueChanged;
        public Field()
        {
            InitializeComponent();
        }


        protected virtual void OnFieldValueChanged()
        {
            var handler = FieldValueChanged;
            if (handler != null)
            {
                handler(this, new ValueChangedEventArgs(GetValue()));
            }
        }

        public virtual object GetValue()
        {
            throw new NotSupportedException();
        }

        public virtual void SetValue(object value)
        {
            throw new NotSupportedException();
        }
    }
    public class ValueChangedEventArgs
    {
        public object NewValue { get; set; }

        public ValueChangedEventArgs(object value)
        {
            NewValue = value;
        }
    }
}
