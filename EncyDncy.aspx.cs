using EmployeeManagementSystem.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmployeeManagementSystem
{
    public partial class EncyDncy : System.Web.UI.Page
    {
        Common cc = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEncrypt_Click(object sender, EventArgs e)
        {
            txtEncrypted.Value=cc.Encrypt(txtDecrypted.Value.ToString());
        }

        protected void btnDecrypt_Click(object sender, EventArgs e)
        {
            txtDecrypted.Value = cc.Decrypt(txtEncrypted.Value.ToString());
        }
    }
}