﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineSupermarketTuto.Views.Admin
{
    public partial class Suppliers : System.Web.UI.Page
    {
        Models.Functions Con;
        protected void Page_Load(object sender, EventArgs e)
        {
            Con = new Models.Functions();
            if (!IsPostBack)
            {
                // 添加默认选项到下拉框首行
                PlaceCb.Items.Insert(0, new ListItem("-- 选择地址 --", ""));
                ShowManufactors();
                ManufactList.HeaderRow.Cells[1].Visible = false;
                foreach (GridViewRow row in ManufactList.Rows)
                {
                    row.Cells[1].Visible = false; // 隐藏每行pid
                }
            }
        }
        protected void ManufctList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // 设置新的页码并重新绑定数据
            ManufactList.PageIndex = e.NewPageIndex;
            ShowManufactors();
        }
        private void ShowManufactors()
        {
            string Query = "select * from ManufactorTb1";

            ManufactList.DataSource = Con.GetData(Query);
            ManufactList.DataBind();

            ManufactList.HeaderRow.Cells[1].Text = "序号";
            ManufactList.HeaderRow.Cells[2].Text = "出版社名称";
            ManufactList.HeaderRow.Cells[3].Text = "联系电话";
            ManufactList.HeaderRow.Cells[4].Text = "所在地";
        }



        protected void SaveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (MNameTb.Value == "" || PermNumTb.Value == "" || PlaceCb.SelectedValue == "")
                {
                    ErrMsg.Text = "信息缺失！";
                }
                else
                {
                    string MName = MNameTb.Value;
                    string PermNum = PermNumTb.Value;
                    string Place = PlaceCb.SelectedItem.ToString();

                    string Query = "insert into ManufactorTb1 values('{0}','{1}','{2}')";
                    Query = string.Format(Query, MName, PermNum, Place);
                    Con.SetData(Query);
                    ShowManufactors();
                    ErrMsg.Text = "生产商信息已添加！";
                    MNameTb.Value = "";
                    PermNumTb.Value = "";
                    PlaceCb.SelectedIndex = -1;
                }
            }
            catch (Exception Ex)
            {
                ErrMsg.Text = Ex.Message;
            }
        }

        int key = 0;
        protected void ManufactList_SelectedIndexChanged(object sender, EventArgs e)
        {
            MNameTb.Value = ManufactList.SelectedRow.Cells[2].Text;
            PermNumTb.Value = ManufactList.SelectedRow.Cells[3].Text;
            PlaceCb.SelectedValue = ManufactList.SelectedRow.Cells[4].Text;
            if(MNameTb.Value == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(ManufactList.SelectedRow.Cells[1].Text);
            }
        }

        protected void EditBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (MNameTb.Value == "" || PermNumTb.Value == "" || PlaceCb.SelectedValue == "")
                {
                    ErrMsg.Text = "信息缺失！";
                }
                else
                {
                    string MName = MNameTb.Value;
                    string PermNum = PermNumTb.Value;
                    string Place = PlaceCb.SelectedItem.ToString();

                    string Query = "update ManufactorTb1 set ManufactName = '{0}', ManufactPermNum = '{1}', ManufactPlace = '{2}' where ManufactId= {3}";
                    Query = string.Format(Query, MName, PermNum, Place, ManufactList.SelectedRow.Cells[1].Text);
                    Con.SetData(Query);
                    ShowManufactors();
                    ErrMsg.Text = "生产商信息已更新！";
                    MNameTb.Value = "";
                    PermNumTb.Value = "";
                    PlaceCb.SelectedIndex = -1;
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
                if (MNameTb.Value == "" || PermNumTb.Value == "" || PlaceCb.SelectedValue == "")
                {
                    ErrMsg.Text = "请选择一条数据！";
                }
                else
                {
                    string MName = MNameTb.Value;
                    string PermNum = PermNumTb.Value;
                    string Place = PlaceCb.SelectedItem.ToString();

                    string Query = "delete from ManufactorTb1 where ManufactId= {0}";
                    Query = string.Format(Query, ManufactList.SelectedRow.Cells[1].Text);
                    Con.SetData(Query);
                    ShowManufactors();
                    ErrMsg.Text = "生产商信息已删除！";
                    MNameTb.Value = "";
                    PermNumTb.Value = "";
                    PlaceCb.SelectedIndex = -1;
                }
            }
            catch (Exception Ex)
            {
                ErrMsg.Text = Ex.Message;
            }
        }
    }
}