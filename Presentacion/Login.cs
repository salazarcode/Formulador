using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Formulador.Dominio;
using Formulador.Negocio;
using Formulador.Transversal;
using System.Configuration;
using System.Data.SQLite;

namespace Formulador
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void Login_Load(object sender, EventArgs e)
        {
            //string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            var res = await Importador.Import(@"MONTANA\pshiomura");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}


