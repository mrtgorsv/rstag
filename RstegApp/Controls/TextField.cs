namespace RstegApp.Controls
{
    public partial class TextField : Field
    {
        public TextField()
        {
            InitializeComponent();
            TextControl.TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, System.EventArgs e)
        {
            OnFieldValueChanged();
        }

        public override object GetValue()
        {
            return TextControl.Text;
        }

        public void SetLabel(string label)
        {
            FieldLabel.Text = label;
        }

        public override void SetValue(object value)
        {
            TextControl.Text = value.ToString();
        }
    }
}