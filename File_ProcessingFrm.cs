using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;
using System.Configuration;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.ApplicationServices;

namespace FileProcessTestClient_APMahomana
{
   /*
    * Full Names: Alex Prince
    *    Surname: Mahomana
    *    Cell NO: 072 071 1650
    
    */
    public partial class File_ProcessingFrm : Form
    {
        protected static readonly ILog _ilog = LogManager.GetLogger(typeof(String));

        static string filePath = "";
        static string logPath = "";
        string sourceFile = "";

        public File_ProcessingFrm()
        {
            InitializeComponent();

            log4net.Config.XmlConfigurator.Configure();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            ProcessFile1();

        }

        public void ProcessFile1()
        {
            try
            {
                filePath = ConfigurationSettings.AppSettings["filePath"].ToString();
                logPath = ConfigurationSettings.AppSettings["logPath"].ToString();

                CreateDirectory(logPath);
                sourceFile = txtfilepath.Text;

                log4net.Config.XmlConfigurator.Configure();
                _ilog.Info("File processing start date and time: " + DateTime.Now);

                CreateDirectory(filePath);

                string fileName = "SortedByFrequencyDescendingAndAlphabeticallyAscending.txt";

                filePath = filePath + fileName;

                StreamWriter writer = new StreamWriter(filePath);

                //Create new intance of DataTable 'dtlistNameSurname'
                DataTable dtlistNameSurname = new DataTable();

                // Read CSV File and Store data to DataTable
                dtlistNameSurname = ConvertCSVtoDataTable(sourceFile);

                _ilog.Info("File 1 - Writing data to: " + filePath);


                int totalRecords = 1;


                //Create Array listNameSurname
                String[] listNameSurname = new String[dtlistNameSurname.Rows.Count * 2];

                //Declare index variable and assign ZERO
                int index = 0;

                //Loop through DataTable 'dtlistNameSurname' while adding to Array 'listNameSurname'
                foreach (DataRow drTransaction in dtlistNameSurname.Rows)
                {
                    listNameSurname[index] = drTransaction["FirstName"].ToString();
                    index++;
                    listNameSurname[index] = drTransaction["LastName"].ToString();
                    index++;
                }

                //Create new intance of dictionary
                Dictionary<string, int> dictionary = new Dictionary<string, int>();

                //Loop through array, count occurrence of string while adding to dictionary
                foreach (string word in listNameSurname)
                {
                    if (dictionary.ContainsKey(word))
                    {
                        dictionary[word] += 1;
                    }
                    else
                    {
                        dictionary.Add(word, 1);
                    }
                }

                //Sort dictionary data by frequency descending and assign to 'sortedDict' object
                var sortedDict = from entry in dictionary orderby entry.Value descending select entry;

                //Declare new instance of dictionary
                Dictionary<string, int> dictionaryV2 = new Dictionary<string, int>();

                _ilog.Info("First and Last name ordered by frequency descending and then alphabetically ascending:");
                //Loop through sortedDict, write all string which its occurrence is greater than ZERO
                //Add strings which its occurrance is equal to ONE to dictionaryV2
                foreach (var item in sortedDict)
                {
                    if (item.Value > 1)
                    {
                        writer.Write(item.Key.ToString() + ",");
                        writer.Write(item.Value.ToString());
                        writer.WriteLine();

                        _ilog.Info(item.Key.ToString() + "," + item.Value.ToString());
                    }
                    else
                    {
                        dictionaryV2.Add(item.Key.ToString(), item.Value);
                    }

                }

                //Sort dictionaryV2 data by Ascending and assign to sortedAsc
                var sortedAsc = from entry in dictionaryV2 orderby entry.Key ascending select entry;

                //Loop through sortedAsc, write all data to .txt file
                foreach (var item in sortedAsc)
                {
                    writer.Write(item.Key.ToString() + ",");
                    writer.Write(item.Value.ToString());
                    writer.WriteLine();

                    _ilog.Info(item.Key.ToString() + "," + item.Value.ToString());

                }


                _ilog.Info("Completed writing data to: " + fileName);
                writer.Close();

                txtFirstFile.Text = "Text File 1 path: " + filePath;
                ProcessFile2(dtlistNameSurname);
            }
            catch (Exception ex)
            {
                _ilog.Info("Error: " + ex.Message);

                txtResults.Text = "Error: " + ex.Message;

            }
        }

        public void ProcessFile2(DataTable datatable)
        {
            try
            {
                filePath = ConfigurationSettings.AppSettings["filePath"].ToString();
                logPath = ConfigurationSettings.AppSettings["logPath"].ToString();

                sourceFile = txtfilepath.Text;
                         
                CreateDirectory(logPath);

                log4net.Config.XmlConfigurator.Configure();

                CreateDirectory(filePath);

                string fileName2 = "SortedAlphabeticallyByStreetNames.txt";

                filePath = filePath + fileName2;

                StreamWriter writer = new StreamWriter(filePath);


                _ilog.Info("File 2 - Writing data to: " + filePath);

                int totalRecords = 1;

                //Create new intance of DataTable 'dtlistNameSurname'
                DataTable dtlistNameSurname = new DataTable();

                // Read CSV File and Store data to DataTable
                dtlistNameSurname = datatable;

                //Create Array listNameSurname
                List<AddressInfo> listAddressInfo = new List<AddressInfo>();

                //Declare index variable and assign ZERO
                int index = 0;
                AddressInfo address = null;
                string value = string.Empty;

                //Loop through DataTable 'dtlistNameSurname' while adding to Array 'listNameSurname'
                foreach (DataRow drTransaction in dtlistNameSurname.Rows)
                {
                    address = new AddressInfo();
                    value = drTransaction["Address"].ToString();

                    string[] datastr = value.Split(' ');
                    int size = datastr.Count() - 1;
                    for (int i = 0; i <= size; i++)
                    {
                        if (i == 0)
                        {
                            address.strNo = datastr[i].ToString();
                        }
                        else if (i == 1)
                        {
                            address.strName = datastr[i].ToString();
                        }
                        else if (i == 2)
                        {
                            address.strAbbreviation = datastr[i].ToString();
                        }

                    }

                    if (address != null)
                    {
                        listAddressInfo.Add(address);
                    }



                }

                //Create new intance of dictionary
                Dictionary<string, string> dictionary = new Dictionary<string, string>();

                //Loop through array, count occurrence of string while adding to dictionary
                int listAddressInfoSize = listAddressInfo.Count() - 1;
                for (int i = 0; i <= listAddressInfoSize; i++)
                {
                    AddressInfo word = listAddressInfo[i];
                    if (word != null)
                    {
                        dictionary.Add(word.strNo, word.strName);
                    }
                }


                //Sort dictionary data by frequency ascending and assign to 'sortedDict' object
                var sortedDict = from entry in dictionary orderby entry.Value ascending select entry;

                _ilog.Info("Addresses sorted aphabetically by street names:");

                //Loop through sortedDict, write all string which its occurrence is greater than ZERO
                //Add strings which its occurrance is equal to ONE to dictionaryV2
                foreach (var item in sortedDict)
                {

                    writer.Write(item.Key.ToString() + " ");
                    writer.Write(item.Value.ToString() + " ");

                    foreach (AddressInfo word in listAddressInfo)
                    {

                        if (word.strNo == item.Key.ToString() && word.strName == item.Value.ToString())
                        {
                            writer.Write(word.strAbbreviation + " ");


                            _ilog.Info(item.Key.ToString() + " " + item.Value.ToString() + " " + word.strAbbreviation);
                            break;
                        }

                    }

                    writer.WriteLine();



                }


                _ilog.Info("Completed writing data to: " + fileName2);
                writer.Close();





                #region Tracing
                _ilog.Info("Execution finished, application will now exit");
                _ilog.Info("File splitting successfully completed");

                #endregion

                txtResults.Text = "Successful completed! Please follow below directories to see generated files!";
                txtSecondFile.Text = "Text File 2 path: " + filePath;

            }
            catch (Exception ex)
            {
                _ilog.Info("Error: " + ex.Message);
                txtResults.Text = "Error: " + ex.Message;

            }
        }

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {

            _ilog.Info("Started reading file " + strFilePath);
            _ilog.Info("Data extracted from CSV file:");
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                        _ilog.Info(rows[i]);
                    }

                    dt.Rows.Add(dr);


                }

            }

            _ilog.Info("Reading file successful completed.");
            return dt;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Select file";
            fdlg.InitialDirectory = @"c:\";
            fdlg.FileName = txtfilepath.Text;
            fdlg.Filter = "Text and CSV Files(*.txt, *.csv)|*.txt;*.csv|Text Files(*.txt)|*.txt|CSV Files(*.csv)|*.csv|All Files(*.*)|*.*";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                txtfilepath.Text = fdlg.FileName;
              
            }

            if (txtfilepath.Text !=string.Empty)
            {
                btnProcess.Enabled = true;
            }
           

        }

        public void CreateDirectory(string strpath)
        {
        
            string path = strpath;

            try
            {
                // Determine whether the directory exists.
                if (!Directory.Exists(path))
                {
                    _ilog.Info("Started creating directory:");
                    _ilog.Info("Directory: "+path);
                    Directory.CreateDirectory(path);
                    _ilog.Info("Directory successful created.");
                }

               
            }
            catch (Exception e)
            {
                _ilog.Info("Directory creation failed.");
            }
            
        }

        private void File_ProcessingFrm_Load(object sender, EventArgs e)
        {
            txtfilepath.Enabled = false;

            if (txtfilepath.Text== string.Empty)
            {
                btnProcess.Enabled = false;
            }
        }
    }

    public class AddressInfo
    {
        public string strNo { set; get; }
        public string strName { set; get; }
        public string strAbbreviation { set; get; }
    }


}
