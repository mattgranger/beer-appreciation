﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

namespace BeerAppreciation.Web
{
	public partial class Startup
	{
	    public void ConfigureSignalR(IAppBuilder app)
	    {
	        app.MapSignalR();

	        //// Branch the pipeline here for requests that start with "/signalr"
	        //app.Map("/signalr", map =>
	        //{
	        //    // Setup the CORS middleware to run before SignalR.
	        //    // By default this will allow all origins. You can 
	        //    // configure the set of origins and/or http verbs by
	        //    // providing a cors options with a different policy.
	        //    map.UseCors(CorsOptions.AllowAll);
	        //    var hubConfiguration = new HubConfiguration
	        //    {
	        //        EnableJavaScriptProxies = true
	        //        // You can enable JSONP by uncommenting line below.
	        //        // JSONP requests are insecure but some older browsers (and some
	        //        // versions of IE) require JSONP to work cross domain
	        //        //EnableJSONP = true
	        //    };
	        //    // Run the SignalR pipeline. We're not using MapSignalR
	        //    // since this branch already runs under the "/signalr"
	        //    // path.
	        //    map.RunSignalR(hubConfiguration);
	        //});
	    }
	}
}