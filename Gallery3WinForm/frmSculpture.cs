namespace Gallery3WinForm
{
    public sealed partial class frmSculpture : frmWork
    {   //Singleton
        public static readonly frmSculpture Instance = new frmSculpture();

        private frmSculpture()
        {
            InitializeComponent();
        }

        public static void Run(clsAllWork prSculpture)
        {
            Instance.SetDetails(prSculpture);
        }

        protected override void updateForm()
        {
            base.updateForm();
            //clsSculpture lcWork = (clsSculpture)this._Work;
            txtWeight.Text = _Work.Weight.ToString();
            txtMaterial.Text = _Work.Material;
        }

        protected override void pushData()
        {
            base.pushData();
            //clsSculpture lcWork = (clsSculpture)_Work;
            _Work.Weight = float.Parse(txtWeight.Text);
            _Work.Material = txtMaterial.Text;
        }
    }
}

