using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Net;
using System.Xml.Linq;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;

namespace WeatherForecast
{
    public partial class Form1 : Form
    {
        string apiKey = "dd25fbbb5bc592da35d856662e9222c7";
        ClassResponseWeather myWeather;
        double lon;
        double lat;
        ClassForecast5Days myForecast5Days;

        public Form1()
        {
            InitializeComponent();
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }


        private async void button1_Click(object sender, EventArgs e)
        {
            using (WebClient web = new WebClient())
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={txtcity.Text}&appid={apiKey}";
                var json = web.DownloadString(url);
                Debug.WriteLine(json);
                myWeather = JsonConvert.DeserializeObject<ClassResponseWeather>(json);
            }

            string currenttemp = Math.Round(myWeather.main.temp - 273.15, 0).ToString();
            string maxtemp = Math.Round(myWeather.main.temp_max-273.15, 0).ToString();
            string mintemp = Math.Round(myWeather.main.temp_min-273.15, 0).ToString();
            string wind = myWeather.wind.speed.ToString();
            string humidity = myWeather.main.humidity.ToString();
            string cloud = myWeather.weather[0].description.ToString();

            txtcurrenttemp.Text = currenttemp;
            txtmaxtemp.Text = maxtemp;
            txtmintemp.Text = mintemp;
            txtwindk.Text = wind;
            txthumidity.Text = humidity;
            label6.Text = cloud;
            pictureBox1.ImageLocation = "https://openweathermap.org/img/w/" + myWeather.weather[0].icon + ".png";

            lon = myWeather.coord.lon;
            lat = myWeather.coord.lat;
        }



        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Min Temp (C)", typeof(string));
            dt.Columns.Add("Mean Temp (C)", typeof(string));
            dt.Columns.Add("Max Temp (C)", typeof(string));
            dt.Columns.Add("Humidity (%)", typeof(string));
            dt.Columns.Add("Cloud", typeof(string));

            

            using (WebClient web = new WebClient())
            {
                string uri = $"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&appid={apiKey}";
                var json = web.DownloadString(uri);
                Debug.WriteLine($"response: {json}");
                myForecast5Days = JsonConvert.DeserializeObject<ClassForecast5Days>(json);
            }

            foreach (var npc in myForecast5Days.list)
            {
                dt.Rows.Add(new object[]
                {
                    npc.dt_txt,
                    Math.Round(npc.main.temp_min-273.15, 0),
                    Math.Round(npc.main.temp-273.15, 0),
                    Math.Round(npc.main.temp_max-273.15, 0),
                    npc.main.humidity,
                    npc.weather[0].description
                });
            }

            dataGridView1.DataSource = dt;

        }
    }
}
