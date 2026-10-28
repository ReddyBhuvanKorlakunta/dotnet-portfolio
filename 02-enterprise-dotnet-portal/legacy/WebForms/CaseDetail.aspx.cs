using System;
using System.Web.UI;

namespace LegacyPortal
{
    // Web Forms lifecycle: Page_Load runs on every request; IsPostBack tells
    // you whether this is the first load or a button postback. ViewState
    // carries control state — keep it small, never put sensitive data in it.
    public partial class CaseDetail : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) LoadCase();
        }
        protected void btnRefresh_Click(object sender, EventArgs e) => LoadCase();
        private void LoadCase()
        {
            // Sample data in this system; production reads via a parameterised
            // query / repository, never string-built SQL.
            lblReference.Text = "CA-1042";
            lblStatus.Text = "In review";
        }
    }
}
