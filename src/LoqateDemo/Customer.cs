using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

using Loqate;
using Loqate.Models;

namespace LoqateDemo
{
    public partial class Customer : Form
    {
        LoqateClient _client;
        TextChangedSource _textChangedBy = TextChangedSource.User;

        public Customer()
        {
            InitializeComponent();
            
            _client = CreateLoqateClient(); // initialise the Loqate

            LoadCountryComboBox();
        }

        #region Event handlers

        private void cmbCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            // DebugOutput(((Country)cmbCountry.SelectedItem).Code);
        }

        private async void txtZip_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtZip.Text))
            {
                ClearAddressHintsList();
                return;
            }

            if (_textChangedBy == TextChangedSource.User)
            {
                lstAddressHints.Top = txtZip.Top + txtZip.Height;
                lstAddressHints.Location = new Point(txtZip.Location.X, txtZip.Location.Y + txtZip.Height);
                lstAddressHints.Visible = true;
                await LoadInitialAddresses(txtZip.Text);
                lstAddressHints.TabIndex = txtZip.TabIndex + 1;
            }
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            await LoadInitialAddresses(txtAddressLine1.Text);
        }

        private async void txtAddressLine1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAddressLine1.Text))
            {
                ClearAddressHintsList();
                return;
            }

            if (_textChangedBy == TextChangedSource.User)
            {
                lstAddressHints.Top = txtAddressLine1.Top + txtAddressLine1.Height;
                lstAddressHints.Location = new Point(txtAddressLine1.Location.X, txtAddressLine1.Location.Y + txtAddressLine1.Height);
                lstAddressHints.Visible = true;
                await LoadInitialAddresses(txtAddressLine1.Text);
                lstAddressHints.TabIndex = txtAddressLine1.TabIndex + 1;
            }
        }

        private async void lstAddressHints_Click(object sender, EventArgs e)
        {
            await ProcessSelectedAddressHint();
        }

        private async void lstAddressHints_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                await ProcessSelectedAddressHint();
            }
        }

        private async void Customer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                ClearAddressHintsList();
                return;
            }

            if (e.KeyChar == 13)
            {
                if (lstAddressHints.Visible)
                {
                    lstAddressHints.Focus();
                    await ProcessSelectedAddressHint();
                }
            }
        }

        private void Customer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                if (lstAddressHints.Visible)
                {
                    lstAddressHints.Focus();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        #endregion Event handlers

        #region Private methods

        private void LoadCountryComboBox()
        {
            cmbCountry.DataSource = GetValidCountries();
            cmbCountry.DisplayMember = "Description";
            cmbCountry.ValueMember = "Code";

            cmbCountry.SelectedIndex = 1;   // display GB country in the list
        }

        private List<Country> GetValidCountries()
        {
            List<Country> countries = new List<Country>
            {
                new Country { Code = "US", Description = "United States"},
                new Country { Code = "GB", Description = "United Kingdom"},
                new Country { Code = "SE", Description = "Sweden"},
            };

            return (countries);
        }

        private void DebugOutput(string text)
        {
            System.Diagnostics.Debug.Write(text + Environment.NewLine);
        }

        private LoqateClient CreateLoqateClient()
        {
            var baseUrl = ConfigurationManager.AppSettings["LoqateBaseUrl"];
            var apiKey = ConfigurationManager.AppSettings["LoqateApiKey"];
            var initialFindResource = ConfigurationManager.AppSettings["LoqateInitialFindResource"];
            var subsequentFindResource = ConfigurationManager.AppSettings["LocateSubsequentFindResource"];
            var retrieveResource = ConfigurationManager.AppSettings["LoqateRetrieveResource"];

            LoqateClient client = new LoqateClient(apiKey, baseUrl, initialFindResource, subsequentFindResource, retrieveResource);

            return (client);
        }

        private async Task LoadInitialAddresses(string query)
        {
            var country = ((Country)cmbCountry.SelectedItem).Code;

            DebugOutput($"Country = {country} && Query = {query}");

            List<AddressSummary> addressSummaries = await _client.InitialFindAsync(country, query);

            lstAddressHints.BringToFront();
            lstAddressHints.DataSource = null;
            lstAddressHints.Items.Clear();
            lstAddressHints.DataSource = addressSummaries;
            lstAddressHints.DisplayMember = "DisplayText";
            lstAddressHints.ValueMember = "Id";

            toolStripStatusLabel1.Text = addressSummaries.Count + " addresses";
        }

        private async Task LoadSubsequentAddresses(AddressSummary addressSummary)
        {
            var country = ((Country)cmbCountry.SelectedItem).Code;

            DebugOutput($"Country = {country} && Query = {addressSummary.Id}");

            List<AddressSummary> addressSummaries = await _client.SubsequentFindAsync(country, addressSummary);

            lstAddressHints.DataSource = null;
            lstAddressHints.Items.Clear();
            lstAddressHints.DataSource = addressSummaries;
            lstAddressHints.DisplayMember = "DisplayText";
            lstAddressHints.ValueMember = "Id";

            toolStripStatusLabel1.Text = addressSummaries.Count + " addresses";
        }

        private void LoadFullAddressFromHint(FullAddress address)
        {
            _textChangedBy = TextChangedSource.Code;

            txtBuildingNumber.Text = address.BuildingNumber;
            txtAddressLine1.Text = address.Line1;
            txtAddressLine2.Text = address.Line2;
            txtAddressLine3.Text = address.Line3;
            txtCity.Text = address.City;
            txtState.Text = address.ProvinceCode;
            txtZip.Text = address.PostalCode;
            cmbCountry.Text = address.CountryName;

            lstAddressHints.Visible = false;

            toolStripStatusLabel1.Text = "";

            _textChangedBy = TextChangedSource.User;
        }

        private void ClearData()
        {
            _textChangedBy = TextChangedSource.Code;

            txtBuildingNumber.Text = string.Empty;
            txtAddressLine1.Text = string.Empty;
            txtAddressLine2.Text = string.Empty;
            txtAddressLine3.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtZip.Text = string.Empty;

            ClearAddressHintsList();

            _textChangedBy = TextChangedSource.User;

            toolStripStatusLabel1.Text = "";
        }

        private void ClearAddressHintsList()
        {
            lstAddressHints.Visible = false;
            lstAddressHints.DataSource = null;
            lstAddressHints.Items.Clear();
            toolStripStatusLabel1.Text = string.Empty;
        }

        private async Task ProcessSelectedAddressHint()
        {
            if (lstAddressHints.SelectedValue != null)
            {
                var id = lstAddressHints.SelectedValue.ToString();
                AddressSummary addressSummary = lstAddressHints.SelectedItem as AddressSummary;

                if (addressSummary != null && addressSummary.Type == "Address")
                {
                    FullAddress address = await _client.RetrieveFullAddressByIdAsync(id);
                    if (address != null)
                    {
                        LoadFullAddressFromHint(address);
                    }
                }
                else
                {
                    await LoadSubsequentAddresses(addressSummary);
                }
            }
        }

        #endregion Private methods
    }
}
