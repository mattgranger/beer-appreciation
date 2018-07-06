﻿namespace Catalog.API.Infrastructure
{
    public class CatalogSettings
    {
        public string PicBaseUrl { get; set; }

        public string EventBusConnection { get; set; }

        public bool UseCustomisationData { get; set; }

        public bool AzureStorageEnabled { get; set; }
    }
}
