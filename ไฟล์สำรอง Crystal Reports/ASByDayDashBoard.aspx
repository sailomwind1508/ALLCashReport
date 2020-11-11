<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ASByDayDashBoard.aspx.cs" Inherits="ASByDayDashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <table>
        <tr>
            <td>//พยากรณ์อากาศ
            </td>
        </tr>
        <tr>
            <td>
                <iframe src="http://www.tmd.go.th/daily_forecast_forweb.php" width="180" height="260" scrolling="no" frameborder="0"></iframe>

            </td>
        </tr>
        <tr>
            <td>//ราคาทองคำ</td>
        </tr>
        <tr>
            <td>
               <iframe src="http://namchiang.com/ncgp2-1.swf" width="420" height="380" frameborder="0" marginheight=0 marginwidth=0 scrolling=no></iframe>
            </td>
        </tr>

        <tr>
            <td>//ราคาน้ำมัน </td>
        </tr>
        <tr>
            <td>
                <iframe width="380" height="420" src="http://www.pttor.com/oilprice-board.aspx" frameborder="0"></iframe>
            </td>
        </tr>

        <tr>
            <td>//อัตราดอกเบี้ยและ แลกเปลี่ยน</td>
        </tr>
        <tr>
            <td>
                <iframe id="ifrmBanner" scrolling="no" src="http://www.bangkokbank.com/MajorRates/MainBannerThai.htm" width="170" height="155" frameborder="0"></iframe>
            </td>
        </tr>

        <tr>
            <td>//หุ้น</td>
        </tr>
        <tr>
            <td>
                <iframe src="http://www.settrade.com/banner/banner3.jsp" marginwidth="0" marginleft="0" height="220" width="210" scrolling=no frameborder=no></iframe>

            </td>
        </tr>
    </table>


</asp:Content>

