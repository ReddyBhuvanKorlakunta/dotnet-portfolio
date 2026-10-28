using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace LegacyPortal
{
    public partial class CaseDetail : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = User.Identity.Name;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) BindCases();
        }
        protected void btnRefresh_Click(object sender, EventArgs e) { BindCases(); resultCount.Focus(); }
        private void BindCases()
        {
            var rows = new[] {
                new { Reference="LEG-1001", Title="Synthetic housing review", Status="Submitted" },
                new { Reference="LEG-1002", Title="Synthetic document review", Status="InReview" }
            }.Where(x => statusFilter.SelectedValue == "" || x.Status == statusFilter.SelectedValue).ToList();
            casesGrid.DataSource = rows;
            casesGrid.DataBind();
            if (casesGrid.HeaderRow != null) casesGrid.HeaderRow.TableSection = TableRowSection.TableHeader;
            resultCount.Text = rows.Count + " cases found.";
            resultCount.Attributes["tabindex"] = "-1";
        }
    }
}
