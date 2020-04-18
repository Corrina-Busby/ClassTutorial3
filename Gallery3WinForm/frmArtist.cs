using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Gallery3WinForm
{
    public partial class frmArtist : Form
    {
        private clsArtist _Artist;
        //private clsWorksList _WorksList;

        private static Dictionary<string, frmArtist> _ArtistFormList =
            new Dictionary<string, frmArtist>();

        private frmArtist()
        {
            InitializeComponent();
        }

        public static void Run(string prArtistName)
        {
            frmArtist lcArtistForm;
            if (string.IsNullOrEmpty(prArtistName) ||
            !_ArtistFormList.TryGetValue(prArtistName, out lcArtistForm))
            {
                lcArtistForm = new frmArtist();
                if (string.IsNullOrEmpty(prArtistName))
                    lcArtistForm.SetDetails(new clsArtist());
                else
                {
                    _ArtistFormList.Add(prArtistName, lcArtistForm);
                    lcArtistForm.refreshFormFromDB(prArtistName);
                }
            }
            else
            {
                lcArtistForm.Show();
                lcArtistForm.Activate();
            }
        }

        private async void refreshFormFromDB(string prArtistName)
        {
            SetDetails(await ServiceClient.GetArtistAsync(prArtistName));
        }

        private void updateTitle(string prGalleryName)
        {
            if (!string.IsNullOrEmpty(prGalleryName))
                Text = "Artist Details - " + prGalleryName;
        }

        private void UpdateDisplay()
        {
            lstWorks.DataSource = null;
            if (_Artist.WorksList != null)
                lstWorks.DataSource = _Artist.WorksList;
        }

        public void UpdateForm()
        {
            txtName.Text = _Artist.Name;
            txtSpeciality.Text = _Artist.Speciality;
            txtPhone.Text = _Artist.Phone;
            //_WorksList = _Artist.WorksList;

            //frmMain.Instance.GalleryNameChanged += new frmMain.Notify(updateTitle);
            //updateTitle(_Artist.ArtistList.GalleryName);
        }

        public void SetDetails(clsArtist prArtist)
        {
            _Artist = prArtist;
            txtName.Enabled = string.IsNullOrEmpty(_Artist.Name);
            UpdateForm();
            UpdateDisplay();
            frmMain.Instance.GalleryNameChanged += new frmMain.Notify(updateTitle);
            //updateTitle(_Artist.ArtistList.GalleryName);
            Show();
        }

        private void pushData()
        {
            _Artist.Name = txtName.Text;
            _Artist.Speciality = txtSpeciality.Text;
            _Artist.Phone = txtPhone.Text;
            //_WorksList.SortOrder = _SortOrder; // no longer required, updated with each rbByDate_CheckedChanged
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            string lcReply = new InputBox(clsAllWork.FACTORY_PROMPT).Answer;
            if (!string.IsNullOrEmpty(lcReply)) // not cancelled?
            {
                clsAllWork lcWork = clsAllWork.NewWork(lcReply[0]);
                if (lcWork != null) // valid artwork created?
                {
                    if (txtName.Enabled)       // new artist not saved?
                    {
                        pushData();
                        await ServiceClient.InsertArtistAsync(_Artist);
                        txtName.Enabled = false;
                    }
                    lcWork.ArtistName = _Artist.Name;
                    frmWork.DispatchWorkForm(lcWork);
                    if (!string.IsNullOrEmpty(lcWork.Name)) // not cancelled?
                    {
                        refreshFormFromDB(_Artist.Name);
                        frmMain.Instance.UpdateDisplay();
                    }
                }
            }
        }

        private void lstWorks_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //_WorksList.EditWork(lstWorks.SelectedIndex);
                frmWork.DispatchWorkForm(lstWorks.SelectedValue as clsAllWork);
                UpdateDisplay();
                frmMain.Instance.UpdateDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            int lcIndex = lstWorks.SelectedIndex;

            if (lcIndex >= 0 && MessageBox.Show("Are you sure?", "Deleting work", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show(await ServiceClient.DeleteArtworkAsync(lstWorks.SelectedItem as clsAllWork));
                refreshFormFromDB(_Artist.Name);
                frmMain.Instance.UpdateDisplay();
            }
        }

        private async void btnClose_Click(object sender, EventArgs e)
        {
            if (isValid() == true)
                try
                {
                    pushData();
                    if (txtName.Enabled)
                    {
                        MessageBox.Show(await ServiceClient.InsertArtistAsync(_Artist));
                        frmMain.Instance.UpdateDisplay();
                        txtName.Enabled = false;
                    }
                    else
                        MessageBox.Show(await ServiceClient.UpdateArtistAsync(_Artist));
                    Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private Boolean isValid()
        {
            //if (txtName.Enabled && txtName.Text != "")
            //    if (_Artist.IsDuplicate(txtName.Text))
            //    {
            //        MessageBox.Show("Artist with that name already exists!", "Error adding artist");
            //        return false;
            //    }
            //    else
            //        return true;
            //else
                return true;
        }

        private void rbByDate_CheckedChanged(object sender, EventArgs e)
        {
            //_WorksList.SortOrder = Convert.ToByte(rbByDate.Checked);
            UpdateDisplay();
        }

    }
}
