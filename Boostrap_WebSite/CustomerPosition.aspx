<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerPosition.aspx.cs" Inherits="CustomerPosition" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title>Multiple Markers Google Maps</title>
    <style>
        #map_canvas {
            height: 98vh;
            margin-left: auto;
            margin-right: auto;
            text-align: left;
            width: 98vw;
        }
    </style>

    <script src="js/jquery.min.js"></script>
    <%--<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>--%>
    <%--    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyACuCBVPmskPtdqdP-cl9do0-W3ZEr6OVc&sensor=false" type="text/javascript"></script>--%>
    <script type="text/javascript" src="https://api.longdo.com/map/?key=8a76770cfbc7306d00e330d581be2ad2"></script>

    <script type="text/javascript">
        // check DOM Ready

        $(document).ready(function () {
            PrepareMapInfo();

        });

        function PrepareMapInfo() {
            PageMethods.GetMapInfo("xxx", OnSuccess);
        }


         function OnSuccess(_result) {
            var result = JSON.parse(_result);

            if (result != null && result != undefined) {
                // execute
                (function () {

                    // set multiple marker
                    if (result.length > 0) {

                        var map = new longdo.Map({ placeholder: document.getElementById('map_canvas') });

                        // init map  

                        for (i = 0; i < result.length; i++) {
                            // init markers

                            if (result[i].lat != "" && result[i].lng != "") {

                                var marker = new longdo.Marker({ lon: result[i].lng, lat: result[i].lat },
                                    {
                                        title: result[i].name,
                                        icon: {
                                            url: 'https://map.longdo.com/mmmap/images/pin_mark.png',
                                            offset: { x: 12, y: 45 }
                                        },
                                        detail: '<div><img src="' + result[i].custImg + '" alt="" height="180" width="180" ></div><br/><div style="background: #eeeeff;">' + result[i].address + '</div><br/><div>' + result[i].tel + '</div>',

                                        draggable: true,
                                        weight: longdo.OverlayWeight.Top,


                                    });
                                map.Overlays.add(marker);

                                map.location({ lon: result[i].lng, lat: result[i].lat }, true);
                                map.zoom(14, true);

                            }
                        }


                    }
                })();
            }
        }

        //function OnSuccess(_result) {
        //    var result = JSON.parse(_result);

        //    if (result != null && result != undefined) {
        //        // execute
        //        (function () {
                   


        //            // set multiple marker
        //            if (result.length > 0) {

        //                // map options
        //                var options = {
        //                    zoom: 8,
        //                    center: new google.maps.LatLng(result[0].lat, result[0].lng), // centered US
        //                    mapTypeId: google.maps.MapTypeId.ROADMAP
        //                };

        //                // init map
        //                var map = new google.maps.Map(document.getElementById('map_canvas'), options);

        //                for (i = 0; i < result.length; i++) {
        //                    // init markers

        //                    if (result[i].lat != "" && result[i].lng != "") {

        //                        var marker = new google.maps.Marker({
        //                            position: new google.maps.LatLng(result[i].lat, result[i].lng),
        //                            map: map,
        //                            title: result[0].name
        //                        });

        //                        document.title = result[0].name;

        //                        // process multiple info windows
        //                        (function (marker, i) {
        //                            // add click event
        //                            google.maps.event.addListener(marker, 'click', function () {

        //                                var infowindow = new google.maps.InfoWindow;

        //                                var infowincontent = document.createElement('div');
        //                                var strong = document.createElement('strong');
        //                                strong.textContent = result[i].name;
        //                                infowincontent.appendChild(strong);
        //                                infowincontent.appendChild(document.createElement('br'));

        //                                var text = document.createElement('text');
        //                                text.textContent = result[i].address;
        //                                infowincontent.appendChild(text);
        //                                infowincontent.appendChild(document.createElement('br'));

        //                                text = document.createElement('text');
        //                                text.textContent = result[i].tel;
        //                                infowincontent.appendChild(text);
        //                                infowincontent.appendChild(document.createElement('br'));

                                        
        //                                var img = img_create(result[i].custImg);
        //                                infowincontent.appendChild(img);

        //                                infowindow.setContent(infowincontent);
        //                                infowindow.open(map, marker);
        //                            });

        //                        })(marker, i);
        //                    }
        //                }
        //            }
        //        })();
        //    }
        //}

        function img_create(src, alt, title) {
            var img = document.createElement('img');
            img.src = src;
            if (alt != null) img.alt = alt;
            if (title != null) img.title = title;
            return img;
        }
    </script>

</head>
<body>
    <form id="form2" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    </form>

    <div id="map_canvas"></div>
</body>
</html>
