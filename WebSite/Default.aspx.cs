using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;

public partial class _Default : Page
{
        /// <summary>
        /// Set this to your issuerId 
        /// </summary>
        private const string issuerId = "";

        /// <summary>
        /// Set this to you issuerKey
        /// </summary>
        private const string issuerKey = "";

        /// <summary>
        /// Set this to the badge you want to give out
        /// </summary>
        private const string badgeId = "";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void apiBtn_OnClick(object sender, EventArgs e)
        {
            String assertion = createAssertion();

            if (!String.IsNullOrEmpty(assertion))
                IssuerAPI(assertion);
        }

        protected void bkrBtn_OnClick(object sender, EventArgs e)
        {
            String assertion = createAssertion();

            if (!String.IsNullOrEmpty(assertion))
                this.FailureText.Text += BakerService(assertion);
        }

        private String createAssertion()
        {
            string output = String.Empty;

            try
            {
                String json = "{\"email\":\"" + this.contact.Text.ToLowerInvariant() + "\"}";
                String url = String.Format("{0}/badges/{1}/{2}", "http://obiissuer.cloudapp.net", issuerId, badgeId);

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.Headers.Add("X-OBI-Auth", issuerKey);
                request.ContentLength = json.Length;

                using (var stream = request.GetRequestStream())
                    stream.Write(Encoding.UTF8.GetBytes(json), 0, json.Length);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
			if (response.StatusCode == HttpStatusCode.OK)
			{
                String assertion = String.Format("{0}/badges/{1}/{2}", "http://obiissuer.cloudapp.net", issuerId, response.Headers["X-OBI-Assertion"]);

			    output += "<br/>Created badge assertion (" + response.Headers["X-OBI-Assertion"] + ")" ;

			    return assertion;
			}
			else
			{
			    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				output += "<br/>Error: " + reader.ReadToEnd();

			    return null;
			}
		}
            }
            catch (Exception ex)
            {
                output += "<br/>Exception - " + ex.ToString() + "<br/>" + ex.StackTrace;
                return null;
            }
            finally
            {
                this.FailureText.Text = output;
            }
        }

        private void IssuerAPI(string assertion)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "OpenBadgesJavaScript", "OpenBadges.issue_no_modal([\"" + assertion + "\"]);", true);
        }

        private String BakerService(string assertion)
        {
            String output = String.Empty;

            try
            {
                String url = String.Format("http://msbackpack.azurewebsites.net/baker?assertion={0}&award={1}", assertion, this.contact.Text.ToLowerInvariant());

                //output += "<br/>Baker URL - " + url;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
		{
			if (response.StatusCode == HttpStatusCode.OK)
			{
			    if (response.Headers["x-badge-awarded"].Equals("false", StringComparison.InvariantCultureIgnoreCase))
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				    output += "<br/>Error: " + reader.ReadToEnd();
			    else
				output += "<br/>Badge baked successfully!<br/><br/>Check your backpack for your new badge!<br/>";
			}
			else
			{
			    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				output += "<br/>Error: " + reader.ReadToEnd();
			}
		}
            }
            catch (Exception ex)
            {
                output += "<br/>Exception - " + ex.ToString();
            }

            return output;
        }
    }