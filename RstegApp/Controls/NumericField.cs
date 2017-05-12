using System;

namespace RstegApp.Controls
{
    public partial class NumericField : Field
    {
        public NumericField()
        {
            InitializeComponent();
            NumericUpDownControl.ValueChanged += OnValueChanged;
        }

        private void OnValueChanged(object sender, System.EventArgs e)
        {
            OnFieldValueChanged();
        }

        public override object GetValue()
        {
            return NumericUpDownControl.Value;
        }

        public void SetLabel(string label)
        {
            FieldLabel.Text = label;
        }

        public void SetLimits(int min , int max)
        {
            SetMin(min);
            SetMax(max);
        }

        public void SetMin(int min)
        {
            if (min > NumericUpDownControl.Value)
            {
                NumericUpDownControl.Value = min;
            }
            NumericUpDownControl.Minimum = min;
        }

        public void SetMax( int max)
        {
            if (max < NumericUpDownControl.Value)
            {
                NumericUpDownControl.Value = max;
            }
            NumericUpDownControl.Maximum = max;
        }

        public override void SetValue(object value)
        {
            NumericUpDownControl.Value = Convert.ToInt32(value);
        }
    }
}
