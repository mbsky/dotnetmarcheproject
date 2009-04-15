/// <reference path="../../jQuery/jquery.js">

$(function() {

   $('#header')
   .setTemplateURL('/MasterT/HeaderTemplate')
   .processTemplateURL('/MasterT/Header');

   $('#menu')
      .setTemplateURL('/MasterT/MenuTemplate')
      .processTemplateURL('/MasterT/Menu');
});

