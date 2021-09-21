using FerryData.Engine.Enums;
using FerryData.Engine.Helpers;
using FerryData.Engine.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FerryData.Client.Shared
{
    public partial class WorkflowActionStepSettingsEdit : ComponentBase
    {

        private List<Tuple<HttpMethods, string>> _methodsSource;

        [Parameter]
        public WorkflowActionStepSettings Step { get; set; }

        public WorkflowHttpAction HttpAction
        {
            get
            {
                if (Step.Action is WorkflowHttpAction httpAction)
                {
                    return httpAction;
                }
                else
                {
                    return null;
                }
            }
        }

        protected override void OnInitialized()
        {
            _methodsSource = new List<Tuple<HttpMethods, string>>();
            _methodsSource.Add(new Tuple<HttpMethods, string>(HttpMethods.Get, "GET"));
            _methodsSource.Add(new Tuple<HttpMethods, string>(HttpMethods.Post, "POST"));

        }

        private void OnAddHeaderRowClick()
        {

            if (HttpAction == null)
            {
                return;
            }

            HttpAction.NewHeaderRow();
        }

        private void OnRemoveHeaderRowClick(NameValueDescriptionRow row)
        {

            if (HttpAction == null)
            {
                return;
            }

            HttpAction.RemoveHeaderRow(row);
        }

        private void OnAddParameterRowClick()
        {

            if (HttpAction == null)
            {
                return;
            }

            HttpAction.NewParameterRow();
        }

        private void OnRemoveParemeterRowClick(NameValueDescriptionRow row)
        {

            if (HttpAction == null)
            {
                return;
            }

            HttpAction.RemoveParameterRow(row);

            HttpAction.Url = UrlBuilder.UpdateUrl(HttpAction.Url, HttpAction.Parameters);
        }

        private void OnUrlParametersChanged()
        {
            if (HttpAction == null)
            {
                return;
            }

            HttpAction.Url = UrlBuilder.UpdateUrl(HttpAction.Url, HttpAction.Parameters);
        }

        private void OnUrlChanged()
        {
            if (HttpAction == null)
            {
                return;
            }

            var parameters = UrlBuilder.ParseParameters(HttpAction.Url);
            HttpAction.UpdateParameters(parameters);
        }

        private void OnAddContentTypeApplicationJsonClick()
        {
            if (HttpAction == null)
            {
                return;
            }

            if (!HttpAction.Headers.Any(x => x.Name?.ToLower().Contains("content-type") ?? false))
            {
                var newRow = HttpAction.NewHeaderRow();
                newRow.Name = "Content-Type";
                newRow.Value = "application/json";
            }
        }
    }
}
