using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineSupermarketTuto.Views.Admin
{
    public partial class Products : System.Web.UI.Page
    {
        Models.Functions Con;
        protected void Page_Load(object sender, EventArgs e)
        {
            Con = new Models.Functions();
            if (!IsPostBack)
            {
                ShowProducts();
                ProductList.HeaderRow.Cells[1].Visible = false;
                foreach (GridViewRow row in ProductList.Rows)
                {
                    row.Cells[1].Visible = false; // 隐藏每行pid
                }
                GetCategories();
                GetManufactors();
                GetSellers();
            }
            
        }
        protected void ProductList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // 设置新的页码并重新绑定数据
            ProductList.PageIndex = e.NewPageIndex;
            ShowProducts();
        }
        private void ShowProducts()
        {
            string Query = @"SELECT P.PId, P.PName, P.PAuthor, M.ManufactName, 
                    C.CatName, P.PQty, P.PPrice, CU.CustName AS SellerName
                    FROM ProductTb1 P
                    INNER JOIN ManufactorTb1 M ON P.PManufact = M.ManufactId
                    INNER JOIN CategoryTb1 C ON P.PCategory = C.CatId
                    INNER JOIN CustomerTb1 CU ON P.PSellerId = CU.CustId";
            ProductList.DataSource = Con.GetData(Query);
            ProductList.DataBind();
            ProductList.HeaderRow.Cells[1].Text = "序号";
            ProductList.HeaderRow.Cells[2].Text = "书名";
            ProductList.HeaderRow.Cells[3].Text = "作者";
            ProductList.HeaderRow.Cells[4].Text = "出版社";
            ProductList.HeaderRow.Cells[5].Text = "分类";
            ProductList.HeaderRow.Cells[6].Text = "库存";
            ProductList.HeaderRow.Cells[7].Text = "价格";
            ProductList.HeaderRow.Cells[8].Text = "卖家";

        }

        private void GetCategories()
        {
            string Query = "select * from CategoryTb1";
            PCatCb.DataTextField = Con.GetData(Query).Columns["CatName"].ToString();
            PCatCb.DataValueField = Con.GetData(Query).Columns["CatId"].ToString();
            PCatCb.DataSource = Con.GetData(Query);
            PCatCb.DataBind();
            PCatCb.Items.Insert(0, new ListItem("-- 选择种类 --", "-1"));
        }
        private void GetManufactors()
        {
            string Query = "select * from ManufactorTb1";
            PManufactCb.DataTextField = Con.GetData(Query).Columns["ManufactName"].ToString();
            PManufactCb.DataValueField = Con.GetData(Query).Columns["ManufactId"].ToString();
            PManufactCb.DataSource = Con.GetData(Query);
            PManufactCb.DataBind();
            PManufactCb.Items.Insert(0, new ListItem("-- 选择出版社 --", "-1"));
        }
        private void GetSellers()
        {
            string query = "SELECT CustId, CustName FROM CustomerTb1";
            SellerCb.DataTextField = "CustName";
            SellerCb.DataValueField = "CustId";
            SellerCb.DataSource = Con.GetData(query);
            SellerCb.DataBind();
            SellerCb.Items.Insert(0, new ListItem("-- 选择卖家 --", "-1"));
        }
        protected void SaveBtn_Click(object sender, EventArgs e)
        {
            try
            {

                if (PNameTb.Value == "" || PManufactCb.SelectedIndex == -1 || PCatCb.SelectedIndex == -1 || PriceTb.Value == "" || QtyTb.Value == "" || SellerCb.SelectedIndex == -1 || AuthorTb.Value=="")
                {
                    ErrMsg.Text = "信息缺失！";
                }
                else
                {
                    string PName = PNameTb.Value;
                    string PManufact = PManufactCb.SelectedValue.ToString();
                    string PCat = PCatCb.SelectedValue.ToString();
                    int Quantity = Convert.ToInt32(QtyTb.Value);
                    int Price = Convert.ToInt32(PriceTb.Value);
                    string author = AuthorTb.Value;
                    string sellerId = SellerCb.SelectedValue.ToString();

                    string Query = "insert into ProductTb1 values('{0}','{1}',{2},{3},{4},{5},{6})";
                    Query = string.Format(Query, PName, author, PManufact, PCat, Quantity, Price,sellerId);
                    Con.SetData(Query);
                    ShowProducts();
                    ErrMsg.Text = "商品信息已添加！";
                    PNameTb.Value = "";
                    PManufactCb.SelectedIndex = -1;
                    PCatCb.SelectedIndex = -1;
                    QtyTb.Value = "";
                    PriceTb.Value = "";
                    AuthorTb.Value = "";
                    SellerCb.SelectedIndex = -1;
                }
            }
            catch (Exception Ex)
            {
                ErrMsg.Text = Ex.Message;
            }
        }
        int key = 0;
        protected void ProductList_SelectedIndexChanged(object sender, EventArgs e)
        {
            PNameTb.Value = ProductList.SelectedRow.Cells[2].Text;
            AuthorTb.Value = ProductList.SelectedRow.Cells[3].Text;
            string manufctName = ProductList.SelectedRow.Cells[4].Text;
            PManufactCb.SelectedIndex = PManufactCb.Items.IndexOf(
                PManufactCb.Items.FindByText(manufctName));
            string catcName = ProductList.SelectedRow.Cells[5].Text;
            PCatCb.SelectedIndex = PCatCb.Items.IndexOf(
                PCatCb.Items.FindByText(catcName));
            QtyTb.Value = ProductList.SelectedRow.Cells[6].Text;
            PriceTb.Value = ProductList.SelectedRow.Cells[7].Text;

            // 设置卖家下拉框选中值
            string sellerName = ProductList.SelectedRow.Cells[8].Text;
            SellerCb.SelectedIndex = SellerCb.Items.IndexOf(
                SellerCb.Items.FindByText(sellerName));
            if (PNameTb.Value == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(ProductList.SelectedRow.Cells[1].Text);
            }
        }

        protected void EditBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (PNameTb.Value == "" || PManufactCb.SelectedIndex == -1 || PCatCb.SelectedIndex == -1 || PriceTb.Value == "" || QtyTb.Value == "" || SellerCb.SelectedIndex == -1 || AuthorTb.Value == "")
                {
                    ErrMsg.Text = "信息缺失！";
                }
                else
                {
                    string PName = PNameTb.Value;
                    string PManufact = PManufactCb.SelectedValue.ToString();
                    string PCat = PCatCb.SelectedValue.ToString();
                    int Quantity = Convert.ToInt32(QtyTb.Value);
                    int Price = Convert.ToInt32(PriceTb.Value);
                    string author = AuthorTb.Value;
                    string sellerId = SellerCb.SelectedValue.ToString();

                    string Query = "Update ProductTb1 set PName='{0}',PAuthor ='{1}',PManufact = {2},PCategory = {3},PQty = {4},PPrice = {5},PSellerId = {6} WHERE PId = {7}";
                    Query = string.Format(Query, PName, author, PManufact, PCat, Quantity, Price, sellerId, ProductList.SelectedRow.Cells[1].Text);
                    Con.SetData(Query);
                    ShowProducts();
                    ErrMsg.Text = "商品信息已更新！";
                    PNameTb.Value = "";
                    PManufactCb.SelectedIndex = -1;
                    PCatCb.SelectedIndex = -1;
                    QtyTb.Value = "";
                    PriceTb.Value = "";
                    AuthorTb.Value = "";
                    SellerCb.SelectedIndex = -1;
                }
            }
            catch (Exception Ex)
            {
                ErrMsg.Text = Ex.Message;
            }
        }

        protected void DeleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (PNameTb.Value == "" || PManufactCb.SelectedIndex == -1 || PCatCb.SelectedIndex == -1 || PriceTb.Value == "" || QtyTb.Value == "" || SellerCb.SelectedIndex == -1 || AuthorTb.Value == "")
                {
                    ErrMsg.Text = "信息缺失！";
                }
                else
                {
                    string PName = PNameTb.Value;
                    string PManufact = PManufactCb.SelectedValue.ToString();
                    string PCat = PCatCb.SelectedValue.ToString();
                    int Quantity = Convert.ToInt32(QtyTb.Value);
                    int Price = Convert.ToInt32(PriceTb.Value);
                    string author = AuthorTb.Value;
                    string sellerId = SellerCb.SelectedValue.ToString();

                    string Query = "Delete ProductTb1 where PId={0}";
                    Query = string.Format(Query, ProductList.SelectedRow.Cells[1].Text);
                    Con.SetData(Query);
                    ShowProducts();
                    ErrMsg.Text = "商品信息已删除！";
                    PNameTb.Value = "";
                    PManufactCb.SelectedIndex = -1;
                    PCatCb.SelectedIndex = -1;
                    QtyTb.Value = "";
                    PriceTb.Value = "";
                    AuthorTb.Value = "";
                    SellerCb.SelectedIndex = -1;
                }
            }
            catch (Exception Ex)
            {
                ErrMsg.Text = Ex.Message;
            }
        }
    }
}