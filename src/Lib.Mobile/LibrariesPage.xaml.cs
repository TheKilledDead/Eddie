﻿using Eddie.Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Eddie
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LibrariesPage : ContentPage
	{
		public LibrariesPage ()
		{
			InitializeComponent();

			Title = "Libraries";

			m_webView.Navigating += OnNavigating;
			m_webView.Source = Utils.GetAssetURI("libraries.html");
		}

		private void OnNavigating(object sender, WebNavigatingEventArgs e)
		{
			e.Cancel = true;

			Utils.OpenURL(e.Url);			
		}
	}
}