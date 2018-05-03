# Alexa.NET.InSkillPricing
A simple package built to work with Alexa In Skill Products

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
