<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Page1.aspx.cs" Inherits="Page1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <style>
        .text-img-centered {
            position: absolute;
            top: 80%;
            left: 50%;
            transform: translate(-50%, -50%);
            text-align: center;
            color: white;
        }
    </style>

    <script>
        var x = $('[id*=latlong]'); //document.getElementById('latlong');

        function alert_confirm_save() {

            var listOfRdo = [];
            _gps = $('[id*=latlong]'); //document.getElementById('latlong');
            _asn = $('[id*=AssessorName]');
            _custType = $('[id*=ddlCustType] :selected').text();

            if (_custType == 'ลูกค้า WS') {
                _cust = $('[id*=CustID]');
                if (_cust.val() == '' || _cust.val() == undefined) {
                    alert('กรุณากรอกรหัสร้านค้า!!!');
                    _cust.focus();
                    return;
                }
            }
            else {
                _ddlCustID = $('[id*=ddlCustomer] :selected').text();
                if (_ddlCustID == '' || _ddlCustID == undefined) {
                    alert('กรุณาเลือกร้านค้า!!!');
                    _cust.focus();
                    return;
                }
            }

            if (_asn.val() == '' || _asn.val() == undefined) {
                alert('กรุณากรอกชื่อผู้ประเมิน!!!');
                _asn.focus();
                return;
            }
            if (_gps.text() == '' || _gps.text() == undefined) {
                alert('กรุณาบันทึก GPS!!!');
                return;
            }

            x = $('[id*=latlong]');

            $('input[type=radio]:checked').each(function () {
                var $checkedRadio = $(this);
                listOfRdo.push($checkedRadio.attr('id') + '|' + x.text() + '|' + $checkedRadio.attr('name') + '|');
            });

            $('input[type=checkbox]:checked').each(function () {
                var $checkedCheckbox = $(this);
                listOfRdo.push($checkedCheckbox.attr('id') + '|' + x.text() + '|' + $checkedCheckbox.attr('name') + '|');
            });

            $('input[type=text]').each(function () {
                var $_textbox = $(this);
                listOfRdo.push($_textbox.attr('id') + '|' + x.text() + '|' + $_textbox.attr('name') + '|' + $_textbox.val());
            });

            $('textarea').each(function () {
                var $_textarea = $(this);

                listOfRdo.push($_textarea.attr('id') + '|' + x.text() + '|' + $_textarea.attr('name') + '|' + $_textarea.val());
            });

            __doPostBack('upload', listOfRdo);
        }

        function ShowSuccess(msg) {
            $("#mi-modal").modal('show');
            $('[id*=lblMsg]').text(msg);
        }

        function ShowFail(msg) {
            $("#mi-modal").modal('show');
            $('[id*=lblMsg]').text(msg);
        }

        function showpreview1(input) {
            displayImg($('#imgpreview1'), input);
        }

        function showpreview2(input) {
            displayImg($('#imgpreview2'), input);
        }

        function showpreview3(input) {
            displayImg($('#imgpreview3'), input);
        }

        function showpreview4(input) {
            displayImg($('#imgpreview4'), input);
        }

        function displayImg(control, input) {
            if (input.files && input.files[0]) {

                var reader = new FileReader();
                reader.onload = function (e) {
                    control.css('display', 'block');
                    control.css('visibility', 'visible');
                    control.attr('src', e.target.result);
                }
                reader.readAsDataURL(input.files[0]);
            }
        }

        function getLocation() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(success, error);
            } else {
                x = $('[id*=latlong]');
                x.text("Geolocation is not supported by this browser.");
                console.log("Geolocation is not supported by this browser.");
            }
        }

        function success(position) {
            alert(position.coords.latitude + ":" + position.coords.longitude)
            x = $('[id*=latlong]');
            x.text(position.coords.latitude + ":" + position.coords.longitude);
        }

        function error(msg) {
            x = $('[id*=latlong]');
            x.text(typeof msg == 'string' ? msg : "failed");

            // console.log(arguments);
        }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="col-lg-12 mb-3" style="text-align: center; white-space: nowrap;">
        <p>ประเภทร้าน : </p>
        <asp:DropDownList ID="ddlCustType" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlCustType_SelectedIndexChanged"></asp:DropDownList>

        <br />
        <br />
        <div id="div_ws" runat="server">
            <p>รหัสร้านค้า : </p>
            <asp:TextBox ID="CustID" runat="server" MaxLength="20"></asp:TextBox>
            
            <asp:LinkButton ID="btnSearchCust" runat="server" OnClick="btnSearchCust_Click" Style="width: 50px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary">
                <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/search.png" Width="23px" />
            </asp:LinkButton>
            <br />
            <asp:Label ID="lblCustWSName" runat="server" Text=""></asp:Label>
        </div>

        <div id="div_allc" visible="false" runat="server">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="divLink" runat="server">
                        <p>ศูนย์ : </p>
                        <asp:DropDownList ID="ddlProvince" runat="server" Width="150px" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged"></asp:DropDownList>
                        <br />
                        <p>แวน : </p>
                        <asp:DropDownList ID="ddlVanID" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlVanID_SelectedIndexChanged"></asp:DropDownList>
                        <br />
                        <p>ตลาด : </p>
                        <asp:DropDownList ID="ddlSalAreaID" runat="server" Width="150px" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlSalAreaID_SelectedIndexChanged"></asp:DropDownList>
                        <br />
                    </div>
                    <p>ร้านค้า : </p>
                    <asp:DropDownList ID="ddlCustomer" runat="server" Width="250px" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>

                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:LinkButton ID="btnSearchCust_ALLC" runat="server" OnClick="btnSearchCust_Click" Style="width: 50px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary">
                <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/search.png" Width="23px" />
            </asp:LinkButton>
        </div>

        <p>ผู้ประเมิน : </p>
        <asp:TextBox ID="AssessorName" runat="server" MaxLength="150"></asp:TextBox>
    </div>

    <div class="col-lg-12 mb-0" style="text-align: center;">
        <a id="btnGPS" href="#" title="" visible="false" style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;"
            class="btn btn-primary" onclick="getLocation()">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/placeholder.png" Width="23px" />บันทึก GPS
        </a>
        <br />
        <asp:Label ID="latlong" runat="server" Text=""></asp:Label>
        <br />
    </div>

    <div class="col-lg-12 mb-0" style="text-align: center; color: red; font-weight: bold;">
        <br />
        <asp:CheckBox ID="chkNotActivate" runat="server" Text="ร้านปิด/ไม่ให้ประเมิน" />
        <br />
    </div>
    <div class="divider-custom">
        <div class="divider-custom-line"></div>
        <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
        <div class="divider-custom-line"></div>
    </div>

    <div class="col-md-6 col-lg-12 mb-5" id="topic1">
        <div class="portfolio-item mx-auto" data-toggle="modal" data-target="#portfolioModal1">
            <div class="portfolio-item-caption d-flex align-items-center justify-content-center h-100 w-100">
                <div class="portfolio-item-caption-content text-center text-white"><i class="fas fa-plus fa-3x"></i></div>
            </div>
            <img class="img-fluid" src="assets/img/portfolio/cake.png" />
            <div class="text-img-centered">
                <p class="lead mb-0" style="font-size: 24px">แบบสอบถาม</p>
            </div>
        </div>
    </div>

    <div class="divider-custom">
        <div class="divider-custom-line"></div>
        <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
        <div class="divider-custom-line"></div>
    </div>
    <div class="col-md-6 col-lg-12 mb-3" style="text-align: center;">
        <div class="portfolio-item mx-auto" data-toggle="modal">

            <h4 class="text-uppercase mb-4">เลือกรูปภาพที่ 1</h4>
            <div class="input-group">
                <div class="custom-file">
                    <asp:FileUpload ID="FileUpload1" runat="server" data-sigil="photo-input" accept="image/*;capture=camera" onchange="showpreview1(this);" />
                </div>
            </div>
            <br />
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <div style="text-align: center">
                <image id="imgpreview1" height="150" width="150" style="border-width: 0px; visibility: hidden; display: none; margin: auto; width: 50%; padding: 10px;" />
            </div>

        </div>

    </div>

    <div class="divider-custom">
        <div class="divider-custom-line"></div>
        <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
        <div class="divider-custom-line"></div>
    </div>

    <div class="col-md-6 col-lg-12 mb-0" style="text-align: center;">
        <div class="portfolio-item mx-auto" data-toggle="modal">

            <h4 class="text-uppercase mb-4">เลือกรูปภาพที่ 2</h4>
            <div class="input-group">
                <div class="custom-file">
                    <asp:FileUpload ID="FileUpload2" runat="server" data-sigil="photo-input" accept="image/*;capture=camera" onchange="showpreview2(this);" />
                </div>
            </div>
            <br />
            <asp:Label ID="Label1" runat="server"></asp:Label>
            <img id="imgpreview2" height="150" width="150" style="border-width: 0px; visibility: hidden; display: none; margin: auto; width: 50%; padding: 10px;" />
        </div>

    </div>
    <div class="divider-custom">
        <div class="divider-custom-line"></div>
        <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
        <div class="divider-custom-line"></div>
    </div>
    <div class="col-md-6 col-lg-12 mb-3" style="text-align: center;">
        <div class="portfolio-item mx-auto" data-toggle="modal">

            <h4 class="text-uppercase mb-4">เลือกรูปภาพที่ 3</h4>
            <div class="input-group">
                <div class="custom-file">
                    <asp:FileUpload ID="FileUpload3" runat="server" data-sigil="photo-input" accept="image/*;capture=camera" onchange="showpreview3(this);" />
                </div>
            </div>
            <br />
            <asp:Label ID="Label2" runat="server"></asp:Label>
            <img id="imgpreview3" height="150" width="150" style="border-width: 0px; visibility: hidden; display: none; margin: auto; width: 50%; padding: 10px;" />
        </div>

    </div>
    <div class="divider-custom">
        <div class="divider-custom-line"></div>
        <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
        <div class="divider-custom-line"></div>
    </div>
    <div class="col-md-6 col-lg-12 mb-3" style="text-align: center;">
        <div class="portfolio-item mx-auto" data-toggle="modal">

            <h4 class="text-uppercase mb-4">เลือกรูปภาพที่ 4</h4>
            <div class="input-group">
                <div class="custom-file">
                    <asp:FileUpload ID="FileUpload4" runat="server" data-sigil="photo-input" accept="image/*;capture=camera" onchange="showpreview4(this);" />
                </div>
            </div>
            <br />
            <asp:Label ID="Label3" runat="server"></asp:Label>
            <img id="imgpreview4" height="150" width="150" style="border-width: 0px; visibility: hidden; display: none; margin: auto; width: 50%; padding: 10px;" />
        </div>

    </div>
    <div class="divider-custom">
        <div class="divider-custom-line"></div>
        <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
        <div class="divider-custom-line"></div>
    </div>

    <div class="col-md-6 col-lg-12 mb-3" style="text-align: center;">
        <div class="portfolio-item mx-auto" data-toggle="modal">

            <h4 class="text-uppercase mb-4">หมายเหตุ</h4>
            <div class="input-group">
                <div class="custom-file">
                    <textarea id="remark" name="remark" style="width: 400px;" maxlength="255"></textarea>
                </div>
            </div>
            <br />
        </div>

    </div>

    <div class="col-lg-12 mb-0" style="text-align: center;">
        <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#exampleModal">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/paper-plane.png" Width="23px" />ส่งข้อมูล</button>
    </div>

    <!-- Portfolio Modals-->
    <!-- Portfolio Modal 1-->
    <div class="portfolio-modal modal fade" id="portfolioModal1" tabindex="-1" role="dialog" aria-labelledby="portfolioModal1Label" aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true"><i class="fas fa-times"></i></span>
                </button>
                <div class="modal-body text-center">
                    <div class="container">
                        <div class="row justify-content-center">
                            <div class="col-lg-8" id="_data">
                                <!-- Portfolio Modal - Title-->

                                <% foreach (var head in QuestionnaireList)
                                    { %>
                                <h2 class="portfolio-modal-title text-secondary text-uppercase mb-0" style="font-size: 20px;" id="topic<%= head.QuestionnaireID %>"><%= head.Topic %></h2>
                                <!-- Icon Divider-->

                                <!-- Portfolio Modal - Image-->
                                <!-- Group of default radios - option 1 -->

                                <% 
                                    if (head.Pattern == "multi-checkbox")
                                    {
                                %>
                                <div class="custom-control custom-checkbox">
                                    <table style="width: 100%">
                                        <%
                                            var dt = QuestionnaireDetailsList.Where(x => x.QuestionnaireID == head.QuestionnaireID).ToList();
                                            var dt1 = dt.Where(x => (x.Seq >= 1 && x.Seq <= 20) || (x.Seq >= 41 && x.Seq <= 60)).ToList();
                                            var dt2 = dt.Where(x => x.Seq >= 21 && x.Seq <= 40 || (x.Seq >= 61 && x.Seq <= 80)).ToList();
                                            //var dt3 = dt.Where(x => x.Seq >= 41 && x.Seq <= 60).ToList();
                                            //var dt4 = dt.Where(x => x.Seq >= 61 && x.Seq <= 80).ToList();
                                        %>
                                        <tr style="width: 50%; white-space: nowrap;">
                                            <td>
                                                <% foreach (var _dt in dt1)
                                                    {
                                                %>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <!-- using HTML -->
                                                            <input type="checkbox" class="custom-control-input" id="<%= _dt.QuestionnaireDetailsID %>" name="rdog<%= _dt.QuestionnaireID %>">
                                                            <label class="custom-control-label" for="<%= _dt.QuestionnaireDetailsID %>"><%= _dt.Question %></label>

                                                        </td>
                                                    </tr>
                                                </table>
                                                <%} %>
                                            </td>
                                            <td style="width: 50%%">
                                                <% foreach (var _dt in dt2)
                                                    {%>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <!-- using HTML -->
                                                            <input type="checkbox" class="custom-control-input" id="<%= _dt.QuestionnaireDetailsID %>" name="rdog<%= _dt.QuestionnaireID %>">
                                                            <label class="custom-control-label" for="<%= _dt.QuestionnaireDetailsID %>"><%= _dt.Question %></label>

                                                        </td>
                                                    </tr>
                                                </table>
                                                <%} %>

                                            </td>
                                            <%--<td style="width:25%">

                                                <% foreach (var _dt in dt3)
                                                    {%>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <!-- using HTML -->
                                                            <input type="radio" class="custom-control-input" id="<%= _dt.QuestionnaireDetailsID %>" name="rdog<%= _dt.QuestionnaireID %>">
                                                            <label class="custom-control-label" for="<%= _dt.QuestionnaireDetailsID %>"><%= _dt.Question %></label>

                                                        </td>
                                                    </tr>
                                                </table>
                                                <%} %>
                                            </td>
                                            <td style="width:25%">
                                                <% foreach (var _dt in dt4)
                                                    {%>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <!-- using HTML -->
                                                            <input type="radio" class="custom-control-input" id="<%= _dt.QuestionnaireDetailsID %>" name="rdog<%= _dt.QuestionnaireID %>">
                                                            <label class="custom-control-label" for="<%= _dt.QuestionnaireDetailsID %>"><%= _dt.Question %></label>

                                                        </td>
                                                    </tr>
                                                </table>
                                                <%} %>
                                            </td>--%>
                                        </tr>
                                    </table>
                                </div>

                                <%}
                                    else
                                    {
                                        foreach (var dt in QuestionnaireDetailsList.Where(x => x.QuestionnaireID == head.QuestionnaireID).ToList())
                                        { %>
                                <% if (head.Pattern == "radio")
                                    { %>
                                <div class="custom-control custom-radio mb-3">

                                    <!-- using HTML -->
                                    <input type="radio" class="custom-control-input" id="<%= dt.QuestionnaireDetailsID %>" name="rdog<%= dt.QuestionnaireID %>">
                                    <label class="custom-control-label" for="<%= dt.QuestionnaireDetailsID %>"><%= dt.Question %></label>
                                </div>
                                <% }%>


                                <% else if (head.Pattern == "checkbox")
                                    { %>
                                <div class="custom-control custom-checkbox mb-3">

                                    <!-- using HTML -->
                                    <input type="checkbox" class="custom-control-input" id="<%= dt.QuestionnaireDetailsID %>" name="rdog<%= dt.QuestionnaireID %>">
                                    <label class="custom-control-label" for="<%= dt.QuestionnaireDetailsID %>"><%= dt.Question %></label>
                                </div>
                                <% }%>

                                <%   else if (head.Pattern == "text")
                                    {%>
                                <div class="custom-control mb-3">

                                    <!-- using HTML -->
                                    <label><%= dt.Question %></label>
                                    <input type="text" id="<%= dt.QuestionnaireDetailsID %>" name="rdog<%= dt.QuestionnaireID %>" maxlength="150">
                                </div>
                                <% }
                                    else if (head.Pattern == "radio-text")
                                    {
                                %>
                                <div class="custom-control custom-radio mb-3">
                                    <input type="radio" class="custom-control-input" id="<%= dt.QuestionnaireDetailsID %>" name="rdog<%= dt.QuestionnaireID %>">
                                    <label class="custom-control-label" for="<%= dt.QuestionnaireDetailsID %>"><%= dt.Question %></label>
                                    <input type="text" id="<%= dt.QuestionnaireDetailsID %>" name="rdog<%= dt.QuestionnaireID %>" maxlength="150">
                                </div>
                                <% }%>

                                <% 
                                    else if (head.Pattern == "checkbox-text")
                                    {
                                %>
                                <div class="custom-control custom-checkbox mb-3">
                                    <input type="checkbox" class="custom-control-input" id="<%= dt.QuestionnaireDetailsID %>" name="rdog<%= dt.QuestionnaireID %>">
                                    <label class="custom-control-label" for="<%= dt.QuestionnaireDetailsID %>"><%= dt.Question %></label>
                                    <input type="text" id="<%= dt.QuestionnaireDetailsID %>" name="rdog<%= dt.QuestionnaireID %>" maxlength="150">
                                </div>
                                <% }%>

                                <%else if (head.Pattern == "text-area")
                                    {%>


                                <textarea id="<%= dt.QuestionnaireDetailsID %>" name="rdog<%= dt.QuestionnaireID %>" form="usrform" maxlength="255"></textarea>

                                <% }
                                    }%>


                                <div class="divider-custom">
                                    <div class="divider-custom-line"></div>
                                    <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
                                    <div class="divider-custom-line"></div>
                                </div>
                                <%  }
                                    } %>

                                <button class="btn btn-primary" data-dismiss="modal"><i class="fas fa-times fa-fw"></i>Close Window</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">ข้อความ</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <p>ต้องการส่งข้อมูลใช่หรือไม่?</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">ยกเลิก</button>
                    <a id="btnSave" onclick="return alert_confirm_save();" href="#" title="" style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block">ส่งข้อมูล
                    </a>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true" id="mi-modal">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="myModalLabel1">ข้อความ</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">ปิด</button>

                </div>
            </div>
        </div>
    </div>

    <div class="alert" role="alert" id="result"></div>

</asp:Content>

