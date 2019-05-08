using ControlTime.Models;
using System;
using System.Net.Http;
using System.Configuration;
using System.Windows.Markup;
namespace ControlTime
{
    public class PostCall
    {
        #region Private Methods
        public string ProcessorCall(ProcessorRequestModel processorRequest)
        {
            try
            {
                string endpointURL = this.CreateEndpointURL(processorRequest);
                HttpContent content = GetHttpContentFromEndpoint(endpointURL);
                return content.ToString();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return ex.Message;
            }
        }

        #endregion

        #region Private Methods

        private HttpContent GetHttpContentFromEndpoint(string endpointURL)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage serviceResponse = client.GetAsync(endpointURL).Result;
            HttpContent content = serviceResponse.Content;
            return content;
        }

        private string CreateEndpointURL(ProcessorRequestModel processorRequest)
        {
            return !string.IsNullOrEmpty(processorRequest.RuleId) ?
                string.Format("{0}ruleid={1}", ConfigurationManager.AppSettings["ProcessorURL"], processorRequest.RuleId) :
                null;
        }

        #endregion
    }
}
