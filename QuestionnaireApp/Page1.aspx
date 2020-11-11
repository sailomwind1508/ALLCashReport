<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Page1.aspx.cs" Inherits="Page1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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

    <div class="col-lg-12" style="text-align:center;">
        <h4 class="text-uppercase mb-4">ร้านนาย A</h4>
        <%--<select class="browser-default custom-select custom-select-lg mb-3">
            <option selected>---เลือกจังหวัด---</option>
            <option value="1">กรุงเทพ</option>
            <option value="2">ปทุมธานี</option>
            <option value="3">สมุทรสาคร</option>
        </select>
        <select class="browser-default custom-select custom-select-lg mb-3">
            <option selected>---เลือกชื่อร้านค้า---</option>
            <option value="1">ร้านนาย A</option>
            <option value="2">ร้านนาย B</option>
            <option value="3">ร้านนาย C</option>
        </select>--%>
    </div>
    <!-- Portfolio Item 1-->
    <div class="col-md-6 col-lg-4 mb-5">
        <div class="portfolio-item mx-auto" data-toggle="modal" data-target="#portfolioModal1">
            <div class="portfolio-item-caption d-flex align-items-center justify-content-center h-100 w-100">
                <div class="portfolio-item-caption-content text-center text-white"><i class="fas fa-plus fa-3x"></i></div>
            </div>
            <img class="img-fluid" src="assets/img/portfolio/cabin.png" />
            <div class="text-img-centered"><p class="lead mb-0" style="font-size:24px">สภาพตลาด</p></div>

        </div>
    </div>
    <!-- Portfolio Item 2-->
    <div class="col-md-6 col-lg-4 mb-5">
        <div class="portfolio-item mx-auto" data-toggle="modal" data-target="#portfolioModal2">
            <div class="portfolio-item-caption d-flex align-items-center justify-content-center h-100 w-100">
                <div class="portfolio-item-caption-content text-center text-white"><i class="fas fa-plus fa-3x"></i></div>
            </div>
            <img class="img-fluid" src="assets/img/portfolio/cake.png" alt="" />
            <div class="text-img-centered"><p class="lead mb-0" style="font-size:24px">ลักษณะของร้านค้า</p></div>
        </div>
    </div>
    <!-- Portfolio Item 3-->
    <div class="col-md-6 col-lg-4 mb-5">
        <div class="portfolio-item mx-auto" data-toggle="modal" data-target="#portfolioModal3">
            <div class="portfolio-item-caption d-flex align-items-center justify-content-center h-100 w-100">
                <div class="portfolio-item-caption-content text-center text-white"><i class="fas fa-plus fa-3x"></i></div>
            </div>
            <img class="img-fluid" src="assets/img/portfolio/cabin.png" alt="" />
            <div class="text-img-centered"><p class="lead mb-0" style="font-size:24px">ลักษณะของกลุ่มลูกค้า</p></div>
        </div>
    </div>
    <!-- Portfolio Item 4-->
    <div class="col-md-6 col-lg-4 mb-5">
        <div class="portfolio-item mx-auto" data-toggle="modal" data-target="#portfolioModal4">
            <div class="portfolio-item-caption d-flex align-items-center justify-content-center h-100 w-100">
                <div class="portfolio-item-caption-content text-center text-white"><i class="fas fa-plus fa-3x"></i></div>
            </div>
            <img class="img-fluid" src="assets/img/portfolio/game.png" alt="" />
            <div class="text-img-centered"><p class="lead mb-0" style="font-size:22px">ประมาณการยอดขายของร้านค้า : วัน</p></div>
        </div>
    </div>
    <!-- Portfolio Item 5-->
    <div class="col-md-6 col-lg-4 mb-5">
        <div class="portfolio-item mx-auto" data-toggle="modal" data-target="#portfolioModal5">
            <div class="portfolio-item-caption d-flex align-items-center justify-content-center h-100 w-100">
                <div class="portfolio-item-caption-content text-center text-white"><i class="fas fa-plus fa-3x"></i></div>
            </div>
            <img class="img-fluid" src="assets/img/portfolio/safe.png" alt="" />
            <div class="text-img-centered"><p class="lead mb-0" style="font-size:22px">Shelt, การตั้งกองสินค้าค้าแข่ง</p></div>
        </div>
    </div>
    <!-- Portfolio Item 6-->
    <div class="col-md-6 col-lg-4 mb-5">
        <div class="portfolio-item mx-auto" data-toggle="modal" data-target="#portfolioModal6">
            <div class="portfolio-item-caption d-flex align-items-center justify-content-center h-100 w-100">
                <div class="portfolio-item-caption-content text-center text-white"><i class="fas fa-plus fa-3x"></i></div>
            </div>
            <img class="img-fluid" src="assets/img/portfolio/submarine.png" alt="" />
            <div class="text-img-centered"><p class="lead mb-0" style="font-size:24px">จำนวน Sku ในร้านค้า</p></div>
        </div>
    </div>
    <!-- Portfolio Item 7-->
    <div class="col-md-6 col-lg-4 mb-5">
        <div class="portfolio-item mx-auto" data-toggle="modal" data-target="#portfolioModal7">
            <div class="portfolio-item-caption d-flex align-items-center justify-content-center h-100 w-100">
                <div class="portfolio-item-caption-content text-center text-white"><i class="fas fa-plus fa-3x"></i></div>
            </div>
            <img class="img-fluid" src="assets/img/portfolio/cake.png" alt="" />
            <div class="text-img-centered"><p class="lead mb-0" style="font-size:22px">การ Display สินค้า (มองภาพรวม)</p></div>
        </div>
    </div>
    <!-- Portfolio Item 8-->
    <div class="col-md-6 col-lg-4 mb-5">
        <div class="portfolio-item mx-auto" data-toggle="modal" data-target="#portfolioModal8">
            <div class="portfolio-item-caption d-flex align-items-center justify-content-center h-100 w-100">
                <div class="portfolio-item-caption-content text-center text-white"><i class="fas fa-plus fa-3x"></i></div>
            </div>
            <img class="img-fluid" src="assets/img/portfolio/cake.png" alt="" />
            <div class="text-img-centered"><p class="lead mb-0" style="font-size:24px">ประเภทสินค้าที่ขายดี</p></div>
        </div>
    </div>
    <!-- Portfolio Item 9-->
    <div class="col-md-6 col-lg-4 mb-5">
        <div class="portfolio-item mx-auto" data-toggle="modal" data-target="#portfolioModal9">
            <div class="portfolio-item-caption d-flex align-items-center justify-content-center h-100 w-100">
                <div class="portfolio-item-caption-content text-center text-white"><i class="fas fa-plus fa-3x"></i></div>
            </div>
            <img class="img-fluid" src="assets/img/portfolio/cake.png" alt="" />
            <div class="text-img-centered"><p class="lead mb-0" style="font-size:22px">การระบายออกของสินค้า ยูไนเต็ด</p></div>
        </div>
    </div>
    <!-- Portfolio Item 10-->
    <div class="col-md-6 col-lg-4 mb-5">
        <div class="portfolio-item mx-auto" data-toggle="modal" data-target="#portfolioModal10">
            <div class="portfolio-item-caption d-flex align-items-center justify-content-center h-100 w-100">
                <div class="portfolio-item-caption-content text-center text-white"><i class="fas fa-plus fa-3x"></i></div>
            </div>
            <img class="img-fluid" src="assets/img/portfolio/cake.png" alt="" />
            <div class="text-img-centered"><p class="lead mb-0" style="font-size:24px">Stock สินค้าในร้าน</p></div>
        </div>
    </div>
    <!-- Portfolio Item 11-->
    <div class="col-md-6 col-lg-4 mb-5">
        <div class="portfolio-item mx-auto" data-toggle="modal" data-target="#portfolioModal11">
            <div class="portfolio-item-caption d-flex align-items-center justify-content-center h-100 w-100">
                <div class="portfolio-item-caption-content text-center text-white"><i class="fas fa-plus fa-3x"></i></div>
            </div>
            <img class="img-fluid" src="assets/img/portfolio/cake.png" alt="" />
            <div class="text-img-centered"><p class="lead mb-0" style="font-size:24px">New Product</p></div>
        </div>
    </div>
    <!-- Portfolio Item 12-->
    <div class="col-md-6 col-lg-4 mb-5">
        <div class="portfolio-item mx-auto" data-toggle="modal" data-target="#portfolioModal12">
            <div class="portfolio-item-caption d-flex align-items-center justify-content-center h-100 w-100">
                <div class="portfolio-item-caption-content text-center text-white"><i class="fas fa-plus fa-3x"></i></div>
            </div>
            <img class="img-fluid" src="assets/img/portfolio/cake.png" alt="" />
            <div class="text-img-centered"><p class="lead mb-0" style="font-size:22px">Promotion ที่ร้านค้าต้องการ</p></div>
        </div>
    </div>

    <div class="col-lg-12 mb-3" style="text-align:center;border-style:groove;">
        <br />
        <br />
       
        <h4 class="text-uppercase mb-4">เลือกรูปภาพ</h4>
        <div class="input-group">
            <div class="custom-file">
                <asp:FileUpload ID="FileUpload1" runat="server" />

            </div>

            <div class="input-group-append">
                <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-outline-primary" Text="Upload" />
            </div>
        </div>
        <br />
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
      
    </div>
    <div class="col-lg-12 mb-0" style="text-align:center;">

        <button class="btn btn-primary btn-xl" id="sendQuestionnair" type="submit">ส่งข้อมูล</button>

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
                            <div class="col-lg-8">
                                <!-- Portfolio Modal - Title-->
                                <h2 class="portfolio-modal-title text-secondary text-uppercase mb-0" id="portfolioModal1Label">สภาพตลาด</h2>
                                <!-- Icon Divider-->
                                <div class="divider-custom">
                                    <div class="divider-custom-line"></div>
                                    <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
                                    <div class="divider-custom-line"></div>
                                </div>
                                <!-- Portfolio Modal - Image-->
                                <!-- Group of default radios - option 1 -->
                                <div class="custom-control custom-radio">
                                    <input type="radio" class="custom-control-input" id="defaultGroupExample1" name="groupOfDefaultRadios">
                                    <label class="custom-control-label" for="defaultGroupExample1">เงียบ</label>
                                </div>

                                <!-- Group of default radios - option 2 -->
                                <div class="custom-control custom-radio">
                                    <input type="radio" class="custom-control-input" id="defaultGroupExample2" name="groupOfDefaultRadios" checked>
                                    <label class="custom-control-label" for="defaultGroupExample2">ปกติ</label>
                                </div>

                                <!-- Group of default radios - option 3 -->
                                <div class="custom-control custom-radio">
                                    <input type="radio" class="custom-control-input" id="defaultGroupExample3" name="groupOfDefaultRadios">
                                    <label class="custom-control-label" for="defaultGroupExample3">ลูกค้าจำนวนมาก</label>
                                </div>
                                <%--<br />
                                <asp:RadioButton ID="RadioButton2" runat="server" aria-label="Radio button for following text input" Text="ปกติ" ValidationGroup="chk1" GroupName="chk1" /><br />
                                <asp:RadioButton ID="RadioButton3" runat="server" aria-label="Radio button for following text input" Text="ลูกค้าจำนวนมาก" ValidationGroup="chk1" GroupName="chk1" />--%>
                                   <%--<asp:CheckBox ID="CheckBox2" runat="server" aria-label="Checkbox for following text input" text="ปกติ" ValidationGroup="chk1"  />--%>

                                <br />
                                <button class="btn btn-primary" data-dismiss="modal"><i class="fas fa-times fa-fw"></i>Close Window</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Portfolio Modal 2-->
    <div class="portfolio-modal modal fade" id="portfolioModal2" tabindex="-1" role="dialog" aria-labelledby="portfolioModal2Label" aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true"><i class="fas fa-times"></i></span>
                </button>
                <div class="modal-body text-center">
                    <div class="container">
                        <div class="row justify-content-center">
                            <div class="col-lg-8">
                                <!-- Portfolio Modal - Title-->
                                <h2 class="portfolio-modal-title text-secondary text-uppercase mb-0" id="portfolioModal2Label">ลักษณะของร้านค้า</h2>
                                <!-- Icon Divider-->
                                <div class="divider-custom">
                                    <div class="divider-custom-line"></div>
                                    <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
                                    <div class="divider-custom-line"></div>
                                </div>
                                <!-- Portfolio Modal - Image-->
                                <!-- Group of default radios - option 1 -->
                                <div class="custom-control custom-radio">
                                    <input type="radio" class="custom-control-input" id="defaultGroupExample4" name="groupOfDefaultRadios2">
                                    <label class="custom-control-label" for="defaultGroupExample4">มีหน้าร้าน</label>
                                </div>

                                <!-- Group of default radios - option 2 -->
                                <div class="custom-control custom-radio">
                                    <input type="radio" class="custom-control-input" id="defaultGroupExample5" name="groupOfDefaultRadios2" checked>
                                    <label class="custom-control-label" for="defaultGroupExample5">เจ้าจุ้ย</label>
                                </div>

                                <!-- Group of default radios - option 3 -->
                                <div class="custom-control custom-radio">
                                    <input type="radio" class="custom-control-input" id="defaultGroupExample6" name="groupOfDefaultRadios2">
                                    <label class="custom-control-label" for="defaultGroupExample6">รถส่งของ</label>
                                </div>

                                <div class="custom-control custom-radio">
                                    <input type="radio" class="custom-control-input" id="defaultGroupExample7" name="groupOfDefaultRadios2">
                                    <label class="custom-control-label" for="defaultGroupExample7">ร้านธงฟ้า</label>
                                </div>
                                <button class="btn btn-primary" data-dismiss="modal"><i class="fas fa-times fa-fw"></i>Close Window</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Portfolio Modal 3-->
    <div class="portfolio-modal modal fade" id="portfolioModal3" tabindex="-1" role="dialog" aria-labelledby="portfolioModal3Label" aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true"><i class="fas fa-times"></i></span>
                </button>
                <div class="modal-body text-center">
                    <div class="container">
                        <div class="row justify-content-center">
                            <div class="col-lg-8">
                                <!-- Portfolio Modal - Title-->
                                <h2 class="portfolio-modal-title text-secondary text-uppercase mb-0" id="portfolioModal3Label">Circus Tent</h2>
                                <!-- Icon Divider-->
                                <div class="divider-custom">
                                    <div class="divider-custom-line"></div>
                                    <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
                                    <div class="divider-custom-line"></div>
                                </div>
                                <!-- Portfolio Modal - Image-->
                                <!-- Group of default radios - option 1 -->
                                <div class="custom-control custom-radio">
                                    <input type="radio" class="custom-control-input" id="defaultGroupExample8" name="groupOfDefaultRadios2">
                                    <label class="custom-control-label" for="defaultGroupExample4">มีหน้าร้าน</label>
                                </div>

                                <!-- Group of default radios - option 2 -->
                                <div class="custom-control custom-radio">
                                    <input type="radio" class="custom-control-input" id="defaultGroupExample9" name="groupOfDefaultRadios2" checked>
                                    <label class="custom-control-label" for="defaultGroupExample5">เจ้าจุ้ย</label>
                                </div>

                                <!-- Group of default radios - option 3 -->
                                <div class="custom-control custom-radio">
                                    <input type="radio" class="custom-control-input" id="defaultGroupExample10" name="groupOfDefaultRadios2">
                                    <label class="custom-control-label" for="defaultGroupExample6">รถส่งของ</label>
                                </div>

                                <div class="custom-control custom-radio">
                                    <input type="radio" class="custom-control-input" id="defaultGroupExample11" name="groupOfDefaultRadios2">
                                    <label class="custom-control-label" for="defaultGroupExample7">ร้านธงฟ้า</label>
                                </div>
                                <button class="btn btn-primary" data-dismiss="modal"><i class="fas fa-times fa-fw"></i>Close Window</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Portfolio Modal 4-->
    <div class="portfolio-modal modal fade" id="portfolioModal4" tabindex="-1" role="dialog" aria-labelledby="portfolioModal4Label" aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true"><i class="fas fa-times"></i></span>
                </button>
                <div class="modal-body text-center">
                    <div class="container">
                        <div class="row justify-content-center">
                            <div class="col-lg-8">
                                <!-- Portfolio Modal - Title-->
                                <h2 class="portfolio-modal-title text-secondary text-uppercase mb-0" id="portfolioModal4Label">Controller</h2>
                                <!-- Icon Divider-->
                                <div class="divider-custom">
                                    <div class="divider-custom-line"></div>
                                    <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
                                    <div class="divider-custom-line"></div>
                                </div>
                                <!-- Portfolio Modal - Image-->
                                <img class="img-fluid rounded mb-5" src="assets/img/portfolio/game.png" alt="" /><!-- Portfolio Modal - Text-->
                                <p class="mb-5">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Mollitia neque assumenda ipsam nihil, molestias magnam, recusandae quos quis inventore quisquam velit asperiores, vitae? Reprehenderit soluta, eos quod consequuntur itaque. Nam.</p>
                                <button class="btn btn-primary" data-dismiss="modal"><i class="fas fa-times fa-fw"></i>Close Window</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Portfolio Modal 5-->
    <div class="portfolio-modal modal fade" id="portfolioModal5" tabindex="-1" role="dialog" aria-labelledby="portfolioModal5Label" aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true"><i class="fas fa-times"></i></span>
                </button>
                <div class="modal-body text-center">
                    <div class="container">
                        <div class="row justify-content-center">
                            <div class="col-lg-8">
                                <!-- Portfolio Modal - Title-->
                                <h2 class="portfolio-modal-title text-secondary text-uppercase mb-0" id="portfolioModal5Label">Locked Safe</h2>
                                <!-- Icon Divider-->
                                <div class="divider-custom">
                                    <div class="divider-custom-line"></div>
                                    <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
                                    <div class="divider-custom-line"></div>
                                </div>
                                <!-- Portfolio Modal - Image-->
                                <img class="img-fluid rounded mb-5" src="assets/img/portfolio/safe.png" alt="" /><!-- Portfolio Modal - Text-->
                                <p class="mb-5">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Mollitia neque assumenda ipsam nihil, molestias magnam, recusandae quos quis inventore quisquam velit asperiores, vitae? Reprehenderit soluta, eos quod consequuntur itaque. Nam.</p>
                                <button class="btn btn-primary" data-dismiss="modal"><i class="fas fa-times fa-fw"></i>Close Window</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Portfolio Modal 6-->
    <div class="portfolio-modal modal fade" id="portfolioModal6" tabindex="-1" role="dialog" aria-labelledby="portfolioModal6Label" aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true"><i class="fas fa-times"></i></span>
                </button>
                <div class="modal-body text-center">
                    <div class="container">
                        <div class="row justify-content-center">
                            <div class="col-lg-8">
                                <!-- Portfolio Modal - Title-->
                                <h2 class="portfolio-modal-title text-secondary text-uppercase mb-0" id="portfolioModal6Label">Submarine</h2>
                                <!-- Icon Divider-->
                                <div class="divider-custom">
                                    <div class="divider-custom-line"></div>
                                    <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
                                    <div class="divider-custom-line"></div>
                                </div>
                                <!-- Portfolio Modal - Image-->
                                <img class="img-fluid rounded mb-5" src="assets/img/portfolio/submarine.png" alt="" /><!-- Portfolio Modal - Text-->
                                <p class="mb-5">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Mollitia neque assumenda ipsam nihil, molestias magnam, recusandae quos quis inventore quisquam velit asperiores, vitae? Reprehenderit soluta, eos quod consequuntur itaque. Nam.</p>
                                <button class="btn btn-primary" data-dismiss="modal"><i class="fas fa-times fa-fw"></i>Close Window</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    


</asp:Content>

