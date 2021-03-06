using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Epi.Data;
using Epi.Data.MongoDB;

namespace Epi.Data.MongoDB.Forms
{
    /// <summary>
    /// Partial class for Data.SqlServer form ExistingConnectionStringDialog 
    /// </summary>
    public partial class ExistingConnectionStringDialog : Epi.Data.MongoDB.Forms.ConnectionStringDialog
    {   
        /// <summary>
        /// Default constructor
        /// </summary>
        public ExistingConnectionStringDialog()
        {
            InitializeComponent();
        }

        private void ExistingConnectionStringDialog_Load(object sender, EventArgs e)
        {
            this.Text = "Connect to a MongoDB database";
            lblInstructions.Text = "Enter information to connect to the MongoDB database.";
        }
    }
}

