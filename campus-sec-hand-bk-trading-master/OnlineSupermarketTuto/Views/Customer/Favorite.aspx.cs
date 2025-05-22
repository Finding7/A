using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineSupermarketTuto.Views.Customer
{
    public partial class Favorite : System.Web.UI.Page
    {
        Models.Functions Con;
        int customer = Login.User; // 获取当前用户

        protected void Page_Load(object sender, EventArgs e)
        {
            Con = new Models.Functions();
            if (!IsPostBack)
            {
                ShowFavoriteProducts();
            }
        }
        protected void FavoriteList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // 设置新的页码并重新绑定数据
            FavoriteList.PageIndex = e.NewPageIndex;
            ShowFavoriteProducts();
        }
        private void ShowFavoriteProducts()
        {
            // 隐藏提示信息（默认）
            lblNoProducts.Visible = false;

            // 1. 先获取顾客的喜好类别
            string getFavoriteQuery = "SELECT CustFavorite FROM CustomerTb1 WHERE CustId = {0}";
            getFavoriteQuery = string.Format(getFavoriteQuery, customer);
            DataTable favoriteDt = Con.GetData(getFavoriteQuery);

            if (favoriteDt.Rows.Count > 0 && favoriteDt.Rows[0]["CustFavorite"] != DBNull.Value)
            {
                int favoriteCategory = Convert.ToInt32(favoriteDt.Rows[0]["CustFavorite"]);

                // 2. 查询该类别下的商品
                string Query = "SELECT PId as 序号, PName as 商品名称, PPrice as 商品价格, PQty as 库存数量 " +
                              "FROM ProductTb1 WHERE PCategory = {0}";
                Query = string.Format(Query, favoriteCategory);
                DataTable productsDt = Con.GetData(Query);

                if (productsDt.Rows.Count > 0)
                {
                    FavoriteList.DataSource = productsDt;
                    FavoriteList.DataBind();
                    FavoriteList.HeaderRow.Cells[1].Visible = false;
                    foreach (GridViewRow row in FavoriteList.Rows)
                    {
                        row.Cells[1].Visible = false; // 隐藏每行pid
                    }
                }
                else
                {
                    // 该类别下没有商品
                    FavoriteList.DataSource = null;
                    FavoriteList.DataBind();
                    lblNoProducts.Text = "您喜欢的商品类别目前没有商品";
                    lblNoProducts.Visible = true;
                }
            }
            else
            {
                // 如果没有设置喜好
                FavoriteList.DataSource = null;
                FavoriteList.DataBind();
                lblNoProducts.Text = "暂未得知您喜欢的商品类别";
                lblNoProducts.Visible = true;
            }
        }

        protected void FavoriteList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FavoriteList.SelectedRow != null)
            {
                string productName = FavoriteList.SelectedRow.Cells[2].Text;
                string price = FavoriteList.SelectedRow.Cells[3].Text;

                // 同时保存产品ID以便更精确匹配
                string productId = FavoriteList.SelectedRow.Cells[1].Text;

                Session["SelectedProduct"] = new string[] { productName, price, productId };
                Response.Redirect("Billing.aspx");
            }
        }
    }
}