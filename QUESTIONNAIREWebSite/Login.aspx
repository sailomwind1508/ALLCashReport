<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" rel="stylesheet">

    <!-- Custom styles for this template-->
    <link href="css/sb-admin-2.min.css" rel="stylesheet">

    <style>
        .alert-box {
            padding: 15px;
            margin-bottom: 20px;
            border: 1px solid transparent;
            border-radius: 4px;
        }

        .success {
            color: #3c763d;
            background-color: #dff0d8;
            border-color: #d6e9c6;
            display: none;
        }

        .failure {
            color: #a94442;
            background-color: #f2dede;
            border-color: #ebccd1;
            display: none;
        }

        .warning {
            color: #8a6d3b;
            background-color: #fcf8e3;
            border-color: #faebcc;
            display: none;
        }
    </style>

</head>

<body class="bg-gradient-primary">

    <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery.min.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

    <!-- Core plugin JavaScript-->
    <script src="vendor/jquery-easing/jquery.easing.min.js"></script>

    <!-- Custom scripts for all pages-->
    <script src="js/sb-admin-2.min.js"></script>

    <div class="container">

        <!-- Outer Row -->
        <div class="row justify-content-center">

            <div class="col-xl-10 col-lg-12 col-md-9">

                <div class="card o-hidden border-0 shadow-lg my-5">
                    <div class="card-body p-0">
                        <!-- Nested Row within Card Body -->
                        <div class="row">
                            <div class="col-lg-6 d-none d-lg-block bg-login-image"></div>
                            <div class="col-lg-6">
                                <div class="p-5">
                                    <div class="text-center">
                                        <h1 class="h4 text-gray-900 mb-4">Welcome Back!</h1>
                                    </div>
                                    <form runat="server" class="user" id="frmLogin">

                                        <div class="form-group">

                                            <asp:TextBox type="username" ID="txtusername" runat="server" class="form-control form-control-user" placeholder="Enter User Name..."></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvusername" runat="server" Display="Dynamic" ForeColor="Red" ControlToValidate="txtusername" ErrorMessage="Please enter username!" CssClass=""></asp:RequiredFieldValidator>

                                        </div>
                                        <div class="form-group">

                                            <asp:TextBox type="password" ID="txtpassword" runat="server" class="form-control form-control-user" placeholder="Enter Password..."></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvpassword" runat="server" Display="Dynamic" ForeColor="Red" ControlToValidate="txtpassword" ErrorMessage="Please enter password!"></asp:RequiredFieldValidator>
                                        </div>

                                        <asp:LinkButton ID="btnlogin" runat="server" class="btn btn-primary btn-user btn-block" OnClick="btnlogin_Click">
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/img/login.png" Width="23px" />Login
                                        </asp:LinkButton>

                                        <div>&nbsp;</div>
                                        <div class="alert alert-danger" id="myAlert" style="display: none">
                                            <button type="button" class="close">&times;</button>
                                            <strong>Warning!</strong> Please check user name and password!
                                        </div>

                                        <script>
                                            function showValidateMsg() {

                                                $("#myAlert").attr({
                                                    style: "display:block"
                                                });
                                            }

                                            $(document).ready(function () {

                                                $(".close").click(function () {
                                                    $("#myAlert").alert("close");
                                                });

                                            });

                                            $("#<%=txtpassword.ClientID %>").keydown(function (event) {
                                                var code = event.keyCode || event.which;
                                                if (code == 13) { //Enter keycode
                                                    //alert("Handler for .keydown() called.");
                                                    __doPostBack("enter_pass", "");
                                                }
                                            });

                                            $("#<%=txtusername.ClientID %>").keydown(function (event) {
                                                var code = event.keyCode || event.which;
                                                if (code == 13) { //Enter keycode
                                                    $("#<%=txtpassword.ClientID %>").focus();
                                                }
                                            });
                                        </script>
                                    </form>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </div>

    </div>
</body>
</html>
