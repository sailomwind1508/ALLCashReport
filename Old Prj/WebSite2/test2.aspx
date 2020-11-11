<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test2.aspx.cs" Inherits="test2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link href="CSS/responsive-menu.css" rel="stylesheet" />

    <script>

        function myFunction() {
            var x = document.getElementById("myTopnav");
            if (x.className === "topnav") {
                x.className += " responsive";
            } else {
                x.className = "topnav";
            }
        }
    </script>
    <style type="text/css">
        .header {
            position: absolute;
            top: 100px;
            left: 0px;
            width: 100%;
            height: 50px;
            font-size: x-large;
            font-weight: 700;
            text-align: center;
        }

        .line1_left {
            position: absolute;
            top: 150px;
            left: 10%;
            width: 35%;
            height: 30px;
            text-align: right;
        }

        .line1_center {
            position: absolute;
            top: 150px;
            left: 45%;
            width: 20%;
            height: 30px;
            text-align: center;
        }

        .line1_right {
            position: absolute;
            top: 150px;
            left: 65%;
            width: 35%;
            height: 30px;
            text-align: left;
        }

        .line3_btn {
            position: absolute;
            top: 210px;
            left: 0px;
            width: 100%;
            height: 30px;
            text-align: center;
        }

        .line4_footer-b {
            position: absolute;
            top: 200px;
            left: 5%;
            width: 7%;
            height: 600px;
            text-align: center;
        }

        .line4_footer {
            position: absolute;
            top: 200px;
            left: 12%;
            width: 70%;
            height: 600px;
            text-align: center;
        }

        .line4_footer-a {
            position: absolute;
            top: 200px;
            left: 82%;
            width: 10%;
            height: 600px;
            text-align: center;
        }

        @media all and (min-width: 600px) {/* when the width is less than 30em, make both of the widths 100% */
            .line1_left {
                position: absolute;
                top: 150px;
                left: 15%;
                width: 100%;
                height: 30px;
                text-align: left;
            }

            .line1_center {
                position: absolute;
                top: 180px;
                left: 15%;
                width: 100%;
                height: 30px;
                text-align: left;
            }

            .line1_right {
                position: absolute;
                top: 210px;
                left: 15%;
                width: 100%;
                height: 30px;
                text-align: left;
            }

            .line3_btn {
                position: absolute;
                top: 330px;
                left: 0px;
                width: 100%;
                height: 30px;
                text-align: center;
            }


            .line4_footer-b {
                position: absolute;
                top: 250px;
                left: 10%;
                width: 10%;
                height: 600px;
                text-align: center;
            }

            .line4_footer {
                position: absolute;
                top: 250px;
                left: 20%;
                width: 70%;
                height: 600px;
                text-align: center;
            }

            .line4_footer-a {
                position: absolute;
                top: 250px;
                left: 90%;
                width: 10%;
                height: 600px;
                text-align: center;
            }
        }
    </style>

</head>
<body>

    <form id="form1" runat="server">
        <div class="topnav" id="myTopnav">
            <a>
                <img id="logo" src="United-food2222_1.jpg" style="text-align: left;" /></a>
            <a href="#home"><i class="fa fa-fw fa-home"></i>Home</a>
            <a href="#news"><i class="fa fa-newspaper-o"></i>News</a>
            <a href="#contact"><i class="fa fa-newspaper-o"></i>Contact</a>
            <div class="dropdown">
                <button class="dropbtn">
                    Dropdown
                <i class="fa fa-caret-down"></i>
                </button>
                <div class="dropdown-content">
                    <a href="#"><i class="fa fa-newspaper-o"></i>Link 1</a>
                    <a href="#"><i class="fa fa-newspaper-o"></i>Link 2</a>
                    <a href="#"><i class="fa fa-newspaper-o"></i>Link 3</a>
                </div>
            </div>
            <a href="#about">About</a>
            <a href="javascript:void(0);" class="icon" onclick="myFunction()">&#9776;</a>
        </div>

       
        
        <div class="header">
            <asp:Label ID="Label1" runat="server" Text="บริษัท ขายสะดวก จำกัด"></asp:Label>
        </div>
        <div class="line1_left">
            <asp:Label ID="Label3" runat="server" Text="เลือกสาขา : "></asp:Label>&nbsp;
            <asp:DropDownList ID="ddlBranch" runat="server" Height="23px" Width="149px"></asp:DropDownList>
        </div>
        <div class="line1_center">
            <asp:Label ID="Label4" runat="server" Text="กลุ่มสินค้า : "></asp:Label>&nbsp;
                            <asp:DropDownList ID="ddlProductType" runat="server" Height="23px" Width="149px"></asp:DropDownList>
        </div>
        <div class="line1_right">
            <asp:Button ID="btnReport" runat="server" Height="33px" Style="font-weight: 700" Text="แสดงรายงาน" Width="96px" ValidationGroup="Report" Class="button" /></div>
        <div class="line4_footer-b"></div>
        <div class="line4_footer">
        </div>
        <div class="line4_footer-a"></div>
      


    </form>
</body>
</html>
