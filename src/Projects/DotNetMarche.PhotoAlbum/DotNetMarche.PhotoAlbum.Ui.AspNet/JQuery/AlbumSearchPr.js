/// <reference path="jquery.js">
$(function() {
   var div = $('#gridlocation')[0];
   div.curpage = 1;
   div.cursort = "Name";
   div.Load = function() {
//      $('#gridlocation').load(
//         "/Photo/Parts/AlbumSearch.aspx?" +
//            "page=" + div.curpage +
//            "&sort=" + div.cursort +
//            "&name=" + ($('#txtName').val() == undefined ? '' : $('#txtName').val()) +
//            "&description=" + ($('#txtDescription').val() == undefined ? '' : $('#txtDescription').val()) +
//            "&user=" + ($('#txtUser').val() == undefined ? '' : $('#txtUser').val()) +
//            " #thecontent",
   //         WireEvents);
   $('#gridlocation').load(
         "/Photo/Parts/AlbumSearch.aspx?" +
            "page=" + div.curpage +
            "&sort=" + div.cursort +
            "&name=" + ($('#txtName').val() == undefined ? '' : $('#txtName').val()) +
            "&description=" + ($('#txtDescription').val() == undefined ? '' : $('#txtDescription').val()) +
            "&user=" + ($('#txtUser').val() == undefined ? '' : $('#txtUser').val()) +
            " #thecontent",
         WireEvents);
   }

  
   div.Load();

});

function WireEvents(responseText, textStatus, xmlHttpRequest) {

   var pagers = $("#gridlocation tr.pagerStyle a").log();
   var div = $('#gridlocation')[0];
   pagers.click(function() {
      div.curpage = $(this).text();
      div.Load();
      return false;
   });

   var headers = $("#gridlocation tr.headerStyle a").log();
   headers.click(function() {
      div.cursort = $(this).log().text();
      div.Load();
      return false;
   });

   $('#btnSearch').click(function() {
      div.Load();
      return false;
   });
}