using Admission.App_Start;
using CommonUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admission.Admission.Admin
{
    public partial class EncDec : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected void btnEncrypt_Click(object sender, EventArgs e)
        {
            string cipher = null;
            cipher = Encrypt.EncryptString(txtOriginalText.Text);
            if(cipher != null)
            {
                lblEncryptedText.Text = cipher;
            }
            else
            {
                lblEncryptedText.Text = "Error";
            }
        }

        protected void btnDecrypt_Click(object sender, EventArgs e)
        {
            string original = null;
            original = Decrypt.DecryptString(txtEncryptedText.Text);
            if (original != null)
            {
                lblDecryptedText.Text = original;
            }
            else
            {
                lblDecryptedText.Text = "Error";
            }
        }
    }
}