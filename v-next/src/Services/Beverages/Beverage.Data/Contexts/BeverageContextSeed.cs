namespace BeerAppreciation.Beverage.Data.Contexts
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Domain;
    using global::Core.Shared.Extensions;
    using global::Core.Shared.Helpers;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;
    using Polly;
    using Polly.Retry;

    public class BeverageContextSeed
    {
        public const string ColumnRegexPattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        public const string BeverageDataNameSpace = "BeerAppreciation.Beverage.Data.Setup";

        public async Task SeedAsync(BeverageContext context, IHostingEnvironment env, ILogger<BeverageContextSeed> logger)
        {
            var policy = this.CreatePolicy(logger, nameof(BeverageContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                if (!context.Manufacturers.Any())
                {
                    await context.Manufacturers.AddRangeAsync(this.GetManufacturersFromFile(logger));
                    await context.SaveChangesAsync();
                }

                if (!context.BeverageTypes.Any())
                {
                    await context.BeverageTypes.AddRangeAsync(this.GetBeverageTypesFromFile(logger));
                    await context.SaveChangesAsync();
                }

                if (!context.BeverageStyles.Any())
                {
                    await context.BeverageStyles.AddRangeAsync(this.GetBeverageStylesFromFile(context, logger));
                    await context.SaveChangesAsync();
                }

                if (!context.Beverages.Any())
                {
                    await context.Beverages.AddRangeAsync(this.GetBeveragesFromFile(context, logger));
                    await context.SaveChangesAsync();
                }
            });
        }

        private IEnumerable<Manufacturer> GetManufacturersFromFile(ILogger<BeverageContextSeed> logger)
        {
            string csvFileManufacturers = ResourceHelper.GetManifestResourceAsString($"{BeverageDataNameSpace}.Manufacturers.csv");

            string[] csvheaders = null;
            try
            {
                string[] requiredHeaders = { "name", "description", "country" };
                csvheaders = this.GetHeaders( csvFileManufacturers, requiredHeaders );
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return csvFileManufacturers.SplitCsvRows()
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ColumnRegexPattern) )
                                        .SelectTry(column => this.CreateManufacturer(column, csvheaders))
                                        .OnCaughtException(ex => { logger.LogError(ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private Manufacturer CreateManufacturer(string[] column, string[] headers)
        {
            GuardColumnHeaders(column, headers);

            var manufacturer = new Manufacturer
            {
                Description = column[Array.IndexOf(headers, "description")].Trim('"').Trim(),
                Name = column[Array.IndexOf(headers, "name")].Trim('"').Trim(),
                Country = column[Array.IndexOf(headers, "country")].Trim('"').Trim(),
            };

            return manufacturer;
        }

        private static void GuardColumnHeaders(string[] column, string[] headers)
        {
            if (column.Length != headers.Length)
            {
                throw new Exception($"column count '{column.Length}' not the same as headers count'{headers.Length}'");
            }
        }

        private IEnumerable<BeverageType> GetBeverageTypesFromFile(ILogger<BeverageContextSeed> logger)
        {
            string csvFileBeverageTypes = ResourceHelper.GetManifestResourceAsString($"{BeverageDataNameSpace}.BeverageTypes.csv");

            if (csvFileBeverageTypes == null)
            {
                return this.GetPreconfiguredBeverageTypes();
            }

            string[] csvheaders = null;
            try
            {
                string[] requiredHeaders = { "name", "description" };
                csvheaders = this.GetHeaders( csvFileBeverageTypes, requiredHeaders );
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return csvFileBeverageTypes.SplitCsvRows()
                                        .Skip(1) // skip header row
                                        .Select(row => Regex.Split(row, ColumnRegexPattern) )
                                        .SelectTry(column => this.CreateBeverageType(column, csvheaders))
                                        .OnCaughtException(ex => { logger.LogError(ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private BeverageType CreateBeverageType(string[] column, string[] headers)
        {
            GuardColumnHeaders(column, headers);

            var beverageType = new BeverageType
            {
                Description = column[Array.IndexOf(headers, "description")].Trim('"').Trim(),
                Name = column[Array.IndexOf(headers, "name")].Trim('"').Trim()
            };

            return beverageType;
        }

        private IEnumerable<BeverageType> GetPreconfiguredBeverageTypes()
        {
            return new List<BeverageType>
            {
                new BeverageType() { Name = "Beer", Description = "Beer is an alcoholic beverage produced by the saccharification of starch and fermentation of the resulting sugar." },
                new BeverageType() { Name = "Cider", Description = "Cider (/ˈsaɪdər/ sy-dər) is a fermented alcoholic beverage made from the unfiltered juice of apples." },
                new BeverageType() { Name = "Wine", Description = "Wine is an alcoholic beverage made from fermented grapes or other fruits." }
            };
        }

        private IEnumerable<BeverageStyle> GetBeverageStylesFromFile(BeverageContext context, ILogger<BeverageContextSeed> logger)
        {
            string csvFileBeverageStyles = ResourceHelper.GetManifestResourceAsString($"{BeverageDataNameSpace}.BeverageStyles.csv");

            string[] csvheaders = null;
            try
            {
                string[] requiredHeaders = { "name", "description", "beveragetypename" };
                csvheaders = this.GetHeaders(csvFileBeverageStyles, requiredHeaders );
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            var beverageTypeIdLookup = context.BeverageTypes.ToDictionary(ct => ct.Name, ct => ct.Id);

            return csvFileBeverageStyles.SplitCsvRows()
                        .Skip(1) // skip header row
                        .Select(row => Regex.Split(row, ColumnRegexPattern) )
                        .SelectTry(column => this.CreateBeverageStyle(column, csvheaders, beverageTypeIdLookup))
                        .OnCaughtException(ex => { logger.LogError(ex.Message); return null; })
                        .Where(x => x != null);
        }

        private IEnumerable<Beverage> GetBeveragesFromFile(BeverageContext context, ILogger<BeverageContextSeed> logger)
        {
            string csvFileBeverages = ResourceHelper.GetManifestResourceAsString($"{BeverageDataNameSpace}.Beverages.csv");

            string[] csvheaders = null;
            try
            {
                string[] requiredHeaders = { "name", "description", "alcoholpercent", "volume", "url", "beveragestylename", "beveragetypename", "manufacturername" };
                csvheaders = this.GetHeaders(csvFileBeverages, requiredHeaders );
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            var beverageTypeIdLookup = context.BeverageTypes.ToDictionary(ct => ct.Name, ct => ct.Id);
            var beverageStyleIdLookup = context.BeverageStyles.ToDictionary(ct => ct.Name, ct => ct.Id);
            var manufacturerIdLookup = context.Manufacturers.ToDictionary(ct => ct.Name, ct => ct.Id);

            return csvFileBeverages.SplitCsvRows()
                .Skip(1) // skip header row
                .Select(row => Regex.Split(row, ColumnRegexPattern) )
                .SelectTry(column => this.CreateBeverage(column, csvheaders, beverageTypeIdLookup, beverageStyleIdLookup, manufacturerIdLookup, logger))
                .OnCaughtException(ex => { logger.LogError(ex.Message); return null; })
                .Where(x => x != null);
        }

        private BeverageStyle CreateBeverageStyle(string[] column, string[] headers, Dictionary<String, int> beverageTypeIdLookup)
        {
            GuardColumnHeaders(column, headers);

            string beverageTypeName = column[Array.IndexOf(headers, "beveragetypename")].Trim('"').Trim();
            if (!beverageTypeIdLookup.ContainsKey(beverageTypeName))
            {
                throw new Exception($"type={beverageTypeName} does not exist in beverageTypes");
            }

            var beverageStyle = new BeverageStyle
            {
                Description = column[Array.IndexOf(headers, "description")].Trim('"').Trim(),
                Name = column[Array.IndexOf(headers, "name")].Trim('"').Trim(),
                BeverageTypeId = beverageTypeIdLookup[beverageTypeName],
            };

            return beverageStyle;
        }

        private Beverage CreateBeverage(string[] column, string[] headers, Dictionary<String, int> beverageTypeIdLookup, Dictionary<String, int> beverageStyleIdLookup, Dictionary<String, int> manufacturerIdLookup, ILogger<BeverageContextSeed> logger)
        {
            GuardColumnHeaders(column, headers);

            string beverageTypeName = column[Array.IndexOf(headers, "beveragetypename")].Trim('"').Trim();
            if (!beverageTypeIdLookup.ContainsKey(beverageTypeName))
            {
                throw new Exception($"type={beverageTypeName} does not exist in beverageTypes");
            }

            string beverageStyleName = column[Array.IndexOf(headers, "beveragestylename")].Trim('"').Trim();
            if (!beverageStyleIdLookup.ContainsKey(beverageStyleName))
            {
                throw new Exception($"type={beverageStyleName} does not exist in beverageStyles");
            }

            string manufacturerName = column[Array.IndexOf(headers, "manufacturername")].Trim('"').Trim();
            if (!manufacturerIdLookup.ContainsKey(manufacturerName))
            {
                throw new Exception($"type={manufacturerName} does not exist in menufacturers");
            }

            var beverage = new Beverage
            {
                Description = column[Array.IndexOf(headers, "description")].Trim('"').Trim(),
                Name = column[Array.IndexOf(headers, "name")].Trim('"').Trim(),
                Url = column[Array.IndexOf(headers, "url")].Trim('"').Trim(),
                BeverageTypeId = beverageTypeIdLookup[beverageTypeName],
                BeverageStyleId = beverageStyleIdLookup[beverageStyleName],
                ManufacturerId = manufacturerIdLookup[manufacturerName]
            };

            int alcoholPercentIndex = Array.IndexOf(headers, "alcoholpercent");
            if (alcoholPercentIndex != -1)
            {
                string alcoholPercentString = column[alcoholPercentIndex].Trim('"').Trim();
                if (!String.IsNullOrEmpty(alcoholPercentString))
                {
                    if ( decimal.TryParse(alcoholPercentString, out decimal alcoholPercent))
                    {
                        beverage.AlcoholPercent = alcoholPercent;
                    }
                    else
                    {
                        logger.LogError($"alcoholPercent={alcoholPercentString} is not a valid decimal");
                    }
                }
            }

            int volumeIndex = Array.IndexOf(headers, "volume");
            if (volumeIndex != -1)
            {
                string volumeString = column[volumeIndex].Trim('"').Trim();
                if (!String.IsNullOrEmpty(volumeString))
                {
                    if (int.TryParse(volumeString, out int volume))
                    {
                        beverage.Volume = volume;
                    }
                    else
                    {
                        logger.LogError($"volume={volume} is not a valid integer");
                    }
                }
            }


            return beverage;
        }


        private string[] GetHeaders(string csvfile, string[] requiredHeaders, string[] optionalHeaders = null)
        {
            string[] csvheaders = csvfile.GetHeaderRow().ToLowerInvariant().Split(',');

            if (csvheaders.Count() < requiredHeaders.Count())
            {
                throw new Exception($"requiredHeader count '{ requiredHeaders.Count()}' is bigger then csv header count '{csvheaders.Count()}' ");
            }

            if (optionalHeaders != null)
            {
                if (csvheaders.Count() > (requiredHeaders.Count() + optionalHeaders.Count()))
                {
                    throw new Exception($"csv header count '{csvheaders.Count()}'  is larger then required '{requiredHeaders.Count()}' and optional '{optionalHeaders.Count()}' headers count");
                }
            }

            foreach (var requiredHeader in requiredHeaders)
            {
                if (!csvheaders.Contains(requiredHeader))
                {
                    throw new Exception($"does not contain required header '{requiredHeader}'");
                }
            }

            return csvheaders;
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<BeverageContextSeed> logger, string prefix,int retries = 3)
        {
            return Policy.Handle<DbException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogTrace($"[{prefix}] Exception {exception.GetType().Name} with message ${exception.Message} detected on attempt {retry} of {retries}");
                    }
                );
        }
    }

}
