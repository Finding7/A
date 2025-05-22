<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Customer/CustomerMaster.Master" AutoEventWireup="true" CodeBehind="Favorite.aspx.cs" Inherits="OnlineSupermarketTuto.Views.Customer.Favorite" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MyContent" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col">
                <h3 class="text-center" style="color:teal;">猜你喜欢</h3>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-md-12">
                <asp:GridView ID="FavoriteList" runat="server" AllowPaging="True" PageSize="8" OnPageIndexChanging="FavoriteList_PageIndexChanging" class="table" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateSelectButton="True" OnSelectedIndexChanged="FavoriteList_SelectedIndexChanged">
                    <AlternatingRowStyle BackColor="White" />
                    <EditRowStyle BackColor="#7C6F57" />
                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="teal" Font-Bold="false" ForeColor="White" />
                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#E3EAEB" />
                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                    <PagerStyle CssClass="gridview-pager" HorizontalAlign="Center" BackColor="Teal" ForeColor="White" />
                </asp:GridView>
                <!-- 新增的提示信息Label -->
                <asp:Label ID="lblNoProducts" runat="server" Text="" CssClass="text-danger d-block text-center mt-3" Visible="false"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>