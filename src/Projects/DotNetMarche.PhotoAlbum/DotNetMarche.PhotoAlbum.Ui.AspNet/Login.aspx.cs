﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet
{
   public partial class Login : System.Web.UI.Page
   {
      protected void Page_Load(object sender, EventArgs e)
      {

      }

      protected void Login1_LoggedIn(object sender, EventArgs e)
      {
         Debug.Write("Current Use");
      }
   }
}
