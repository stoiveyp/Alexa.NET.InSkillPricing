# Alexa.NET.InSkillPricing
A simple package built to work with Alexa In Skill Products

## Add Directive Support
```csharp
	PaymentDirective.AddSupport();
```

## Getting all products for the current user and skill

```csharp
using Alexa.NET.InSkillPricing
...
var client = new InSkillProductsClient(skillRequest);
var response = client.GetProducts();
var count = response.Products.Length;
```

## Getting a specific product for the current user and skill

```csharp
using Alexa.NET.InSkillPricing
...
var client = new InSkillProductsClient(skillRequest);
var product = client.GetProduct("productId");
```

## Adding a directive to buy a product

```csharp
using Alexa.NET.InSkillPricing.Directives
...
var buyDirective = new BuyDirective("amzn1.adg.productId", "correlationToken");

var response = ResponseBuilder.Empty();
response.Response.ShouldEndSession = true;

response.Response.Directives.Add(buyDirective);
```

## Add Request Handler support
```csharp
using Alexa.NET.InSkillPricing.Responses
...
ConnectionRequestHandler.AddToRequestConverter();
...
switch(request)
{
case ConnectionResponseRequest paymentDetail:
//TODO: Handle payment here
}
```
