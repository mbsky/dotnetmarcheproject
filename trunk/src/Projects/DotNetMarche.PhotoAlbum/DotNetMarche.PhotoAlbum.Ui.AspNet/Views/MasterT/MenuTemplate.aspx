<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" Theme="" %>
<ul>
   {#foreach $T.MenuItems as menuitem}
   <li>
      {$T.menuitem.Text}
      <ul>
         {#foreach $T.menuitem.MenuItems as menuitem2}
         <li>
            <a href="{$T.menuitem2.Url}" >{$T.menuitem2.Text}</a>
         </li>
          {#/for}
      </ul>
   </li> 
   {#/for}
</ul>
