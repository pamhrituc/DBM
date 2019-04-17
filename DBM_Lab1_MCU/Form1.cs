using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBM_Lab1_MCU
{
    public partial class Form1 : Form
    {
        void fillData()
        {
            string parentTable = ConfigurationSettings.AppSettings["parentTable"];
            string childTable = ConfigurationSettings.AppSettings["childTable"];
            string relation = ConfigurationSettings.AppSettings["relation"];
            string pk = ConfigurationSettings.AppSettings["pk"];
            string fk = ConfigurationSettings.AppSettings["fk"];

            string conn = "Data Source = DESKTOP-U7E7QTV\\SQLEXPRESS;" + "Initial Catalog = MCU; Integrated Security = true";
            SqlConnection sqlconn = new SqlConnection(conn);

            DataSet dset = new DataSet();

            SqlDataAdapter da_parent = new SqlDataAdapter("select * from " + parentTable, sqlconn); //opens and closes the connection by itself
            da_parent.Fill(dset, parentTable);

            SqlDataAdapter da_child = new SqlDataAdapter("select * from " + childTable, sqlconn);
            da_child.Fill(dset, childTable);

            DataRelation rel = new DataRelation(relation, dset.Tables[parentTable].Columns[pk], dset.Tables[childTable].Columns[fk]);
            dset.Relations.Add(rel);

            this.dataGridView1.DataSource = dset.Tables[parentTable];
            this.dataGridView2.DataSource = this.dataGridView1.DataSource; //chaining dgv1 to dgv2
            this.dataGridView2.DataMember = relation;

            //BindingSource bsParent = new BindingSource();
            //bsParent.DataSource = dset.Tables["Character"];
            //BindingSource bsChild = new BindingSource(bsParent, "CharacterAccessory");
            //this.dataGridView1.DataSource = bsParent;
            //this.dataGridView2.DataSource = bsChild;

            //SqlCommandBuilder builder = new SqlCommandBuilder(da_accessory);
            //builder.GetUpdateCommand();
            //da_accessory.Update(dset, "Accessory");
            button1.Click += (sender, args) =>
            {
                SqlCommandBuilder builder = new SqlCommandBuilder();
                builder.DataAdapter = da_child;
                da_child.Update(dset, childTable);
            };
        }
        
        public Form1()
        {
            InitializeComponent();
            fillData();
        }
    }
}
