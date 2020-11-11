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

        function alert_confirm_save() {

            var listOfRdo = [];
            var listOfRank = [];
            
            _asn = $('[id*=AssessorName]');
            _custType = $('[id*=ddlCustType] :selected').text();

            _ddlCustID = $('[id*=ddlCustomer] :selected').text();
            if (_ddlCustID == '' || _ddlCustID == undefined) {
                alert('กรุณาเลือกร้านค้า!!!');
                _cust.focus();
                return;
            }


            x = $('[id*=latlong]');

            //$('input[type=radio]:checked').each(function (index, element ) {
            //    var $checkedRadio = $(element);
            //    listOfRdo.push($checkedRadio.attr('id') + '|0:0|' + $checkedRadio.attr('name') + '|');
            //});

            $('input[type=checkbox]:checked').each(function (index, element ) {
                var $checkedCheckbox = $(element);
                //alert($checkedCheckbox.attr('id'));
                listOfRdo.push($checkedCheckbox.attr('id') + '|0:0|' + $checkedCheckbox.attr('name') + '|');
            });

            $('input[type=text]').each(function (index, element ) {
                var $_textbox = $(element);
                if ($_textbox.val() != undefined && $_textbox.val() != null && $_textbox.val() != '') {
                    //alert($_textbox.attr('id'));
                    listOfRdo.push($_textbox.attr('id') + '|0:0|' + $_textbox.attr('name') + '|' + $_textbox.val());
                }
            });

            $('input[type=number]').each(function (index, element ) {
                var $_textbox = $(element);
                if ($_textbox.val() != undefined && $_textbox.val() != null && $_textbox.val() != '') {
                    //alert($_textbox.attr('id'));
                    listOfRdo.push($_textbox.attr('id') + '|0:0|' + $_textbox.attr('name') + '|' + $_textbox.val());
                }
            });

            $("select").each(function (index, element ) {
                var $_select = $(element);
                var selected = $_select.children("option:selected").val();
                if (selected != undefined && selected != '' && selected != 0) {
                    //alert($_select.attr('id'));
                    listOfRdo.push($_select.attr('id') + '|0:0|' + $_select.attr('name') + '|' + selected);
                } 
            });

            $('textarea').each(function (index, element ) {
                var $_textarea = $(element);
                if ($_textarea.val() != undefined && $_textarea.val() != null && $_textarea.val() != '') {
                    listOfRdo.push($_textarea.attr('id') + '|0:0|' + $_textarea.attr('name') + '|' + $_textarea.val());
                }
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

    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


    <div class="col-lg-12 mb-3" style="text-align: center; white-space: nowrap;">
        <p>ประเภทร้าน : </p>
        <asp:DropDownList ID="ddlCustType" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlCustType_SelectedIndexChanged"></asp:DropDownList>

        <br />
        <br />
        

        <div id="div_allc" runat="server">
            
            <p>ร้านค้า : </p>
            <asp:DropDownList ID="ddlCustomer" runat="server" Width="250px" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
            <br />
            <asp:Label ID="lblCustWSName" runat="server" Text=""></asp:Label>

        </div>

        <p>ผู้ประเมิน : </p>
        <asp:TextBox ID="AssessorName" runat="server" MaxLength="150" ReadOnly="true"></asp:TextBox>
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
                                    if (head.Pattern == "text-score") { %>
                                        <select id="rank<%= head.QuestionnaireID  %>" name="rank<%= head.QuestionnaireID  %>" style="width:100px;color:black;">
                                            <option value="0" title="อันดับ">--อันดับ--</option>
                                            <option value="1" title="1">1</option>
                                            <option value="2" title="2">2</option>
                                            <option value="3" title="3">3</option>
                                            <option value="4" title="4">4</option>
                                        </select>
                                
                                  <%  }
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
                                            <td style="width: 50%">
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
                                <div class="custom-control custom-text mb-3">

                                    
                                    <!-- using HTML -->
                                    <label><%= dt.Question %></label>
                                    <input type="text" id="<%= dt.QuestionnaireDetailsID %>" name="rdog<%= dt.QuestionnaireID %>" maxlength="150">
                                </div>
                                <% }%>
                                <%   else if (head.Pattern == "dropdown")
                                {%>
                                <div class="custom-control custom-text mb-3">

                                    
                                    <!-- using HTML -->
                                    <label><%= dt.Question %></label>
                                    <select id="ddl<%= head.QuestionnaireID  %>" name="rank<%= head.QuestionnaireID  %>" style="width: 180px; color: black;">
                                        
                                        <option value="เครื่องดื่มชูกำลัง">เครื่องดื่มชูกำลัง</option>
                                        <option value="กาแฟพร้อมดื่ม">กาแฟพร้อมดื่ม</option>
                                        <option value="ผงปรุงรส">ผงปรุงรส</option>
                                        <option value="ผงชูรส">ผงชูรส</option>
                                        <option value="น้ำปล">น้ำปลา</option>
                                        <option value="น้ำปลาร้า">น้ำปลาร้า</option>
                                        <option value="เครื่องปรุงรส อื่น ">เครื่องปรุงรส อื่น ๆ</option>
                                        <option value="เครื่องดื่มรสหวาน">เครื่องดื่มรสหวาน</option>
                                        <option value="น้ำเกลือแร่-วิตามิน">น้ำเกลือแร่-วิตามิน</option>
                                        <option value="น้ำเปล่า">น้ำเปล่า</option>
                                        <option value="Snak">Snak</option>
                                        <option value="ปลากระป๋อง">ปลากระป๋อง</option>
                                        <option value="อาการแห้ง">อาการแห้ง</option>
                                        <option value="ผงซักฟอก">ผงซักฟอก</option>
                                        <option value="น้ำยาปรับผ้านุ่ม">น้ำยาปรับผ้านุ่ม</option>
                                        <option value="น้ำยาล้างจาน">น้ำยาล้างจาน</option>
                                        <option value="น้ำยาล้างห้องน้ำ">น้ำยาล้างห้องน้ำ</option>
                                        <option value="กระดาษทิชชู่">กระดาษทิชชู่</option>
                                        <option value="แชมพูสระผม">แชมพูสระผม</option>
                                        <option value="สบู่อาบน้ำ">สบู่อาบน้ำ</option>
                                        <option value="ผลิตภัณฑ์ดูแลช่องปาก">ผลิตภัณฑ์ดูแลช่องปาก</option>
                                    </select>
                                </div>
                                <% }%>
                                <%   else if (head.Pattern == "number")
                                {%>
                                <div class="custom-control mb-3">

                                    
                                    <!-- using HTML -->
                                    <label><%= dt.Question %></label>
                                    <input type="number" id="<%= dt.QuestionnaireDetailsID %>" name="rdog<%= dt.QuestionnaireID %>" maxlength="150">
                                </div>
                                <% }%>
                                <%   else if (head.Pattern == "text-score")
                                {%>
                                <div class="custom-control mb-3">

                                    
                                    <!-- using HTML -->
                                    <label><%= dt.Question %></label>
                                    <input type="text" id="<%= dt.QuestionnaireDetailsID %>" name="rdog<%= dt.QuestionnaireID %>" maxlength="150">
                                </div>
                                <% }%>
                                <%else if (head.Pattern == "radio-text")
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
                                <div class="custom-control custom-checkbox mb-3" style="white-space:nowrap;">
                                    <input type="checkbox" class="custom-control-input" id="<%= dt.QuestionnaireDetailsID %>" name="rdog<%= dt.QuestionnaireID %>">
                                    <label class="custom-control-label" for="<%= dt.QuestionnaireDetailsID %>"><%= dt.Question %></label>
                                    <%if (dt.Question != "อื่น ๆ ")
                                            {%>
                                    <input type="text" id="<%= dt.QuestionnaireDetailsID %>" name="rdog<%= dt.QuestionnaireID %>" maxlength="150" style="width:30px;">
                                    <%}
                                            else
                                            { %>
                                    <input type="text" id="<%= dt.QuestionnaireDetailsID %>" name="rdog<%= dt.QuestionnaireID %>" maxlength="150">

                                    <%} %>
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

