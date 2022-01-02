# HttpClientSdk

A practical example of how to write HTTP Client SDKs in .NET. 

API used for testing: https://rapidapi.com/KegenGuyll/api/dad-jokes/

```bash
$ dotnet example

# ┌──────────────────────────────────┬──────────────────────────────────────────────────────────────────────────────┐
# │ Example                          │ Description                                                                  │
# ├──────────────────────────────────┼──────────────────────────────────────────────────────────────────────────────┤
# │ httpclient.api-host              │ MinimalAPI + IHttpClientFactory + HttpClient (HttpClient)                    │
# │ cross-cutting.delegating-handler │ MinimalAPI + IHttpClientFactory + DelegatingHandler (Cross Cutting Concerns) │
# │ cross-cutting.polly              │ MinimalAPI + IHttpClientFactory + DelegatingHandler (Cross Cutting Concerns) │
# │ httpclient.console-di-host       │ Console + IHttpClientFactory + HttpClient (HttpClient)                       │
# │ httpclient.console-host          │ Console + HttpClient (HttpClient)                                            │
# │ refit.api-host                   │ MinimalAPI + IHttpClientFactory + Refit (Refit)                              │
# │ nswag.api-host                   │ MinimalAPI + IHttpClientFactory + NSwag (NSwag)                              │
# └──────────────────────────────────┴──────────────────────────────────────────────────────────────────────────────┘
```

## Provide Token 

All examples assumes you to provide apiKey through environment variables, you can do it like this: `export DADJOKES_TOKEN=<your_token>`.