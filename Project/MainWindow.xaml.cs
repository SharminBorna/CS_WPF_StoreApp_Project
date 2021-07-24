using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project.Models;

namespace Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<string> specialties = new List<string>()
            {
                "Pediatrics",
                "Gynecology",
                "Neurology",
                "Dermatology",
                "Psychiatry",
                "Surgery"
            };
            this.cmbSpecialties.ItemsSource = specialties;
            cmbSpecialties.Text = "Pediatrics";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Doctor dr = new Doctor();
            dr.ID = Convert.ToInt32(txtId.Text);
            dr.FirstName = txtFirstName.Text;
            dr.LastName = txtLastName.Text;
            dr.Specialties = cmbSpecialties.SelectedItem.ToString();
            dr.Email = txtEmail.Text;
            dr.Contact = txtContactNo.Text;

            var newDoctorAdded = "{'ID':'" + dr.ID + "','FirstName':'" + dr.FirstName + "','LastName':'" + dr.LastName + "','Specialties':'" + dr.Specialties + "','Email':'" + dr.Email + "','Contact':'" + dr.Contact + "'}";

            var json = File.ReadAllText(@"doctor.json");
            var jsonObj = JObject.Parse(json);
            var doctorArray = jsonObj.GetValue("Doctor") as JArray;
            var newDoctor = JObject.Parse(newDoctorAdded);
            doctorArray.Add(newDoctor);

            jsonObj["Doctor"] = doctorArray;
            string newJsonResult = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(@"doctor.json",newJsonResult);

            ShowData();
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            ShowData();
        }

        private void ShowData()
        {
            var json = File.ReadAllText(@"doctor.json");
            var jsonObj = JObject.Parse(json);
            if (jsonObj != null)
            {
                JArray drArray = (JArray)jsonObj["Doctor"];
                List<Doctor> dr = new List<Doctor>();
                foreach (var item in drArray)
                {
                    dr.Add(new Doctor()
                    {
                        ID = Convert.ToInt32(item["ID"]),
                        FirstName = item["FirstName"].ToString(),
                        LastName = item["LastName"].ToString(),
                        Specialties = item["Specialties"].ToString(),
                        Email = item["Email"].ToString(),
                        Contact = item["Contact"].ToString()
                    });
                }
                lstDoctor.ItemsSource = dr;
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            btnUpdate.Visibility = Visibility.Visible;
            Button b = sender as Button;
            Doctor drBtn = b.CommandParameter as Doctor;
            int drId = drBtn.ID;
            txtId.Text = drId.ToString();
            txtFirstName.Text = drBtn.FirstName.ToString();
            txtLastName.Text = drBtn.LastName.ToString();
            cmbSpecialties.SelectedValue = drBtn.Specialties.ToString();
            txtEmail.Text = drBtn.Email.ToString();
            txtContactNo.Text = drBtn.Contact.ToString();
            txtId.IsEnabled = false;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var json = File.ReadAllText(@"doctor.json");
            var jsonObj = JObject.Parse(json);
            JArray drArray = (JArray)jsonObj["Doctor"];

            Button b = sender as Button;
            Doctor drBtn = b.CommandParameter as Doctor;
            int drId = drBtn.ID;

            if (drId > 0)
            {
                var doctorToDeleted = drArray.FirstOrDefault(obj => obj["ID"].Value<int>() == drId);
                drArray.Remove(doctorToDeleted);
                string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                File.WriteAllText(@"doctor.json", output);
                MessageBox.Show("Data Deleted Successfully!!!");
                ShowData();
            }
            else
            {
                MessageBox.Show("Data not found...");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var jsonU = File.ReadAllText(@"doctor.json");
            var jsonObj = JObject.Parse(jsonU);
            JArray drUpdateArray = (JArray)jsonObj["Doctor"];

            var ID = Convert.ToInt32(txtId.Text);
            var FirstName = txtFirstName.Text;
            var LastName = txtLastName.Text;
            var Specialties = cmbSpecialties.SelectedItem.ToString();
            var Email = txtEmail.Text;
            var Contact = txtContactNo.Text;

            foreach (var doctor in drUpdateArray.Where(obj=>obj["ID"].Value<int>()==ID))
            {
                doctor["FirstName"] = !string.IsNullOrEmpty(FirstName) ? FirstName : "";
                doctor["LastName"] = !string.IsNullOrEmpty(LastName) ? LastName : "";
                doctor["Specialties"] = !string.IsNullOrEmpty(Specialties) ? Specialties : "";
                doctor["Email"] = !string.IsNullOrEmpty(Email) ? Email : "";
                doctor["Contact"] = !string.IsNullOrEmpty(Contact) ? Contact : "";
            }
            jsonObj["Doctor"] = drUpdateArray;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(@"doctor.json",output);
            ShowData();
        }
    }
}
