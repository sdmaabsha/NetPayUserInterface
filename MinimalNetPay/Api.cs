using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using NetPay.DataAccess;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Web;

namespace MinimalNetPay.Api
{
    public static class Api
    {

        public static void ConfigureApi(this WebApplication app, WebApplicationBuilder builder)
        {
            app.MapGet("/GetAuthenticationRestService/", GetAuthenticationRestService).AllowAnonymous();
            app.MapGet("/GetAccountingRestService/", GetAccountingRestService).AllowAnonymous();
            app.MapGet("/GetContractsRestService/", GetContractsRestService).AllowAnonymous();
            app.MapGet("/GetLegacyNpaApi/", GetLegacyNpaApi).AllowAnonymous();
            app.MapGet("/GetNpaReactWeb/", GetNpaReactWeb).AllowAnonymous();

            app.MapGet("/GetPaydiniAspx/", GetPaydiniAspx).AllowAnonymous();
            app.MapGet("/GetUnderwritingReactWeb/", GetUnderwritingReactWeb).AllowAnonymous();
            app.MapGet("/GetLeadsRestService/", GetLeadsRestService).AllowAnonymous();
            app.MapGet("/GetCustomersRestService/", GetCustomersRestService).AllowAnonymous();
            app.MapGet("/GetLoansRestService/", GetLoansRestService).AllowAnonymous();

            app.MapGet("/GetUnderwritingRestService/", GetUnderwritingRestService).AllowAnonymous();
            
        }

       

        internal static async Task<IResult> GetAuthenticationRestService()
        {
            try
            {
                //var connString = "Server=localhost;User ID=root;Password=root;Database=netpay_db";

                //await using var connection = new MySqlConnection(connString);
                //await connection.OpenAsync();

                //using (var cmd = new MySqlCommand())
                //{
                //    cmd.Connection = connection;
                //    cmd.CommandText = "INSERT INTO sonarcube_data (idsonarcube,sonarcube_api_prefix,sonarcube_data) VALUES (@p)";
                //    cmd.Parameters.AddWithValue("p", "Hello world");
                //    await cmd.ExecuteNonQueryAsync();
                //}

                string apiUrl2 = BuildApiUrl("Net-Pay-Advance-Inc_authentication-rest-service_AYTOgXUc2-bht3MdDOKg", "qa");
                WebRequest request2 = WebRequest.Create(apiUrl2);

                string token2 = "squ_da59eacbda0568f0f849e709b1fa175558e30024:";
                string credentials2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(token2));
                string authorizationHeader2 = $"basic {credentials2}";

                request2.Headers.Add("Authorization", authorizationHeader2);
                // Get the response.
                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();

                // Get the stream containing content returned by the server.
                Stream dataStream2 = response2.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader2 = new StreamReader(dataStream2);
                // Read the content.
                string responseFromServer2 = reader2.ReadToEnd();

                RootComponent? measuresComponent = default(RootComponent);
                //Dictionary<string, MeasureHistory> lastIndexData = new Dictionary<string, MeasureHistory>();

                if (response2.StatusCode == HttpStatusCode.OK)
                {
                    // Deserialize JSON
                    measuresComponent = JsonSerializer.Deserialize<RootComponent>(responseFromServer2);
                }

                //Retrieve last index data for each metric

                WebRequest request = WebRequest.Create("https://sast.f2crew.com/api/qualitygates/project_status?projectKey=Net-Pay-Advance-Inc_authentication-rest-service_AYTOgXUc2-bht3MdDOKg&branch=qa");

                string token = "sqa_8a9268e3a4d27e1b1d1288840262795afa6e6e39:";
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                string authorizationHeader = $"Basic {credentials}";

                request.Headers.Add("Authorization", authorizationHeader);
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                string[] metricKeyOrder = MetricOrder();


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Root? projectStatus = JsonSerializer.Deserialize<Root>(responseFromServer);
                    projectStatus.projectStatus.respository = "Authentication-Rest-Service";


                    projectStatus.projectStatus.conditions.ForEach(condition =>
                    {
                        var Value = NumberToWords(condition.periodIndex.ToString());
                        condition.periodIndexValue = Value;


                    });

                    var datasecurityhotspots = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_security_hotspots_reviewed");

                    if (datasecurityhotspots == null)
                    {
                        Condition newdatasecurityhotspots = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newdatasecurityhotspots);
                    }

                    var datanewmaintainabilityrating = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_maintainability_rating");

                    if (datanewmaintainabilityrating == null)
                    {
                        Condition datanewmaintainabilityrating10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_maintainability_rating",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewmaintainabilityrating10);
                    }

                    var dataduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "duplicated_lines_density");

                    if (dataduplicatedlinesdensity == null)
                    {
                        Condition dataduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(dataduplicatedlinesdensity10);
                    }
                    var datanewcoverage = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_coverage");

                    if (datanewcoverage == null)
                    {
                        Condition datanewcoverage10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_coverage",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewcoverage10);
                    }

                    var datanewduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_duplicated_lines_density");

                    if (datanewduplicatedlinesdensity == null)
                    {
                        Condition datanewduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewduplicatedlinesdensity10);
                    }

                    var hotspotsValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots");
                    if (hotspotsValues != null)
                    {

                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }
                    else
                    {
                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }

                    var hotspotsreviewedValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots_reviewed");
                    if (hotspotsreviewedValues != null)
                    {

                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsreviewedValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }
                    else
                    {
                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }

                    var coverageValues = measuresComponent.component.measures.Find(condition => condition.metric == "coverage");
                    if (coverageValues != null)
                    {

                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = coverageValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }
                    else
                    {
                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }

                    var newreliabilityratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "reliability_rating");
                    if (newreliabilityratingValues != null)
                    {
                        var Value = default(string);
                        int versionInt = default(int);
                        var lastIndex = newreliabilityratingValues.value;
                        if (decimal.TryParse(lastIndex, out decimal versionDecimal))
                        {
                            versionInt = (int)Math.Round(versionDecimal);
                            // Now, versionInt will contain the rounded integer value
                        }
                        Value = NumberToWords(versionInt.ToString());
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newreliabilityratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);
                    }
                    else
                    {
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);

                    }

                    var newsecurityrating = measuresComponent.component.measures.Find(condition => condition.metric == "security_rating");
                    if (newsecurityrating != null)
                    {
                        var Value = default(string);
                        var lastIndex = newsecurityrating.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newsecurityrating.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }
                    else
                    {
                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-"

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }

                    var bugsValues = measuresComponent.component.measures.Find(condition => condition.metric == "bugs");
                    if (bugsValues != null)
                    {

                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = bugsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }
                    else
                    {
                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }

                    var vulnerabilitiesValues = measuresComponent.component.measures.Find(condition => condition.metric == "new_vulnerabilities");
                    if (vulnerabilitiesValues != null)
                    {
                        var valueVulData = vulnerabilitiesValues.periods;
                        var valueVulData2 = default(Period2);
                        var valueVulData1 = default(string);
                        if (valueVulData != null)
                        {
                            valueVulData2 = vulnerabilitiesValues.periods.Last();
                            valueVulData1 = valueVulData2.value;
                        }
                        else
                        {
                             valueVulData1 = vulnerabilitiesValues.period.value;
                        }
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = valueVulData1,

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }
                    else
                    {
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }

                    var codesmellsValues = measuresComponent.component.measures.Find(condition => condition.metric == "code_smells");
                    if (codesmellsValues != null)
                    {

                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = codesmellsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }
                    else
                    {
                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }

                    var ncLocationValues = measuresComponent.component.measures.Find(condition => condition.metric == "ncloc");
                    if (ncLocationValues != null)
                    {
                        string input = ncLocationValues.value;
                        string result = Regex.Match(input, @"\d+").Value;
                        string formattedNumber = default(string);
                        if (int.TryParse(result, out int number))
                        {
                            int originalNumber = Convert.ToInt32(result);
                            formattedNumber = FormatNumber(originalNumber);
                        }
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = formattedNumber,

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }
                    else
                    {
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }

                    var sqaleindexValues = measuresComponent.component.measures.Find(condition => condition.metric == "sqale_index");
                    if (sqaleindexValues != null)
                    {
                        string result = string.Empty;
                        var daysValue = default(double);
                        string formattedValue = default(string);
                        var sqaleValue = sqaleindexValues.value;
                        if (sqaleValue.Length == 1 || sqaleValue.Length == 2)
                        {
                            daysValue = Convert.ToDouble(sqaleValue);
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            result = $"{formattedValue}min";
                        }
                        else
                        {
                            var NewLastIndex = Convert.ToDouble(sqaleValue) / 60;
                            daysValue = NewLastIndex / 8;
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            string[] parts = Convert.ToString(formattedValue).Split('.');
                            result = $"{parts[0]}d {parts[1]}h";
                        }


                        if (formattedValue == "0")
                        {
                            result = "0";
                        }

                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = result,

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }
                    else
                    {
                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }

                    var securityreviewratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_review_rating");
                    if (securityreviewratingValues != null)
                    {

                        var Value = default(string);
                        var lastIndex = securityreviewratingValues.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = securityreviewratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }
                    else
                    {
                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }

                    var duplicatedblocksValues = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_blocks");
                    if (duplicatedblocksValues != null)
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = duplicatedblocksValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    else
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    var duplicatedlinesdensity = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_lines_density");
                    if (duplicatedlinesdensity != null)
                    {

                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = duplicatedlinesdensity.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }
                    else
                    {
                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }

                    projectStatus.projectStatus.conditions = projectStatus.projectStatus.conditions.OrderBy(c => Array.IndexOf(metricKeyOrder, c.metricKey)).ToList();
                    return Results.Ok(projectStatus);

                }
                return Results.NotFound(responseFromServer);

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }


        internal static async Task<IResult> GetAccountingRestService()
        {
            try
            {
                string apiUrl2 = BuildApiUrl("Net-Pay-Advance-Inc_accounting-rest-service_AYTPIQnD2-bht3MdDOM2", "qa");
                WebRequest request2 = WebRequest.Create(apiUrl2);

                string token2 = "squ_da59eacbda0568f0f849e709b1fa175558e30024:";
                string credentials2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(token2));
                string authorizationHeader2 = $"basic {credentials2}";

                request2.Headers.Add("Authorization", authorizationHeader2);
                // Get the response.
                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();

                // Get the stream containing content returned by the server.
                Stream dataStream2 = response2.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader2 = new StreamReader(dataStream2);
                // Read the content.
                string responseFromServer2 = reader2.ReadToEnd();

                RootComponent? measuresComponent = default(RootComponent);
                //Dictionary<string, MeasureHistory> lastIndexData = new Dictionary<string, MeasureHistory>();

                if (response2.StatusCode == HttpStatusCode.OK)
                {
                    // Deserialize JSON
                    measuresComponent = JsonSerializer.Deserialize<RootComponent>(responseFromServer2);
                }

                //Retrieve last index data for each metric



               WebRequest request = WebRequest.Create("https://sast.f2crew.com/api/qualitygates/project_status?projectKey=Net-Pay-Advance-Inc_accounting-rest-service_AYTPIQnD2-bht3MdDOM2&branch=qa");

                string token = "sqa_8a9268e3a4d27e1b1d1288840262795afa6e6e39:";
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                string authorizationHeader = $"Basic {credentials}";

                request.Headers.Add("Authorization", authorizationHeader2);
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string[] metricKeyOrder = MetricOrder();

                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Root? projectStatus = JsonSerializer.Deserialize<Root>(responseFromServer);
                    projectStatus.projectStatus.respository = "Accounting-Rest-Service";
                    projectStatus.projectStatus.conditions.ForEach(condition =>
                    {
                        var Value = NumberToWords(condition.periodIndex.ToString());
                        condition.periodIndexValue = Value;
                    });

                    var datasecurityhotspots = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_security_hotspots_reviewed");

                    if (datasecurityhotspots == null)
                    {
                        Condition newdatasecurityhotspots = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newdatasecurityhotspots);
                    }

                    var datanewmaintainabilityrating = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_maintainability_rating");

                    if (datanewmaintainabilityrating == null)
                    {
                        Condition datanewmaintainabilityrating10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_maintainability_rating",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewmaintainabilityrating10);
                    }

                    var dataduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "duplicated_lines_density");

                    if (dataduplicatedlinesdensity == null)
                    {
                        Condition dataduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(dataduplicatedlinesdensity10);
                    }
                    var datanewcoverage = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_coverage");

                    if (datanewcoverage == null)
                    {
                        Condition datanewcoverage10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_coverage",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewcoverage10);
                    }

                    var datanewduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_duplicated_lines_density");

                    if (datanewduplicatedlinesdensity == null)
                    {
                        Condition datanewduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewduplicatedlinesdensity10);
                    }

                    var hotspotsValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots");
                    if (hotspotsValues != null)
                    {

                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }
                    else
                    {
                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }

                    var hotspotsreviewedValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots_reviewed");
                    if (hotspotsreviewedValues != null)
                    {

                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsreviewedValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }
                    else
                    {
                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }

                    var coverageValues = measuresComponent.component.measures.Find(condition => condition.metric == "coverage");
                    if (coverageValues != null)
                    {

                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = coverageValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }
                    else
                    {
                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }

                    var newreliabilityratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "reliability_rating");
                    if (newreliabilityratingValues != null)
                    {
                        var Value = default(string);
                        int versionInt = default(int);
                        var lastIndex = newreliabilityratingValues.value;
                        if (decimal.TryParse(lastIndex, out decimal versionDecimal))
                        {
                            versionInt = (int)Math.Round(versionDecimal);
                            // Now, versionInt will contain the rounded integer value
                        }
                        Value = NumberToWords(versionInt.ToString());
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newreliabilityratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);
                    }
                    else
                    {
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);

                    }

                    var newsecurityrating = measuresComponent.component.measures.Find(condition => condition.metric == "security_rating");
                    if (newsecurityrating != null)
                    {
                        var Value = default(string);
                        var lastIndex = newsecurityrating.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newsecurityrating.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }
                    else
                    {
                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-"

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }

                    var bugsValues = measuresComponent.component.measures.Find(condition => condition.metric == "bugs");
                    if (bugsValues != null)
                    {

                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = bugsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }
                    else
                    {
                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }

                    var vulnerabilitiesValues = measuresComponent.component.measures.Find(condition => condition.metric == "new_vulnerabilities");
                    if (vulnerabilitiesValues != null)
                    {
                        var valueVulData = vulnerabilitiesValues.periods;
                        var valueVulData2 = default(Period2);
                        var valueVulData1 = default(string);
                        if (valueVulData != null)
                        {
                            valueVulData2 = vulnerabilitiesValues.periods.Last();
                            valueVulData1 = valueVulData2.value;
                        }
                        else
                        {
                            valueVulData1 = vulnerabilitiesValues.period.value;
                        }
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = valueVulData1,

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }
                    else
                    {
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }

                    var codesmellsValues = measuresComponent.component.measures.Find(condition => condition.metric == "code_smells");
                    if (codesmellsValues != null)
                    {

                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = codesmellsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }
                    else
                    {
                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }

                    var ncLocationValues = measuresComponent.component.measures.Find(condition => condition.metric == "ncloc");
                    if (ncLocationValues != null)
                    {
                        string input = ncLocationValues.value;
                        string result = Regex.Match(input, @"\d+").Value;
                        string formattedNumber = default(string);
                        if (int.TryParse(result, out int number))
                        {
                            int originalNumber = Convert.ToInt32(result);
                            formattedNumber = FormatNumber(originalNumber);
                        }
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = formattedNumber,

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }
                    else
                    {
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }

                    var sqaleindexValues = measuresComponent.component.measures.Find(condition => condition.metric == "sqale_index");
                    if (sqaleindexValues != null)
                    {
                        string result = string.Empty;
                        var daysValue = default(double);
                        string formattedValue = default(string);
                        var sqaleValue = sqaleindexValues.value;
                        if (sqaleValue.Length == 1 || sqaleValue.Length == 2)
                        {
                            daysValue = Convert.ToDouble(sqaleValue);
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            result = $"{formattedValue}min";
                        }
                        else
                        {
                            var NewLastIndex = Convert.ToDouble(sqaleValue) / 60;
                            daysValue = NewLastIndex / 8;
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            string[] parts = Convert.ToString(formattedValue).Split('.');
                            result = $"{parts[0]}d {parts[1]}h";
                        }


                        if (formattedValue == "0")
                        {
                            result = "0";
                        }

                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = result,

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }
                    else
                    {
                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }

                    var securityreviewratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_review_rating");
                    if (securityreviewratingValues != null)
                    {

                        var Value = default(string);
                        var lastIndex = securityreviewratingValues.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = securityreviewratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }
                    else
                    {
                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }

                    var duplicatedblocksValues = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_blocks");
                    if (duplicatedblocksValues != null)
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = duplicatedblocksValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    else
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    var duplicatedlinesdensity = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_lines_density");
                    if (duplicatedlinesdensity != null)
                    {

                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = duplicatedlinesdensity.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }
                    else
                    {
                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }
                    projectStatus.projectStatus.conditions = projectStatus.projectStatus.conditions.OrderBy(c => Array.IndexOf(metricKeyOrder, c.metricKey)).ToList();
                    return Results.Ok(projectStatus);
                }
                return Results.NotFound(responseFromServer2);

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        internal static async Task<IResult> GetContractsRestService()
        {
            try
            {
                string apiUrl2 = BuildApiUrl("Net-Pay-Advance-Inc_contracts-rest-service_AYTPOc-l2-bht3MdDONH", "qa");
                WebRequest request2 = WebRequest.Create(apiUrl2);

                string token2 = "squ_da59eacbda0568f0f849e709b1fa175558e30024:";
                string credentials2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(token2));
                string authorizationHeader2 = $"basic {credentials2}";

                request2.Headers.Add("Authorization", authorizationHeader2);
                // Get the response.
                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();

                // Get the stream containing content returned by the server.
                Stream dataStream2 = response2.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader2 = new StreamReader(dataStream2);
                // Read the content.
                string responseFromServer2 = reader2.ReadToEnd();

                RootComponent? measuresComponent = default(RootComponent);
                //Dictionary<string, MeasureHistory> lastIndexData = new Dictionary<string, MeasureHistory>();

                if (response2.StatusCode == HttpStatusCode.OK)
                {
                    // Deserialize JSON
                    measuresComponent = JsonSerializer.Deserialize<RootComponent>(responseFromServer2);
                }


                WebRequest request = WebRequest.Create("https://sast.f2crew.com/api/qualitygates/project_status?projectKey=Net-Pay-Advance-Inc_contracts-rest-service_AYTPOc-l2-bht3MdDONH&branch=qa");

                string token = "sqa_8a9268e3a4d27e1b1d1288840262795afa6e6e39:";
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                string authorizationHeader = $"Basic {credentials}";

                request.Headers.Add("Authorization", authorizationHeader);
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string[] metricKeyOrder = MetricOrder();

                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Root? projectStatus = JsonSerializer.Deserialize<Root>(responseFromServer);
                    projectStatus.projectStatus.respository = "Contracts-Rest-Service";
                    projectStatus.projectStatus.conditions.ForEach(condition =>
                    {
                        var Value = NumberToWords(condition.periodIndex.ToString());
                        condition.periodIndexValue = Value;
                        

                    });

                    var datasecurityhotspots = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_security_hotspots_reviewed");

                    if (datasecurityhotspots == null)
                    {
                        Condition newdatasecurityhotspots = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newdatasecurityhotspots);
                    }

                    var datanewmaintainabilityrating = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_maintainability_rating");

                    if (datanewmaintainabilityrating == null)
                    {
                        Condition datanewmaintainabilityrating10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_maintainability_rating",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewmaintainabilityrating10);
                    }

                    var dataduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "duplicated_lines_density");

                    if (dataduplicatedlinesdensity == null)
                    {
                        Condition dataduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(dataduplicatedlinesdensity10);
                    }
                    var datanewcoverage = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_coverage");

                    if (datanewcoverage == null)
                    {
                        Condition datanewcoverage10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_coverage",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewcoverage10);
                    }

                    var datanewduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_duplicated_lines_density");

                    if (datanewduplicatedlinesdensity == null)
                    {
                        Condition datanewduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewduplicatedlinesdensity10);
                    }

                    var hotspotsValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots");
                    if (hotspotsValues != null)
                    {

                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }
                    else
                    {
                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }

                    var hotspotsreviewedValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots_reviewed");
                    if (hotspotsreviewedValues != null)
                    {

                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsreviewedValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }
                    else
                    {
                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }

                    var coverageValues = measuresComponent.component.measures.Find(condition => condition.metric == "coverage");
                    if (coverageValues != null)
                    {

                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = coverageValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }
                    else
                    {
                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }

                    var newreliabilityratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "reliability_rating");
                    if (newreliabilityratingValues != null)
                    {
                        var Value = default(string);
                        int versionInt = default(int);
                        var lastIndex = newreliabilityratingValues.value;
                        if (decimal.TryParse(lastIndex, out decimal versionDecimal))
                        {
                            versionInt = (int)Math.Round(versionDecimal);
                            // Now, versionInt will contain the rounded integer value
                        }
                        Value = NumberToWords(versionInt.ToString());
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newreliabilityratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);
                    }
                    else
                    {
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);

                    }

                    var newsecurityrating = measuresComponent.component.measures.Find(condition => condition.metric == "security_rating");
                    if (newsecurityrating != null)
                    {
                        var Value = default(string);
                        var lastIndex = newsecurityrating.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newsecurityrating.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }
                    else
                    {
                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-"

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }

                    var bugsValues = measuresComponent.component.measures.Find(condition => condition.metric == "bugs");
                    if (bugsValues != null)
                    {

                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = bugsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }
                    else
                    {
                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }

                    var vulnerabilitiesValues = measuresComponent.component.measures.Find(condition => condition.metric == "new_vulnerabilities");
                    if (vulnerabilitiesValues != null)
                    {
                        var valueVulData = vulnerabilitiesValues.periods;
                        var valueVulData2 = default(Period2);
                        var valueVulData1 = default(string);
                        if (valueVulData != null)
                        {
                            valueVulData2 = vulnerabilitiesValues.periods.Last();
                            valueVulData1 = valueVulData2.value;
                        }
                        else
                        {
                            valueVulData1 = vulnerabilitiesValues.period.value;
                        }
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = valueVulData1,

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }
                    else
                    {
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }

                    var codesmellsValues = measuresComponent.component.measures.Find(condition => condition.metric == "code_smells");
                    if (codesmellsValues != null)
                    {

                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = codesmellsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }
                    else
                    {
                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }

                    var ncLocationValues = measuresComponent.component.measures.Find(condition => condition.metric == "ncloc");
                    if (ncLocationValues != null)
                    {
                        string input = ncLocationValues.value;
                        string result = Regex.Match(input, @"\d+").Value;
                        string formattedNumber = default(string);
                        if (int.TryParse(result, out int number))
                        {
                            int originalNumber = Convert.ToInt32(result);
                            formattedNumber = FormatNumber(originalNumber);
                        }
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = formattedNumber,

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }
                    else
                    {
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }

                    var sqaleindexValues = measuresComponent.component.measures.Find(condition => condition.metric == "sqale_index");
                    if (sqaleindexValues != null)
                    {
                        string result = string.Empty;
                        var daysValue = default(double);
                        string formattedValue = default(string);
                        var sqaleValue = sqaleindexValues.value;
                        if (sqaleValue.Length == 1 || sqaleValue.Length == 2)
                        {
                            daysValue = Convert.ToDouble(sqaleValue);
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            result = $"{formattedValue}min";
                        }
                        else
                        {
                            var NewLastIndex = Convert.ToDouble(sqaleValue) / 60;
                            daysValue = NewLastIndex / 8;
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            string[] parts = Convert.ToString(formattedValue).Split('.');
                            result = $"{parts[0]}d {parts[1]}h";
                        }


                        if (formattedValue == "0")
                        {
                            result = "0";
                        }

                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = result,

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }
                    else
                    {
                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }

                    var securityreviewratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_review_rating");
                    if (securityreviewratingValues != null)
                    {

                        var Value = default(string);
                        var lastIndex = securityreviewratingValues.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = securityreviewratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }
                    else
                    {
                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }

                    var duplicatedblocksValues = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_blocks");
                    if (duplicatedblocksValues != null)
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = duplicatedblocksValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    else
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    var duplicatedlinesdensity = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_lines_density");
                    if (duplicatedlinesdensity != null)
                    {

                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = duplicatedlinesdensity.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }
                    else
                    {
                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }
                    projectStatus.projectStatus.conditions = projectStatus.projectStatus.conditions.OrderBy(c => Array.IndexOf(metricKeyOrder, c.metricKey)).ToList();
                    return Results.Ok(projectStatus);

                }
                return Results.NotFound(responseFromServer);

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }


        internal static async Task<IResult> GetLegacyNpaApi()
        {
            try
            {

                string apiUrl2 = BuildApiUrl("Net-Pay-Advance-Inc_legacy-npa-api_AYR9eG1sbQDryOR5J_O7", "qa");
                WebRequest request2 = WebRequest.Create(apiUrl2);

                string token2 = "squ_da59eacbda0568f0f849e709b1fa175558e30024:";
                string credentials2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(token2));
                string authorizationHeader2 = $"basic {credentials2}";

                request2.Headers.Add("Authorization", authorizationHeader2);
                // Get the response.
                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();

                // Get the stream containing content returned by the server.
                Stream dataStream2 = response2.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader2 = new StreamReader(dataStream2);
                // Read the content.
                string responseFromServer2 = reader2.ReadToEnd();

                RootComponent? measuresComponent = default(RootComponent);
                //Dictionary<string, MeasureHistory> lastIndexData = new Dictionary<string, MeasureHistory>();

                if (response2.StatusCode == HttpStatusCode.OK)
                {
                    // Deserialize JSON
                    measuresComponent = JsonSerializer.Deserialize<RootComponent>(responseFromServer2);
                }


                WebRequest request = WebRequest.Create("https://sast.f2crew.com/api/qualitygates/project_status?projectKey=Net-Pay-Advance-Inc_legacy-npa-api_AYR9eG1sbQDryOR5J_O7&branch=qa");

                string token = "sqa_8a9268e3a4d27e1b1d1288840262795afa6e6e39:";
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                string authorizationHeader = $"Basic {credentials}";

                request.Headers.Add("Authorization", authorizationHeader);
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string[] metricKeyOrder = MetricOrder();

                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Root? projectStatus = JsonSerializer.Deserialize<Root>(responseFromServer);
                    projectStatus.projectStatus.respository = "Legacy-Npa-Api";
                    projectStatus.projectStatus.conditions.ForEach(condition =>
                    {
                        var Value = NumberToWords(condition.periodIndex.ToString());
                        condition.periodIndexValue = Value;
                        

                    });

                    var datasecurityhotspots = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_security_hotspots_reviewed");

                    if (datasecurityhotspots == null)
                    {
                        Condition newdatasecurityhotspots = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newdatasecurityhotspots);
                    }

                    var datanewmaintainabilityrating = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_maintainability_rating");

                    if (datanewmaintainabilityrating == null)
                    {
                        Condition datanewmaintainabilityrating10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_maintainability_rating",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewmaintainabilityrating10);
                    }

                    var dataduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "duplicated_lines_density");

                    if (dataduplicatedlinesdensity == null)
                    {
                        Condition dataduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(dataduplicatedlinesdensity10);
                    }

                    var datanewcoverage = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_coverage");

                    if (datanewcoverage == null)
                    {
                        Condition datanewcoverage10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_coverage",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewcoverage10);
                    }

                    var datanewduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_duplicated_lines_density");

                    if (datanewduplicatedlinesdensity == null)
                    {
                        Condition datanewduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewduplicatedlinesdensity10);
                    }

                    var hotspotsValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots");
                    if (hotspotsValues != null)
                    {

                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }
                    else
                    {
                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }

                    var hotspotsreviewedValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots_reviewed");
                    if (hotspotsreviewedValues != null)
                    {

                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsreviewedValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }
                    else
                    {
                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }

                    var coverageValues = measuresComponent.component.measures.Find(condition => condition.metric == "coverage");
                    if (coverageValues != null)
                    {

                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = coverageValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }
                    else
                    {
                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }

                    var newreliabilityratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "reliability_rating");
                    if (newreliabilityratingValues != null)
                    {
                        var Value = default(string);
                        int versionInt = default(int);
                        var lastIndex = newreliabilityratingValues.value;
                        if (decimal.TryParse(lastIndex, out decimal versionDecimal))
                        {
                            versionInt = (int)Math.Round(versionDecimal);
                            // Now, versionInt will contain the rounded integer value
                        }
                        Value = NumberToWords(versionInt.ToString());
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newreliabilityratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);
                    }
                    else
                    {
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);

                    }

                    var newsecurityrating = measuresComponent.component.measures.Find(condition => condition.metric == "security_rating");
                    if (newsecurityrating != null)
                    {
                        var Value = default(string);
                        var lastIndex = newsecurityrating.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newsecurityrating.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }
                    else
                    {
                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-"

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }

                    var bugsValues = measuresComponent.component.measures.Find(condition => condition.metric == "bugs");
                    if (bugsValues != null)
                    {

                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = bugsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }
                    else
                    {
                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }

                    var vulnerabilitiesValues = measuresComponent.component.measures.Find(condition => condition.metric == "new_vulnerabilities");
                    if (vulnerabilitiesValues != null)
                    {
                        var valueVulData = vulnerabilitiesValues.periods;
                        var valueVulData2 = default(Period2);
                        var valueVulData1 = default(string);
                        if (valueVulData != null)
                        {
                            valueVulData2 = vulnerabilitiesValues.periods.Last();
                            valueVulData1 = valueVulData2.value;
                        }
                        else
                        {
                            valueVulData1 = vulnerabilitiesValues.period.value;
                        }
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = valueVulData1,

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }
                    else
                    {
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }

                    var codesmellsValues = measuresComponent.component.measures.Find(condition => condition.metric == "code_smells");
                    if (codesmellsValues != null)
                    {

                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = codesmellsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }
                    else
                    {
                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }

                    var ncLocationValues = measuresComponent.component.measures.Find(condition => condition.metric == "ncloc");
                    if (ncLocationValues != null)
                    {
                        string input = ncLocationValues.value;
                        string result = Regex.Match(input, @"\d+").Value;
                        string formattedNumber = default(string);
                        if (int.TryParse(result, out int number))
                        {
                            int originalNumber = Convert.ToInt32(result);
                            formattedNumber = FormatNumber(originalNumber);
                        }
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = formattedNumber,

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }
                    else
                    {
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }

                    var sqaleindexValues = measuresComponent.component.measures.Find(condition => condition.metric == "sqale_index");
                    if (sqaleindexValues != null)
                    {
                        string result = string.Empty;
                        var daysValue = default(double);
                        string formattedValue = default(string);
                        var sqaleValue = sqaleindexValues.value;
                        if (sqaleValue.Length == 1 || sqaleValue.Length == 2)
                        {
                            daysValue = Convert.ToDouble(sqaleValue);
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            result = $"{formattedValue}min";
                        }
                        else
                        {
                            var NewLastIndex = Convert.ToDouble(sqaleValue) / 60;
                            daysValue = NewLastIndex / 8;
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            string[] parts = Convert.ToString(formattedValue).Split('.');
                            result = $"{parts[0]}d {parts[1]}h";
                        }


                        if (formattedValue == "0")
                        {
                            result = "0";
                        }

                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = result,

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }
                    else
                    {
                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }

                    var securityreviewratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_review_rating");
                    if (securityreviewratingValues != null)
                    {

                        var Value = default(string);
                        var lastIndex = securityreviewratingValues.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = securityreviewratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }
                    else
                    {
                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }

                    var duplicatedblocksValues = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_blocks");
                    if (duplicatedblocksValues != null)
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = duplicatedblocksValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    else
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    var duplicatedlinesdensity = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_lines_density");
                    if (duplicatedlinesdensity != null)
                    {

                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = duplicatedlinesdensity.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }
                    else
                    {
                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }

                    projectStatus.projectStatus.conditions = projectStatus.projectStatus.conditions.OrderBy(c => Array.IndexOf(metricKeyOrder, c.metricKey)).ToList();
                    return Results.Ok(projectStatus);

                }
                return Results.NotFound(responseFromServer);

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }


        internal static async Task<IResult> GetNpaReactWeb()
        {
            try
            {
                string apiUrl2 = BuildApiUrl("Net-Pay-Advance-Inc_npa-react-web_AYR9priBbQDryOR5J_Pi", "qa");
                WebRequest request2 = WebRequest.Create(apiUrl2);

                string token2 = "squ_da59eacbda0568f0f849e709b1fa175558e30024:";
                string credentials2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(token2));
                string authorizationHeader2 = $"basic {credentials2}";

                request2.Headers.Add("Authorization", authorizationHeader2);
                // Get the response.
                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();

                // Get the stream containing content returned by the server.
                Stream dataStream2 = response2.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader2 = new StreamReader(dataStream2);
                // Read the content.
                string responseFromServer2 = reader2.ReadToEnd();

                RootComponent? measuresComponent = default(RootComponent);
                //Dictionary<string, MeasureHistory> lastIndexData = new Dictionary<string, MeasureHistory>();

                if (response2.StatusCode == HttpStatusCode.OK)
                {
                    // Deserialize JSON
                    measuresComponent = JsonSerializer.Deserialize<RootComponent>(responseFromServer2);
                }


                WebRequest request = WebRequest.Create("https://sast.f2crew.com/api/qualitygates/project_status?projectKey=Net-Pay-Advance-Inc_npa-react-web_AYR9priBbQDryOR5J_Pi&branch=qa");

                string token = "sqa_8a9268e3a4d27e1b1d1288840262795afa6e6e39:";
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                string authorizationHeader = $"Basic {credentials}";

                request.Headers.Add("Authorization", authorizationHeader);
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string[] metricKeyOrder = MetricOrder();

                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Root? projectStatus = JsonSerializer.Deserialize<Root>(responseFromServer);
                    projectStatus.projectStatus.respository = "Npa-React-Web";
                    projectStatus.projectStatus.conditions.ForEach(condition =>
                    {
                        var Value = NumberToWords(condition.periodIndex.ToString());
                        condition.periodIndexValue = Value;
                        

                    });

                    var datasecurityhotspots = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_security_hotspots_reviewed");

                    if (datasecurityhotspots == null)
                    {
                        Condition newdatasecurityhotspots = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newdatasecurityhotspots);
                    }

                    var datanewmaintainabilityrating = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_maintainability_rating");

                    if (datanewmaintainabilityrating == null)
                    {
                        Condition datanewmaintainabilityrating10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_maintainability_rating",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewmaintainabilityrating10);
                    }

                    var dataduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "duplicated_lines_density");

                    if (dataduplicatedlinesdensity == null)
                    {
                        Condition dataduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(dataduplicatedlinesdensity10);
                    }

                    var datanewcoverage = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_coverage");

                    if (datanewcoverage == null)
                    {
                        Condition datanewcoverage10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_coverage",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewcoverage10);
                    }

                    var datanewduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_duplicated_lines_density");

                    if (datanewduplicatedlinesdensity == null)
                    {
                        Condition datanewduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewduplicatedlinesdensity10);
                    }

                    var hotspotsValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots");
                    if (hotspotsValues != null)
                    {

                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }
                    else
                    {
                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }

                    var hotspotsreviewedValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots_reviewed");
                    if (hotspotsreviewedValues != null)
                    {

                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsreviewedValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }
                    else
                    {
                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }

                    var coverageValues = measuresComponent.component.measures.Find(condition => condition.metric == "coverage");
                    if (coverageValues != null)
                    {

                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = coverageValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }
                    else
                    {
                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }

                    var newreliabilityratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "reliability_rating");
                    if (newreliabilityratingValues != null)
                    {
                        var Value = default(string);
                        int versionInt = default(int);
                        var lastIndex = newreliabilityratingValues.value;
                        if (decimal.TryParse(lastIndex, out decimal versionDecimal))
                        {
                            versionInt = (int)Math.Round(versionDecimal);
                            // Now, versionInt will contain the rounded integer value
                        }
                        Value = NumberToWords(versionInt.ToString());
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newreliabilityratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);
                    }
                    else
                    {
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);

                    }

                    var newsecurityrating = measuresComponent.component.measures.Find(condition => condition.metric == "security_rating");
                    if (newsecurityrating != null)
                    {
                        var Value = default(string);
                        var lastIndex = newsecurityrating.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newsecurityrating.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }
                    else
                    {
                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-"

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }

                    var bugsValues = measuresComponent.component.measures.Find(condition => condition.metric == "bugs");
                    if (bugsValues != null)
                    {

                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = bugsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }
                    else
                    {
                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }

                    var vulnerabilitiesValues = measuresComponent.component.measures.Find(condition => condition.metric == "new_vulnerabilities");
                    if (vulnerabilitiesValues != null)
                    {
                        var valueVulData = vulnerabilitiesValues.periods;
                        var valueVulData2 = default(Period2);
                        var valueVulData1 = default(string);
                        if (valueVulData != null)
                        {
                            valueVulData2 = vulnerabilitiesValues.periods.Last();
                            valueVulData1 = valueVulData2.value;
                        }
                        else
                        {
                            valueVulData1 = vulnerabilitiesValues.period.value;
                        }
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = valueVulData1,

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }
                    else
                    {
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }

                    var codesmellsValues = measuresComponent.component.measures.Find(condition => condition.metric == "code_smells");
                    if (codesmellsValues != null)
                    {

                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = codesmellsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }
                    else
                    {
                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }

                    var ncLocationValues = measuresComponent.component.measures.Find(condition => condition.metric == "ncloc");
                    if (ncLocationValues != null)
                    {
                        string input = ncLocationValues.value;
                        string result = Regex.Match(input, @"\d+").Value;
                        string formattedNumber = default(string);
                        if (int.TryParse(result, out int number))
                        {
                            int originalNumber = Convert.ToInt32(result);
                            formattedNumber = FormatNumber(originalNumber);
                        }
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = formattedNumber,

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }
                    else
                    {
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }

                    var sqaleindexValues = measuresComponent.component.measures.Find(condition => condition.metric == "sqale_index");
                    if (sqaleindexValues != null)
                    {
                        string result = string.Empty;
                        var daysValue = default(double);
                        string formattedValue = default(string);
                        var sqaleValue = sqaleindexValues.value;
                        if (sqaleValue.Length == 1 || sqaleValue.Length == 2)
                        {
                            daysValue = Convert.ToDouble(sqaleValue);
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            result = $"{formattedValue}min";
                        }
                        else
                        {
                            var NewLastIndex = Convert.ToDouble(sqaleValue) / 60;
                            daysValue = NewLastIndex / 8;
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            string[] parts = Convert.ToString(formattedValue).Split('.');
                            result = $"{parts[0]}d {parts[1]}h";
                        }


                        if (formattedValue == "0")
                        {
                            result = "0";
                        }

                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = result,

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }
                    else
                    {
                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }

                    var securityreviewratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_review_rating");
                    if (securityreviewratingValues != null)
                    {

                        var Value = default(string);
                        var lastIndex = securityreviewratingValues.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = securityreviewratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }
                    else
                    {
                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }

                    var duplicatedblocksValues = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_blocks");
                    if (duplicatedblocksValues != null)
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = duplicatedblocksValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    else
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    var duplicatedlinesdensity = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_lines_density");
                    if (duplicatedlinesdensity != null)
                    {

                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = duplicatedlinesdensity.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }
                    else
                    {
                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }
                    projectStatus.projectStatus.conditions = projectStatus.projectStatus.conditions.OrderBy(c => Array.IndexOf(metricKeyOrder, c.metricKey)).ToList();
                    return Results.Ok(projectStatus);

                }
                return Results.NotFound(responseFromServer);

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

 
        internal static async Task<IResult> GetPaydiniAspx()
        {
            try
            {
                string apiUrl2 = BuildApiUrl("Net-Pay-Advance-Inc_paydini-aspx_AYSGgY9WbQDryOR5J_Rc", "qa");
                WebRequest request2 = WebRequest.Create(apiUrl2);

                string token2 = "squ_da59eacbda0568f0f849e709b1fa175558e30024:";
                string credentials2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(token2));
                string authorizationHeader2 = $"basic {credentials2}";

                request2.Headers.Add("Authorization", authorizationHeader2);
                // Get the response.
                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();

                // Get the stream containing content returned by the server.
                Stream dataStream2 = response2.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader2 = new StreamReader(dataStream2);
                // Read the content.
                string responseFromServer2 = reader2.ReadToEnd();

                RootComponent? measuresComponent = default(RootComponent);
                //Dictionary<string, MeasureHistory> lastIndexData = new Dictionary<string, MeasureHistory>();

                if (response2.StatusCode == HttpStatusCode.OK)
                {
                    // Deserialize JSON
                    measuresComponent = JsonSerializer.Deserialize<RootComponent>(responseFromServer2);
                }


                
                WebRequest request = WebRequest.Create("https://sast.f2crew.com/api/qualitygates/project_status?projectKey=Net-Pay-Advance-Inc_paydini-aspx_AYSGgY9WbQDryOR5J_Rc&branch=qa");

                string token = "sqa_8a9268e3a4d27e1b1d1288840262795afa6e6e39:";
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                string authorizationHeader = $"Basic {credentials}";

                request.Headers.Add("Authorization", authorizationHeader);
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string[] metricKeyOrder = MetricOrder();

                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Root? projectStatus = JsonSerializer.Deserialize<Root>(responseFromServer);
                    projectStatus.projectStatus.respository = "Paydini-Aspx";
                    projectStatus.projectStatus.conditions.ForEach(condition =>
                    {
                        var Value = NumberToWords(condition.periodIndex.ToString());
                        condition.periodIndexValue = Value;
                       

                    });

                    var datasecurityhotspots = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_security_hotspots_reviewed");

                    if (datasecurityhotspots == null)
                    {
                        Condition newdatasecurityhotspots = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newdatasecurityhotspots);
                    }

                    var datanewmaintainabilityrating = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_maintainability_rating");

                    if (datanewmaintainabilityrating == null)
                    {
                        Condition datanewmaintainabilityrating10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_maintainability_rating",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewmaintainabilityrating10);
                    }

                    var datanewcoverage = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_coverage");

                    if (datanewcoverage == null)
                    {
                        Condition datanewcoverage10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_coverage",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewcoverage10);
                    }

                    var datanewduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_duplicated_lines_density");

                    if (datanewduplicatedlinesdensity == null)
                    {
                        Condition datanewduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewduplicatedlinesdensity10);
                    }

                    var dataduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "duplicated_lines_density");

                    if (dataduplicatedlinesdensity == null)
                    {
                        Condition dataduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(dataduplicatedlinesdensity10);
                    }

                    var hotspotsValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots");
                    if (hotspotsValues != null)
                    {

                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }
                    else
                    {
                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }

                    var hotspotsreviewedValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots_reviewed");
                    if (hotspotsreviewedValues != null)
                    {

                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsreviewedValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }
                    else
                    {
                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }

                    var coverageValues = measuresComponent.component.measures.Find(condition => condition.metric == "coverage");
                    if (coverageValues != null)
                    {

                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = coverageValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }
                    else
                    {
                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }

                    var newreliabilityratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "reliability_rating");
                    if (newreliabilityratingValues != null)
                    {
                        var Value = default(string);
                        int versionInt = default(int);
                        var lastIndex = newreliabilityratingValues.value;
                        if (decimal.TryParse(lastIndex, out decimal versionDecimal))
                        {
                            versionInt = (int)Math.Round(versionDecimal);
                            // Now, versionInt will contain the rounded integer value
                        }
                        Value = NumberToWords(versionInt.ToString());
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newreliabilityratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);
                    }
                    else
                    {
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);

                    }

                    var newsecurityrating = measuresComponent.component.measures.Find(condition => condition.metric == "security_rating");
                    if (newsecurityrating != null)
                    {
                        var Value = default(string);
                        var lastIndex = newsecurityrating.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newsecurityrating.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }
                    else
                    {
                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-"

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }

                    var bugsValues = measuresComponent.component.measures.Find(condition => condition.metric == "bugs");
                    if (bugsValues != null)
                    {

                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = bugsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }
                    else
                    {
                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }

                    var vulnerabilitiesValues = measuresComponent.component.measures.Find(condition => condition.metric == "new_vulnerabilities");
                    if (vulnerabilitiesValues != null)
                    {
                        var valueVulData = vulnerabilitiesValues.periods;
                        var valueVulData2 = default(Period2);
                        var valueVulData1 = default(string);
                        if (valueVulData != null)
                        {
                            valueVulData2 = vulnerabilitiesValues.periods.Last();
                            valueVulData1 = valueVulData2.value;
                        }
                        else
                        {
                            valueVulData1 = vulnerabilitiesValues.period.value;
                        }
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = valueVulData1,

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }
                    else
                    {
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }

                    var codesmellsValues = measuresComponent.component.measures.Find(condition => condition.metric == "code_smells");
                    if (codesmellsValues != null)
                    {

                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = codesmellsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }
                    else
                    {
                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }

                    var ncLocationValues = measuresComponent.component.measures.Find(condition => condition.metric == "ncloc");
                    if (ncLocationValues != null)
                    {
                        string input = ncLocationValues.value;
                        string result = Regex.Match(input, @"\d+").Value;
                        string formattedNumber = default(string);
                        if (int.TryParse(result, out int number))
                        {
                            int originalNumber = Convert.ToInt32(result);
                            formattedNumber = FormatNumber(originalNumber);
                        }
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = formattedNumber,

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }
                    else
                    {
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }

                    var sqaleindexValues = measuresComponent.component.measures.Find(condition => condition.metric == "sqale_index");
                    if (sqaleindexValues != null)
                    {
                        string result = string.Empty;
                        var daysValue = default(double);
                        string formattedValue = default(string);
                        var sqaleValue = sqaleindexValues.value;
                        if (sqaleValue.Length == 1 || sqaleValue.Length == 2)
                        {
                            daysValue = Convert.ToDouble(sqaleValue);
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            result = $"{formattedValue}min";
                        }
                        else
                        {
                            var NewLastIndex = Convert.ToDouble(sqaleValue) / 60;
                            daysValue = NewLastIndex / 8;
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            string[] parts = Convert.ToString(formattedValue).Split('.');
                            result = $"{parts[0]}d {parts[1]}h";
                        }


                        if (formattedValue == "0")
                        {
                            result = "0";
                        }

                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = result,

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }
                    else
                    {
                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }

                    var securityreviewratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_review_rating");
                    if (securityreviewratingValues != null)
                    {

                        var Value = default(string);
                        var lastIndex = securityreviewratingValues.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = securityreviewratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }
                    else
                    {
                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }

                    var duplicatedblocksValues = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_blocks");
                    if (duplicatedblocksValues != null)
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = duplicatedblocksValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    else
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    var duplicatedlinesdensity = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_lines_density");
                    if (duplicatedlinesdensity != null)
                    {

                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = duplicatedlinesdensity.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }
                    else
                    {
                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }
                    projectStatus.projectStatus.conditions = projectStatus.projectStatus.conditions.OrderBy(c => Array.IndexOf(metricKeyOrder, c.metricKey)).ToList();
                    return Results.Ok(projectStatus);

                }

                return Results.NotFound(responseFromServer);

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }


        internal static async Task<IResult> GetUnderwritingReactWeb()
        {
            try
            {
                string apiUrl2 = BuildApiUrl("Net-Pay-Advance-Inc_underwriting-react-web_AYTPQEUM2-bht3MdDONV", "qa");
                WebRequest request2 = WebRequest.Create(apiUrl2);

                string token2 = "squ_da59eacbda0568f0f849e709b1fa175558e30024:";
                string credentials2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(token2));
                string authorizationHeader2 = $"basic {credentials2}";

                request2.Headers.Add("Authorization", authorizationHeader2);
                // Get the response.
                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();

                // Get the stream containing content returned by the server.
                Stream dataStream2 = response2.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader2 = new StreamReader(dataStream2);
                // Read the content.
                string responseFromServer2 = reader2.ReadToEnd();

                RootComponent? measuresComponent = default(RootComponent);
                //Dictionary<string, MeasureHistory> lastIndexData = new Dictionary<string, MeasureHistory>();

                if (response2.StatusCode == HttpStatusCode.OK)
                {
                    // Deserialize JSON
                    measuresComponent = JsonSerializer.Deserialize<RootComponent>(responseFromServer2);
                }



                WebRequest request = WebRequest.Create("https://sast.f2crew.com/api/qualitygates/project_status?projectKey=Net-Pay-Advance-Inc_underwriting-react-web_AYTPQEUM2-bht3MdDONV&branch=qa");

                string token = "sqa_8a9268e3a4d27e1b1d1288840262795afa6e6e39:";
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                string authorizationHeader = $"Basic {credentials}";

                request.Headers.Add("Authorization", authorizationHeader);
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string[] metricKeyOrder = MetricOrder();

                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Root? projectStatus = JsonSerializer.Deserialize<Root>(responseFromServer);
                    projectStatus.projectStatus.respository = "Underwriting-React-Web";
                    projectStatus.projectStatus.conditions.ForEach(condition =>
                    {
                        var Value = NumberToWords(condition.periodIndex.ToString());
                        condition.periodIndexValue = Value;
                        

                    });

                    var datasecurityhotspots = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_security_hotspots_reviewed");

                    if (datasecurityhotspots == null)
                    {
                        Condition newdatasecurityhotspots = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newdatasecurityhotspots);
                    }

                    var datanewmaintainabilityrating = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_maintainability_rating");

                    if (datanewmaintainabilityrating == null)
                    {
                        Condition datanewmaintainabilityrating10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_maintainability_rating",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewmaintainabilityrating10);
                    }


                    var dataduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "duplicated_lines_density");

                    if (dataduplicatedlinesdensity == null)
                    {
                        Condition dataduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(dataduplicatedlinesdensity10);
                    }

                    var datanewcoverage = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_coverage");

                    if (datanewcoverage == null)
                    {
                        Condition datanewcoverage10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_coverage",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewcoverage10);
                    }

                    var datanewduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_duplicated_lines_density");

                    if (datanewduplicatedlinesdensity == null)
                    {
                        Condition datanewduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewduplicatedlinesdensity10);
                    }

                    var hotspotsValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots");
                    if (hotspotsValues != null)
                    {

                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }
                    else
                    {
                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }

                    var hotspotsreviewedValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots_reviewed");
                    if (hotspotsreviewedValues != null)
                    {

                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsreviewedValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }
                    else
                    {
                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }

                    var coverageValues = measuresComponent.component.measures.Find(condition => condition.metric == "coverage");
                    if (coverageValues != null)
                    {

                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = coverageValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }
                    else
                    {
                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }

                    var newreliabilityratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "reliability_rating");
                    if (newreliabilityratingValues != null)
                    {
                        var Value = default(string);
                        int versionInt = default(int);
                        var lastIndex = newreliabilityratingValues.value;
                        if (decimal.TryParse(lastIndex, out decimal versionDecimal))
                        {
                            versionInt = (int)Math.Round(versionDecimal);
                            // Now, versionInt will contain the rounded integer value
                        }
                        Value = NumberToWords(versionInt.ToString());

                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newreliabilityratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);
                    }
                    else
                    {
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);

                    }

                    var newsecurityrating = measuresComponent.component.measures.Find(condition => condition.metric == "security_rating");
                    if (newsecurityrating != null)
                    {
                        var Value = default(string);
                        var lastIndex = newsecurityrating.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newsecurityrating.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }
                    else
                    {
                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-"

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }

                    var bugsValues = measuresComponent.component.measures.Find(condition => condition.metric == "bugs");
                    if (bugsValues != null)
                    {

                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = bugsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }
                    else
                    {
                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }

                    var vulnerabilitiesValues = measuresComponent.component.measures.Find(condition => condition.metric == "new_vulnerabilities");
                    if (vulnerabilitiesValues != null)
                    {
                        var valueVulData = vulnerabilitiesValues.periods;
                        var valueVulData2 = default(Period2);
                        var valueVulData1 = default(string);
                        if (valueVulData != null)
                        {
                            valueVulData2 = vulnerabilitiesValues.periods.Last();
                            valueVulData1 = valueVulData2.value;
                        }
                        else
                        {
                            valueVulData1 = vulnerabilitiesValues.period.value;
                        }
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = valueVulData1,

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }
                    else
                    {
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }

                    var codesmellsValues = measuresComponent.component.measures.Find(condition => condition.metric == "code_smells");
                    if (codesmellsValues != null)
                    {

                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = codesmellsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }
                    else
                    {
                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }

                    var ncLocationValues = measuresComponent.component.measures.Find(condition => condition.metric == "ncloc");
                    if (ncLocationValues != null)
                    {
                        string input = ncLocationValues.value;
                        string result = Regex.Match(input, @"\d+").Value;
                        string formattedNumber = default(string);
                        if (int.TryParse(result, out int number))
                        {
                            int originalNumber = Convert.ToInt32(result);
                            formattedNumber = FormatNumber(originalNumber);
                        }
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = formattedNumber,

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }
                    else
                    {
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }

                    var sqaleindexValues = measuresComponent.component.measures.Find(condition => condition.metric == "sqale_index");
                    if (sqaleindexValues != null)
                    {
                        string result = string.Empty;
                        var daysValue = default(double);
                        string formattedValue = default(string);
                        var sqaleValue = sqaleindexValues.value;
                        if (sqaleValue.Length == 1 || sqaleValue.Length == 2)
                        {
                            daysValue = Convert.ToDouble(sqaleValue);
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            result = $"{formattedValue}min";
                        }
                        else
                        {
                            var NewLastIndex = Convert.ToDouble(sqaleValue) / 60;
                            daysValue = NewLastIndex / 8;
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            string[] parts = Convert.ToString(formattedValue).Split('.');
                            result = $"{parts[0]}d {parts[1]}h";
                        }


                        if (formattedValue == "0")
                        {
                            result = "0";
                        }

                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = result,

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }
                    else
                    {
                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }

                    var securityreviewratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_review_rating");
                    if (securityreviewratingValues != null)
                    {

                        var Value = default(string);
                        var lastIndex = securityreviewratingValues.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = securityreviewratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }
                    else
                    {
                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }

                    var duplicatedblocksValues = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_blocks");
                    if (duplicatedblocksValues != null)
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = duplicatedblocksValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    else
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    var duplicatedlinesdensity = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_lines_density");
                    if (duplicatedlinesdensity != null)
                    {

                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = duplicatedlinesdensity.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }
                    else
                    {
                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }
                    projectStatus.projectStatus.conditions = projectStatus.projectStatus.conditions.OrderBy(c => Array.IndexOf(metricKeyOrder, c.metricKey)).ToList();
                    return Results.Ok(projectStatus);

                }
                return Results.NotFound(responseFromServer);

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        internal static async Task<IResult> GetLeadsRestService()
        {
            try
            {

                string apiUrl2 = BuildApiUrl("Net-Pay-Advance-Inc_leads-rest-service_AYTPC45e2-bht3MdDOMl", "qa");
                WebRequest request2 = WebRequest.Create(apiUrl2);

                string token2 = "squ_da59eacbda0568f0f849e709b1fa175558e30024:";
                string credentials2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(token2));
                string authorizationHeader2 = $"basic {credentials2}";

                request2.Headers.Add("Authorization", authorizationHeader2);
                // Get the response.
                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();

                // Get the stream containing content returned by the server.
                Stream dataStream2 = response2.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader2 = new StreamReader(dataStream2);
                // Read the content.
                string responseFromServer2 = reader2.ReadToEnd();

                RootComponent? measuresComponent = default(RootComponent);
                //Dictionary<string, MeasureHistory> lastIndexData = new Dictionary<string, MeasureHistory>();

                if (response2.StatusCode == HttpStatusCode.OK)
                {
                    // Deserialize JSON
                    measuresComponent = JsonSerializer.Deserialize<RootComponent>(responseFromServer2);
                }


                WebRequest request = WebRequest.Create("https://sast.f2crew.com/api/qualitygates/project_status?projectKey=Net-Pay-Advance-Inc_leads-rest-service_AYTPC45e2-bht3MdDOMl&branch=qa");

                string token = "sqa_8a9268e3a4d27e1b1d1288840262795afa6e6e39:";
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                string authorizationHeader = $"Basic {credentials}";

                request.Headers.Add("Authorization", authorizationHeader);
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string[] metricKeyOrder = MetricOrder();

                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Root? projectStatus = JsonSerializer.Deserialize<Root>(responseFromServer);
                    projectStatus.projectStatus.respository = "Leads-Rest-Service";
                    projectStatus.projectStatus.conditions.ForEach(condition =>
                    {
                        var Value = NumberToWords(condition.periodIndex.ToString());
                        condition.periodIndexValue = Value;
                        projectStatus.projectStatus.conditions = projectStatus.projectStatus.conditions.OrderBy(c => Array.IndexOf(metricKeyOrder, c.metricKey)).ToList();

                    });
                    var datasecurityhotspots = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_security_hotspots_reviewed");

                    if (datasecurityhotspots == null)
                    {
                        Condition newdatasecurityhotspots = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newdatasecurityhotspots);
                    }

                    var datanewmaintainabilityrating = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_maintainability_rating");

                    if (datanewmaintainabilityrating == null)
                    {
                        Condition datanewmaintainabilityrating10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_maintainability_rating",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewmaintainabilityrating10);
                    }

                    var dataduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "duplicated_lines_density");

                    if (dataduplicatedlinesdensity == null)
                    {
                        Condition dataduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(dataduplicatedlinesdensity10);
                    }

                    var datanewcoverage = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_coverage");

                    if (datanewcoverage == null)
                    {
                        Condition datanewcoverage10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_coverage",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewcoverage10);
                    }

                    var datanewduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_duplicated_lines_density");

                    if (datanewduplicatedlinesdensity == null)
                    {
                        Condition datanewduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewduplicatedlinesdensity10);
                    }

                    var hotspotsValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots");
                    if (hotspotsValues != null)
                    {

                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }
                    else
                    {
                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }

                    var hotspotsreviewedValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots_reviewed");
                    if (hotspotsreviewedValues != null)
                    {

                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsreviewedValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }
                    else
                    {
                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }

                    var coverageValues = measuresComponent.component.measures.Find(condition => condition.metric == "coverage");
                    if (coverageValues != null)
                    {

                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = coverageValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }
                    else
                    {
                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }

                    var newreliabilityratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "reliability_rating");
                    if (newreliabilityratingValues != null)
                    {
                        var Value = default(string);
                        int versionInt = default(int);
                        var lastIndex = newreliabilityratingValues.value;
                        if (decimal.TryParse(lastIndex, out decimal versionDecimal))
                        {
                            versionInt = (int)Math.Round(versionDecimal);
                            // Now, versionInt will contain the rounded integer value
                        }
                        Value = NumberToWords(versionInt.ToString());
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newreliabilityratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);
                    }
                    else
                    {
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);

                    }

                    var newsecurityrating = measuresComponent.component.measures.Find(condition => condition.metric == "security_rating");
                    if (newsecurityrating != null)
                    {
                        var Value = default(string);
                        var lastIndex = newsecurityrating.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newsecurityrating.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }
                    else
                    {
                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-"

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }

                    var bugsValues = measuresComponent.component.measures.Find(condition => condition.metric == "bugs");
                    if (bugsValues != null)
                    {

                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = bugsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }
                    else
                    {
                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }

                    var vulnerabilitiesValues = measuresComponent.component.measures.Find(condition => condition.metric == "new_vulnerabilities");
                    if (vulnerabilitiesValues != null)
                    {
                        var valueVulData = vulnerabilitiesValues.periods;
                        var valueVulData2 = default(Period2);
                        var valueVulData1 = default(string);
                        if (valueVulData != null)
                        {
                            valueVulData2 = vulnerabilitiesValues.periods.Last();
                            valueVulData1 = valueVulData2.value;
                        }
                        else
                        {
                            valueVulData1 = vulnerabilitiesValues.period.value;
                        }
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = valueVulData1,

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }
                    else
                    {
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }

                    var codesmellsValues = measuresComponent.component.measures.Find(condition => condition.metric == "code_smells");
                    if (codesmellsValues != null)
                    {

                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = codesmellsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }
                    else
                    {
                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }

                    var ncLocationValues = measuresComponent.component.measures.Find(condition => condition.metric == "ncloc");
                    if (ncLocationValues != null)
                    {
                        string input = ncLocationValues.value;
                        string result = Regex.Match(input, @"\d+").Value;
                        string formattedNumber = default(string);
                        if (int.TryParse(result, out int number))
                        {
                            int originalNumber = Convert.ToInt32(result);
                            formattedNumber = FormatNumber(originalNumber);
                        }
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = formattedNumber,

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }
                    else
                    {
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }

                    var sqaleindexValues = measuresComponent.component.measures.Find(condition => condition.metric == "sqale_index");
                    if (sqaleindexValues != null)
                    {
                        string result = string.Empty;
                        var daysValue = default(double);
                        string formattedValue = default(string);
                        var sqaleValue = sqaleindexValues.value;
                        if (sqaleValue.Length == 1 || sqaleValue.Length == 2)
                        {
                            daysValue = Convert.ToDouble(sqaleValue);
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            result = $"{formattedValue}min";
                        }
                        else
                        {
                            var NewLastIndex = Convert.ToDouble(sqaleValue) / 60;
                            daysValue = NewLastIndex / 8;
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            string[] parts = Convert.ToString(formattedValue).Split('.');
                            result = $"{parts[0]}d {parts[1]}h";
                        }


                        if (formattedValue == "0")
                        {
                            result = "0";
                        }

                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = result,

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }
                    else
                    {
                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }

                    var securityreviewratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_review_rating");
                    if (securityreviewratingValues != null)
                    {

                        var Value = default(string);
                        var lastIndex = securityreviewratingValues.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = securityreviewratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }
                    else
                    {
                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }

                    var duplicatedblocksValues = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_blocks");
                    if (duplicatedblocksValues != null)
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = duplicatedblocksValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    else
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    var duplicatedlinesdensity = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_lines_density");
                    if (duplicatedlinesdensity != null)
                    {

                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = duplicatedlinesdensity.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }
                    else
                    {
                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }
                    projectStatus.projectStatus.conditions = projectStatus.projectStatus.conditions.OrderBy(c => Array.IndexOf(metricKeyOrder, c.metricKey)).ToList();
                    return Results.Ok(projectStatus);

                }
                return Results.NotFound(responseFromServer);

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        internal static async Task<IResult> GetCustomersRestService()
        {
            try
            {

                string apiUrl2 = BuildApiUrl("Net-Pay-Advance-Inc_customers-rest-service_AYTOmsnn2-bht3MdDOLi", "qa");
                WebRequest request2 = WebRequest.Create(apiUrl2);

                string token2 = "squ_da59eacbda0568f0f849e709b1fa175558e30024:";
                string credentials2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(token2));
                string authorizationHeader2 = $"basic {credentials2}";

                request2.Headers.Add("Authorization", authorizationHeader2);
                // Get the response.
                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();

                // Get the stream containing content returned by the server.
                Stream dataStream2 = response2.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader2 = new StreamReader(dataStream2);
                // Read the content.
                string responseFromServer2 = reader2.ReadToEnd();

                RootComponent? measuresComponent = default(RootComponent);
                //Dictionary<string, MeasureHistory> lastIndexData = new Dictionary<string, MeasureHistory>();

                if (response2.StatusCode == HttpStatusCode.OK)
                {
                    // Deserialize JSON
                    measuresComponent = JsonSerializer.Deserialize<RootComponent>(responseFromServer2);
                }


                WebRequest request = WebRequest.Create("https://sast.f2crew.com/api/qualitygates/project_status?projectKey=Net-Pay-Advance-Inc_customers-rest-service_AYTOmsnn2-bht3MdDOLi&branch=qa");

                string token = "sqa_8a9268e3a4d27e1b1d1288840262795afa6e6e39:";
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                string authorizationHeader = $"Basic {credentials}";

                request.Headers.Add("Authorization", authorizationHeader);
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string[] metricKeyOrder = MetricOrder();

                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Root? projectStatus = JsonSerializer.Deserialize<Root>(responseFromServer);
                    projectStatus.projectStatus.respository = "Customers-Rest-Service";
                    projectStatus.projectStatus.conditions.ForEach(condition =>
                    {
                        var Value = NumberToWords(condition.periodIndex.ToString());
                        condition.periodIndexValue = Value;
                        projectStatus.projectStatus.conditions = projectStatus.projectStatus.conditions.OrderBy(c => Array.IndexOf(metricKeyOrder, c.metricKey)).ToList();

                    });

                    var datasecurityhotspots = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_security_hotspots_reviewed");

                    if (datasecurityhotspots == null)
                    {
                        Condition newdatasecurityhotspots = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newdatasecurityhotspots);
                    }

                    var datanewmaintainabilityrating = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_maintainability_rating");

                    if (datanewmaintainabilityrating == null)
                    {
                        Condition datanewmaintainabilityrating10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_maintainability_rating",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewmaintainabilityrating10);
                    }

                    var dataduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "duplicated_lines_density");

                    if (dataduplicatedlinesdensity == null)
                    {
                        Condition dataduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(dataduplicatedlinesdensity10);
                    }

                    var datanewcoverage = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_coverage");

                    if (datanewcoverage == null)
                    {
                        Condition datanewcoverage10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_coverage",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewcoverage10);
                    }

                    var datanewduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_duplicated_lines_density");

                    if (datanewduplicatedlinesdensity == null)
                    {
                        Condition datanewduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewduplicatedlinesdensity10);
                    }

                    var hotspotsValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots");
                    if (hotspotsValues != null)
                    {

                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }
                    else
                    {
                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }

                    var hotspotsreviewedValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots_reviewed");
                    if (hotspotsreviewedValues != null)
                    {

                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsreviewedValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }
                    else
                    {
                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }

                    var coverageValues = measuresComponent.component.measures.Find(condition => condition.metric == "coverage");
                    if (coverageValues != null)
                    {

                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = coverageValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }
                    else
                    {
                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }

                    var newreliabilityratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "reliability_rating");
                    if (newreliabilityratingValues != null)
                    {
                        var Value = default(string);
                        int versionInt = default(int);
                        var lastIndex = newreliabilityratingValues.value;
                        if (decimal.TryParse(lastIndex, out decimal versionDecimal))
                        {
                            versionInt = (int)Math.Round(versionDecimal);
                            // Now, versionInt will contain the rounded integer value
                        }
                        Value = NumberToWords(versionInt.ToString());
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newreliabilityratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);
                    }
                    else
                    {
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);

                    }

                    var newsecurityrating = measuresComponent.component.measures.Find(condition => condition.metric == "security_rating");
                    if (newsecurityrating != null)
                    {
                        var Value = default(string);
                        var lastIndex = newsecurityrating.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newsecurityrating.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }
                    else
                    {
                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-"

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }

                    var bugsValues = measuresComponent.component.measures.Find(condition => condition.metric == "bugs");
                    if (bugsValues != null)
                    {

                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = bugsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }
                    else
                    {
                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }

                    var vulnerabilitiesValues = measuresComponent.component.measures.Find(condition => condition.metric == "new_vulnerabilities");
                    if (vulnerabilitiesValues != null)
                    {
                        var valueVulData = vulnerabilitiesValues.periods;
                        var valueVulData2 = default(Period2);
                        var valueVulData1 = default(string);
                        if (valueVulData != null)
                        {
                            valueVulData2 = vulnerabilitiesValues.periods.Last();
                            valueVulData1 = valueVulData2.value;
                        }
                        else
                        {
                            valueVulData1 = vulnerabilitiesValues.period.value;
                        }
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = valueVulData1,

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }
                    else
                    {
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }

                    var codesmellsValues = measuresComponent.component.measures.Find(condition => condition.metric == "code_smells");
                    if (codesmellsValues != null)
                    {

                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = codesmellsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }
                    else
                    {
                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }

                    var ncLocationValues = measuresComponent.component.measures.Find(condition => condition.metric == "ncloc");
                    if (ncLocationValues != null)
                    {
                        string input = ncLocationValues.value;
                        string result = Regex.Match(input, @"\d+").Value;
                        string formattedNumber = default(string);
                        if (int.TryParse(result, out int number))
                        {
                            int originalNumber = Convert.ToInt32(result);
                            formattedNumber = FormatNumber(originalNumber);
                        }
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = formattedNumber,

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }
                    else
                    {
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }

                    var sqaleindexValues = measuresComponent.component.measures.Find(condition => condition.metric == "sqale_index");
                    if (sqaleindexValues != null)
                    {
                        string result = string.Empty;
                        var daysValue = default(double);
                        string formattedValue = default(string);
                        var sqaleValue = sqaleindexValues.value;
                        if (sqaleValue.Length == 1 || sqaleValue.Length == 2)
                        {
                            daysValue = Convert.ToDouble(sqaleValue);
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            result = $"{formattedValue}min";
                        }
                        else
                        {
                            var NewLastIndex = Convert.ToDouble(sqaleValue) / 60;
                            daysValue = NewLastIndex / 8;
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            string[] parts = Convert.ToString(formattedValue).Split('.');
                            result = $"{parts[0]}d {parts[1]}h";
                        }


                        if (formattedValue == "0")
                        {
                            result = "0";
                        }

                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = result,

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }
                    else
                    {
                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }

                    var securityreviewratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_review_rating");
                    if (securityreviewratingValues != null)
                    {

                        var Value = default(string);
                        var lastIndex = securityreviewratingValues.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = securityreviewratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }
                    else
                    {
                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }

                    var duplicatedblocksValues = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_blocks");
                    if (duplicatedblocksValues != null)
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = duplicatedblocksValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    else
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    var duplicatedlinesdensity = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_lines_density");
                    if (duplicatedlinesdensity != null)
                    {

                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = duplicatedlinesdensity.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }
                    else
                    {
                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }
                    projectStatus.projectStatus.conditions = projectStatus.projectStatus.conditions.OrderBy(c => Array.IndexOf(metricKeyOrder, c.metricKey)).ToList();
                    return Results.Ok(projectStatus);

                }
                return Results.NotFound(responseFromServer);

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        internal static async Task<IResult> GetLoansRestService()
        {
            try
            {

                string apiUrl2 = BuildApiUrl("Net-Pay-Advance-Inc_loans-rest-service_AYTPQ15b2-bht3MdDONm", "qa");
                WebRequest request2 = WebRequest.Create(apiUrl2);

                string token2 = "squ_da59eacbda0568f0f849e709b1fa175558e30024:";
                string credentials2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(token2));
                string authorizationHeader2 = $"basic {credentials2}";

                request2.Headers.Add("Authorization", authorizationHeader2);
                // Get the response.
                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();

                // Get the stream containing content returned by the server.
                Stream dataStream2 = response2.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader2 = new StreamReader(dataStream2);
                // Read the content.
                string responseFromServer2 = reader2.ReadToEnd();

                RootComponent? measuresComponent = default(RootComponent);
                //Dictionary<string, MeasureHistory> lastIndexData = new Dictionary<string, MeasureHistory>();

                if (response2.StatusCode == HttpStatusCode.OK)
                {
                    // Deserialize JSON
                    measuresComponent = JsonSerializer.Deserialize<RootComponent>(responseFromServer2);
                }


                WebRequest request = WebRequest.Create("https://sast.f2crew.com/api/qualitygates/project_status?projectKey=Net-Pay-Advance-Inc_loans-rest-service_AYTPQ15b2-bht3MdDONm&branch=qa");

                string token = "sqa_8a9268e3a4d27e1b1d1288840262795afa6e6e39:";
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                string authorizationHeader = $"Basic {credentials}";

                request.Headers.Add("Authorization", authorizationHeader);
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string[] metricKeyOrder = MetricOrder();

                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Root? projectStatus = JsonSerializer.Deserialize<Root>(responseFromServer);
                    projectStatus.projectStatus.respository = "Loans-Rest-Service";
                    projectStatus.projectStatus.conditions.ForEach(condition =>
                    {
                        var Value = NumberToWords(condition.periodIndex.ToString());
                        condition.periodIndexValue = Value;
                        projectStatus.projectStatus.conditions = projectStatus.projectStatus.conditions.OrderBy(c => Array.IndexOf(metricKeyOrder, c.metricKey)).ToList();

                    });

                    var datasecurityhotspots = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_security_hotspots_reviewed");

                    if (datasecurityhotspots == null)
                    {
                        Condition newdatasecurityhotspots = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newdatasecurityhotspots);
                    }

                    var datanewmaintainabilityrating = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_maintainability_rating");

                    if (datanewmaintainabilityrating == null)
                    {
                        Condition datanewmaintainabilityrating10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_maintainability_rating",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewmaintainabilityrating10);
                    }

                    var dataduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "duplicated_lines_density");

                    if (dataduplicatedlinesdensity == null)
                    {
                        Condition dataduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(dataduplicatedlinesdensity10);
                    }

                    var datanewcoverage = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_coverage");

                    if (datanewcoverage == null)
                    {
                        Condition datanewcoverage10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_coverage",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewcoverage10);
                    }

                    var datanewduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_duplicated_lines_density");

                    if (datanewduplicatedlinesdensity == null)
                    {
                        Condition datanewduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewduplicatedlinesdensity10);
                    }

                    var hotspotsValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots");
                    if (hotspotsValues != null)
                    {

                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }
                    else
                    {
                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }

                    var hotspotsreviewedValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots_reviewed");
                    if (hotspotsreviewedValues != null)
                    {

                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsreviewedValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }
                    else
                    {
                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }

                    var coverageValues = measuresComponent.component.measures.Find(condition => condition.metric == "coverage");
                    if (coverageValues != null)
                    {

                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = coverageValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }
                    else
                    {
                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }

                    var newreliabilityratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "reliability_rating");
                    if (newreliabilityratingValues != null)
                    {
                        var Value = default(string);
                        int versionInt = default(int);
                        var lastIndex = newreliabilityratingValues.value;
                        if (decimal.TryParse(lastIndex, out decimal versionDecimal))
                        {
                            versionInt = (int)Math.Round(versionDecimal);
                            // Now, versionInt will contain the rounded integer value
                        }
                        Value = NumberToWords(versionInt.ToString());
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newreliabilityratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);
                    }
                    else
                    {
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);

                    }

                    var newsecurityrating = measuresComponent.component.measures.Find(condition => condition.metric == "security_rating");
                    if (newsecurityrating != null)
                    {
                        var Value = default(string);
                        var lastIndex = newsecurityrating.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newsecurityrating.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }
                    else
                    {
                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-"

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }

                    var bugsValues = measuresComponent.component.measures.Find(condition => condition.metric == "bugs");
                    if (bugsValues != null)
                    {

                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = bugsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }
                    else
                    {
                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }

                    var vulnerabilitiesValues = measuresComponent.component.measures.Find(condition => condition.metric == "new_vulnerabilities");
                    if (vulnerabilitiesValues != null)
                    {
                        var valueVulData = vulnerabilitiesValues.periods;
                        var valueVulData2 = default(Period2);
                        var valueVulData1 = default(string);
                        if (valueVulData != null)
                        {
                            valueVulData2 = vulnerabilitiesValues.periods.Last();
                            valueVulData1 = valueVulData2.value;
                        }
                        else
                        {
                            valueVulData1 = vulnerabilitiesValues.period.value;
                        }
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = valueVulData1,

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }
                    else
                    {
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }

                    var codesmellsValues = measuresComponent.component.measures.Find(condition => condition.metric == "code_smells");
                    if (codesmellsValues != null)
                    {

                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = codesmellsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }
                    else
                    {
                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }

                    var ncLocationValues = measuresComponent.component.measures.Find(condition => condition.metric == "ncloc");
                    if (ncLocationValues != null)
                    {
                        string input = ncLocationValues.value;
                        string result = Regex.Match(input, @"\d+").Value;
                        string formattedNumber = default(string);
                        if (int.TryParse(result, out int number))
                        {
                            int originalNumber = Convert.ToInt32(result);
                            formattedNumber = FormatNumber(originalNumber);
                        }
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = formattedNumber,

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }
                    else
                    {
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }

                    var sqaleindexValues = measuresComponent.component.measures.Find(condition => condition.metric == "sqale_index");
                    if (sqaleindexValues != null)
                    {
                        string result = string.Empty;
                        var daysValue = default(double);
                        string formattedValue = default(string);
                        var sqaleValue = sqaleindexValues.value;
                        if (sqaleValue.Length == 1 || sqaleValue.Length == 2)
                        {
                            daysValue = Convert.ToDouble(sqaleValue);
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            result = $"{formattedValue}min";
                        }
                        else
                        {
                            var NewLastIndex = Convert.ToDouble(sqaleValue) / 60;
                            daysValue = NewLastIndex / 8;
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            string[] parts = Convert.ToString(formattedValue).Split('.');
                            result = $"{parts[0]}d {parts[1]}h";
                        }


                        if (formattedValue == "0")
                        {
                            result = "0";
                        }

                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = result,

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }
                    else
                    {
                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }

                    var securityreviewratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_review_rating");
                    if (securityreviewratingValues != null)
                    {

                        var Value = default(string);
                        var lastIndex = securityreviewratingValues.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = securityreviewratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }
                    else
                    {
                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }

                    var duplicatedblocksValues = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_blocks");
                    if (duplicatedblocksValues != null)
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = duplicatedblocksValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    else
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    var duplicatedlinesdensity = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_lines_density");
                    if (duplicatedlinesdensity != null)
                    {

                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = duplicatedlinesdensity.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }
                    else
                    {
                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }

                    projectStatus.projectStatus.conditions = projectStatus.projectStatus.conditions.OrderBy(c => Array.IndexOf(metricKeyOrder, c.metricKey)).ToList();
                    return Results.Ok(projectStatus);

                }

                return Results.NotFound(responseFromServer);

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        internal static async Task<IResult> GetUnderwritingRestService()
        {
            try
            {

                string apiUrl2 = BuildApiUrl("Net-Pay-Advance-Inc_underwriting-rest-service_AYTOk_XY2-bht3MdDOLD", "qa");
                WebRequest request2 = WebRequest.Create(apiUrl2);

                string token2 = "squ_da59eacbda0568f0f849e709b1fa175558e30024:";
                string credentials2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(token2));
                string authorizationHeader2 = $"basic {credentials2}";

                request2.Headers.Add("Authorization", authorizationHeader2);
                // Get the response.
                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();

                // Get the stream containing content returned by the server.
                Stream dataStream2 = response2.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader2 = new StreamReader(dataStream2);
                // Read the content.
                string responseFromServer2 = reader2.ReadToEnd();

                RootComponent? measuresComponent = default(RootComponent);
                //Dictionary<string, MeasureHistory> lastIndexData = new Dictionary<string, MeasureHistory>();

                if (response2.StatusCode == HttpStatusCode.OK)
                {
                    // Deserialize JSON
                    measuresComponent = JsonSerializer.Deserialize<RootComponent>(responseFromServer2);
                }


                WebRequest request = WebRequest.Create("https://sast.f2crew.com/api/qualitygates/project_status?projectKey=Net-Pay-Advance-Inc_underwriting-rest-service_AYTOk_XY2-bht3MdDOLD&branch=qa");

                string token = "sqa_8a9268e3a4d27e1b1d1288840262795afa6e6e39:";
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                string authorizationHeader = $"Basic {credentials}";

                request.Headers.Add("Authorization", authorizationHeader);
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string[] metricKeyOrder = MetricOrder();

                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Root? projectStatus = JsonSerializer.Deserialize<Root>(responseFromServer);
                    projectStatus.projectStatus.respository = "Underwriting-Rest-Service";
                    projectStatus.projectStatus.conditions.ForEach(condition =>
                    {
                        var Value = NumberToWords(condition.periodIndex.ToString());
                        condition.periodIndexValue = Value;
                      
                    });

                    var datasecurityhotspots = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_security_hotspots_reviewed");

                    if (datasecurityhotspots == null)
                    {
                        Condition newdatasecurityhotspots = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newdatasecurityhotspots);
                    }

                    var datanewmaintainabilityrating = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_maintainability_rating");

                    if (datanewmaintainabilityrating == null)
                    {
                        Condition datanewmaintainabilityrating10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_maintainability_rating",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewmaintainabilityrating10);
                    }

                    var dataduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "duplicated_lines_density");

                    if (dataduplicatedlinesdensity == null)
                    {
                        Condition dataduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(dataduplicatedlinesdensity10);
                    }

                    var datanewcoverage = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_coverage");

                    if (datanewcoverage == null)
                    {
                        Condition datanewcoverage10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_coverage",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewcoverage10);
                    }

                    var datanewduplicatedlinesdensity = projectStatus.projectStatus.conditions.Find(condition => condition.metricKey == "new_duplicated_lines_density");

                    if (datanewduplicatedlinesdensity == null)
                    {
                        Condition datanewduplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_duplicated_lines_density",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(datanewduplicatedlinesdensity10);
                    }

                    var hotspotsValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots");
                    if (hotspotsValues != null)
                    {

                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }
                    else
                    {
                        Condition hotspotsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsValues10);
                    }

                    var hotspotsreviewedValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_hotspots_reviewed");
                    if (hotspotsreviewedValues != null)
                    {

                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = hotspotsreviewedValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }
                    else
                    {
                        Condition hotspotsreviewedValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_hotspots_reviewed",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(hotspotsreviewedValues10);
                    }

                    var coverageValues = measuresComponent.component.measures.Find(condition => condition.metric == "coverage");
                    if (coverageValues != null)
                    {

                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = coverageValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }
                    else
                    {
                        Condition coverageValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "coverage_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(coverageValues10);
                    }

                    var newreliabilityratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "reliability_rating");
                    if (newreliabilityratingValues != null)
                    {
                        var Value = default(string);
                        int versionInt = default(int);
                        var lastIndex = newreliabilityratingValues.value;
                        if (decimal.TryParse(lastIndex, out decimal versionDecimal))
                        {
                            versionInt = (int)Math.Round(versionDecimal);
                            // Now, versionInt will contain the rounded integer value
                        }
                        Value = NumberToWords(versionInt.ToString());
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newreliabilityratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);
                    }
                    else
                    {
                        Condition newreliabilityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_reliability_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(newreliabilityratingValues11);

                    }

                    var newsecurityrating = measuresComponent.component.measures.Find(condition => condition.metric == "security_rating");
                    if (newsecurityrating != null)
                    {
                        var Value = default(string);
                        var lastIndex = newsecurityrating.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = newsecurityrating.value,

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }
                    else
                    {
                        Condition newsecurityratingValues11 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "new_security_rating_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-"

                        };
                        projectStatus.projectStatus.conditions.Add(newsecurityratingValues11);
                    }

                    var bugsValues = measuresComponent.component.measures.Find(condition => condition.metric == "bugs");
                    if (bugsValues != null)
                    {

                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = bugsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }
                    else
                    {
                        Condition bugsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "bugs_ADD",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(bugsValues10);
                    }

                    var vulnerabilitiesValues = measuresComponent.component.measures.Find(condition => condition.metric == "new_vulnerabilities");
                    if (vulnerabilitiesValues != null)
                    {
                        var valueVulData = vulnerabilitiesValues.periods;
                        var valueVulData2 = default(Period2);
                        var valueVulData1 = default(string);
                        if (valueVulData != null)
                        {
                            valueVulData2 = vulnerabilitiesValues.periods.Last();
                            valueVulData1 = valueVulData2.value;
                        }
                        else
                        {
                            valueVulData1 = vulnerabilitiesValues.period.value;
                        }
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = valueVulData1,

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }
                    else
                    {
                        Condition vulnerabilitiesValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "vulnerabilities_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(vulnerabilitiesValues10);
                    }

                    var codesmellsValues = measuresComponent.component.measures.Find(condition => condition.metric == "code_smells");
                    if (codesmellsValues != null)
                    {

                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = codesmellsValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }
                    else
                    {
                        Condition codesmellsValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "code_smells_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(codesmellsValues10);
                    }

                    var ncLocationValues = measuresComponent.component.measures.Find(condition => condition.metric == "ncloc");
                    if (ncLocationValues != null)
                    {
                        string input = ncLocationValues.value;
                        string result = Regex.Match(input, @"\d+").Value;
                        string formattedNumber = default(string);
                        if (int.TryParse(result, out int number))
                        {
                            int originalNumber = Convert.ToInt32(result);
                            formattedNumber = FormatNumber(originalNumber);
                        }
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = formattedNumber,

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }
                    else
                    {
                        Condition ncLocationValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "numberofLines_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(ncLocationValues10);
                    }

                    var sqaleindexValues = measuresComponent.component.measures.Find(condition => condition.metric == "sqale_index");
                    if (sqaleindexValues != null)
                    {
                        string result = string.Empty;
                        var daysValue = default(double);
                        string formattedValue = default(string);
                        var sqaleValue = sqaleindexValues.value;
                        if (sqaleValue.Length == 1 || sqaleValue.Length == 2)
                        {
                            daysValue = Convert.ToDouble(sqaleValue);
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            result = $"{formattedValue}min";
                        }
                        else
                        {
                            var NewLastIndex = Convert.ToDouble(sqaleValue) / 60;
                            daysValue = NewLastIndex / 8;
                            formattedValue = ConvertToFormattedString(Convert.ToDecimal(daysValue));
                            string[] parts = Convert.ToString(formattedValue).Split('.');
                            result = $"{parts[0]}d {parts[1]}h";
                        }


                        if (formattedValue == "0")
                        {
                            result = "0";
                        }

                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = result,

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }
                    else
                    {
                        Condition sqaleindexValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "technicaldebt_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(sqaleindexValues10);
                    }

                    var securityreviewratingValues = measuresComponent.component.measures.Find(condition => condition.metric == "security_review_rating");
                    if (securityreviewratingValues != null)
                    {

                        var Value = default(string);
                        var lastIndex = securityreviewratingValues.value;
                        if (float.TryParse(lastIndex, out float floatValue))
                        {
                            Value = NumberToWords(floatValue.ToString());
                        }

                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = Value,
                            errorThreshold = "-",
                            actualValue = securityreviewratingValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }
                    else
                    {
                        Condition securityreviewratingValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "security_review_rating_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(securityreviewratingValues10);
                    }

                    var duplicatedblocksValues = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_blocks");
                    if (duplicatedblocksValues != null)
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = duplicatedblocksValues.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    else
                    {
                        Condition duplicatedblocksValues10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_blocks",
                            comparator = "GT",
                            periodIndexValue = "",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedblocksValues10);
                    }
                    var duplicatedlinesdensity = measuresComponent.component.measures.Find(condition => condition.metric == "duplicated_lines_density");
                    if (duplicatedlinesdensity != null)
                    {

                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = duplicatedlinesdensity.value,

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }
                    else
                    {
                        Condition duplicatedlinesdensity10 = new Condition
                        {
                            status = "ERROR",
                            metricKey = "duplicated_lines_density_add",
                            comparator = "GT",
                            periodIndexValue = "-",
                            errorThreshold = "-",
                            actualValue = "-",

                        };
                        projectStatus.projectStatus.conditions.Add(duplicatedlinesdensity10);
                    }

                    projectStatus.projectStatus.conditions = projectStatus.projectStatus.conditions.OrderBy(c => Array.IndexOf(metricKeyOrder, c.metricKey)).ToList();
                    return Results.Ok(projectStatus);

                }
                
                return Results.NotFound(responseFromServer);

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }  
        }


        static string BuildApiUrl(string component, string branch)
        {
            string baseUrl = "https://sast.f2crew.com/api/measures/component";
            string additionalFields = "period,metrics";
            string metricKeys = "alert_status,quality_gate_details,bugs,new_bugs,reliability_rating,new_reliability_rating,vulnerabilities,new_vulnerabilities,security_rating,new_security_rating,security_hotspots,new_security_hotspots,security_hotspots_reviewed,new_security_hotspots_reviewed,security_review_rating,new_security_review_rating,code_smells,new_code_smells,sqale_rating,new_maintainability_rating,sqale_index,new_technical_debt,coverage,new_coverage,lines_to_cover,new_lines_to_cover,tests,duplicated_lines_density,new_duplicated_lines_density,duplicated_blocks,ncloc,ncloc,projects,lines,new_lines";

            StringBuilder apiUrlBuilder = new StringBuilder(baseUrl);
            apiUrlBuilder.Append($"?additionalFields={Uri.EscapeDataString(additionalFields)}");
            apiUrlBuilder.Append($"&component={Uri.EscapeDataString(component)}");
            apiUrlBuilder.Append($"&metricKeys={Uri.EscapeDataString(metricKeys)}");
            apiUrlBuilder.Append($"&branch={Uri.EscapeDataString(branch)}");

            return apiUrlBuilder.ToString();
        }

        public static string NumberToWords(string Number)
        {
            if(Convert.ToInt64(Number) == 1)
            {
                return Number = "A";
            }
            else if (Convert.ToInt64(Number) == 1.0)
            {
                return Number = "A";
            }
            else if(Convert.ToInt64(Number) == 2)
            {
                return Number = "B";
            }
            else if (Convert.ToInt64(Number) == 2.0)
            {
                return Number = "B";
            }
            else if(Convert.ToInt64(Number) == 3)
            {
                return Number = "C";
            }
            else if (Convert.ToInt64(Number) == 3.0)
            {
                return Number = "C";
            }
            else if (Convert.ToInt64(Number) == 4)
            {
                return Number = "D";
            }
            else if (Convert.ToInt64(Number) == 4.0)
            {
                return Number = "D";
            }
            else if (Convert.ToInt64(Number) == 5)
            {
                return Number = "E";
            }
            else if (Convert.ToInt64(Number) == 5.0)
            {
                return Number = "E";
            }
            else if (Convert.ToInt64(Number) == 6)
            {
                return Number = "F";
            }
            else if (Convert.ToInt64(Number) == 6.0)
            {
                return Number = "F";
            }

            return Number;
        }

        static string ConvertToFormattedString(decimal value)
        {
            // Round to one decimal place
            decimal roundedValue = Math.Round(value, 1);

            // Convert to string
            string formattedValue = roundedValue.ToString();

            return formattedValue;
        }


        private static string[] MetricOrder()
        {
            string[] metricKeyOrder = {
                        "new_reliability_rating_ADD",
                        "bugs_ADD",
                        "new_security_rating_ADD",
                        "vulnerabilities_add",
                        "new_security_hotspots_reviewed",
                        "new_maintainability_rating",
                        "technicaldebt_add",
                        "code_smells_add",
                        "coverage_add",
                        "duplicated_lines_density_add",
                        "numberofLines_add",
                        "security_hotspots_ADD",
                        "security_review_rating_add",
                        "new_coverage",
                        "new_duplicated_lines_density",
                        "duplicated_blocks",
                        "security_hotspots_reviewed",
                        "coverage",
                        "sqale_rating",
                        "bugs",
                        "new_reliability_rating",
                        "new_security_rating",
                        "duplicated_lines_density"
                    };

            return metricKeyOrder;
        }

        static string FormatNumber(int number)
        {
            if (number >= 1000)
            {
                double kNumber = Math.Round(number / 1000.0, 1);
                return $"{kNumber}K";
            }
            else
            {
                return number.ToString();
            }
        }

    }
}
